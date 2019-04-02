using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;


namespace ZadanieRekrutacyjneMVC.Models
{
    [Serializable()]
    [XmlRoot("requests")]
    public class RequestCollection
    {
        [XmlElement("request")]
        public List<Request> Requests { get; set; }
    }
}