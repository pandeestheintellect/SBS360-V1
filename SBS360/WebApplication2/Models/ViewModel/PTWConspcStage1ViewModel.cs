using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class PTWConspcStage1ViewModel
    {
        public int PTW_Conspc_Stage1_ID { get; set; }
        public Nullable<int> PTW_Master_ID { get; set; }
        public Nullable<int> PTW_Stage_One_ID { get; set; }
        public Nullable<int> Is_Applicable_Applicant { get; set; }
        public Nullable<int> Is_Applicable_Assessor { get; set; }
        public string Assessor_Remarks { get; set; }

        public string item { get; set; }

        public string Applicable_Applicant
        {
            get
            {
                if (Is_Applicable_Applicant == 0)
                    return "";
                else if (Is_Applicable_Applicant == 1)
                    return "√";
                else if (Is_Applicable_Applicant == 2)
                    return "X";
                else if (Is_Applicable_Applicant == 3)
                    return "NA";

                return "";
            }
        }


        public string Applicable_Assessor
        {
            get
            {
                if (Is_Applicable_Assessor == 0)
                    return "";
                else if (Is_Applicable_Assessor == 1)
                    return "√";
                else if (Is_Applicable_Assessor == 2)
                    return "X";
                else if (Is_Applicable_Assessor == 3)
                    return "NA";

                return "";
            }
        }


    }
}