using Eng360Web.Models.Security;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class CompanyController : SuperBaseController
    {
        private readonly ICompanyServices companyService;

        public CompanyController(ICompanyServices _companyService)
        {

            companyService = _companyService;
        }


        // GET: Company
        public ActionResult Index()
        {
            var company = companyService.getCompany();
            return View(company);
        }

        [HttpPost]
        public ActionResult Index(CompanyViewModel company)
        {
            string halfPath = "/images/CompanyLogo/logo.png";


            var path = Path.Combine(Server.MapPath("~/images/CompanyLogo/"), "logo.png");
            if (company.profile_Path != null)
            {
                company.profile_Path.SaveAs(path);
                
            }
            company.LogoPath = path;

            var result = companyService.CreateAndUpdate(company);
            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }

           // return View();
        }
    }
}