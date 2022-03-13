using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class PaymentPayableViewModel
    {
        public int PymtID { get; set; }

        public string VoucherNo { get; set; }
        [Display(Name = "Pay To")]
        public Nullable<int> SupplierID { get; set; }
        [Display(Name = "Transaction Date"), Required]
        public string Tr_date { get; set; }

        public string ClaimNo { get; set; }

        [Display(Name = "Pay To Emp")]
        public Nullable<int> EmpID { get; set; }

        [Display(Name = "Due Date")]
        public string Due_date { get; set; }
        [Display(Name = "PO Number")]
        public string PoNo { get; set; }
        [Display(Name = "Transaction Status")]
        public Nullable<int> Tr_status { get; set; }
        [Display(Name = "Your Reference")]
        public string reference { get; set; }
        [Display(Name = "Financial Year")]
        public string FY { get; set; }
        public string Particulars { get; set; }
        [Display(Name = "Amount"),
        RegularExpression(@"^(0|-?\d{0,16}(\.\d{0,2})?)$", ErrorMessage = "Invalid Amount"),Required]
        public Nullable<decimal> Amount { get; set; }
        public string GTAX { get; set; }
        public string PreparedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string ReceivedBy { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public string PayableType { get; set; }
        public Nullable<int> ClaimID { get; set; }

        public Nullable<int> PoID { get; set; }

        public string NPoID { get; set; }

        public string DummyEmp { get; set; }

        public decimal TotAmt { get; set; }
        public decimal AlreadyPaid { get; set; }

        public PoViewModel eng_po_master { get; set; }
        public SupplierViewModel eng_supplier_master { get; set; }
        public PaymentStatusViewModel eng_sys_pymt_status { get; set; }
        public EmployeeViewModel eng_employee_profile { get; set; }
        public ClaimViewModel eng_claim { get; set; }
        //public List< QuoteViewModel> eng_quote_master { get; set; }
    }
}