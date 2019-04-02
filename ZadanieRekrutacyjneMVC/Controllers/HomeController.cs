using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ZadanieRekrutacyjneMVC.Models;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;
using CsvHelper;
using ZadanieRekrutacyjneMVC.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;
using ZadanieRekrutacyjneMVC.RaportGenerator;


namespace ZadanieRekrutacyjneMVC.Controllers
{
    public class HomeController : Controller
    {
        public  RequestCollection reqCollection= new RequestCollection();
        public static List<Request> reqList = new List<Request>();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase inputFile)
        {
            try
            {
                if (!Path.GetFileName(inputFile.FileName).EndsWith(".json")
                   && !Path.GetFileName(inputFile.FileName).EndsWith(".xml")
                   && !Path.GetFileName(inputFile.FileName).EndsWith(".csv"))
                {
                    ViewBag.Error = "Invalid file type!";
                }
                else
                {
                    reqList = SaveInputFileInFolderAndGetData(inputFile);
                }
            }
            catch(NullReferenceException ex)
            {
                ViewBag.ex = ex.Message;
            }
            return View("Index");
        }

        public List<Request> SaveInputFileInFolderAndGetData(HttpPostedFileBase inputFile)
        {
            if (Path.GetFileName(inputFile.FileName).EndsWith(".json"))
            {
                inputFile.SaveAs(Server.MapPath("~/JsonFiles/" + Path.GetFileName(inputFile.FileName)));
                return GetJsonData(inputFile);
            }
            else if (Path.GetFileName(inputFile.FileName).EndsWith(".xml"))
            {
                inputFile.SaveAs(Server.MapPath("~/XmlFiles/" + Path.GetFileName(inputFile.FileName)));
                return GetXmlData(inputFile);
            }
            else
            {
                inputFile.SaveAs(Server.MapPath("~/CsvFiles/" + Path.GetFileName(inputFile.FileName)));
                return GetCsvData(inputFile);
            }
        }

        ///JSON///
        public List<Request> GetJsonData(HttpPostedFileBase jsonFile)
        {
            try
            {
                StreamReader sr = new StreamReader(Server.MapPath("~/JsonFiles/" + Path.GetFileName(jsonFile.FileName)));
                string jsonData = sr.ReadToEnd();
                reqCollection = JsonConvert.DeserializeObject<RequestCollection>(jsonData);

                foreach (Request r in reqCollection.Requests)
                {
                    reqList.Add(r);
                    if (r.ClientId == null || r.Name == null)
                    {
                        reqList.Clear();
                        throw new JsonSerializationException();
                    }
                }
                ViewBag.Success = "Success";

                sr.Close();
            }
            catch (JsonSerializationException ex)
            {
                ViewBag.ex = "Plik zawiera nie prawidłowe dane: " + ex.Message;
            }

            return reqList;
        }

        ///XML///
        public List<Request> GetXmlData(HttpPostedFileBase xmlFile)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(RequestCollection));
                StreamReader sr = new StreamReader(Server.MapPath("~/XmlFiles/" + Path.GetFileName(xmlFile.FileName)));
                reqCollection = (RequestCollection)xmlSerializer.Deserialize(sr);

                foreach (Request r in reqCollection.Requests)
                {
                    reqList.Add(r);
                    if (r.ClientId == null || r.Name == null)
                    {
                        reqList.Clear();
                        throw new InvalidOperationException();
                    }
                }
                ViewBag.Success = "Success";
                sr.Close();
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.ex = "Plik zawiera nie prawidłowe dane: " + ex.Message;
            }
            return reqList;
        }

        ///CSV///
        public List<Request> GetCsvData(HttpPostedFileBase csvFile)
        {
            try
            {
                StreamReader sr = new StreamReader(Server.MapPath("~/CsVFiles/" + Path.GetFileName(csvFile.FileName)));
                using (var csv = new CsvReader(sr))
                {
                    reqCollection.Requests = new List<Request>();
                    csv.Configuration.RegisterClassMap<UserMap>();
                    var doubleOptions = new TypeConverterOptions
                    {
                        CultureInfo = new CultureInfo("en"), //<-- kropka zamiast przecinka przy czytaniu danych!
                        NumberStyle = NumberStyles.Number
                    };

                    csv.Configuration.TypeConverterOptionsCache.AddOptions(typeof(double), doubleOptions);
                    csv.Configuration.Delimiter = ",";
                    while (csv.Read())
                    {
                        Request re = csv.GetRecord<Request>();
                        reqList.Add(re);
                        if (re.ClientId == null || re.Name == null)
                        {
                            reqList.Clear();
                            throw new Exception();
                        }
                    }
                    ViewBag.Success = "Success";
                    sr.Close();
                }
            }
            catch(Exception ex)
            {
                ViewBag.ex = "Plik zawiera nie prawidłowe dane: " + ex.Message;
            }          
            return reqList;
        }


        ///Akcje raportów///

        public ActionResult NumberOfRequestRepo()
        {
            Raport raport = new Raport(reqList);
            int numOfRequests = raport.GetNumberOfRequests();
            return View(numOfRequests);
        }

        public ActionResult NumberOfRequestByClientIDRepo(string clientID)
        {
            Raport raport = new Raport(reqList);
            int numOfRequests = raport.GetNumberOfRequestsByClientID(clientID);
            return View("NumberOfRequestRepo", numOfRequests);
        }

        public ActionResult TotalAmountRepo()
        {
            Raport raport = new Raport(reqList);
            double totalPrice = raport.GetTotalAmount();
            return View(totalPrice);
        }

        public ActionResult TotalAmountByClientID(string clientID)
        {
            Raport raport = new Raport(reqList);
            double num = raport.GetTotalAmountByClientID(clientID);
            return View("TotalAmountRepo", num);
        }

        public ActionResult ListOfAllRequests(string sortBy)
        {
            ViewBag.SortNameParam = string.IsNullOrEmpty(sortBy) ? "Name desc" : "";

            Raport raport = new Raport(reqList);
            reqList = raport.GetSortedList(sortBy);         
            return View(reqList);
        }

        public ActionResult ListOfAllRequestsByClientID(string clientID, string sortBy)
        {
            ViewBag.SortNameParam = string.IsNullOrEmpty(sortBy) ? "Name desc" : "";
            ViewBag.clientID = clientID;

            Raport raport = new Raport(reqList);
            List<Request> Listparam = raport.GetListOfAllRequestsByClientID(clientID);
            Raport raport2 = new Raport(Listparam);
            Listparam = raport2.GetSortedList(sortBy);
            return View(Listparam);
        }

        public ActionResult AverageValueOfRequest()
        {
            Raport raport = new Raport(reqList);
            double averageValue = Math.Round
                ((raport.GetTotalAmount()) / (raport.GetNumberOfRequests()),2);
            return View(averageValue);
        }

        public ActionResult AverageValueOfRequestByClientId(string clientID)
        {
            Raport raport = new Raport(reqList);
            List<Request> Listparam = raport.GetListOfAllRequestsByClientID(clientID);
            Raport raport2 = new Raport(Listparam);
            double averageValue = Math.Round
                ((raport.GetTotalAmount()) / (raport.GetNumberOfRequests()), 2);
            return View("AverageValueOfRequest", averageValue);
        }

        public ActionResult AmountRequestGrupByName()
        {
            Raport raport = new Raport(reqList);
            Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
            keyValuePairs = raport.GetKeyValuePairs();
            return View(keyValuePairs);
        }

        public ActionResult AmountRequestGrupByNameAndClientID(string clientID)
        {
            Raport raport = new Raport(reqList);
            List<Request> Listparam = raport.GetListOfAllRequestsByClientID(clientID);
            Raport raport2 = new Raport(Listparam);
            Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
            keyValuePairs = raport2.GetKeyValuePairs();
            return View("AmountRequestGrupByName",keyValuePairs);
        }

        public ActionResult RangePriceRequests(int fromPrice , int toPrice )
        {
            return View(reqList.Where(q => q.Price >= fromPrice && q.Price <= toPrice));
        }
    }
}