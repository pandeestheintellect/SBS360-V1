using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class MMOutwardViewModel
    {

        public int Outward_ID { get; set; }
        [Display(Name = "Outward Number"), StringLength(50)]
        public string Outward_Number { get; set; }

        public Nullable<int> StoreID { get; set; }

        [Display(Name = "Branch Name")]
        public string Branch_Name { get; set; }

        public Nullable<int> ClientID { get; set; }

        [Display(Name = "DO Number")]
        public string DO_Number { get; set; }

        [Display(Name = "DO Date")]
        public string DO_Date { get; set; }

        public int Outward_Type { get; set; }

        public string Delivery_Date { get; set; }

        public Nullable<int> DeliveredBy { get; set; }

        public string Vehicle_Number { get; set; }
        public string Delivery_Mode { get; set; }
        public string Project_Location { get; set; }


        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        public ClientViewModel eng_client_master { get; set; }
        public StoreMasterViewModel eng_store_master { get; set; }
        public EmployeeViewModel eng_employee_profile { get; set; }

        public List<MMOutwardDescViewModel> eng_mm_outdesc { get; set; }

        public string Product_Code { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public string UOM { get; set; }
        public string Remarks { get; set; }

        public int PID { get; set; }

        public int DraftFlag { get; set; }


    }
}