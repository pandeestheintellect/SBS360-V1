using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Interface
{
    public class PoServices : IPoServices
    {
        private readonly IPoRepository poRepository;

        public PoServices(IPoRepository _poRepository)
        {
            poRepository = _poRepository;
        }

        public List<PoViewModel> getAllPos()
        {
            return poRepository.getAllPos();
        }

        public List<QuoteStatusViewModel> getStatus()
        {
            return poRepository.getStatus();
        }


        public int CreatePo(PoViewModel po, List<PoDescriptionViewModel> poDescription)
        {
            return poRepository.CreatePo(po, poDescription);
        }

        public int SavePo(PoViewModel po, List<PoDescriptionViewModel> poDescription, List<int> deleted)
        {
            return poRepository.SavePo(po, poDescription, deleted);
        }
        public int DeletePo(int poID)
        {
            return poRepository.DeletePo(poID);
        }

        public PoViewModel getPo(int poID)
        {

            return poRepository.getPo(poID);
        }

        public PoDescriptionViewModel getDesc(int? id)
        {
            return poRepository.getDesc(id);
        }
        public List<PoViewModel> getFilterPos(FilterViewModel filter)
        {
            return poRepository.getFilterPos(filter);
        }
        public List<PoViewModel> getAllPayPos(int id)
        {
            return poRepository.getAllPayPos(id);
        }
    }
}