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
    
    public partial class Configuration
    {
        public string ConfigurationID { get; set; }
        public Nullable<bool> IsEmailIntegrationRequired { get; set; }
        public Nullable<bool> IsSMSRequired { get; set; }
        public string VillageName { get; set; }
        public string SirpanchName { get; set; }
        public string GramSevak_Name { get; set; }
    }
}
