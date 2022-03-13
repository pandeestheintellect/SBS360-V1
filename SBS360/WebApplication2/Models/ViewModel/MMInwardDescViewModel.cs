using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class MMInwardDescViewModel
    {

        public int InDescID { get; set; }
        public int Inward_ID { get; set; }
        

        public Nullable<int> ProductID { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Unit of Measure")]
        public string UoM { get; set; }

        public string Remarks { get; set; }

        public ProductViewModel eng_product_master { get; set; }


    }
}