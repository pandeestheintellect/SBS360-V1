using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class UserViewModel
    {

        public int UserID { get; set; }
        [Display(Name = "User Name"), Required, StringLength(80)]
        public string UserName { get; set; }
        [Display(Name = "Password"), Required, DataType(DataType.Password),
      RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{8,120}$", ErrorMessage = "Password error")]
        public string Password { get; set; }
        [Display(Name = "Display Name"), Required, StringLength(80)]
        public string DisplayName { get; set; }

        [Display(Name = "Employee Name")]
        public Nullable<int> EmpID { get; set; }

        [Display(Name = "User Group"), Required]
        public Nullable<int> GroupID { get; set; }
        public string LastLogin { get; set; }
        [Display(Name = "User ID")]
        public string UID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> IsActive { get; set; }




        public GroupViewModel eng_usergroup { get; set; }

        public EmployeeViewModel eng_employee_profile { get; set; }

        public CompanyViewModel eng_company { get; set; }


    }
}