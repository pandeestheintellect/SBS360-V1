using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Interface
{
    public class ClaimServices : IClaimServices
    {
        private readonly IClaimRepository claimRepository;
        public ClaimServices(IClaimRepository _claimRepository)
        {
            claimRepository = _claimRepository;
        }

        public List<ClaimViewModel> getAllClaims(int userid, int groupid)
        {
            return claimRepository.getAllClaims(userid, groupid);
        }


        public int CreateClaim(ClaimViewModel eng_claim, string path, string halfPath)
        {
            return claimRepository.CreateClaim(eng_claim, path, halfPath);
        }

        public int SaveClaim(ClaimViewModel eng_claim, string path, string halfPath)
        {
            return claimRepository.SaveClaim(eng_claim, path, halfPath);
        }
        public int DeleteClaim(int claimID)
        {
            return claimRepository.DeleteClaim(claimID);
        }

        public ClaimViewModel getClaim(int claimID)
        {

            return claimRepository.getClaim(claimID);
        }
        public ClaimDescriptionViewModel getClaimDesc(int? id)
        {
            return claimRepository.getClaimDesc(id);
        }

        public List<ClaimTypeViewModel> getClaimType()
        {
            return claimRepository.getClaimType();
        }
        public int deleteClaimDesc(int? id)
        {
            return claimRepository.deleteClaimDesc(id);
        }
        public int deleteClaimFile(int? id)
        {
            return claimRepository.deleteClaimFile(id);
        }
        public int ApproveRejectClaim(ClaimViewModel claim)
        {
            return claimRepository.ApproveRejectClaim(claim);
        }
        public List<ClaimViewModel> getAllReportClaims()
        {
            return claimRepository.getAllReportClaims();
        }
        public List<ClaimViewModel> getFilterClaims(FilterViewModel filter)
        {
            return claimRepository.getFilterClaims(filter);
        }
        public List<ClaimViewModel> getAllEmpClaims(int id)
        {
            return claimRepository.getAllEmpClaims(id);
        }
    }
}