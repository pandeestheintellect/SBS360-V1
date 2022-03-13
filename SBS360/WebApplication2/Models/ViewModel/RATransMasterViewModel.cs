using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class RATransMasterViewModel
    {
        public int RAID { get; set; }
        public string RARefNum { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public string CompanyName { get; set; }
        public string ContractNumber { get; set; }
        public string Process { get; set; }
        public string ActivityLocation { get; set; }
        public string AssessmentDate { get; set; }
        public string LastReviewDate { get; set; }
        public string NextReviewDate { get; set; }
        public string RALeader { get; set; }
        public string RAMember1 { get; set; }
        public string RAMember2 { get; set; }
        public string RAMember3 { get; set; }
        public string RAMember4 { get; set; }
        public string RAMember5 { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedDesig { get; set; }
        public string ApprovedDate { get; set; }
        public string Reference_Number { get; set; }
        public string PreparedBy { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public string dummyLoc { get; set; }
        public string dummyPro { get; set; }
        public string dummyWac { get; set; }

        public string LocString { get; set; }
        public string ProString { get; set; }
        public string WacString { get; set; }

        public string ImpOfficer { get; set; }
        public string DueDate { get; set; }
        public string Remarks { get; set; }

        public ProjectViewModel eng_project_master { get; set; }
        public List<RATransDetail1ViewModel> eng_ra_trans_detail1 { get; set; }

        public List<RATransRACMViewModel> eng_ra_trans_racm { get; set; }

    }
}