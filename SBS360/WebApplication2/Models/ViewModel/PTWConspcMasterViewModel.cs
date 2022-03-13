using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Eng360Web.Models.ViewModel
{
    public class PTWConspcMasterViewModel
    {

        public int PTW_master_ID { get; set; }
        public string ContractorName { get; set; }
        public string Work_Description { get; set; }
        public string Applicant_Name { get; set; }
        public string Applicant_Desig { get; set; }
        public string Applicant_Date_Time { get; set; }
        public string LocationOfWork { get; set; }
        public string Start_Date_Time { get; set; }
        public string End_Date_Time { get; set; }
        public Nullable<int> No_of_workers_involved { get; set; }
        public string Stage1_Watchman_Name { get; set; }
        public string Stage1_Watchman_ID { get; set; }
        public string Stage1_Watchman_Company { get; set; }
        public string Stage2_O2 { get; set; }
        public string Stage2_CO2 { get; set; }
        public string Stage2_LEL { get; set; }
        public string Stage2_H2S { get; set; }
        public string Safe_for_Entry { get; set; }
        public string Stage2_Assessor_Name { get; set; }
        public string Stage2_Assessor_Desig { get; set; }
        public string Stage2_Assessor_Date_Time { get; set; }
        public string Stage2_Comments { get; set; }
        public string Stage3_WSH_Name { get; set; }
        public string Stage3_WSH_Desig { get; set; }
        public string Stage3_WSH_Date_Time { get; set; }
        public string Stage3_Comments { get; set; }
        public string Stage4_Mng_Name { get; set; }
        public string Stage4_Mng_Desig { get; set; }
        public string Stage4_Date_Time { get; set; }
        public string Stage4_Comments { get; set; }
        public string Stage6_Person_Name { get; set; }
        public string Stage6_Person_Desig { get; set; }
        public string Stage6_Date_Time { get; set; }
        public string Revoke_Desc { get; set; }
        public string Revoke_WSH_Name { get; set; }
        public string Revoke_PM_Name { get; set; }
        public string Revoke_Date_Time { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public string PTW_type { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<int> Updated_By { get; set; }
        public Nullable<System.DateTime> Updated_Date { get; set; }
        public Nullable<int> CompletedStage { get; set; }

        public string ProjectTitle { get; set; }


        public List<MobilePTWstages> stages { get; set; }

        public List<MobilePTWRConfigSatgeOne> stage1Config { get; set; }

        public List<PTWConspcStage1ViewModel> eng_PTW_Conspc_Detail_Stage1 { get; set; }

        public List<PTWConspcEmployeeViewModel> eng_PTW_Conspc_Employee_Details { get; set; }

        public List<PTWConspcStage5ViewModel> eng_PTW_Conspc_Detail_Stage5 { get; set; }


        public string stage1String { get; set; }
        public string stage1asrString { get; set; }
        public string stage1remString { get; set; }
        public string EmpString { get; set; }

        public string Stage5_O2 { get; set; }
        public string Stage5_CO2 { get; set; }
        public string Stage5_LEL { get; set; }
        public string Stage5_H2S { get; set; }
        public string Stage5_Safe_for_Entry { get; set; }
        public string Stage5_Assessor_Name { get; set; }        
        public string Stage5_Comments { get; set; }
        public string Stage5_Date_Time { get; set; }

        public bool is_already_created { get; set; }

        public string Stage_One_ItemIds { get; set; }
        public string EmployeeIDs { get; set; }

        public List<string> daysPtwCreated { get; set; }
    }
}