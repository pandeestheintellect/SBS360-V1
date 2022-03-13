using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class MMTransMasterViewModel
    {

        public int MMTRID { get; set; }
       
        

        public Nullable<int> ProductID { get; set; }

        public Nullable<int> StoreID { get; set; }

        public int Quantity { get; set; }
                
        public string inoutadj_ref { get; set; }

        public string UoM { get; set; }

        public string Trn_Date { get; set; }

        public ProductViewModel eng_product_master { get; set; }


    }
}