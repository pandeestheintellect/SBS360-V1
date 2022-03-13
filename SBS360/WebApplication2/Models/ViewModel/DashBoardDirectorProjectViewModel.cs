using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class DashBoardDirectorProjectViewModel
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public string ProjectStatus { get; set; }
        public string QuoteRefNum { get; set; }
        public string QuoteStatus { get; set; }


    }
}