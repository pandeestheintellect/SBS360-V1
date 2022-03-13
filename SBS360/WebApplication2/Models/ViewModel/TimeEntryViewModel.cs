using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class TimeEntryViewModel
    {

        public int TEID { get; set; }
        [Display(Name = "Employee Name"),Required]
        public Nullable<int> EmpID { get; set; }
        [Display(Name = "Project Name"),Required]
        public Nullable<int> ProjectID { get; set; }

        public string ReportDate { get; set; }       

        public string Work_Start_Time { get; set; }        
        public string Work_End_Time { get; set; }        
        public string Ot_Start_Time { get; set; }
        public string Ot_End_Time { get; set; }
        public Nullable<decimal> No_of_WorkHours { get; set; }
        public Nullable<decimal> No_of_OtHours { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> SubmittedBy { get; set; }
        public Nullable<System.DateTime> SubmittedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string FirstName { get; set; }
        public string ProjectName { get; set; }

        public string SVName { get; set; }

        public bool WEHflag { get; set; }
        public bool LBflag { get; set;  }

        public Nullable<int> Leave { get; set; }

        public EmployeeViewModel eng_employee_profile { get; set; }
        public ProjectViewModel eng_project_master { get; set; }
        public UserViewModel eng_users { get; set; }

        public string ReportEndDate { get; set; }
        public string SelectedEmpList { get; set; }

    }

    public class MobileTimeEntryViewModel
    {

        public int TEID { get; set; }

        public Nullable<int> EmpID { get; set; }

        public Nullable<int> ProjectID { get; set; }

        public string ReportDate { get; set; }

        public string Work_Start_Time { get; set; }

        public string Work_End_Time { get; set; }
        public string Ot_Start_Time { get; set; }
        public string Ot_End_Time { get; set; }
        public Nullable<decimal> No_of_WorkHours { get; set; }
        public Nullable<decimal> No_of_OtHours { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> SubmittedBy { get; set; }
        public Nullable<System.DateTime> SubmittedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string FirstName { get; set; }
        public string ProjectName { get; set; }

        public int islunchtime { get; set; }
        public int ispublicholiday { get; set; }

        public bool WEHflag { get; set; }
        public bool LBflag { get; set; }

        public Nullable<int> Leave { get; set; }

        public string ReportEndDate { get; set; }
        public string SelectedEmpList { get; set; }

    }
}