using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class NewSafetyInspectionDetailViewModel
    {
        public int NSIFileID { get; set; }
        public Nullable<int> NSIID { get; set; }
        public string FileCaption { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Observation { get; set; }
        public string RemedialAction { get; set; }
        public string ActionBy_Deadline { get; set; }
        public string Rectification_Remarks { get; set; }
        public string Status { get; set; }
        public string InspectionDate { get; set; }

       
    }
}