using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZadanieRekrutacyjneMVC.Models;
using ZadanieRekrutacyjneMVC.RaportGenerator;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private List<Request> requests = new List<Request> {
            new Request{ClientId = "1", RequestId = 1, Name="Chleb", Price = 2, Quantity = 3 },
            new Request{ClientId = "1", RequestId = 2, Name="Chleb", Price = 2, Quantity = 3},
            new Request{ClientId = "1", RequestId = 2, Name="Banan", Price = 1, Quantity = 5 },
            new Request{ClientId = "2", RequestId = 1, Name="Bułka", Price = 3, Quantity = 5 },
        };

        [TestMethod]
        public void Sum_Requests_Correctly()
        {
            Raport raport = new Raport(requests);
            var result = raport.GetNumberOfRequests();
            Assert.AreEqual(result, 3);
        }

        [TestMethod]
        public void ClientId_Sum_Requests_Correctly()
        {
            Raport raport = new Raport(requests);
            string clientId = "1";
            var result = raport.GetNumberOfRequestsByClientID(clientId);
            Assert.AreEqual(result, 2);
        }

        [TestMethod]
        public void Sum_Total_Ammount_Correctly()
        {
            Raport raport = new Raport(requests);
            var result = raport.GetTotalAmount();
            Assert.AreEqual(result, requests.Sum(p=>p.Price*p.Quantity));
        }

        [TestMethod]
        public void ClientId_Sum_Total_Ammount_Correctly()
        {
            Raport raport = new Raport(requests);
            string clientId = "2";
            double result = raport.GetTotalAmountByClientID(clientId);
            Assert.AreEqual(result, requests.Where(q=>q.ClientId == clientId).
                Sum(p=>p.Quantity*p.Price));
        }

        [TestMethod]
        public void Sort_byName_Correctly()
        {
            Raport raport = new Raport(requests);
            var result = raport.GetSortedList("Name desc");
            Assert.AreEqual("Banan" ,result[0].Name);
        }

        [TestMethod]
        public void Get_RequestList_ClientId_Correctly()
        {
            Raport raport = new Raport(requests);
            string clientId="1";
            var result = raport.GetListOfAllRequestsByClientID(clientId);


            Assert.AreEqual(requests.Where(q=>q.ClientId == clientId).Count(), result.Count);
        }

        [TestMethod]
        public void Requests_Number_byName_Correctly()
        {
            Raport raport = new Raport(requests);
            var result = raport.GetKeyValuePairs();


            Assert.AreEqual(5, result["Banan"]);
            Assert.AreEqual(6, result["Chleb"]);
        }

    }
}
