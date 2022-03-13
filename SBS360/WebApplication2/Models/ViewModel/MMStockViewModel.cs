using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class MMStockViewModel
    {
        public int ProductID { get; set; }

        public int StoreID { get; set; }

        public int Quantity { get; set; }    

        public string Measuring_Unit { get; set; }       

       public string Product_Code { get; set; }
        public string Product_Name { get; set; }
        public string Product_Description { get; set; }
        public string Product_Company_Name { get; set; }
        public string Product_Type { get; set; }

        public string Store_Code { get; set; }
        public string Store_Name { get; set; }
    }
}