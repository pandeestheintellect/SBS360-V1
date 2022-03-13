using AutoMapper;
using Eng360Web.Models.Domain;
using Eng360Web.Models.Repository.Interface;
using Eng360Web.Models.Utility;
using Eng360Web.Models.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Repository.Imp
{
    public class ClientRepository : IClientRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
        Logger logger = LogManager.GetCurrentClassLogger();
        public List<ClientViewModel> getAllClients()
        {
            var eng_client_master = Eng360DB.eng_client_master.ToList();
            var lstClientView = Mapper.Map<List<ClientViewModel>>(eng_client_master);
            return lstClientView;
        }

        public int CreateClient(ClientViewModel eng_client_master, List<ClientContactViewModel> clientContact)
        {
            
            try
            {

                eng_address_master address = new eng_address_master()
                {
                    Address1 = eng_client_master.eng_address_master.Address1,
                    Address2 = eng_client_master.eng_address_master.Address2,
                    City = eng_client_master.eng_address_master.City,
                    Country = eng_client_master.eng_address_master.Country,
                    Fax1 = eng_client_master.eng_address_master.Fax1,
                    Email = eng_client_master.eng_address_master.Email,
                    Web = eng_client_master.eng_address_master.Web,
                    Tel = eng_client_master.eng_address_master.Tel,
                    Mobile = eng_client_master.eng_address_master.Mobile,
                    Postal_Code = eng_client_master.eng_address_master.Postal_Code
                };
                eng_client_master.AddressID = address.AddressID;

                eng_client_master domainClient = Mapper.Map<eng_client_master>(eng_client_master);
                Eng360DB.eng_client_master.Add(domainClient);

                foreach (var cont in clientContact)
                {
                    eng_client_contact domainContact = Mapper.Map<eng_client_contact>(cont);
                    domainContact.ClientID = domainClient.ClientID;
                    Eng360DB.eng_client_contact.Add(domainContact);


                }


                return Eng360DB.SaveChanges();
            }
            catch (Exception ex)
            {
                //logger
                return -1;
            }
        }

        public int SaveClient(ClientViewModel client, List<ClientContactViewModel> clientContact, List<int> deleted)
        {
            try
            {
                var _db_client = Mapper.Map<eng_client_master>(client);
                Eng360DB.Entry(_db_client).State = EntityState.Modified;

                var domainAddress = Eng360DB.eng_address_master.First(a => a.AddressID == client.AddressID);

                Eng360DB.Entry(domainAddress).State = EntityState.Modified;

                foreach (var cont in clientContact)
                {
                    //insert 
                    if (cont.CCID == 0)
                    {
                        cont.ClientID = _db_client.ClientID;
                        eng_client_contact domainCont = Mapper.Map<eng_client_contact>(cont);
                        Eng360DB.eng_client_contact.Add(domainCont);
                    }
                    if (cont.CCID > 0)
                    {
                        cont.ClientID = _db_client.ClientID;
                        eng_client_contact domainCont = Mapper.Map<eng_client_contact>(cont);
                        Eng360DB.Entry(domainCont).State = EntityState.Modified;
                    }
                    //edit
                }

                foreach (var deleteCont in deleted)
                {
                    var willbeDeleted = Eng360DB.eng_client_contact.Find(deleteCont);
                    Eng360DB.eng_client_contact.Remove(willbeDeleted);
                }

            }
            catch (Exception ex)
            {
                //logger
                //return -1;
            }
            return Eng360DB.SaveChanges();
        }

        public int DeleteClient(int clientID)
        {


            var _db_client = Eng360DB.eng_client_master.First(a => a.ClientID == clientID);

            _db_client.UpdatedBy = AppSession.GetCurrentUserId();
            _db_client.UpdatedDate = DateTime.Now;
            _db_client.IsActive = 0;


            Eng360DB.Entry(_db_client).State = EntityState.Modified;
            return Eng360DB.SaveChanges();
        }



        public ClientViewModel getClient(int clientID)
        {
            eng_client_master eng_client_master = Eng360DB.eng_client_master.Find(clientID);

            return Mapper.Map<ClientViewModel>(eng_client_master);
        }

        public List<FunctionViewModel> getAllFunctions()
        {
            var eng_sys_function = Eng360DB.eng_sys_function.ToList();
            var lstFunctionView = Mapper.Map<List<FunctionViewModel>>(eng_sys_function);
            return lstFunctionView;
        }

        public List<IndustryViewModel> getAllIndustries()
        {
            var eng_sys_industry = Eng360DB.eng_sys_industry.ToList();
            var lstIndustryView = Mapper.Map<List<IndustryViewModel>>(eng_sys_industry);
            return lstIndustryView;
        }

        public ClientContactViewModel getContact(int? id)
        {

            var domainCont = Eng360DB.eng_client_contact.Find(id);
            return Mapper.Map<ClientContactViewModel>(domainCont);
        }
    }
}