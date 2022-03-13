using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Repository.Interface
{
    public interface ICompanyRepository
    {

        int CreateAndUpdate(CompanyViewModel company);
        CompanyViewModel getCompany();
    }
}