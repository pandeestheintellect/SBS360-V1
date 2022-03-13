using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eng360Web.Models.Service.Interface
{
    public interface IInvoiceServices
    {
        List<InvoiceMasterViewModel> getAllInvoices();
        int CreateInvoice(InvoiceMasterViewModel eng_invoice_master, List<QuoteDescriptionViewModel> quoteDescription);
        InvoiceMasterViewModel getInvoice(int invID);
        int SaveInvoice(InvoiceMasterViewModel eng_invoice_master, List<QuoteDescriptionViewModel> quoteDescription);
        List<InvoiceMasterViewModel> getAllClientInvoices(int id);
        List<InvoiceMasterViewModel> getAllOtherInvoices();
        List<InvoiceMasterViewModel> getFilterInvoices(FilterViewModel filter);

    }
}
