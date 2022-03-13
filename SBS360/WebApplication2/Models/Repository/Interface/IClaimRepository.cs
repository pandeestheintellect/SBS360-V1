using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Repository.Imp
{
    public interface IClaimRepository
    {
        

        int CreateClaim(ClaimViewModel eng_claim, string path, string halfPath);
        int SaveClaim(ClaimViewModel eng_claim, string path, string halfPath);
        int DeleteClaim(int claimID);
        ClaimViewModel getClaim(int claimID);
        List<ClaimViewModel> getAllClaims(int userid, int groupid);
        ClaimDescriptionViewModel getClaimDesc(int? id);

        List<ClaimTypeViewModel> getClaimType();
        int deleteClaimDesc(int? id);
        int deleteClaimFile(int? id);
        int ApproveRejectClaim(ClaimViewModel claim);
        List<ClaimViewModel> getAllReportClaims();
        List<ClaimViewModel> getFilterClaims(FilterViewModel filter);
        List<ClaimViewModel> getAllEmpClaims(int id);


    }
}