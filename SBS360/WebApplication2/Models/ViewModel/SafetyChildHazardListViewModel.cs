using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class SafetyChildHazardListViewModel
    {
        public int HLID { get; set; }
        public int SafetyID { get; set; }
        public int HazardID { get; set; }

       public bool isChecked { get; set; }
    }
}