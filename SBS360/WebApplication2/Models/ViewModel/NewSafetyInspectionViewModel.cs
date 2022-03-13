using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class NewSafetyInspectionViewModel
    {
        public int NSIID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public string InspectionDate { get; set; }
        public string ProjectLocation { get; set; }
        public string InspectedBy { get; set; }
        public string Observation { get; set; }
        public string RemedialAction { get; set; }
        public string ActionBy_Deadline { get; set; }
        public string Rectification_Remarks { get; set; }
        public string Status { get; set; }
        public string EHSName { get; set; }
        public string AcknowlegeBy { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string FileCaption { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public ProjectViewModel eng_project_master { get; set; }

        public List<HttpPostedFileBase> files { get; set; }

        public List<NewSafetyInspectionDetailViewModel> eng_safety_esh_files { get; set; }

    }
}