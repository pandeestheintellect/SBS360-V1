using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class ProjectReportFilesViewModel
    {
        public int ProjectSupportFileID { get; set; }
        public Nullable<int> ProjectReportID { get; set; }
        public string FIleCaption { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}