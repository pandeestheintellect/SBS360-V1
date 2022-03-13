using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class LoginResponseViewModel : ResponseViewModel
    {
        public EngLiteUserViewModel User { get; set; }
        public String downtimeMessage { get; set; }
    }

    public class EngLiteUserViewModel
    {
        public string loginUserType;
        public string userId { get; set; }
        public string groupID { get; set; }
        public string userName { get; set; }
        public string userFullName { get; set; }

        public string Email { get; set; }

         
    }

    public class EngProjectReportsViewModel : ResponseViewModel
    {

    }


    public class ResponseViewModel
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
     
        public String ToString()
        {
            return string.Format("Success:{0}  } ErrorMessage:{2}", Success, ErrorMessage);
        }

        // New variables required for content service
   
        public string Credentials { get; set; }
         
        
        public int userid { get; set; }
    }
}