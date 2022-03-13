using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Imp
{
    public interface IRAServices
    {
        List<RATransMasterViewModel> getAllRiskAssessments();
        List<RAMastersViewModel> getAllWorkActivities();
        List<RAMastersViewModel> getAllRAProcesses();
        List<RAMastersViewModel> getAllLocations();
        List<RAMastersViewModel> getAllHazardList();
        List<RAMastersViewModel> getAllPAHs();
        List<RAMastersViewModel> getAllControlMeasures();

        int CreateRA(RAMastersViewModel ra);
        int EditRA(RAMastersViewModel ra);
        RAMastersViewModel getRAItem(string typ, int itemID);
        int DeleteRA(string typ, int id);
        int CreateRATrans(RATransMasterViewModel ra);
        RATransMasterViewModel getRATransMaster(int raID);
        int EditRA(RATransMasterViewModel ra);
        int AddRADetailsOne(RATransMasterViewModel ra, List<RATransDetail1ViewModel> raDetails);
        List<RALikelihoodViewModel> getAllLikelihoods();
        List<RASeverityViewModel> getAllSeverities();
        List<RATransDetail1ViewModel> getAllProjectWorkActivities(int id);
        int AddRACM(RATransMasterViewModel ra, List<RATransRACMViewModel> raDetails);
        int AddRACMMobile(RATransRACMViewModel raDetails);
        List<MobileRATransRACMViewModel> GetRaDetails(int id);
    }
}