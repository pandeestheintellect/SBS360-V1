using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class CustomInvoiceViewModel
    {

        public int InvoiceID { get; set; }
        public string InvoiceDate { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> QuotationID { get; set; }
        public string InvoiceNo { get; set; }
        public string DoNo { get; set; }
        public Nullable<System.DateTime> DODate { get; set; }

        public string GTAX { get; set; }

        public Nullable<decimal> FinalInvoiceAmount { get; set; }

        public Nullable<decimal> QuoteFinalAmount { get; set; }

        public Nullable<decimal> Discount { get; set; }

        public string ProjectNo { get; set; }

        public decimal prjAmt { get; set; }
        public decimal invAmt { get; set; }

        public string QuoteNo { get; set; }
        public List<QuoteDescriptionViewModel> eng_quote_description { get; set; }

    }
}