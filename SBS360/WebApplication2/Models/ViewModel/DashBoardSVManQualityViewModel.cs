using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class DashBoardSVManQualityViewModel
    {
        public string ProjectName { get; set; }
        public Nullable<System.DateTime> InspectedDate { get; set; }
        public string DefStatus { get; set; }
        public Nullable<int> defcnt { get; set; }
        public Nullable<int> cmpcnt { get; set; }


    }
}