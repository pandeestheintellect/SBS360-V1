//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Eng360Web.Models.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class eng_ra_trans_detail1
    {
        public int RAWADetID { get; set; }
        public Nullable<int> RAID { get; set; }
        public string Locations { get; set; }
        public string Process { get; set; }
        public string WorkActivities { get; set; }
    
        public virtual eng_ra_trans_master eng_ra_trans_master { get; set; }
    }
}
