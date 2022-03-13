using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class QualityDefectDetailViewModel
    {
        public int DefectDetailID { get; set; }

        public string DefectTrackNum { get; set; }

        public Nullable<int> DefectID { get; set; }
        
        [Display(Name = "Defect Title"), Required]
        public string DefectTitle { get; set; }

        [Display(Name = "Defect Impact Description"), Required]
        public string DefectImpactDetails { get; set; }

        [Display(Name = "Defect Status")]
        public string DefectStatus { get; set; }


        public QualityDefectViewModel eng_qa_defect { get; set; }

    }
}