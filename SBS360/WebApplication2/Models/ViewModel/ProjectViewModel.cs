using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class ProjectViewModel
    {
        public int ProjectID { get; set; }
        [Display(Name = "Project Number"), Required, StringLength(50)]
        public string ProjectNo { get; set; }
        [Display(Name = "Project Name"), Required, StringLength(50)]
        public string ProjectName { get; set; }
        [Display(Name = "Location"), Required]
        public Nullable<int> LocationId { get; set; }
        [Display(Name = "Quotation Number")]
        public Nullable<int> QuotationID { get; set; }
        [Display(Name = "Do Number"),  StringLength(50)]
        public string DoNo { get; set; }
        [Display(Name = "Start Date") ]
        public string Start_Date { get; set; }
        [Display(Name = "End Date")]
        public string End_Date { get; set; }
        [Display(Name = "Key Milestones")]
        public string Key_Milestones { get; set; }
        [Display(Name = "Service Description")]
        public string Service_Desc { get; set; }
        [Display(Name = "Project Status"),Required]
        public Nullable<int> Project_Status_ID { get; set; }
        [Display(Name = "Payment Status")]
        public string Payment_Status { get; set; }
        [Display(Name = "Client Acceptance Status")]
        public string Client_Acceptance_Status { get; set; }
        [Display(Name = "Received Amount")]
        public Nullable<double> Received_Amount { get; set; }
        [Display(Name = "Pending Amount")]
        public Nullable<double> Pending_Amount { get; set; }
        [Display(Name = "Project Cost")]
        public Nullable<decimal> Project_Cost { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public string InvoiceNo { get; set; }

        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<System.DateTime> DoDate { get; set; }

        public Nullable<int> Is_Project_level_inv { get; set; }
        public Nullable<int> Is_Custom_level_inv { get; set; }

        public Nullable<int> Is_Project_level_do { get; set; }

        // public string Dono { get; set; }
        public   QuoteViewModel eng_quote_master { get; set; }
        public LocationViewModel eng_sys_location { get; set; }
        public  ProjectStatusViewModel eng_sys_project_status { get; set; }
    }
}