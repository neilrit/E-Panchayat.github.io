//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EGram.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public string MessageID { get; set; }
        public string MessageType { get; set; }
        public Nullable<bool> IsEmailRequired { get; set; }
        public Nullable<bool> IsSMSRequired { get; set; }
        public string MessageContent { get; set; }
        public string OTPText { get; set; }
        public string MobileNumber { get; set; }
        public string EmailID { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    }
}