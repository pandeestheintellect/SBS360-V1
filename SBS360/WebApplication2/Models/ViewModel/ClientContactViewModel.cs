using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class ClientContactViewModel
    {
        public int CCID { get; set; }
        public Nullable<int> ClientID { get; set; }
        public string NamePrefix { get; set; }
        public string SPOCName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Tel { get; set; }
        public string Remarks { get; set; }

        public ClientViewModel eng_client_master { get; set; }
    }
}