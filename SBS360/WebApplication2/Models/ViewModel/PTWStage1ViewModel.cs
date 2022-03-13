using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class PTWStage1ViewModel
    {
        public int PTW_Detail_Satge1_ID { get; set; }
        public Nullable<int> PTW_Master_ID { get; set; }
        public Nullable<int> PTW_Stage_One_ID { get; set; }
        public Nullable<int> Is_Applicable { get; set; }

        public string item { get; set; }

        public string Applicable
        {
            get
            {
                if (Is_Applicable == 0)
                    return "";
                else if (Is_Applicable == 1)
                    return "√";
                else if (Is_Applicable == 2)
                    return "X";
                else if (Is_Applicable == 3)
                    return "NA";

                return "";
            }
        }
    }
}