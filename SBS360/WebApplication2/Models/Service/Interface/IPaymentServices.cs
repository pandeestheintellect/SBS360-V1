using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eng360Web.Models.Service.Interface
{
    public interface IPaymentServices
    {
        int CreateReceivable(PaymentReceivableViewModel receivable);
        int CreatePayable(PaymentPayableViewModel payable);
        int SaveReceivable(PaymentReceivableViewModel receivable);
        int SavePayable(PaymentPayableViewModel payable);
        PaymentReceivableViewModel getReceivable(int pymtID);
        PaymentPayableViewModel getPayable(int pymtID);
        List<PaymentReceivableViewModel> getAllReceivables();
        List<PaymentPayableViewModel> getAllPayables();
        List<PaymentStatusViewModel> getAllPaymentStatus();
        int DeleteReceivable(int pymtID);
        int DeletePayable(int pymtID);

        decimal GetInvoiceAmount(string invno, string invfrom);
    }
}
