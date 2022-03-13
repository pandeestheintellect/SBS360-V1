using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class DashBoardOperationViewModel
    {
        public int ProjectID { get; set; }
        public string ProjectNo { get; set; }
        public string ProjectName { get; set; }
        public string TaskStatus { get; set; }
        public string PR { get; set; }
        public string TE { get; set; }
        public string EE { get; set; }

    }
}