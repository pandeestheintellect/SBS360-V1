using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Repository.Imp
{
    public interface IPoRepository
    {
        

        int CreatePo(PoViewModel po, List<PoDescriptionViewModel> poDescription);
        int SavePo(PoViewModel po, List<PoDescriptionViewModel> poDescription, List<int> deleted);
        int DeletePo(int poID);
        PoViewModel getPo(int poID);
        List<PoViewModel> getAllPos();
        List<QuoteStatusViewModel> getStatus();
        PoDescriptionViewModel getDesc(int? id);
        List<PoViewModel> getFilterPos(FilterViewModel filter);
        List<PoViewModel> getAllPayPos(int id);
    }
}