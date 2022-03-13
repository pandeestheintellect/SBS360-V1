using Eng360Web.Models.Repository.Interface;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Imp
{
    public class QuoteServices : IQuoteServices
    {
        private readonly IQuoteRepository quoteRepository;
        public QuoteServices(IQuoteRepository _quoteRepository)
        {
            quoteRepository = _quoteRepository;
        }
        public List<QuoteViewModel> getAllQuotes()
        {
            return quoteRepository.getAllQuotes();
        }
        public QuoteViewModel getQuote(int quoteID)
        {

            return quoteRepository.getQuote(quoteID);
        }

        public List<QuoteStatusViewModel> getStatus()
        {
            return quoteRepository.getStatus();
        }

        public int CreateQuote(QuoteViewModel eng_quote_master, List<QuoteDescriptionViewModel> quoteDescription, int revflag)
        {
            return quoteRepository.CreateQuote(eng_quote_master, quoteDescription, revflag);
        }

        public int projectCreationSelection(List<int> ids, int LocationID, string startDate, string endDate, string ProjectName)
        {
            return quoteRepository.projectCreationSelection(ids, LocationID, startDate, endDate, ProjectName);
        }

        public List<LocationViewModel> getLocations()
        {
            return quoteRepository.getLocations();
        }

        public List<QuoteDescriptionViewModel> getAllDescriptions()
        {
            return quoteRepository.getAllDescriptions();
        }

        public int saveQuote(QuoteViewModel eng_quote_master, List<QuoteDescriptionViewModel> quoteDescription, List<int> deleted)
        {
            return quoteRepository.saveQuote(eng_quote_master, quoteDescription, deleted);
        }

        public QuoteDescriptionViewModel getDesc(int? id)
        {
            return quoteRepository.getDesc(id);
        }

        public int DeleteQuote(int quoteID)
        {
            return quoteRepository.DeleteQuote(quoteID);
        }
        public int UpdateInvDoDate(string invDate, string doDate, int invReleased, int QuoteID, string typeChk)
        {
            return quoteRepository.UpdateInvDoDate(invDate, doDate, invReleased, QuoteID, typeChk);
        }
        public List<QuoteViewModel> getFilterQuotes(FilterViewModel filter)
        {
            return quoteRepository.getFilterQuotes(filter);
        }
        public List<QuoteViewModel> getFilterDOs(FilterViewModel filter)
        {
            return quoteRepository.getFilterDOs(filter);
        }
        public List<QuoteViewModel> getFilterInvoices(FilterViewModel filter)
        {
            return quoteRepository.getFilterInvoices(filter);
        }
        public int ReviseQuoteFlag(int quoteid)
        {
            return quoteRepository.ReviseQuoteFlag(quoteid);
        }
        public List<QuoteViewModel> getAllClientInvoices(int id)
        {
            return quoteRepository.getAllClientInvoices(id);
        }
        public List<InvoiceInfoViewModel> getAllPaymentInvoices(int id)
        {
            return quoteRepository.getAllPaymentInvoices(id);
        }
        public List<QuoteViewModel> getAllClientQuotes(int id)
        {
            return quoteRepository.getAllClientQuotes(id);
        }


    }
}