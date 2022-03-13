using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class TransportViewModel
    {

        public int TransportID { get; set; }
        [Required]
        public string Vehicle_Name { get; set; }
        public string Vehicle_Company { get; set; }
        public string Vehicle_Model { get; set; }
        [Required]
        public string Vehicle_Type { get; set; }
        [Required]
        public string Vehicle_Number { get; set; }
        public string COE_Regn_Number { get; set; }
        public string COE_Issue_Date { get; set; }
        public string COE_Expiry_Date { get; set; }
        public string RoadTax_Regn_Number { get; set; }
        public string RoadTax_Issue_Date { get; set; }
        public string RoadTax_Expiry_Date { get; set; }
        public string Insurance_Policy_Number { get; set; }
        public string Insurance_Issue_Date { get; set; }
        public string Insurance_Expiry_Date { get; set; }
        public string Insurance_Company { get; set; }
        public string Last_Insurance_Renew_Date { get; set; }
        public string Vehicle_Inspection_Date { get; set; }
        public string Inspection_Due_Date { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int IsActive { get; set; }
        public string AgreementNumber { get; set; }



    }
}