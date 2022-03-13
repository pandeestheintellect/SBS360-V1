using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class RATransRACMViewModel
    {
        public int RACMID { get; set; }
        public Nullable<int> RAID { get; set; }
        public Nullable<int> RAWADetID { get; set; }
        public string RAWADetail { get; set; }
        public string HazardID { get; set; }
        public string PAHID { get; set; }
        public string REvExRCID { get; set; }
        public Nullable<int> REvRMLHID { get; set; }
        public Nullable<int> REvRMSVID { get; set; }
        public Nullable<int> REvRPN { get; set; }
        public string RCAdRCID { get; set; }
        public Nullable<int> RCRMLHID { get; set; }
        public Nullable<int> RCRMSVID { get; set; }
        public Nullable<int> RCRPN { get; set; }
        public string ImpOfficer { get; set; }
        public string DueDate { get; set; }
        public string Remarks { get; set; }
        public string PreparedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

    }
}