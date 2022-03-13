using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Imp
{
    public interface ITransportServices
    {
       
        int CreateTransport(TransportViewModel transport);
        int SaveTransport(TransportViewModel transport);
        int DeleteTransport(int transportID);
        TransportViewModel getTransport(int transportID);
        List<TransportViewModel> getAllTransports();
        List<TransportViewModel> getFilterTransports(FilterViewModel filter);
    }
}