using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class PTWConspcEmployeeViewModel
    {
        public int PTWEmployeeID { get; set; }
        public Nullable<int> PTW_Master_ID { get; set; }
        public Nullable<int> EmployeeID { get; set; }

        public string EmpName { get; set; }
        public string Desig { get; set; }
    }
}