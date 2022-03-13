using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Eng360Web.Models.ViewModel
{
    public class PoDescriptionViewModel
    {
        public int PDID { get; set; }
        public Nullable<int> PoID { get; set; }
        public Nullable<int> Quantity { get; set; }
        [Display(Name = "PO Description")]
        public string PODescription { get; set; }
        [Display(Name = "Unit Of Measure(UoM)")]
        public string UnitOfMeasure { get; set; }
        [Display(Name = "PO Price"),
        RegularExpression(@"^(0|-?\d{0,16}(\.\d{0,2})?)$",ErrorMessage="Invalid Price")]        
        public Nullable<decimal> PoPrice { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> AddedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }


        

    }
}