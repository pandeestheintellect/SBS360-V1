using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class PaymentReceivableViewModel
    {
        public int PymtID { get; set; }
        public string VoucherNo { get; set; }
        [Display(Name = "Pay From")]
        public Nullable<int> ClientID { get; set; }
        [Display(Name = "Transaction Date"), Required]
        public string Tr_date { get; set; }
        [Display(Name = "Due Date"), Required]
        public string Due_date { get; set; }
        [Display(Name = "Invoice Number"), Required]
        public string DummyNo { get; set; }

        public string InvoiceNo { get; set; }
        public string ClientName { get; set; }

        public string InvoiceType { get; set; }

        public string InvoiceNum { get; set; }
        public Nullable<int> InvoiceID { get; set; }


        [Display(Name = "Transaction Status"), Required]
        public Nullable<int> Tr_status { get; set; }
        [Display(Name = "Your Reference"), Required]
        public string reference { get; set; }
        [Display(Name = "Financial Year"), Required]
        public string FY { get; set; }
        public string Particulars { get; set; }
        [Display(Name = "Amount"),
        RegularExpression(@"^(0|-?\d{0,16}(\.\d{0,2})?)$", ErrorMessage = "Invalid Amount"), Required]
        public Nullable<decimal> Amount { get; set; }
        public string GTAX { get; set; }
        public string PreparedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string ReceivedBy { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public Nullable<int> QuoteID { get; set; }

        public Nullable<int> ReceivableType { get; set; }

        public decimal TotAmt { get; set; }
        public decimal AlreadyPaid { get; set; }


        public ClientViewModel eng_client_master { get; set; }
        public PaymentStatusViewModel eng_sys_pymt_status { get; set; }
        //public List< QuoteViewModel> eng_quote_master { get; set; }
    }
}