using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eng360Web.Models.ViewModel
{
    public class SafetyMasterViewModel
    {
        public int SafetyID { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        
        public string ProjectTitle { get; set; }

        [Display(Name = "Date")]
        public string RepDate { get; set; }

        [Display(Name = "Time")]
        public string RepTime { get; set; }

        public IList<SelectListItem> HazardList { get; set; }

        public IList<SelectListItem> PPEList { get; set; }

        public IList<SelectListItem> WorkerList { get; set; }

        [Display(Name = "Location of Work")]
        public string LocationOfWork { get; set; }


        [Display(Name = "Other Hazards"),  StringLength(50)]
        public string OtherHazard { get; set; }

        [Display(Name = "Additional Safety Health Measures")]
        public string ASHMeasures { get; set; }

        
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Nullable<int> SubmittedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> Status { get; set; }

        public SafetyHazardListViewModel eng_sys_safety_hazard { get; set; }
        public SafetyPPEListViewModel eng_sys_safety_ppelist { get; set; }

        public List<SafetyChildHazardListViewModel> eng_safety_hazard_list { get; set; }
        public List<SafetyChildPPEListViewModel> eng_safety_ppe_list { get; set; }
        public List<SafetyChildWorkerListViewModel> eng_safety_worker_list { get; set; }

        public UserViewModel eng_users { get; set; }

        // public   eng_contact_master eng_contact_master { get; set; }


    }
}