using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class SafetyInspectionMasterViewModel
    {
        public int SAFINSID { get; set; }
        public string SafetyRefNum { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public string SIDate { get; set; }
        public string ProjectLocation { get; set; }
        public string InspectedBy { get; set; }
        public string Address { get; set; }
        public string Safety_Cert_Info { get; set; }
        public string Others1 { get; set; }
        public string Others2 { get; set; }
        public string Others3 { get; set; }
        public string Others4 { get; set; }
        public string Others5 { get; set; }
        public string Others6 { get; set; }
        public string Others7 { get; set; }
        public string Others8 { get; set; }
        public string Others9 { get; set; }
        public string Others10 { get; set; }
        public string RP1 { get; set; }
        public string RP2 { get; set; }
        public string RP3 { get; set; }
        public string RP4 { get; set; }
        public string RP5 { get; set; }
        public string RP6 { get; set; }
        public string RP7 { get; set; }
        public string RP8 { get; set; }
        public string RP9 { get; set; }
        public string RP10 { get; set; }
        public string ACDate1 { get; set; }
        public string ACDate2 { get; set; }
        public string ACDate3 { get; set; }
        public string ACDate4 { get; set; }
        public string ACDate5 { get; set; }
        public string ACDate6 { get; set; }
        public string ACDate7 { get; set; }
        public string ACDate8 { get; set; }
        public string ACDate9 { get; set; }
        public string ACDate10 { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string Senior_Construction_Manager { get; set; }
        public string Project_Manager { get; set; }
        public string Site_Manager { get; set; }
        public string Zone_Construction_Manager { get; set; }
        public string Safety_Manager { get; set; }
        public string Safety_Officer { get; set; }

        public ProjectViewModel eng_project_master { get; set; }

        public string stage1String { get; set; }

        public List<SafetyInspectionItemsViewModel> Items { get; set; }
        public List<SafetyInspectionDetailViewModel> eng_safety_insp_detail { get; set; }
    }
}