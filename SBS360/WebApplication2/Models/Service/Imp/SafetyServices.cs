using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Interface
{
    public class SafetyServices : ISafetyServices
    {
        private readonly ISafetyRepository safetyRepository;
        public SafetyServices(ISafetyRepository _safetyRepository)
        {
            safetyRepository = _safetyRepository;
        }

        public List<SafetyMasterViewModel> getAllSafetys()
        {
            return safetyRepository.getAllSafetys();
        }


        public int CreateSafety(SafetyMasterViewModel safety, List<int> hazardList, List<int> ppeList, List<int> workerList)
        {
            return safetyRepository.CreateSafety(safety, hazardList, ppeList, workerList);
        }

        public int SaveSafety(SafetyMasterViewModel safety, List<int> hazardList, List<int> ppeList, List<int> workerList)
        {
            return safetyRepository.SaveSafety(safety, hazardList, ppeList, workerList);
        }
        public int DeleteSafety(int safetyID)
        {
            return safetyRepository.DeleteSafety(safetyID);
        }

        public SafetyMasterViewModel getSafety(int safetyID)
        {

            return safetyRepository.getSafety(safetyID);
        }

        public List<SafetyMasterViewModel> getFilterSafety(FilterViewModel filter)
        {
            return safetyRepository.getFilterSafety(filter);
        }
        public List<SafetyHazardListViewModel> getAllHazards()
        {
            return safetyRepository.getAllHazards();
        }
        public List<SafetyPPEListViewModel> getAllPPEs()
        {
            return safetyRepository.getAllPPEs();
        }
        public List<SafetyInspectionMasterViewModel> getAllSafetyInspections()
        {
            return safetyRepository.getAllSafetyInspections();
        }
        public List<SafetyInspectionItemsViewModel> getAllItems()
        {
            return safetyRepository.getAllItems();
        }
        public int SICreate(SafetyInspectionMasterViewModel eng_Si)
        {
            return safetyRepository.SICreate(eng_Si);
        }
        public SafetyInspectionMasterViewModel getSafetyInspectionMaster(int? id)
        {
            return safetyRepository.getSafetyInspectionMaster(id);
        }
        public int SIEdit(SafetyInspectionMasterViewModel eng_Si)
        {
            return safetyRepository.SIEdit(eng_Si);
        }
        public List<NewSafetyInspectionViewModel> getAllNewSafetyInspections()
        {
            return safetyRepository.getAllNewSafetyInspections();
        }
        public int NewSICreate(NewSafetyInspectionViewModel eng_safety_esh, string path, string halfPath)
        {
            return safetyRepository.NewSICreate(eng_safety_esh, path, halfPath);
        }
        public NewSafetyInspectionViewModel NewSIEdit(int safetyID)
        {
            return safetyRepository.NewSIEdit(safetyID);
        }
        public int deleteSafetyFile(int? id)
        {
            return safetyRepository.deleteSafetyFile(id);
        }
        public int NewSIEdit(NewSafetyInspectionViewModel eng_safety_esh, string path, string halfPath)
        {
            return safetyRepository.NewSIEdit(eng_safety_esh,path,halfPath);
        }
        public int NewSICreate(MobileNewSafetyInspectionViewModel eng_safety_esh, string path, string halfPath)
        {
            return safetyRepository.NewSICreate(eng_safety_esh, path, halfPath);
        }

        public List<MobileNewSafetyInspectionViewModel> getAllSafetyInspections(int userID)
        {
            return safetyRepository.getAllSafetyInspections(userID);
        }
    }
}