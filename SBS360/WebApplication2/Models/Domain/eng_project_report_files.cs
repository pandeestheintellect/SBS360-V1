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
    
    public partial class eng_project_report_files
    {
        public int ProjectSupportFileID { get; set; }
        public Nullable<int> ProjectReportID { get; set; }
        public string FIleCaption { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    
        public virtual eng_project_report eng_project_report { get; set; }
    }
}
