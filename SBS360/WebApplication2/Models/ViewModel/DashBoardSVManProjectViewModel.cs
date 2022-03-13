using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class DashBoardSVManProjectViewModel
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ReportDate { get; set; }
        public string Task_Description { get; set; }
        public Nullable<System.TimeSpan> Start_Date_Time { get; set; }
        public Nullable<System.TimeSpan> End_Date_Time { get; set; }
        public string TaskStatus { get; set; }


    }
}