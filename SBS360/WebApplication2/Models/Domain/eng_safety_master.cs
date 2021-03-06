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
    
    public partial class eng_safety_master
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public eng_safety_master()
        {
            this.eng_safety_hazard_list = new HashSet<eng_safety_hazard_list>();
            this.eng_safety_ppe_list = new HashSet<eng_safety_ppe_list>();
            this.eng_safety_worker_list = new HashSet<eng_safety_worker_list>();
        }
    
        public int SafetyID { get; set; }
        public string CompanyName { get; set; }
        public string ProjectTitle { get; set; }
        public Nullable<System.DateTime> RepDate { get; set; }
        public Nullable<System.TimeSpan> RepTime { get; set; }
        public string LocationOfWork { get; set; }
        public string OtherHazard { get; set; }
        public Nullable<int> SubmittedBy { get; set; }
        public string ASHMeasures { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string SignaturePath { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eng_safety_hazard_list> eng_safety_hazard_list { get; set; }
        public virtual eng_users eng_users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eng_safety_ppe_list> eng_safety_ppe_list { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eng_safety_worker_list> eng_safety_worker_list { get; set; }
    }
}
