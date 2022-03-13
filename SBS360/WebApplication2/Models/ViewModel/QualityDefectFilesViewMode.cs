using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class QualityDefectFilesViewModel
    {
        public int DefectFileID { get; set; }
        public Nullable<int> DefectID { get; set; }
        public string FIleCaption { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Nullable<int> DefectType { get; set; }
    }
}