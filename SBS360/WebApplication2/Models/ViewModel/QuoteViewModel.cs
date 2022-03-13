using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class QuoteViewModel
    {
        public int QuoteID { get; set; }
        public string QuoteRefNum { get; set; }

        public int RevFlag { get; set; }
        public int RvFlag { get; set; }

        [Display(Name = "Quotation  Date"), Required]
        public string QuoteDate { get; set; }
        [Display(Name = "Client Name"), Required]
        public Nullable<int> ClientID { get; set; }

        [Display(Name = "Attention To"), Required, Range(1, int.MaxValue, ErrorMessage = "Please select attention to")]
        public Nullable<int> Attention_CCID { get; set; }
        public string Branch_code { get; set; }

        [Display(Name = "Category"), Required]
        public string QuoteCategory { get; set; }

        public string QuoteTitle { get; set; }

        [Display(Name = "Valid Till")]
        public string ValidTill { get; set; }
        [Display(Name = "Reference")]
        public string YourRef { get; set; }
        [Display(Name = "Payments Terms")]
        public string PaymentTerms { get; set; }
        [Display(Name = "Terms & Conditions")]
        public string TermsAndCond { get; set; }
        [Display(Name = "GST")]
        public string GTAX { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> FinalAmount { get; set; }

        public Nullable<decimal> QuoteAmount { get; set; }
        public Nullable<decimal> QuoteDiscount { get; set; }
        [Display(Name = "Quote Amount")]
        public Nullable<decimal> QuoteTotValue { get; set; }
        public Nullable<int> QuoteStatusID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        [Display(Name = "Auto Project Creation")]
        public bool isAutoApproved { get; set; }
        [Display(Name = "Multiple Projects Creation")]
        public bool isProjectCreated { get; set; }
        public string InvoiceNo { get; set; }
        public string ClientQuote { get; set; }

        [Display(Name = "Discount"),
        RegularExpression(@"^(0|-?\d{0,16}(\.\d{0,2})?)$", ErrorMessage = "Invalid Price")]
        public Nullable<decimal> Discount { get; set; }

        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<System.DateTime> DODate { get; set; }

        public string CompanyName { get; set; }

        public string ProjectTitle { get; set; }

        public string InvoiceTerms { get; set; }
        public string Auth_InvoiceName { get; set; }
        public string DoNo { get; set; }
        public Nullable<int> Is_invoice_released { get; set; }
        public Nullable<int> Is_Quote_level_inv { get; set; }
        public Nullable<int> Is_Project_level_inv { get; set; }
        public Nullable<int> Is_Custom_level_inv { get; set; }
        public Nullable<int> isFullyPaid { get; set; }
        public Nullable<int> Is_do_released { get; set; }
        public string invdochk {get; set;}
        public Nullable<int> Is_Quote_level_do { get; set; }

        public Nullable<int> InvoiceFlag { get; set; }

        public ClientViewModel eng_client_master { get; set; }
        //public QuoteDescriptionViewModel eng_quote_description { get; set; }
        public List<QuoteDescriptionViewModel> eng_quote_description { get; set; }
        public QuoteStatusViewModel eng_sys_quotestatus { get; set; }

        public List<ClientContactViewModel> attention { get; set; }

    }
}