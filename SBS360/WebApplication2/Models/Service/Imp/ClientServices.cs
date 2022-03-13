using Eng360Web.Models.Repository.Interface;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Imp
{
    public class ClientServices : IClientServices
    {
        private readonly IClientRepository clientRepository;
        public ClientServices(IClientRepository _clientRepository)
        {
            clientRepository = _clientRepository;
        }
        public List<ClientViewModel> getAllClients()
        {
            return   clientRepository.getAllClients();
        }
        public ClientViewModel getClient(int clientID)
        {

            return clientRepository.getClient(clientID);
        }
        public List<FunctionViewModel> getAllFunctions()
        {
            return clientRepository.getAllFunctions();
        }
        public List<IndustryViewModel> getAllIndustries()
        {
            return clientRepository.getAllIndustries();
        }
        public int CreateClient(ClientViewModel eng_client_master, List<ClientContactViewModel> clientContact)
        {
            return clientRepository.CreateClient(eng_client_master, clientContact);
        }
        public int SaveClient(ClientViewModel client, List<ClientContactViewModel> clientContact, List<int> deleted)
        {
            return clientRepository.SaveClient(client, clientContact, deleted);
        }
        public int DeleteClient(int clientID)
        {
            return clientRepository.DeleteClient(clientID);
        }
        public ClientContactViewModel getContact(int? id)
        {
            return clientRepository.getContact(id);
        }
    }
}