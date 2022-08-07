using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EGram.Models
{
   
        public  class ComplaintModal
        {
            public int Complaint_Number { get; set; }
            public string Raised_By { get; set; }
            [Display(Name = "MobilePhone")]
            public string RaisedByUniqueID { get; set; }
            public string AgainstParty { get; set; }
            public string Subject { get; set; }
            public string Description { get; set; }
            public string CreatedOn { get; set; }
            public string Purpose { get; set; }
        public IEnumerable<SelectListItem> Status { get; set; }

        public string StatusText { get; set; }
        [Display(Name = "Review")]
            public string Reviewbycomitee { get; set; }

            public Nullable<int> Meeting { get; set; }
            public string FileName { get; set; }
            public HttpPostedFile ImageFile { get; set; }
        }
    
}