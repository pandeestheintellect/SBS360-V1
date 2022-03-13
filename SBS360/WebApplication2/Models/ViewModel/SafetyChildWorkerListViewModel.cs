using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class SafetyChildWorkerListViewModel
    {
        public int WLID { get; set; }
        public int SafetyID { get; set; }
        public int EmpID { get; set; }

        public EmployeeViewModel eng_employee_profile { get; set; }
    }
}