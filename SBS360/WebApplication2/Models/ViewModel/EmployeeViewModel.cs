using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eng360Web.Models.ViewModel
{
    public class EmployeeViewModel
    {
        public int UserID { get; set; }
        
        [Display(Name = "Employee ID"), StringLength(50)]
        public string EmpID { get; set; }
        [Display(Name = "Operation Branch"),  StringLength(50)]
        public string OpBranch { get; set; }
        
        [Display(Name = "First Name"), Required, StringLength(50)]

        public string FirstName { get; set; }

        public Nullable<int> GroupID { get; set; }

        [Display(Name = "Last Name"),  StringLength(50)]
        public string LastName { get; set; }
        public Nullable<int> ContactID { get; set; }

        public string Nationality { get; set; }

        [Display(Name = "Date of Birth"), StringLength(50)]
        public string DoB { get; set; }

        [Display(Name = "SOC Number")]
        public string SOC_number { get; set; }
        [Display(Name = "SOC Issue Date")]
        public string SOC_Issue_Date { get; set; }
        [Display(Name = "SOC Expiry Date")]
        public string SOC_Expiry_Date { get; set; }

        public Nullable<decimal> Salary { get; set; }
        public Nullable<decimal> Levy { get; set; }

        public Nullable<int> AddressID { get; set; }

        [Display(Name = "Date of Join"), StringLength(50)]
        public string DoJ { get; set; }

        [Display(Name = "Date of Resignation ")]
        public string DoR { get; set; }

        public string Gender { get; set; }
        [Required]
        public string Designation { get; set; }
        [Display(Name = "Type of ID"), StringLength(50)]
        public string ID_Type { get; set; }
        [Display(Name = "ID Number"), StringLength(50), Required]
        public string ID_Number { get; set; }
        [Display(Name = "Profile Picture"), StringLength(500)]
        public HttpPostedFileBase Profile_Photo_Path1 { get; set; }
        [Display(Name = "Profile Picture"), StringLength(500)]
        public String Profile_Photo_Path { get; set; }
        [Display(Name = "Licence Class")]        
        public Nullable<int> llevel { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public string LastLogin { get; set; }
        [Display(Name = "Passport Number"), StringLength(50)]
        public string Passport_Number { get; set; }

        [Display(Name = "Profile Description"), StringLength(50)]
        public string Profile_Desc { get; set; }

        [Display(Name = "Passport Expiry Date"), StringLength(50)]
        public string Passport_Valid_Till { get; set; }
        [Display(Name = "Permit Number"), StringLength(50)]
        public string Permit_Number { get; set; }
        [Display(Name = "Permit Valid From"), StringLength(50)]
        public string Permit_Valid_From { get; set; }
        [Display(Name = "Permit Valid To"), StringLength(50)]
        public string Permit_Valid_To { get; set; }
        [Display(Name = "Licence Number"), StringLength(50)]
        public string Licence_Number { get; set; }
        [Display(Name = "Licence Expiry Date"), StringLength(50)]
        public string Licence_Valid_Till { get; set; }
        [Display(Name = "Insurance Number"), StringLength(50)]
        public string Insurance_Number { get; set; }
        [Display(Name = "Insurance Expiry Date"), StringLength(50)]
        public string Insurance_Valid_Till { get; set; }
        public Nullable<int> IsActive { get; set; }

        [Display(Name = "Scissor Lift License Number"), StringLength(50)]
        public string License_Scissor_Lift_Number { get; set; }
        [Display(Name = "Scissor Lift Licence Expiry Date"), StringLength(50)]
        public string License_Scissor_Lift_ExpiryDate { get; set; }

        [Display(Name = "Boom Lift License Number"), StringLength(50)]
        public string License_Boom_Lift_Number { get; set; }
        [Display(Name = "Boom Lift Licence Expiry Date"), StringLength(50)]
        public string License_Boom_Lift_ExpiryDate { get; set; }

        [Display(Name = "Work at Height License Number"), StringLength(50)]
        public string License_WorkatHeight_Number { get; set; }
        [Display(Name = "Work at Height Licence Expiry Date"), StringLength(50)]
        public string License_WorkatHeight_ExpiryDate { get; set; }

        [Display(Name = "Island Pass License Number"), StringLength(50)]
        public string License_IslandPass_Number { get; set; }
        [Display(Name = "Island Pass License Expiry Date"), StringLength(50)]
        public string License_IslandPass_ExpiryDate { get; set; }

        [Display(Name = "Skilled Level")]
        public Nullable<int> Skilled_Level { get; set; }

        public string Safety_Supervisor_Name { get; set; }

        [Display(Name = "License/Courses"), StringLength(50)]
        public string License_Course { get; set; }

        [Display(Name = "License/Course Expiry Date"), StringLength(50)]
        public string License_Course_Expiry_Date { get; set; }

        public string FullName { get; set; }

        public AddressViewModel eng_address_master { get; set; }
        public GroupViewModel eng_usergroup { get; set; }


    }
}