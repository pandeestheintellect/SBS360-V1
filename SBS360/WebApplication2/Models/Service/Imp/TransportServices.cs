using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Interface
{
    public class TransportServices : ITransportServices
    {
        private readonly ITransportRepository transportRepository;
        public TransportServices(ITransportRepository _transportRepository)
        {
            transportRepository = _transportRepository;
        }

        public List<TransportViewModel> getAllTransports()
        {
            return transportRepository.getAllTransports();
        }


        public int CreateTransport(TransportViewModel transport)
        {
            return transportRepository.CreateTransport(transport);
        }

        public int SaveTransport(TransportViewModel transport)
        {
            return transportRepository.SaveTransport(transport);
        }
        public int DeleteTransport(int transportID)
        {
            return transportRepository.DeleteTransport(transportID);
        }

        public TransportViewModel getTransport(int transportID)
        {

            return transportRepository.getTransport(transportID);
        }

        public List<TransportViewModel> getFilterTransports(FilterViewModel filter)
        {
            return transportRepository.getFilterTransports(filter);
        }
    }
}