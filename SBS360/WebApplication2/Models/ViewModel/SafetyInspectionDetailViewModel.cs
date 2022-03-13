using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class SafetyInspectionDetailViewModel
    {
        public int SIDetailID { get; set; }
        public Nullable<int> SAFINSID { get; set; }
        public Nullable<int> SIItemID { get; set; }
        public Nullable<int> Is_Applicable { get; set; }
        public string Recommendation { get; set; }
        public string ResponsiblePerson { get; set; }
        public Nullable<System.DateTime> ACDate { get; set; }
    }
}