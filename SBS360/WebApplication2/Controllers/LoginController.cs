using Eng360Web.Models.ViewModel;
using Eng360Web.Models.Service.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Eng360Web.Models.Utility;
using Eng360Web.Models.Service.Interface;

namespace Eng360Web.Controllers
{
    public class LoginController : SuperBaseController
    {
        private readonly IUserServices userService;
        private readonly ICompanyServices companyService;

        /// <summary>
        /// Initializes the controller.
        /// </summary>
        /// <param name="_userService"></param>
        public LoginController(IUserServices _userService, ICompanyServices _companyService)
        {
            userService = _userService;
            companyService = _companyService;


        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {

                var returnUser = userService.ValidateUserLogin(model.UserName, model.Password);
                var company = companyService.getCompany();
                if (returnUser != null)
                {
                    if (returnUser.IsActive == 1)
                    {

                        FormsAuthentication.SetAuthCookie(returnUser.UserName,false);
                        AppSession.SetCurrentUserId(returnUser.UserID);
                        AppSession.SetCurrentUserGroup(returnUser.GroupID?? default(int));
                        AppSession.SetCurrentUserName(returnUser.DisplayName);

                        AppSession.SetCompanyDetail(company);

                        return RedirectToAction("Index", "Home");
                    }
                    else {
                        return View();
                    }

                }

            }

            return View();
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();

            return RedirectToAction("Index", "Login");
        }
    }
}