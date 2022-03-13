using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class RATransDetail1ViewModel
    {
        public int RAWADetID { get; set; }
        public Nullable<int> RAID { get; set; }
        public string Locations { get; set; }
        public string Process { get; set; }
        public string WorkActivities { get; set; }

        public RATransMasterViewModel eng_ra_trans_master { get; set; }

    }
}