using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Imp
{
    public interface IMaterialManagementServices
    {

        int CreateStore(StoreMasterViewModel store);
        int SaveStore(StoreMasterViewModel store);

        StoreMasterViewModel getStore(int storeID);
        List<StoreMasterViewModel> getAllStores();
       // List<StoreMasterViewModel> getFilterStores(FilterViewModel filter);

        List<MMInwardViewModel> getAllInwards();
        List<MMOutwardViewModel> getAllOutwards();

        int CreateInward(MMInwardViewModel eng_inward, List<MMInwardDescViewModel> inwardDescription);
        int CreateOutward(MMOutwardViewModel eng_outward, List<MMOutwardDescViewModel> outwardDescription);
        List<MMStockViewModel> getStock();
        MMInwardViewModel getInward(int inwID);
        MMOutwardViewModel getOutward(int ouwID);
        List<MMStockAdjustViewModel> getAllStockAdjusts();
        int GetCurrentStock(int pid, int storeid);

        int SaveInward(MMInwardViewModel eng_inward, List<MMInwardDescViewModel> inwardDescription);
        int SaveOutward(MMOutwardViewModel eng_outward, List<MMOutwardDescViewModel> outwardDescription);

        int CreateStockAdjust(MMStockAdjustViewModel eng_stockadj);
        MMStockAdjustViewModel getStockAdjust(int sajID);

        List<MMInwardViewModel> getFilterInwardReports(FilterViewModel filter);
        List<MMOutwardViewModel> getFilterOutwardReports(FilterViewModel filter);

    }
}