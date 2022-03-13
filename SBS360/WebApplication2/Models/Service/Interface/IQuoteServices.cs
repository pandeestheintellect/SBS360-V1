using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eng360Web.Models.Service.Interface
{
    public interface IQuoteServices
    {
        List<QuoteViewModel> getAllQuotes();
        QuoteViewModel getQuote(int quoteID);

        List<QuoteStatusViewModel> getStatus();
        int CreateQuote(QuoteViewModel eng_quote_master, List<QuoteDescriptionViewModel> quoteDescription, int revflag);

        int projectCreationSelection(List<int> ids, int LocationID, string startDate, string endDate, string ProjectName);

        List<LocationViewModel> getLocations();
        List<QuoteDescriptionViewModel> getAllDescriptions();

        int saveQuote(QuoteViewModel eng_quote_master, List<QuoteDescriptionViewModel> quoteDescription, List<int> deleted);

        QuoteDescriptionViewModel getDesc(int? id);
        int DeleteQuote(int quoteID);
        int UpdateInvDoDate(string invDate, string doDate, int invReleased, int QuoteID, string typeChk);
        List<QuoteViewModel> getFilterQuotes(FilterViewModel filter);
        List<QuoteViewModel> getFilterDOs(FilterViewModel filter);
        List<QuoteViewModel> getFilterInvoices(FilterViewModel filter);
        List<QuoteViewModel> getAllClientInvoices(int id);
        List<InvoiceInfoViewModel> getAllPaymentInvoices(int id);
        int ReviseQuoteFlag(int quoteid);
        List<QuoteViewModel> getAllClientQuotes(int id);
    }
}
