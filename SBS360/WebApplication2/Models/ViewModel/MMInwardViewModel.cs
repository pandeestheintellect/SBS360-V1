using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class MMInwardViewModel
    {

        public int Inward_ID { get; set; }
        [Display(Name = "Inward Number"), StringLength(50)]
        public string Inward_Number { get; set; }

        public Nullable<int> StoreID { get; set; }

        [Display(Name = "Branch Name")]
        public string Branch_Name { get; set; }

        public Nullable<int> SupplierID { get; set; }

        [Display(Name = "DO/Invoice No")]
        public string Invoice_or_DO_Number { get; set; }

        [Display(Name = "DO/Invoice Date")]
        public string Invoice_or_DO_Date { get; set; }

        public int Receipt_Type { get; set; }

        public string Received_Date { get; set; }

        public Nullable<int> ReceivedBy { get; set; }


        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        public SupplierViewModel eng_supplier_master { get; set; }
        public StoreMasterViewModel eng_store_master { get; set; }
        public EmployeeViewModel eng_employee_profile { get; set; }

        public List<MMInwardDescViewModel> eng_mm_inwdesc { get; set; }

        public string Product_Code { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public string UOM { get; set; }
        public string Remarks { get; set; }
        public int PID { get; set; }

        public int DraftFlag { get; set; }


    }
}