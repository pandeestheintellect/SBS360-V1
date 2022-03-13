using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Imp
{
    public interface ISafetyServices
    {
       
        int CreateSafety(SafetyMasterViewModel safety, List<int> hazardList, List<int> ppeList, List<int> workerList);
        int SaveSafety(SafetyMasterViewModel safety, List<int> hazardList, List<int> ppeList, List<int> workerList);
        int DeleteSafety(int safetyID);
        SafetyMasterViewModel getSafety(int safetyID);
        List<SafetyMasterViewModel> getAllSafetys();
        List<SafetyMasterViewModel> getFilterSafety(FilterViewModel filter);
        List<SafetyHazardListViewModel> getAllHazards();
        List<SafetyPPEListViewModel> getAllPPEs();
        List<SafetyInspectionMasterViewModel> getAllSafetyInspections();
        List<SafetyInspectionItemsViewModel> getAllItems();
        int SICreate(SafetyInspectionMasterViewModel eng_Si);
        SafetyInspectionMasterViewModel getSafetyInspectionMaster(int? id);
        int SIEdit(SafetyInspectionMasterViewModel eng_Si);

        List<NewSafetyInspectionViewModel> getAllNewSafetyInspections();
        int NewSICreate(NewSafetyInspectionViewModel eng_safety_esh, string path, string halfPath);
        NewSafetyInspectionViewModel NewSIEdit(int safetyID);
        int deleteSafetyFile(int? id);
        int NewSIEdit(NewSafetyInspectionViewModel eng_safety_esh, string path, string halfPath);
        int NewSICreate(MobileNewSafetyInspectionViewModel eng_safety_esh, string path, string halfPath);
        List<MobileNewSafetyInspectionViewModel> getAllSafetyInspections(int userID);
    }
}