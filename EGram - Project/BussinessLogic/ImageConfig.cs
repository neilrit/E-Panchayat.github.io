using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EGram.BussinessLogic
{
    public class ImageConfigMiddle
    {
        public string ID { get; set; }
        public string RecordType { get; set; }
        public string ImageName { get; set; }
        public string SPPath { get; set; }
        public string RecordNo { get; set; }
        public string Purpose { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Note { get; set; }

        public string attachmentbase { get; set; }
    }
}