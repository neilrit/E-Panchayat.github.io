using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_GramProject.Models
{
    public class ServiceModel
    {
        public string id { get; set; }
        public string Subject { get; set; }
        public string Raisedby{ get; set; }
        public string Raisedagainst { get; set; }
        public string Description { get; set; }
        public DateTime dateofevent { get; set; }
        
   }

    public class Attachment
    {
        public string ID { get; set; }
        public string proofof { get; set; }
        public string proofdoc { get; set; }
        public string serviceid { get; set; }
    }
}