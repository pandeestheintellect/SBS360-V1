using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class RASeverityViewModel
    {
        public int RMSVID { get; set; }
        public Nullable<int> Severity_Value { get; set; }
        public string Severity_Type { get; set; }
        public string Severity_Description { get; set; }

    }
}