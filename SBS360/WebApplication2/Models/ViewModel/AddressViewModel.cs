using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class AddressViewModel
    {
        public int AddressID { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Tel { get; set; }
        public string Web { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Postal_Code { get; set; }
        public string Fax1 { get; set; }
        public string SkypeID { get; set; }
        public string Remarks { get; set; }
    }
}