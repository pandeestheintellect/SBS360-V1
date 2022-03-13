using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class FilterViewModel
    {
        
        public string FirstName { get; set; }
        public string DoJ { get; set; }
        public string OpBranch { get; set; }
        public string Company_Name { get; set; }
        public string Product_Name { get; set; }
        public string Product_Company_Name { get; set; }
        public string Product_Code { get; set; }
        public int UserID { get; set; }

        public string dateFrom { get; set; }
        public string dateTo { get; set; }

        public string QuoteRefNum { get; set; }
        public int QuoteStatusID { get; set; }
        public int ClientID { get; set; }
        public int SupplierID { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }

        public string DoNo { get; set; }
        public string InvoiceNo { get; set; }

        public string PoRefNum { get; set; }

        public string ProjectName { get; set; }
        public int Project_Status_ID { get; set; }
        public int ProjectID { get; set; }

        public int CreatedBy { get; set; }

        public string MMType { get; set; }


        public List<EmployeeViewModel> eng_employee_master { get; set; }
        public List<ClientViewModel> eng_client_master { get; set; }
        public List<ProductViewModel> eng_product_master { get; set; }
        public List<SupplierViewModel> eng_supplier_master { get; set; }

        public List<QuoteViewModel> eng_quote_master { get; set; }
        public List<PoViewModel> eng_po_master { get; set; }
        public List<ProjectViewModel> eng_project_master { get; set; }

        public List<ProjectReportViewModel> eng_project_report { get; set; }
        public List<ClaimViewModel> eng_claim { get; set; }
        public List<TimeEntryViewModel> eng_time_entry { get; set; }

        public List<InvoiceMasterViewModel> eng_invoice_master { get; set; }

        public List<MMInwardViewModel> eng_inward { get; set; }
        public List<MMOutwardViewModel> eng_outward { get; set; }
    }
}