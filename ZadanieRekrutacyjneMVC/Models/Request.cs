using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ZadanieRekrutacyjneMVC.Models
{
    [Serializable()]
    public class Request
    {
        private string _clientId;

        [XmlElement("clientId")]
        public string ClientId
        {
            get {return _clientId;}
            set
            {
                if (value.Length > 6 || value.Contains(" "))
                {
                    _clientId = null;               
                }
                else
                {
                     _clientId = value;
                }
            }
        }

        [XmlElement("requestId")]
        public long RequestId { get; set; }

        private string _name;

        [XmlElement("name")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value.Length > 255)
                {
                    _name = null;
                }
                else
                {
                    _name = value;
                }
            }
        }

        [XmlElement("quantity")]
        public int Quantity { get; set; }

        [XmlElement("price")]
        public double Price { get; set; }
    }
}