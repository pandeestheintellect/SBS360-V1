using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class QualityDefectCPAViewModel
    {
             
        public int DPCID { get; set; }
        public Nullable<int> DefectDetailID { get; set; }
        [Display(Name = "Preventive Action")]
        public bool PAflag { get; set; }
        [Display(Name = "Corrective Action")]
        public bool CAflag { get; set; }
        [Display(Name = "Non-Confirming Products / Services")]
        public bool rfp_ncps { get; set; }
        [Display(Name = "Management Review")]
        public bool rfp_mgmtreview { get; set; }
        [Display(Name = "Customer Complaints")]
        public bool rfp_cc { get; set; }
        [Display(Name = "External Audits")]
        public bool rfp_ea { get; set; }
        [Display(Name = "Others")]
        public bool rfp_other { get; set; }
        [Display(Name = "Others Detail")]
        public string rfp_other_remarks { get; set; }
        [Display(Name = "Environment")]
        public bool ipt_envmt { get; set; }
        [Display(Name = "Safety")]
        public bool ipt_safety { get; set; }
        [Display(Name = "Health")]
        public bool ipt_health { get; set; }
        [Display(Name = "Installation and Operation")]
        public bool ipt_insandops { get; set; }
        [Display(Name = "Suggestion for Improvement")]
        public bool ipt_suggestion { get; set; }
        [Display(Name = "Actions Taken"), Required]
        public string ActionTaken { get; set; }
        [Display(Name = "Action By"), Required]
        public string ActionBy { get; set; }
        public Nullable<System.DateTime> DoI { get; set; }
        [Display(Name = "Follow-up Actions")]
        public string FollowupAction { get; set; }
        public string Remarks { get; set; }
        [Display(Name = "Reviewed By"), Required]
        public string ReviewedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public string TrackStatus { get; set; }

        public  QualityDefectDetailViewModel eng_qa_defect_detail { get; set; }
        public List<HttpPostedFileBase> files { get; set; }
        public List<QualityDefectCPAFilesViewModel> eng_qa_defect_cpa_files { get; set; }

    }
}