using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class InvoiceDescriptionViewModel
    {
        public int InvoiceDescID { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public Nullable<decimal> Price { get; set; }


    }
}