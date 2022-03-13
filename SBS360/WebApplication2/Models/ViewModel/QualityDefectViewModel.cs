using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class QualityDefectViewModel
    {

        public int DefectID { get; set; }

        [Display(Name = "Defect Tracking Ref")]
        public string DefectDisplayID { get; set; }
        [Display(Name = "Project Name"),Required]
        public Nullable<int> ProjectID { get; set; }
        public string Location { get; set; }

        [Display(Name = "Drawing / Record #")]
        public string DrawingRecordNo { get; set; }
        [Display(Name = "Inspection Area")]
        public string InspectionArea { get; set; }
        [Display(Name = "Assigned To Supplier")]
        public bool SupplierFlag { get; set; }
        [Display(Name = "Supplier Name")]
        public Nullable<int> SupplierID { get; set; }
        public string Remarks { get; set; }
        [Display(Name = "Defect Status")]
        public string DefStatus { get; set; }
        [Display(Name = "Inspection By"), Required]
        public Nullable<int> InspectedBy { get; set; }
        [Display(Name = "Inspection Date"), Required]
        public string InspectedDate { get; set; }
        [Display(Name = "Manager Assigned"),Required]
        public Nullable<int> PM_Incharge { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        [Display(Name = "Inspection Type")]
        public string InspectionType { get; set; }

        [Display(Name = "External Audit Person Name")]
        public string Ext_AuditedBy { get; set; }

        [Display(Name = "Audit Person Designation")]
        public string Ext_Auditor_Desig { get; set; }

        [Display(Name = "Defect Count"), Required]
        public int IssueCount { get; set; }
        

        public EmployeeViewModel eng_employee_profile { get; set; }
        public EmployeeViewModel eng_employee_profile1 { get; set; }
        public ProjectViewModel eng_project_master { get; set; }
        public List<HttpPostedFileBase> files { get; set; }
        public List<QualityDefectDetailViewModel> eng_qa_defect_detail { get; set; }
        public SupplierViewModel eng_supplier_master { get; set; }
        public List<QualityDefectFilesViewModel> eng_qa_defect_files { get; set; }


                
    }
}