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
    
    public partial class eng_mm_inwdesc
    {
        public int InDescID { get; set; }
        public Nullable<int> Inward_ID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string UoM { get; set; }
        public string Remarks { get; set; }
    
        public virtual eng_inward eng_inward { get; set; }
        public virtual eng_product_master eng_product_master { get; set; }
    }
}
