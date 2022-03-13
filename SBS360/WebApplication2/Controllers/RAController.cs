using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eng360Web.Models.Domain;
using Eng360Web.Models.ViewModel;
using AutoMapper;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.Utility;
using Eng360Web.Models.Security;
using System.Globalization;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class RAController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly IRAServices raService;
        private readonly IProjectServices projectService;
        private readonly IEmployeeServices employeeService;

        public RAController(IRAServices _raService, IProjectServices _projectService, IEmployeeServices _employeeService)
        {
            raService = _raService;
            projectService = _projectService;
            employeeService = _employeeService;
        }

        public ActionResult Index()
        {
            return View(raService.getAllRiskAssessments().ToList());
        }
        

        // GET: Work Activity
        public ActionResult IndexRAWA()
        {
            return View(raService.getAllWorkActivities().ToList());
        }

        // GET: Process
        public ActionResult IndexRAPS()
        {
            return View(raService.getAllRAProcesses().ToList());
        }

        // GET: Location
        public ActionResult IndexRALN()
        {
            return View(raService.getAllLocations().ToList());
        }

        // GET: HazardList
        public ActionResult IndexRAHZ()
        {
            return View(raService.getAllHazardList().ToList());
        }

        // GET: HazardList
        public ActionResult IndexRAPA()
        {
            return View(raService.getAllPAHs().ToList());
        }

        // GET: Control MEasures
        public ActionResult IndexRACM()
        {
            return View(raService.getAllControlMeasures().ToList());
        }


        // GET: RA/Masters
        public ActionResult Create(string id)
        {
            ViewBag.MasterType = id;
            ViewBag.ItemID = 0;
            return View();
        }

        // POST: RA/Masters
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RAMastersViewModel eng_ra_detail)
        {
            if (ModelState.IsValid)
            {
                int result = 0;
                if (eng_ra_detail.ItemID == 0)
                {
                    eng_ra_detail.CreatedBy = AppSession.GetCurrentUserId();
                    eng_ra_detail.CreatedDate = DateTime.Now;
                    result = raService.CreateRA(eng_ra_detail);
                }
                else
                {
                    eng_ra_detail.UpdatedBy = AppSession.GetCurrentUserId();
                    eng_ra_detail.UpdatedDate = DateTime.Now;
                    result = raService.EditRA(eng_ra_detail);

                }               
                //var result = raService.CreateRA(eng_ra_detail);                
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }

            }

            return View(eng_ra_detail);
        }


        // GET: RAMaster/Edit/5
        public ActionResult Edit(string typ, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //   eng_product_master eng_product_master = db.eng_product_master.Find(id);
            var raitem = raService.getRAItem(typ, id ?? default(int));
            if (raitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.MasterType = typ;
            ViewBag.ItemID = raitem.ItemID;
            //return View(raitem);
            return View("~\\Views\\RA\\Create.cshtml", raitem);
        }

        // POST: RA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRAConfirmed(string typ , int id)
        {
            var result = raService.DeleteRA(typ, id);

            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }
        }

        [HttpGet]
        public ActionResult CreateRA()
        {
            var returnObject = new RATransMasterViewModel();

            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID != 4 && a.GroupID != 5 && a.GroupID != 7).ToList();
            //inds.Insert(0, new SafetyHazardListViewModel() { Id = 0, Industry_Title = "Select" });

            List<SelectListItem> emplist = new List<SelectListItem>();
            foreach (var emp in emps)
            {
                emplist.Add(new SelectListItem { Text = (emp.FirstName + " " + emp.LastName), Value = (emp.FirstName + " " + emp.LastName) });
            }

            ViewBag.WorkerList = emplist;

            var project = projectService.getAllProjects().Where(a => a.Project_Status_ID == 1 || a.Project_Status_ID == 2 || a.Project_Status_ID == 4).ToList();

            if (project == null)
            {
                project.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName");
            }
            else
            {
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName");
            }

            var svs = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 3).ToList();
            svs.Insert(0, new EmployeeViewModel() { FullName = "Select" });

            ViewBag.SVList = new SelectList(svs, "FullName", "FullName");

            return View(returnObject);
        }


        [HttpPost]
        public ActionResult CreateRA(RATransMasterViewModel ra)
        {
            int result = 0;

            if (!string.IsNullOrEmpty(ra.AssessmentDate))
                ra.AssessmentDate = DateTime.ParseExact(ra.AssessmentDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            if (!string.IsNullOrEmpty(ra.LastReviewDate))
                ra.LastReviewDate = DateTime.ParseExact(ra.LastReviewDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            if (!string.IsNullOrEmpty(ra.NextReviewDate))
                ra.NextReviewDate = DateTime.ParseExact(ra.NextReviewDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            
            if (!string.IsNullOrEmpty(ra.ApprovedDate))
                ra.ApprovedDate = DateTime.ParseExact(ra.ApprovedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


            ra.CreatedBy = AppSession.GetCurrentUserId();
            ra.CreatedDate = DateTime.Now;

            result = raService.CreateRATrans(ra);
            
            return getSuccessfulOperation();
        }

        [HttpGet]
        public ActionResult EditRA(int id)
        {
            
            var returnObject = raService.getRATransMaster(id);

            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID != 4 && a.GroupID != 5 && a.GroupID != 7).ToList();
            //inds.Insert(0, new SafetyHazardListViewModel() { Id = 0, Industry_Title = "Select" });

            List<SelectListItem> emplist = new List<SelectListItem>();
            foreach (var emp in emps)
            {
                emplist.Add(new SelectListItem { Text = (emp.FirstName + " " + emp.LastName), Value = (emp.FirstName + " " + emp.LastName) });
            }

            ViewBag.WorkerList = emplist;

            var project = projectService.getAllProjects().Where(a => a.Project_Status_ID == 1 || a.Project_Status_ID == 2 || a.Project_Status_ID == 4).ToList();

            if (project == null)
            {
                project.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName");
            }
            else
            {
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName", returnObject.ProjectID);
            }

            var svs = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 3).ToList();
            svs.Insert(0, new EmployeeViewModel() { FullName = "Select" });

            ViewBag.SVList = new SelectList(svs, "FullName", "FullName");

            return View(returnObject);
        }



        [HttpPost]
        public ActionResult EditRA(RATransMasterViewModel ra)
        {
            int result = 0;

            if (!string.IsNullOrEmpty(ra.AssessmentDate))
                ra.AssessmentDate = DateTime.ParseExact(ra.AssessmentDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            if (!string.IsNullOrEmpty(ra.LastReviewDate))
                ra.LastReviewDate = DateTime.ParseExact(ra.LastReviewDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            if (!string.IsNullOrEmpty(ra.NextReviewDate))
                ra.NextReviewDate = DateTime.ParseExact(ra.NextReviewDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            if (!string.IsNullOrEmpty(ra.ApprovedDate))
                ra.ApprovedDate = DateTime.ParseExact(ra.ApprovedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


            ra.UpdatedBy = AppSession.GetCurrentUserId();
            ra.UpdatedDate = DateTime.Now;

            result = raService.EditRA(ra);

            return getSuccessfulOperation();
        }

        [HttpGet]
        public ActionResult AddRADetailsOne(int id)
        {

            var returnObject = raService.getRATransMaster(id);

            var LocAll = raService.getAllLocations().ToList();
            var ProcessAll = raService.getAllRAProcesses().ToList();
            var WorkAll = raService.getAllWorkActivities().ToList();

            LocAll.Insert(0, new RAMastersViewModel() { ItemID = 0, ItemDescription = "Select" });
            ProcessAll.Insert(0, new RAMastersViewModel() { ItemID = 0, ItemDescription = "Select" });
            WorkAll.Insert(0, new RAMastersViewModel() { ItemID = 0, ItemDescription = "Select" });

            ViewBag.LocAll = new SelectList(LocAll, "ItemID", "ItemDescription");
            ViewBag.ProcessAll = new SelectList(ProcessAll, "ItemID", "ItemDescription");
            ViewBag.WorkAll = new SelectList(WorkAll, "ItemID", "ItemDescription");


            return View(returnObject);
        }

        [HttpPost]
        public ActionResult AddRADetailsOne(RATransMasterViewModel ra, List<RATransDetail1ViewModel> raDetails)
        {
            int result = 0;
            result = raService.AddRADetailsOne(ra, raDetails);
            return getSuccessfulOperation();
        }

        [HttpGet]
        public ActionResult AddRACM(int id)
        {

            var returnObject = raService.getRATransMaster(id);

            var HazardAll = raService.getAllHazardList().ToList();
            var ControlAll = raService.getAllControlMeasures().ToList();
            var WorkAll = raService.getAllProjectWorkActivities(id).ToList();
            var LhAll = raService.getAllLikelihoods().ToList();
            var SvAll = raService.getAllSeverities().ToList();
            var PahAll = raService.getAllPAHs().ToList();

            HazardAll.Insert(0, new RAMastersViewModel() { ItemID = 0, ItemDescription = "Select" });
            ControlAll.Insert(0, new RAMastersViewModel() { ItemID = 0, ItemDescription = "Select" });
            WorkAll.Insert(0, new RATransDetail1ViewModel() { RAWADetID = 0, WorkActivities = "Select" });
            LhAll.Insert(0, new RALikelihoodViewModel() { RMLHID = 0, Likelihood_Value = 0 });
            SvAll.Insert(0, new RASeverityViewModel() { RMSVID = 0, Severity_Value = 0 });
            PahAll.Insert(0, new RAMastersViewModel() { ItemID = 0, ItemDescription = "Select" });


            ViewBag.HazardAll = new SelectList(HazardAll, "ItemID", "ItemDescription");
            ViewBag.ControlAll = new SelectList(ControlAll, "ItemID", "ItemDescription");
            ViewBag.WorkAll = new SelectList(WorkAll, "RAWADetID", "WorkActivities");

            ViewBag.LhAll = new SelectList(LhAll, "RMLHID", "Likelihood_Value");
            ViewBag.SvAll = new SelectList(SvAll, "RMSVID", "Severity_Value");
            ViewBag.PahAll = new SelectList(PahAll, "ItemID", "ItemDescription");

            ViewBag.RCLhAll = new SelectList(LhAll, "RMLHID", "Likelihood_Value");
            ViewBag.RCSvAll = new SelectList(SvAll, "RMSVID", "Severity_Value");
            ViewBag.AddControlAll = new SelectList(ControlAll, "ItemID", "ItemDescription");

            var mng = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 2).ToList();
            mng.Insert(0, new EmployeeViewModel() { FullName = "Select" });

            ViewBag.MNGList = new SelectList(mng, "FullName", "FullName");


            return View(returnObject);
        }


        [HttpPost]
        public ActionResult AddRACM(RATransMasterViewModel ra, List<RATransRACMViewModel> raDetails)
        {
            int result = 0;
            result = raService.AddRACM(ra, raDetails);
            return getSuccessfulOperation();
        }


        // GET: Quote/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  eng_quote_master eng_quote_master = db.eng_quote_master.Find(id);
            var ra = raService.getRATransMaster(id ??default(int));
            if (ra == null)
            {
                return HttpNotFound();
            }
            
            return View(ra);
        }


    }
}
