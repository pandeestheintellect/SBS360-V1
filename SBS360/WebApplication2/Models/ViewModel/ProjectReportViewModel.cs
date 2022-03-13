using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class ProjectReportViewModel
    {

        public int ProjectReportID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        [Display(Name = "Reported Date"),Required]
        public string ReportDate { get; set; }

        [Display(Name = "Start Time"),Required]
        public string Start_Date_Time { get; set; }
        [Display(Name = "End Time"),Required]
        public string End_Date_Time { get; set; }
        [Display(Name = "Description")]
        public string Task_Description { get; set; }
        [Display(Name = "Quantity")]
        public Nullable<int> Quantity { get; set; }
        [Display(Name = "Task Status")]
        public Nullable<int> TaskStatusID { get; set; }
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
        [Display(Name = "Resource Name"),Required]
        public string Resource_name { get; set; }
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }
        [Display(Name = "Location")]
        public string Location { get; set; }

        public string ProgressPercentage { get; set; }
        public string SVName { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        public  ProjectViewModel eng_project_master { get; set; }
        public ProjectReportTaskStatusViewModel eng_sys_task_status { get; set; }

        public List<HttpPostedFileBase> files { get; set; }

        public   List<ProjectReportFilesViewModel> eng_project_report_files { get; set; }

    }

    public class MobileProjectReportViewModel
    {
        public int ProjectReportID { get; set; }
        public Nullable<int> ProjectID { get; set; }

        public string ReportDate { get; set; }

        public string Start_Date_Time { get; set; }

        public string End_Date_Time { get; set; }

        public string Task_Description { get; set; }

        public Nullable<int> Quantity { get; set; }

        public Nullable<int> TaskStatusID { get; set; }

        public string Remarks { get; set; }

        public string Resource_name { get; set; }

        public string ProjectName { get; set; }

        public string ClientName { get; set; }

        public string Location { get; set; }
        public string ProgressPercentage { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        public List<FileUploadViewModel> files { get; set; }
        public string TotalHrsReported { get; set; }
        public string TotalOTHrsReported { get; set; }
    }
    public class MobileNewSafetyInspectionViewModel
    {
        public int NSIID { get; set; }
        public int ProjectID { get; set; }
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

        public List<FileUploadViewModel> files { get; set; }
        public string ProjectName { get; set; }

    }
 
    public class MobileCliamViewModel
    {
        public int ClaimID { get; set; }
        
        public Nullable<int> UserID { get; set; }
      
        public Nullable<int> ProjectID { get; set; }
        public string ClaimDisplayID { get; set; }
        public Nullable<decimal> ClaimAmount { get; set; }
        public Nullable<int> Status { get; set; }
        public string ClaimAgainst { get; set; }
        
        public string SVRemarks { get; set; }
        public string ApprovalRemarks { get; set; }
        public string RejectRemarks { get; set; }
     
        public string ApprovedBy { get; set; }
        public string ApprovedDate { get; set; }
        public string SubmittedBy { get; set; }
      
        public string SubmittedDate { get; set; }

        public List<FileUploadViewModel> files { get; set; }
        public List<MobileClaimDescViewModel> eng_claim_description { get; set; }
        public string filepath { get; set; }
        public string ProjectName { get; set; }
        public string EmployeeName { get; set; }
        public string ManagerName { get; set; }

    }
    public class MobileCompanyViewModel
    {
        public string CompanyName { get; set; }
        public decimal Normal_Work_Hours { get; set; }
        public decimal Weekend_Work_Hours { get; set; }
        public decimal Lunch_Break_Hours { get; set; }
    }
    public class MobileClaimDescViewModel
    {
        public int ClaimDescID { get; set; }
        public Nullable<int> ClaimID { get; set; }
 
        public Nullable<int> ClaimTypeID { get; set; }

        public string ClaimCategoryName { get; set; }
        public string RecpDate { get; set; }
 
        public string ClaimDescription { get; set; }
      
        public Nullable<decimal> RecpAmount { get; set; }
        public string GST { get; set; }
    }
    public class UserMobileViewModel
    {

        public int UserID { get; set; }
        public string DisplayName { get; set; }
    }
    public class FileUploadViewModel
    {
        public string filename { get; set; }
        public string data { get; set; }
    }

    public class MobileSafetyMasterViewModel
    {
        public int SafetyID { get; set; }

        public string CompanyName { get; set; }

        public string supperviosrName { get; set; }

        public string ProjectTitle { get; set; }

        public string RepDate { get; set; }

        public string RepTime { get; set; }


        public string LocationOfWork { get; set; }


        public string OtherHazard { get; set; }


        public string ASHMeasures { get; set; }


        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Nullable<int> SubmittedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> Status { get; set; }
        public List<int> hazardList1 { get; set; }
        public List<int> ppeList1 { get; set; }
        public List<int> workerList1 { get; set; }
        public string HazardList { get; set; }
        public string WorkerList { get; set; }
        public string PPEList { get; set; }

    }
    public class MobileQualityDefectViewModel
    {

        public int DefectID { get; set; }


        public string DefectDisplayID { get; set; }

        public Nullable<int> ProjectID { get; set; }
        public string Location { get; set; }

        public string DrawingRecordNo { get; set; }

        public string InspectionArea { get; set; }

        public bool SupplierFlag { get; set; }

        public Nullable<int> SupplierID { get; set; }
        public string Remarks { get; set; }

        public string DefStatus { get; set; }

        public Nullable<int> InspectedBy { get; set; }

        public string InspectedDate { get; set; }

        public Nullable<int> PM_Incharge { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        //public Nullable<int> UpdatedBy { get; set; }
        //public Nullable<System.DateTime> UpdatedDate { get; set; }


        public string InspectionType { get; set; }


        public string Ext_AuditedBy { get; set; }


        public string Ext_Auditor_Desig { get; set; }


        public int IssueCount { get; set; }

        public string DefectTitle { get; set; }

        public string DefectImpactDetails { get; set; }


        //public EmployeeViewModel eng_employee_profile { get; set; }
        //public EmployeeViewModel eng_employee_profile1 { get; set; }
        //public ProjectViewModel eng_project_master { get; set; }
        ////public List<HttpPostedFileBase> files { get; set; }
        //public List<QualityDefectDetailViewModel> eng_qa_defect_detail { get; set; }
        //public SupplierViewModel eng_supplier_master { get; set; }
        //public List<QualityDefectFilesViewModel> eng_qa_defect_files { get; set; }

        public List<FileUploadViewModel> mobileFiles { get; set; }

        public string ProjectName { get; set; }

        public string InsName { get; set; }

        public string ManagerName { get; set; }

    }

    public class MobileSupplierViewModel
    {
        public int SupplierID { get; set; }
        public string Company_Name { get; set; }
        public string SupplierDisplayID { get; set; }
    }

    public class MobileQualityDefectCPAViewModel
    {

        public int DPCID { get; set; }
        public int DefectDetailID { get; set; }

        public bool PAflag { get; set; }

        public bool CAflag { get; set; }

        public bool rfp_ncps { get; set; }

        public bool rfp_mgmtreview { get; set; }

        public bool rfp_cc { get; set; }

        public bool rfp_ea { get; set; }

        public bool rfp_other { get; set; }

        public string rfp_other_remarks { get; set; }

        public bool ipt_envmt { get; set; }

        public bool ipt_safety { get; set; }

        public bool ipt_health { get; set; }

        public bool ipt_insandops { get; set; }

        public bool ipt_suggestion { get; set; }

        public string ActionTaken { get; set; }

        public string ActionBy { get; set; }
        public System.DateTime DoI { get; set; }

        public string FollowupAction { get; set; }
        public string Remarks { get; set; }

        public string ReviewedBy { get; set; }
        public int UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }

        public string TrackStatus { get; set; }

        public string ProjectName { get; set; }
        public string DefectTrackNum { get; set; }

        public string Inspected_Date { get; set; }

        public string DefectTitle { get; set; }
        public string DefectImpactDetails { get; set; }
        public string SupplierName { get; set; }

        public List<FileUploadViewModel> mobileFiles { get; set; }

    }

    public class MobileQualityDefectDetailViewModel
    {
        public int DefectDetailID { get; set; }

        public string DefectTrackNum { get; set; }

        public int DefectID { get; set; }


        public string DefectTitle { get; set; }


        public string DefectImpactDetails { get; set; }


        public string DefectStatus { get; set; }

        public string InspectionNO { get; set; }

        public string ProjectName { get; set; }


    }
    public class MobilePTWRConfigSatgeOne
    {
        public int PTW_Stage_One_ID { get; set; }
        public string PTW_Type { get; set; }
        public string PTW_Title { get; set; }
        public string Item { get; set; }
        public Nullable<int> Order_By { get; set; }
    }

    public class MobilePTWstages
    {
        public int StageCofigID { get; set; }
        public string Stage_Type { get; set; }
        public string Stages { get; set; }
    }
    public class MobilePTWMaster
    {
        public int PTW_master_ID { get; set; }
        public string CompanyName { get; set; }
        public string ProjectTitle { get; set; }
        public int ProjectID { get; set; }

        public string NameOfApplicant { get; set; }
        public DateTime Date_Time { get; set; }
        public string Sub_con_Name { get; set; }
        public string Loc_or_GridLineNo { get; set; }
        public Nullable<System.DateTime> Start_Date_Time { get; set; }
        public Nullable<System.DateTime> End_Date_Time { get; set; }
        public Nullable<int> No_of_workers_involved { get; set; }
        public string Stage1_Person_Name { get; set; }
        public Nullable<System.DateTime> Stage1_Date_Time { get; set; }
        public string Stage2_Person_Name { get; set; }
        public Nullable<System.DateTime> Stage2_Date_Time { get; set; }
        public string Stage3_Person_Name { get; set; }
        public Nullable<System.DateTime> Stage3_Date_Time { get; set; }
        public string Stage4_Sup_Name { get; set; }
        public Nullable<System.DateTime> Stage4_Sup_Date_Time { get; set; }
        public string Stage4_WSH_Name { get; set; }
        public Nullable<System.DateTime> Stage4_WSH_Date_Time { get; set; }
        public string Stage5_Sup_Person_Name { get; set; }
        public Nullable<System.DateTime> Stage5_Sup_Date_Time { get; set; }
        public string Stage5_mng_Person_Name { get; set; }
        public Nullable<System.DateTime> Stage5_mng_Date_Time { get; set; }
        public string Stage_One_ItemIds { get; set; }

        public string Stage4_Sup_Days { get; set; }
        public string Stage4_Mng_Days { get; set; }
        public string PTWReportType { get; set; }

        public string PTW_Type { get; set; }
        public string Revoke_Desc { get; set; }
        public string Revoke_Sup_Name { get; set; }
        public string Revoke_Mng_Name { get; set; }
        public string EmployeeIDs { get; set; }

        public int Id { get; set; }
        public int UserId { get; set; }

        public int CompletedStage { get; set; }
        public bool is_already_created { get; set; }

        public List<string> daysPtwCreated { get; set; }

        public string Day { get; set; }

    }

    public class MobileRATransMasterViewModel
    {
        public int RAID { get; set; }
        public string RARefNum { get; set; }
        public int ProjectID { get; set; }
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
        public string ProjectTitle { get; set; }
        public List<MobileRATransRACMViewModel> eng_ra_trans_racm { get; set; }


    }
    public class MobileRATransRACMViewModel
    {
        public int RACMID { get; set; }
        public int RAID { get; set; }
        public int RAWADetID { get; set; }
        public string RAWADetail { get; set; }
        public string HazardID { get; set; }
        public string PAHID { get; set; }
        public string REvExRCID { get; set; }
        public int REvRMLHID { get; set; }
        public int REvRMSVID { get; set; }
        public int REvRPN { get; set; }
        public string RCAdRCID { get; set; }
        public int RCRMLHID { get; set; }
        public int RCRMSVID { get; set; }
        public int RCRPN { get; set; }
        public string ImpOfficer { get; set; }
        public string DueDate { get; set; }
        public string Remarks { get; set; }
        public string PreparedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

    }

    public class MobileRATransDetail1ViewModel
    {
        public int RAWADetID { get; set; }
        public Nullable<int> RAID { get; set; }
        public string Locations { get; set; }
        public string Process { get; set; }
        public string WorkActivities { get; set; }



    }


}