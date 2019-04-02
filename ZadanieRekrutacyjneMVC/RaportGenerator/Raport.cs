using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZadanieRekrutacyjneMVC.Models;
using System.Web.Mvc;

namespace ZadanieRekrutacyjneMVC.RaportGenerator
{
    public class Raport
    {
        private static List<Request> myRequests;

        //ctor
        public Raport(List<Request> req)
        {
            myRequests = req;
        }

        public int GetNumberOfRequests()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            foreach (Request r in myRequests)
            {
                dict[r.ClientId + r.RequestId] = 0;
            }

            foreach (Request r in myRequests)
            {
                dict[r.ClientId + r.RequestId]=1;
            }

            return dict.Values.Sum();
        }

        public int GetNumberOfRequestsByClientID(string clientID)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            foreach (Request r in myRequests.Where(q=>q.ClientId == clientID))
            {
                dict[r.ClientId + r.RequestId] = 0;
            }

            foreach (Request r in myRequests.Where(q=>q.ClientId == clientID))
            {
                dict[r.ClientId + r.RequestId] =  1;
            }

            return dict.Values.Sum();
        }

        public double GetTotalAmount()
        {
            double num = 0;
            foreach(Request r in myRequests)
            {
                num = num + (r.Quantity * r.Price);
            }

            return Math.Round(num, 2); ;
        }

        public double GetTotalAmountByClientID(string clientID)
        {
            double num = 0;
            num = myRequests.Where(q => q.ClientId == clientID).
                Sum(q=>q.Price*q.Quantity);

            return num;
        }

        public  List<Request> GetSortedList(string sortBy)
        {
            var sortedList = myRequests.AsQueryable();

            switch (sortBy)
            {
                case "Name desc":
                    sortedList = sortedList.OrderBy(q => q.Name);
                    break;
            }
            return sortedList.ToList();
        }

        public List<Request> GetListOfAllRequestsByClientID(string clientID)
        {
            var listByClientId = myRequests.Where(q => q.ClientId == clientID).ToList();
            return listByClientId;
        }

        public Dictionary<string, int> GetKeyValuePairs()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            foreach(Request r in myRequests)
            {
                dict[r.Name] = 0;

            }
            foreach (Request r in myRequests)
            {
                dict[r.Name] = dict[r.Name]+r.Quantity;
            }

            return dict;
        }

    }
}