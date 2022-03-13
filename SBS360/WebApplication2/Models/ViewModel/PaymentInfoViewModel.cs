using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class PaymentInfoViewModel
    {
        public string InvoiceNum { get; set; }
        public string QuotationNum { get; set; }
        public string DONum { get; set; }
        public string InvoiceDate { get; set; }
        public decimal InvAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public string PymtStatus { get; set; }
        public int QuoteID { get; set; }
        public int ProjectID { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal ExcessReceived { get; set; }

        public List<InvoiceInfoViewModel> eng_invoice_info { get; set; }
        public List<PaymentReceivableViewModel> eng_pymt_receivable { get; set; }

    }
}