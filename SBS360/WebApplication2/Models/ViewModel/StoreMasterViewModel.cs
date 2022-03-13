using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class StoreMasterViewModel
    {

        public int StoreID { get; set; }
        [Display(Name = "Store Code"), StringLength(50)]
        public string Store_Code { get; set; }
        [Display(Name = "Company Name")]
        public string Branch_Name { get; set; }
        [Display(Name = "Start Date")]
        public string Start_Date { get; set; }
        [Display(Name = "Store Name"), Required]
        public string Store_Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Store_Description { get; set; }
        public string Incharge_Name { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
       


    }
}