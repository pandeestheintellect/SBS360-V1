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
    
    public partial class eng_pymt_receivable
    {
        public int PymtID { get; set; }
        public string VoucherNo { get; set; }
        public Nullable<int> ClientID { get; set; }
        public Nullable<System.DateTime> Tr_date { get; set; }
        public Nullable<System.DateTime> Due_date { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<int> Tr_status { get; set; }
        public string reference { get; set; }
        public string FY { get; set; }
        public string Particulars { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string GTAX { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> ReceivableType { get; set; }
        public Nullable<int> QuoteID { get; set; }
        public string InvoiceType { get; set; }
        public Nullable<int> InvoiceID { get; set; }
    
        public virtual eng_client_master eng_client_master { get; set; }
        public virtual eng_sys_pymt_status eng_sys_pymt_status { get; set; }
    }
}