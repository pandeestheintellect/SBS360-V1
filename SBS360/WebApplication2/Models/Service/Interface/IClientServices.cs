using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eng360Web.Models.Service.Interface
{
    public interface IClientServices
    {
        List<ClientViewModel> getAllClients();
        ClientViewModel getClient(int clientID);
        List<FunctionViewModel> getAllFunctions();
        List<IndustryViewModel> getAllIndustries();
        int CreateClient(ClientViewModel eng_client_master, List<ClientContactViewModel> clientContact);
        int SaveClient(ClientViewModel client, List<ClientContactViewModel> clientContact, List<int> deleted);
        int DeleteClient(int clientID);
        ClientContactViewModel getContact(int? id);
    }
}
