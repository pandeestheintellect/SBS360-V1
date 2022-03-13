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
    
    public partial class eng_time_entry
    {
        public int TEID { get; set; }
        public Nullable<int> EmpID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<System.DateTime> ReportDate { get; set; }
        public Nullable<System.TimeSpan> Work_Start_Time { get; set; }
        public Nullable<System.TimeSpan> Work_End_Time { get; set; }
        public Nullable<System.TimeSpan> Ot_Start_Time { get; set; }
        public Nullable<System.TimeSpan> Ot_End_Time { get; set; }
        public Nullable<decimal> No_of_WorkHours { get; set; }
        public Nullable<decimal> No_of_OtHours { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> SubmittedBy { get; set; }
        public Nullable<System.DateTime> SubmittedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> WEHflag { get; set; }
        public Nullable<int> LBflag { get; set; }
        public Nullable<int> Leave { get; set; }
        public Nullable<System.DateTime> ReportEndDate { get; set; }
    
        public virtual eng_employee_profile eng_employee_profile { get; set; }
        public virtual eng_project_master eng_project_master { get; set; }
        public virtual eng_users eng_users { get; set; }
    }
}