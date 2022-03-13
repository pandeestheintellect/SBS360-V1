using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{

    public class ProjectMobileViewModel
    {
        public int ProjectID { get; set; }

        public string ProjectNo { get; set; }

        public string ProjectName { get; set; }

    }

    public class EmployeeMobileViewModel
    {
        public string EmpID { get; set; }

        public string EmpNo { get; set; }

        public string Designation { get; set; }

        public int IsActive { get; set; }


        public string FirstName { get; set; }


        public string LastName { get; set; }

    }
}