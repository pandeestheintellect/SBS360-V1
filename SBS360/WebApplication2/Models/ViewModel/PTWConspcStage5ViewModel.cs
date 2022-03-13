using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class PTWConspcStage5ViewModel
    {
        public int PTW_Conspc_Stage5_ID { get; set; }
        public Nullable<int> PTW_Master_ID { get; set; }
        public Nullable<System.DateTime> Stage5_Date_Time { get; set; }
        public string O2 { get; set; }
        public string CO2 { get; set; }
        public string LEL { get; set; }
        public string H2S { get; set; }
        public string Safe_for_Entry { get; set; }
        public string Stage5_Assessor_Name { get; set; }
        public string Assessor_Comments { get; set; }
    }
}