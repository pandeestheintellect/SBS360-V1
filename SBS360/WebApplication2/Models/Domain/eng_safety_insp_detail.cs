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
    
    public partial class eng_safety_insp_detail
    {
        public int SIDetailID { get; set; }
        public Nullable<int> SAFINSID { get; set; }
        public Nullable<int> SIItemID { get; set; }
        public Nullable<int> Is_Applicable { get; set; }
        public string Recommendation { get; set; }
        public string ResponsiblePerson { get; set; }
        public Nullable<System.DateTime> ACDate { get; set; }
    
        public virtual eng_safety_insp_master eng_safety_insp_master { get; set; }
        public virtual eng_sys_safety_insp_items eng_sys_safety_insp_items { get; set; }
    }
}
