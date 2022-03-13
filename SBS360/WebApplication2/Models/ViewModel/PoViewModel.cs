using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class PoViewModel
    {

        public int PoID { get; set; }
        public string PoRefNum { get; set; }

        [Display(Name = "PO Date"), Required]
        public string PoDate { get; set; }
        [Display(Name = "Supplier Name"), Required]
        public Nullable<int> SupplierID { get; set; }
        [Display(Name = "Attention To")]
        public string Attention { get; set; }
        public string Branch_code { get; set; }
        [Display(Name = "Reference")]
        public string YourRef { get; set; }
        [Display(Name = "Payments Terms")]
        public string PaymentTerms { get; set; }
        [Display(Name = "Delivery Location Info")]
        public string DeliveryAddress { get; set; }
        public string GTAX { get; set; }
        [Display(Name = "Order Status")]
        public Nullable<int> OrderStatusID { get; set; }
        public string SupplierPO { get; set; }

        [Display(Name = "PO Amount")]
        public Nullable<decimal> PoValue { get; set; }

        public Nullable<decimal> FinalAmount { get; set; }

        public Nullable<int> isFullyPaid { get; set; }

        public Nullable<decimal> PoAmount { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> ProjectID { get; set; }

      

        public SupplierViewModel eng_supplier_master { get; set; }
        //public QuoteDescriptionViewModel eng_quote_description { get; set; }
        public List<PoDescriptionViewModel> eng_po_description { get; set; }
        public QuoteStatusViewModel eng_sys_quotestatus { get; set; }
        public ProjectViewModel eng_project_master { get; set; }



    }
}