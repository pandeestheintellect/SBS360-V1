using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class MMStockAdjustViewModel
    {

        public int StockAdjID { get; set; }
        public string Stock_Taking_Number { get; set; }
        public Nullable<int> StoreID { get; set; }
        public string Branch_Name { get; set; }
        public string Stock_Taking_Date { get; set; }
        public Nullable<int> Stock_Taken_By { get; set; }
        public int AdjReason { get; set; }
        public int AdjType { get; set; }
        public string Adj_Ref_Number { get; set; }
        public string Adj_Ref_Date { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public  EmployeeViewModel eng_employee_profile { get; set; }
        public  ProductViewModel eng_product_master { get; set; }
        public  StoreMasterViewModel eng_store_master { get; set; }
          

        public string Measuring_Unit { get; set; }       

        public string Product_Code { get; set; }
        public string Product_Name { get; set; }
        public string Product_Description { get; set; }
        public string Product_Company_Name { get; set; }
        public string Product_Type { get; set; }

        public int PID { get; set; }
        public Nullable<int> ActualStock { get; set; }
        public Nullable<int> CurrentStock { get; set; }

        public string Store_Code { get; set; }
        public string Store_Name { get; set; }
    }
}