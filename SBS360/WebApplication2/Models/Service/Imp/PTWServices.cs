using Eng360Web.Models.Repository.Interface;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Imp
{
    public class PTWServices : IPTWServices
    {
        private readonly IPTWRepository ptwRepository;
        public PTWServices(IPTWRepository _ptwRepository)
        {
            ptwRepository = _ptwRepository;
        }

        public List<MobilePTWRConfigSatgeOne> getPTWStageOneConfig()
        {

            return ptwRepository.getPTWStageOneConfig();
        }

        public List<MobilePTWRConfigSatgeOne> getPTWStageOneConfigWeb(string ptwType)
        {

            return ptwRepository.getPTWStageOneConfigWeb(ptwType);

        }



        public List<MobilePTWstages> getPTWStages(string ptwType = "")
        {
            return ptwRepository.getPTWStages(ptwType);
        }

        public int createPtw(MobilePTWMaster eng_Ptw)
        {
            return ptwRepository.createPtw(eng_Ptw);
        }

        public List<MobilePTWMaster> getALLPTWMaster()
        {
            return ptwRepository.getALLPTWMaster();
        }

        public List<PTWMasterViewModel> getALLPTWMasterWeb()
        {
            return ptwRepository.getALLPTWMasterWeb();
        }

        public int createPtw(PTWMasterViewModel eng_Ptw)
        {
            return ptwRepository.createPtw(eng_Ptw);
        }

        public PTWMasterViewModel getPTWMaster(int? id)
        {
            return ptwRepository.getPTWMaster(id);
        }

        public int EditPtw(PTWMasterViewModel eng_Ptw)
        {
            return ptwRepository.EditPtw(eng_Ptw);
        }

        public List<PTWConspcMasterViewModel> getALLPTWConspcMasterWeb()
        {
            return ptwRepository.getALLPTWConspcMasterWeb();
        }
        public int createPtwConspc(PTWConspcMasterViewModel eng_Ptw)
        {
            return ptwRepository.createPtwConspc(eng_Ptw);
        }
        public int EditPtwConspc(PTWConspcMasterViewModel eng_Ptw)
        {
            return ptwRepository.EditPtwConspc(eng_Ptw);
        }
        public PTWConspcMasterViewModel getPTWConspcMaster(int? id)
        {
            return ptwRepository.getPTWConspcMaster(id);
        }
        public List<MobilePTWMaster> getAllPtwDetails(int? id)
        {
            return ptwRepository.getAllPtwDetails(id);
        }
        public int EditPtw(MobilePTWMaster eng_Ptw)
        {
            return ptwRepository.EditPtw(eng_Ptw);
        }
    }
}