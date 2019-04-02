using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsvHelper.Configuration;
using ZadanieRekrutacyjneMVC.Models;

namespace ZadanieRekrutacyjneMVC.Configuration
{
    public sealed class UserMap : ClassMap<Request>
    {
        public UserMap()
        {
            Map(m => m.ClientId).Index(0);
            Map(m => m.RequestId).Index(1);
            Map(m => m.Name).Index(2);
            Map(m => m.Quantity).Index(3);
            Map(m => m.Price).Index(4);
        }
    }
}