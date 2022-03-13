using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class SafetyInspectionItemsViewModel
    {
        public int SIItemID { get; set; }
        public string SIHeaderID { get; set; }
        public string SITitle { get; set; }
        public string SIItemDescription { get; set; }
        public Nullable<int> OrderBy { get; set; }
    }
}