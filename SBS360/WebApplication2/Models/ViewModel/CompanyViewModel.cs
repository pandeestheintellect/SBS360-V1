using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class CompanyViewModel
    {

       
        public int CompanyID { get; set; }
        [Display(Name = "Company Name"), Required, StringLength(200)]
        public string CompanyName { get; set; }
        [Display(Name = "Invoice Authorization Name"), StringLength(200)]
        public string Auth_InvoiceName { get; set; }
        [Display(Name = "Invoice Terms"),  StringLength(200)]
        public string InvoiceTerms { get; set; }
        [Display(Name = "Company Logo")]
        public HttpPostedFileBase profile_Path { get; set; }
               
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; } 

        public string Pincode { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

        public string RegNo { get; set; }
        public string GstRegNo { get; set; }

        public string LogoPath { get; set; }

        public decimal Normal_Work_Hours { get; set; }
        public decimal Weekend_Work_Hours { get; set; }
        public decimal Lunch_Break_Hours { get; set; }
        public decimal GST { get; set; }




    }
}