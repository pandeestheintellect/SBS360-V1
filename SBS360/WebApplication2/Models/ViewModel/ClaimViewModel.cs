using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class ClaimViewModel
    {

       
        public int ClaimID { get; set; }
        [Display(Name = "Employee Name"), Required]
        public Nullable<int> UserID { get; set; }
        [Display(Name = "Project Name")]
        public Nullable<int> ProjectID { get; set; }
        public string ClaimDisplayID { get; set; }        
        public Nullable<decimal> ClaimAmount { get; set; }
        public Nullable<int> Status { get; set; }
        public string ClaimAgainst { get; set; }
        [Display(Name = "Supervisor Remarks")]
        public string SVRemarks { get; set; }
        public string ApprovalRemarks { get; set; }
        public string RejectRemarks { get; set; }
        [Display(Name = "Approver")]
        public Nullable<int> ApprovedBy { get; set; }
        public string ApprovedDate { get; set; }
        public Nullable<int> SubmittedBy { get; set; }
        [Display(Name = "Submit Date"), Required]
        public string SubmittedDate { get; set; }

        public string PaymentSource { get; set; }

        public Nullable<int> ClaimStatus { get; set; }

        public Nullable<int> isFullyPaid { get; set; }

        public Nullable<decimal> TotalClaim { get; set; }

        public EmployeeViewModel eng_employee_profile { get; set; }
        public UserViewModel eng_users { get; set; }
        public UserViewModel eng_users1 { get; set; }
        public ProjectViewModel eng_project_master { get; set; }
        public List<HttpPostedFileBase> files { get; set; }
        public List<ClaimDescriptionViewModel> eng_claim_description { get; set; }
        public List<ClaimFilesViewModel> eng_claim_files { get; set; }

        public List<FileUploadViewModel> Mobilefiles { get; set; }


    }
}