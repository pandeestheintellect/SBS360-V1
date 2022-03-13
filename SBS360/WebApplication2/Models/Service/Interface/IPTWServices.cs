using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eng360Web.Models.Service.Interface
{
    public interface IPTWServices
    {
        List<MobilePTWRConfigSatgeOne> getPTWStageOneConfig();
        List<MobilePTWstages> getPTWStages(string ptwType = "");
        int createPtw(MobilePTWMaster eng_Ptw);
        List<MobilePTWMaster> getALLPTWMaster();
        List<PTWMasterViewModel> getALLPTWMasterWeb();
               

        List<MobilePTWRConfigSatgeOne> getPTWStageOneConfigWeb(string ptwType);
        int createPtw(PTWMasterViewModel eng_Ptw);
        PTWMasterViewModel getPTWMaster(int? id);
        int EditPtw(PTWMasterViewModel eng_Ptw);

        List<PTWConspcMasterViewModel> getALLPTWConspcMasterWeb();
        int createPtwConspc(PTWConspcMasterViewModel eng_Ptw);
        int EditPtwConspc(PTWConspcMasterViewModel eng_Ptw);
        PTWConspcMasterViewModel getPTWConspcMaster(int? id);
        int EditPtw(MobilePTWMaster eng_Ptw);
        List<MobilePTWMaster> getAllPtwDetails(int? id);
    }
}
