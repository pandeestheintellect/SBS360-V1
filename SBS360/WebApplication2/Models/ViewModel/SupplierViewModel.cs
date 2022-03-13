using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class SupplierViewModel
    {
        public int SupplierID { get; set; }

        [Display(Name = "Supplier Code"), StringLength(50)]
        public string SupplierDisplayID { get; set; }
        
        [Display(Name = "Industry")]
        public Nullable<int> IndustryID { get; set; }

        [Display(Name = "Company Name"),  Required, StringLength(150)]
        public string Company_Name { get; set; }

        [Display(Name = "SPOC Name"), StringLength(100)]
        public string Spoc_Name { get; set; }


        [Display(Name = "Supplier Description"), Required, StringLength(250)]
        public string Supplier_Description { get; set; }
        
        [Display(Name = "Product Code"), StringLength(50)]
        public string Product_Code { get; set; }

        public int AddressID { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> IsActive { get; set; }

        public AddressViewModel eng_address_master { get; set; }
        public IndustryViewModel eng_sys_industry { get; set; }
        // public   eng_supplier_master eng_supplier_master { get; set; }


    }
}