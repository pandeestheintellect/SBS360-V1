using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }

        [Display(Name = "Product Name"), Required, StringLength(150)]
        public string Product_Name { get; set; }
        
        [Display(Name = "Product Type"), StringLength(100)]
        public string Product_Type { get; set; }

        [Display(Name = "Company Name"),  StringLength(150)]
        public string Product_Company_Name { get; set; }

        
        [Display(Name = "Product Description"), StringLength(250)]
        public string Product_Description { get; set; }


        [Display(Name = "Dimension"),  StringLength(50)]
        public string Dimension { get; set; }

        [Display(Name = "Unit of Mesaure"), StringLength(50)]
        public string Measuring_Unit { get; set; }

        [Display(Name = "Unit Price"), Required]
        public decimal Unit_Price { get; set; }


        [Display(Name = "Product Code"), StringLength(50)]
        public string Product_Code { get; set; }


        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> IsActive { get; set; }


        // public   eng_contact_master eng_contact_master { get; set; }


    }
}