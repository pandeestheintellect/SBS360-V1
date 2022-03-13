using Eng360Web.Models.Repository.Interface;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Imp
{
    public class CompanyServices : ICompanyServices
    {

        private readonly ICompanyRepository companyRepository;

        public CompanyServices(ICompanyRepository _companyRepository)
        {

            companyRepository = _companyRepository;
        }

        public CompanyViewModel getCompany()
        {
            return companyRepository.getCompany();
        }

        public int CreateAndUpdate(CompanyViewModel company)
        {


            return companyRepository.CreateAndUpdate(company);
        }
    }
}