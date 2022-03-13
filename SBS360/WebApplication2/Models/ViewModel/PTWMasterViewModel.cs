using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Eng360Web.Models.ViewModel
{
    public class PTWMasterViewModel
    {
        public int PTW_master_ID { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Project Title")]
        public string ProjectTitle { get; set; }
        [Display(Name = "Name of Applicant")]
        public string NameOfApplicant { get; set; }
        [Display(Name = "Date of PTW")]
        public string Date_Time { get; set; }
        [Display(Name = "Sub Con name")]
        public string Sub_con_Name { get; set; }

        [Display(Name = "Location/GridLineNo")]
        public string Loc_or_GridLineNo { get; set; }

        public string New_Start_Date_Time { get; set; }
        public string New_End_Date_Time { get; set; }

        [Display(Name = "Start Date")]
        public string Start_Date_Time { get; set; }
        [Display(Name = "End Date")]
        public string End_Date_Time { get; set; }
        [Display(Name = "No of Workers Involved")]
        public Nullable<int> No_of_workers_involved { get; set; }
        [Display(Name = "Name/Signature")]
        public string Stage1_Person_Name { get; set; }
        [Display(Name = "Date/Time")]
        public string Stage1_Date_Time { get; set; }
        [Display(Name = "Name/Signature")]
        public string Stage2_Person_Name { get; set; }
        [Display(Name = "Date/Time")]
        public string Stage2_Date_Time { get; set; }
        [Display(Name = "Name/Signature")]
        public string Stage3_Person_Name { get; set; }
        [Display(Name = "Date/Time")]
        public string Stage3_Date_Time { get; set; }
        [Display(Name = "Name/Signature")]
        public string Stage4_Sup_Name { get; set; }
        //  public Nullable<System.DateTime> Stage4_Sup_Date_Time { get; set; }
        [Display(Name = "Name/Signature")]
        public string Stage4_WSH_Name { get; set; }
        //   public Nullable<System.DateTime> Stage4_WSH_Date_Time { get; set; }
        [Display(Name = "Name/Signature")]
        public string Stage5_Sup_Person_Name { get; set; }
        [Display(Name = "Date/Time")]
        public string Stage5_Sup_Date_Time { get; set; }
        [Display(Name = "Name/Signature")]
        public string Stage5_Mng_Person_Name { get; set; }
        [Display(Name = "Date/Time")]
        public string Stage5_Mng_Date_Time { get; set; }

        [Display(Name = "Reason for revoke or cancellation & date / time")]
        public string Revoke_Desc { get; set; }

        [Display(Name = "Signed by WSH Personnel")]
        public string Revoke_Sup_Name { get; set; }


        [Display(Name = "Signed by Project Manager")]
        public string Revoke_Mng_Name { get; set; }
        public Nullable<int> ProjectID { get; set; }
        [Display(Name = "PTW Type")]
        public string PTW_type { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<int> Updated_By { get; set; }
        public Nullable<System.DateTime> Updated_Date { get; set; }
        public Nullable<int> CompletedStage { get; set; }

        public List<MobilePTWstages> stages { get; set; }

        public List<MobilePTWRConfigSatgeOne> stage1Config { get; set; }

        public List<PTWStage1ViewModel> eng_PTW_Detail_Satge1 { get; set; }

        public List<PTWEmployeeViewModel> eng_PTW_Employee_Details { get; set; }

        public List<PTWStage4ViewModel> eng_PTW_Detail_Satge4 { get; set; }


        public string stage1String { get; set; }
        public string EmpString { get; set; }

        public bool is_already_created { get; set; }

        public string Stage_One_ItemIds { get; set; }
        public string EmployeeIDs { get; set; }

        public List<string> daysPtwCreated { get; set; }
    }
}