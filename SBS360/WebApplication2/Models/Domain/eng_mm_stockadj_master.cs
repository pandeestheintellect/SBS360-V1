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
    
    public partial class eng_mm_stockadj_master
    {
        public int StockAdjID { get; set; }
        public string Stock_Taking_Number { get; set; }
        public Nullable<int> StoreID { get; set; }
        public string Branch_Name { get; set; }
        public Nullable<System.DateTime> Stock_Taking_Date { get; set; }
        public Nullable<int> Stock_Taken_By { get; set; }
        public Nullable<int> AdjReason { get; set; }
        public Nullable<int> AdjType { get; set; }
        public string Adj_Ref_Number { get; set; }
        public Nullable<System.DateTime> Adj_Ref_Date { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> ActualStock { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        public virtual eng_employee_profile eng_employee_profile { get; set; }
        public virtual eng_product_master eng_product_master { get; set; }
        public virtual eng_store_master eng_store_master { get; set; }
    }
}
