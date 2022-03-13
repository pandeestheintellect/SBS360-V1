using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class ClientViewModel
    {

        //public int ClientID { get; set; }
        //public string ClientDisplayID { get; set; }
        //public string Company_Name { get; set; }
        //public Nullable<int> IndustryID { get; set; }
        //public Nullable<int> FunctionalityID { get; set; }
        //public string Reference { get; set; }
        //public Nullable<int> AddressID { get; set; }


        public int ClientID { get; set; }
        [Display(Name = "Client No"), StringLength(50)]
        public string ClientDisplayID { get; set; }
        [Display(Name = "Company Name"), Required, StringLength(200)]
        public string Company_Name { get; set; }
        [Display(Name = "Industry")]
        public Nullable<int> IndustryID { get; set; }
        [Display(Name = "Function")]
        public Nullable<int> FunctionalityID { get; set; }
        public string Reference { get; set; }
        public Nullable<int> AddressID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> IsActive { get; set; }

        public List<ClientContactViewModel> eng_client_contact { get; set; }
        public AddressViewModel eng_address_master { get; set; }
        public FunctionViewModel eng_sys_function { get; set; }
        public IndustryViewModel eng_sys_industry { get; set; }
        public ClientContactViewModel eng_client_contact1 { get; set; }
        //public List< QuoteViewModel> eng_quote_master { get; set; }
    }
}