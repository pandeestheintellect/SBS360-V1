using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class IndexViewModel
    {
        
        public int OpenQuoteCount { get; set; }
        public List<DashBoardHREmpViewModel> DB_Emp { get; set; }
        public List<DashBoardCompanyViewModel> DB_Company { get; set; }
        public List<DashBoardVehicleViewModel> DB_Vehicle { get; set; }
        public List<DashBoardPayableViewModel> DB_Payable { get; set; }
        public List<DashBoardReceivableViewModel> DB_Receivable { get; set; }
        public List<DashBoardOperationViewModel> DB_Operation { get; set; }
        public List<DashBoardDirectorProjectViewModel> DB_Director_Project { get; set; }
        public List<DashBoardSVManProjectViewModel> DB_SVMan_Project { get; set; }
        public List<DashBoardSVManDTTRViewModel> DB_SVMan_DTTR { get; set; }
        public List<DashBoardSVManQualityViewModel> DB_SVMan_Quality { get; set; }


    }
}