using Eng360Web.Models.Repository.Interface;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Imp
{
    public class InvoiceServices : IInvoiceServices
    {
        private readonly IInvoiceRepository invoiceRepository;
        public InvoiceServices(IInvoiceRepository _invoiceRepository)
        {
            invoiceRepository = _invoiceRepository;
        }

        public List<InvoiceMasterViewModel> getAllInvoices()
        {
            return invoiceRepository.getAllInvoices();
        }
        public int CreateInvoice(InvoiceMasterViewModel eng_invoice_master, List<QuoteDescriptionViewModel> quoteDescription)
        {
            return invoiceRepository.CreateInvoice(eng_invoice_master, quoteDescription);
        }
        public InvoiceMasterViewModel getInvoice(int invID)
        {
            return invoiceRepository.getInvoice(invID);
        }
        public int SaveInvoice(InvoiceMasterViewModel eng_invoice_master, List<QuoteDescriptionViewModel> quoteDescription)
        {
            return invoiceRepository.SaveInvoice(eng_invoice_master, quoteDescription);
        }
        public List<InvoiceMasterViewModel> getAllClientInvoices(int id)
        {
            return invoiceRepository.getAllClientInvoices(id);
        }
        public List<InvoiceMasterViewModel> getAllOtherInvoices()
        {
            return invoiceRepository.getAllOtherInvoices();
        }
        public List<InvoiceMasterViewModel> getFilterInvoices(FilterViewModel filter)
        {
            return invoiceRepository.getFilterInvoices(filter);
        }

    }
}