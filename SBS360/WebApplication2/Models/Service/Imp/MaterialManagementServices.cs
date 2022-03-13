using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Interface
{
    public class MaterialManagementServices : IMaterialManagementServices
    {
        private readonly IMaterialManagementRepository mmRepository;
        public MaterialManagementServices(IMaterialManagementRepository _mmRepository)
        {
            mmRepository = _mmRepository;
        }

        public List<StoreMasterViewModel> getAllStores()
        {
            return mmRepository.getAllStores();
        }


        public int CreateStore(StoreMasterViewModel mm)
        {
            return mmRepository.CreateStore(mm);
        }

        public int SaveStore(StoreMasterViewModel mm)
        {
            return mmRepository.SaveStore(mm);
        }
       

        public StoreMasterViewModel getStore(int mmID)
        {

            return mmRepository.getStore(mmID);
        }

        //public List<StoreMasterViewModel> getFilterStores(FilterViewModel filter)
        //{
        //    return mmRepository.getFilterStores(filter);
        //}

        public List<MMInwardViewModel> getAllInwards()
        {
            return mmRepository.getAllInwards();
        }
        public List<MMOutwardViewModel> getAllOutwards()
        {
            return mmRepository.getAllOutwards();
        }
        public int CreateInward(MMInwardViewModel eng_inward, List<MMInwardDescViewModel> inwardDescription)
        {
            return mmRepository.CreateInward(eng_inward, inwardDescription);
        }
        public int CreateOutward(MMOutwardViewModel eng_outward, List<MMOutwardDescViewModel> outwardDescription)
        {
            return mmRepository.CreateOutward(eng_outward, outwardDescription);
        }
        public List<MMStockViewModel> getStock()
        {
            return mmRepository.getStock();
        }
        public MMInwardViewModel getInward(int inwID)
        {
            return mmRepository.getInward(inwID);
        }
        public int SaveInward(MMInwardViewModel eng_inward, List<MMInwardDescViewModel> inwardDescription)
        {
            return mmRepository.SaveInward(eng_inward, inwardDescription);
        }
        public MMOutwardViewModel getOutward(int ouwID)
        {
            return mmRepository.getOutward(ouwID);
        }
        public int SaveOutward(MMOutwardViewModel eng_outward, List<MMOutwardDescViewModel> outwardDescription)
        {
            return mmRepository.SaveOutward(eng_outward, outwardDescription);
        }
        public List<MMStockAdjustViewModel> getAllStockAdjusts()
        {
            return mmRepository.getAllStockAdjusts();
        }
        public int GetCurrentStock(int pid, int storeid)
        {
            return mmRepository.GetCurrentStock(pid, storeid);
        }
        public int CreateStockAdjust(MMStockAdjustViewModel eng_stockadj)
        {
            return mmRepository.CreateStockAdjust(eng_stockadj);
        }
        public MMStockAdjustViewModel getStockAdjust(int sajID)
        {
            return mmRepository.getStockAdjust(sajID);
        }
        public List<MMInwardViewModel> getFilterInwardReports(FilterViewModel filter)
        {
            return mmRepository.getFilterInwardReports(filter);
        }
        public List<MMOutwardViewModel> getFilterOutwardReports(FilterViewModel filter)
        {
            return mmRepository.getFilterOutwardReports(filter);
        }
    }
}