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
    
    public partial class eng_safety_esh_files
    {
        public int NSIFileID { get; set; }
        public Nullable<int> NSIID { get; set; }
        public string FileCaption { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Observation { get; set; }
        public string RemedialAction { get; set; }
        public string ActionBy_Deadline { get; set; }
        public string Rectification_Remarks { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> InspectionDate { get; set; }
        public Nullable<int> ProjectID { get; set; }
    
        public virtual eng_safety_esh eng_safety_esh { get; set; }
    }
}