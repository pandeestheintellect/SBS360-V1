//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Eng360Web.Models.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class eng_safety_esh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public eng_safety_esh()
        {
            this.eng_safety_esh_files = new HashSet<eng_safety_esh_files>();
        }
    
        public int NSIID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<System.DateTime> InspectionDate { get; set; }
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
        public int UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string FileCaption { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    
        public virtual eng_project_master eng_project_master { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eng_safety_esh_files> eng_safety_esh_files { get; set; }
    }
}
