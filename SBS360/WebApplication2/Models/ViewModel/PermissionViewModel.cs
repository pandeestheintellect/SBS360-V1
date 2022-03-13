using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class PermissionViewModel
    {

        public int PermissionID { get; set; }
        public Nullable<int> GroupID { get; set; }
        public Nullable<int> ModuleID { get; set; }
        public Nullable<int> Access { get; set; }

        public  ModuleViewModel eng_module { get; set; }
        public GroupViewModel eng_usergroup { get; set; }
    }
}