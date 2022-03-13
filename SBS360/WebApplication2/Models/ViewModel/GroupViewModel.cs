using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class GroupViewModel
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        List<PermissionViewModel> eng_permission { get; set; }
    }
}