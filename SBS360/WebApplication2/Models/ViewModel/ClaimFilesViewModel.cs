using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class ClaimFilesViewModel
    {
        public int ClaimFileID { get; set; }
        public Nullable<int> ClaimID { get; set; }
        public string FIleCaption { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}