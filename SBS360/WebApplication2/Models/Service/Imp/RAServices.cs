using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Interface
{
    public class RAServices : IRAServices
    {
        private readonly IRARepository raRepository;
        public RAServices(IRARepository _raRepository)
        {
            raRepository = _raRepository;
        }

        public List<RATransMasterViewModel> getAllRiskAssessments()
        {
            return raRepository.getAllRiskAssessments();
        }

        public List<RAMastersViewModel> getAllWorkActivities()
        {
            return raRepository.getAllWorkActivities();
        }

        public List<RAMastersViewModel> getAllRAProcesses()
        {
            return raRepository.getAllRAProcesses();
        }

        public List<RAMastersViewModel> getAllLocations()
        {
            return raRepository.getAllLocations();
        }

        public List<RAMastersViewModel> getAllHazardList()
        {
            return raRepository.getAllHazardList();
        }

        public List<RAMastersViewModel> getAllPAHs()
        {
            return raRepository.getAllPAHs();
        }

        public List<RAMastersViewModel> getAllControlMeasures()
        {
            return raRepository.getAllControlMeasures();
        }

        public RAMastersViewModel getRAItem(string typ, int itemID)
        {
            return raRepository.getRAItem(typ, itemID);
        }

        public int CreateRA(RAMastersViewModel ra)
        {
            return raRepository.CreateRA(ra);
        }

        public int EditRA(RAMastersViewModel ra)
        {
            return raRepository.EditRA(ra);
        }

        public int DeleteRA(string typ, int id)
        {
            return raRepository.DeleteRA(typ, id);
        }

        public int CreateRATrans(RATransMasterViewModel ra)
        {
            return raRepository.CreateRATrans(ra);
        }

        public RATransMasterViewModel getRATransMaster(int raID)
        {
            return raRepository.getRATransMaster(raID);
        }

        public int EditRA(RATransMasterViewModel ra)
        {
            return raRepository.EditRA(ra);
        }

        public int AddRADetailsOne(RATransMasterViewModel ra, List<RATransDetail1ViewModel> raDetails)
        {
            return raRepository.AddRADetailsOne(ra, raDetails);
        }

        public List<RALikelihoodViewModel> getAllLikelihoods()
        {
            return raRepository.getAllLikelihoods();
        }

        public List<RASeverityViewModel> getAllSeverities()
        {
            return raRepository.getAllSeverities();
        }

        public List<RATransDetail1ViewModel> getAllProjectWorkActivities(int id)
        {
            return raRepository.getAllProjectWorkActivities(id);
        }

        public int AddRACM(RATransMasterViewModel ra, List<RATransRACMViewModel> raDetails)
        {
            return raRepository.AddRACM(ra, raDetails);
        }
        public int AddRACMMobile(RATransRACMViewModel raDetails)
        {
            return raRepository.AddRACMMobile(raDetails);
        }
        public List<MobileRATransRACMViewModel> GetRaDetails(int id)
        {
            return raRepository.GetRaDetails(id);
        }


    }
}