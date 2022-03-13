using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class InvoiceInfoViewModel
    {
        public string InvoiceNum { get; set; }
        
        public string InvoiceDate { get; set; }
        public decimal InvAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public string InvoiceType { get; set; }
        public string PymtStatus { get; set; }
        public int QuoteID { get; set; }
        public int ProjectID { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal ExcessReceived { get; set; }

    }
}