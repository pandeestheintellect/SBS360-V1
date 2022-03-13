using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class QuoteDescriptionViewModel
    {
        public int QDID { get; set; }
        public Nullable<int> QuoteID { get; set; }
        [Display(Name = "Quantity"),
        RegularExpression(@"^(0|-?\d{0,16}(\.\d{0,4})?)$", ErrorMessage = "Invalid Entry"), Required]
        public decimal Quantity { get; set; }
        [Display(Name = "Quote Description"), Required]
        public string QuoteDescription { get; set; }
        [Display(Name = "Unit Of Measure(UoM)")]
        public string UnitOfMeasure { get; set; }
        [Display(Name = "Quote Price"),
        RegularExpression(@"^(0|-?\d{0,16}(\.\d{0,2})?)$", ErrorMessage = "Invalid Price"), Required]
        public Nullable<decimal> QuotePrice { get; set; }
        public Nullable<int> QuoteDiscPercent { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public Nullable<int> AddedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> ProjectID { get; set; }

    }
}