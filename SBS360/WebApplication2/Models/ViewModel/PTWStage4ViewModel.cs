using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class PTWStage4ViewModel
    {
        public int PTW_Detail_Satge4 { get; set; }
        public Nullable<int> PTW_Master_ID { get; set; }
        public string Day { get; set; }
        public Nullable<System.DateTime> DayDate { get; set; }
        public string Sup_Signature { get; set; }
        public Nullable<System.DateTime> Sup_Sig_Date { get; set; }
        public string Mng_Signature { get; set; }
        public Nullable<System.DateTime> Mng_Sig_Date { get; set; }
    }
}