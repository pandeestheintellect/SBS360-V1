using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eng360Web.Models.Security;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.Utility;
using Eng360Web.Models.ViewModel;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class HomeController : Controller
    {
        private readonly IUserServices userService;
        private readonly IEmployeeServices employeeService;
        private readonly IQuoteServices quoteService;

        public HomeController(UserServices _userService, IEmployeeServices _employeeService, IQuoteServices _quoteService)
        {
            userService = _userService;
            employeeService = _employeeService;
            quoteService = _quoteService;
        }
        public ActionResult Index()
        {
            var groupid = Models.Utility.AppSession.GetCurrentUserGroup();
            var userid = Models.Utility.AppSession.GetCurrentUserId();


            var Ivm = new IndexViewModel();

            if (groupid == 1 ||  groupid == 5)
            {
                var HrDbd = employeeService.getHREmpDashBoardResults().ToList();
                Ivm.DB_Emp = HrDbd;

                var CpyDbd = employeeService.getCompanyDashBoardResults().ToList();
                Ivm.DB_Company = CpyDbd;

                var VehDbd = employeeService.getVehicleDashBoardResults().ToList();
                Ivm.DB_Vehicle = VehDbd;
            }

            if (groupid == 1 || groupid == 4 || groupid == 5)
            {
                var PayDbd = employeeService.getPayableDashBoardResults().ToList();
                Ivm.DB_Payable = PayDbd;

                var RecDbd = employeeService.getReceivableDashBoardResults().ToList();
                Ivm.DB_Receivable = RecDbd;

            }

            if (groupid == 1 || groupid == 2 || groupid == 3 || groupid == 5)
            {
                var OpDbd = employeeService.getOperationDashBoardResults(groupid, userid).ToList();
                Ivm.DB_Operation = OpDbd;
            }

            if (groupid == 4)
            {
                var DirProjDbd = employeeService.getDirectorProjectDashBoardResults().ToList();
                Ivm.DB_Director_Project = DirProjDbd;
            }

            if (groupid == 2 || groupid == 3)
            {
                var projDbd = employeeService.getSVManProjectDashBoardResults(groupid, userid).ToList();
                Ivm.DB_SVMan_Project = projDbd;

                var dttrDbd = employeeService.getSVManDTTRDashBoardResults(groupid, userid).ToList();
                Ivm.DB_SVMan_DTTR = dttrDbd;

                var qlyDbd = employeeService.getSVManQualityDashBoardResults(groupid, userid).ToList();
                Ivm.DB_SVMan_Quality = qlyDbd;

                var HrDbd = employeeService.getHREmpDashBoardResults().ToList();
                Ivm.DB_Emp = HrDbd;
            }


            var quote = quoteService.getAllQuotes().Where(a => a.isFullyPaid != 1 && a.QuoteStatusID != 4 && a.QuoteStatusID != 1).ToList();
            Ivm.OpenQuoteCount = quote.Count;

            return View(Ivm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult About1()
        {
            ViewBag.Message = "Your application description page.";

            return Json(new
            {
                value = string.Format("{0}", "OK")
            },
                JsonRequestBehavior.AllowGet);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult LeftMenu()
        {
         List< ModuleViewModel> lstModules=   userService.GetModuleNames(AppSession.GetCurrentUserId());
            if (lstModules != null)
            {

                lstModules= lstModules.OrderBy(o => o.order_by).ToList();
            }
            return PartialView(lstModules);
        }
    }
}