using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class InvoiceMasterViewModel
    {
        public int InvoiceID { get; set; }
        public string InvoiceNum { get; set; }
        public string InvoiceDate { get; set; }
        public Nullable<int> QuoteID { get; set; }
        public Nullable<int> ClientID { get; set; }
        public string DoNo { get; set; }
        public Nullable<System.DateTime> DoDate { get; set; }
        public string AttentionTo { get; set; }
        public Nullable<int> isFullyPaid { get; set; }
        public string GST { get; set; }
        public Nullable<decimal> TotalInvoiceAmount { get; set; }
        public string InvoiceType { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public string dummyQuoteID { get; set; }
        public string ServicesFor { get; set; }

        public string InvCategory { get; set; }

        public decimal INVValue { get; set; }
        public decimal Percentage { get; set; }
        public string InvTermandCond { get; set; }

        public Nullable<decimal> FinalInvoiceAmount { get; set; }

        public Nullable<decimal> QuoteFinalAmount { get; set; }

        public Nullable<decimal> Discount { get; set; }

        public string ClientName { get; set; }

        public decimal prjAmt { get; set; }
        public decimal invAmt { get; set; }

        public int Qflag { get; set; }
        public string ClientOthers { get; set; }

        public string QuoteRefNum { get; set; }
        public string QuoteDate { get; set; }

        public QuoteViewModel eng_quote_master { get; set; }
        public List<InvoiceDescriptionViewModel> eng_invoice_details { get; set; }

    }
}