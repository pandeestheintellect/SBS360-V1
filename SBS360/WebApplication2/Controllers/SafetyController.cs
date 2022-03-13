using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eng360Web.Models.Domain;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.ViewModel;
using System.Globalization;
using Eng360Web.Models.Utility;
using NLog;
using Eng360Web.Models.Security;
using System.IO;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class SafetyController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly ISafetyServices safetyService;
        private readonly IEmployeeServices employeeService;
        private readonly IProjectServices projectService;

        Logger logger = LogManager.GetCurrentClassLogger();

        public SafetyController(ISafetyServices _safetyService, IEmployeeServices _employeeService, IProjectServices _projectService)
        {
            safetyService = _safetyService;
            employeeService = _employeeService;
            projectService = _projectService;

        }

        // GET: Safety
        public ActionResult Index()
        {
            var safety = safetyService.getAllSafetys().ToList();
            return View(safety);
        }

        // GET: Safety
        public ActionResult SIIndex()
        {
            var safety = safetyService.getAllSafetyInspections().ToList();
            return View(safety);
        }

        // GET: Safety
        public ActionResult NewSIIndex()
        {
            var safety = safetyService.getAllNewSafetyInspections().ToList();
            return View(safety);
        }


        // GET: Safety/Details/5
        public ActionResult Details(int? id)
        {
            var safety = safetyService.getSafety(id ?? default(int));

            if (safety == null)
            {
                return getFailedOperation();
            }

            var hazards = safetyService.getAllHazards().ToList();
            List<SelectListItem> names = new List<SelectListItem>();
            foreach (var cnt in hazards)
            {
                names.Add(new SelectListItem { Text = cnt.HazardDesc, Value = cnt.HazardID.ToString() });
            }
            ViewBag.HazardList = names;

            var ppes = safetyService.getAllPPEs().ToList();
            List<SelectListItem> pplist = new List<SelectListItem>();
            foreach (var ppe in ppes)
            {
                pplist.Add(new SelectListItem { Text = ppe.PPE_Desc, Value = ppe.PPEID.ToString() });
            }
            ViewBag.PPEList = pplist;

            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
            List<SelectListItem> emplist = new List<SelectListItem>();
            foreach (var emp in emps)
            {
                emplist.Add(new SelectListItem { Text = emp.FirstName, Value = emp.UserID.ToString() });
            }
            ViewBag.WorkerList = emplist;


            return View(safety);
        }

        // GET: Safety/Create
        public ActionResult Create()
        {
            try
            {
                var hazards = safetyService.getAllHazards().ToList();
                //inds.Insert(0, new SafetyHazardListViewModel() { Id = 0, Industry_Title = "Select" });

                List<SelectListItem> names = new List<SelectListItem>();
                foreach (var cnt in hazards)
                {
                    names.Add(new SelectListItem { Text = cnt.HazardDesc, Value = cnt.HazardID.ToString() });
                }

                ViewBag.HazardList = names;

                var ppes = safetyService.getAllPPEs().ToList();
                //inds.Insert(0, new SafetyHazardListViewModel() { Id = 0, Industry_Title = "Select" });

                List<SelectListItem> pplist = new List<SelectListItem>();
                foreach (var ppe in ppes)
                {
                    pplist.Add(new SelectListItem { Text = ppe.PPE_Desc, Value = ppe.PPEID.ToString() });
                }

                ViewBag.PPEList = pplist;

                var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID != 4 && a.GroupID != 5 && a.GroupID != 7).OrderBy(x => x.FirstName).ToList();
                //inds.Insert(0, new SafetyHazardListViewModel() { Id = 0, Industry_Title = "Select" });

                List<SelectListItem> emplist = new List<SelectListItem>();
                foreach (var emp in emps)
                {
                    emplist.Add(new SelectListItem { Text = (emp.FirstName +" "+ emp.LastName), Value = emp.UserID.ToString() });
                }

                ViewBag.WorkerList = emplist;

                var project = projectService.getAllProjects().Where(a => a.Project_Status_ID == 1 || a.Project_Status_ID == 2 || a.Project_Status_ID == 4).ToList();

                if (project == null)
                {

                    project.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
                    ViewBag.ProjectTitle = new SelectList(project, "ProjectID", "ProjectName");
                }
                else
                {
                    ViewBag.ProjectTitle = new SelectList(project, "ProjectName", "ProjectName");
                }
            }
            catch (Exception ex)
            {
                logger.Debug("Claim Report Error:");
                logger.Debug(ex.Message);
                logger.Debug(ex.StackTrace);
            }

            return View();
        }

        // POST: Safety/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SafetyMasterViewModel eng_safety_master, List<int> hazardList, List<int> ppeList, List<int> workerList)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(eng_safety_master.RepDate))
                    eng_safety_master.RepDate = DateTime.ParseExact(eng_safety_master.RepDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_safety_master.RepTime))
                    eng_safety_master.RepTime = DateTime.ParseExact(eng_safety_master.RepTime, "h:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");

                eng_safety_master.SubmittedBy = AppSession.GetCurrentUserId();
                eng_safety_master.CreatedDate = DateTime.Now;

                var result = safetyService.CreateSafety(eng_safety_master, hazardList, ppeList, workerList);

                if (result > 0)
                    return getSuccessfulOperation();
                else
                    return getFailedOperation();
            }

            return View(eng_safety_master);
        }

        // GET: Safety/Edit/5
        public ActionResult Edit(int? id)
        {

            var safety = safetyService.getSafety(id ?? default(int));

            if (safety == null)
            {
                return getFailedOperation();
            }

            var hazards = safetyService.getAllHazards().ToList();
            List<SelectListItem> names = new List<SelectListItem>();
            foreach (var cnt in hazards)
            {
                names.Add(new SelectListItem { Text = cnt.HazardDesc, Value = cnt.HazardID.ToString() });
            }
            ViewBag.HazardList = names;

            var ppes = safetyService.getAllPPEs().ToList();  
            List<SelectListItem> pplist = new List<SelectListItem>();
            foreach (var ppe in ppes)
            {
                pplist.Add(new SelectListItem { Text = ppe.PPE_Desc, Value = ppe.PPEID.ToString() });
            }
            ViewBag.PPEList = pplist;

            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID != 4 && a.GroupID != 5 && a.GroupID != 7).OrderBy(x=>x.FirstName).ToList();
            List<SelectListItem> emplist = new List<SelectListItem>();
            foreach (var emp in emps)
            {
                emplist.Add(new SelectListItem { Text = (emp.FirstName + " " + emp.LastName), Value = emp.UserID.ToString() });
            }
            ViewBag.WorkerList = emplist;

            var project = projectService.getAllProjects().Where(a => a.Project_Status_ID == 1 || a.Project_Status_ID == 2 || a.Project_Status_ID == 4).ToList();
            ViewBag.ProjectTitle = new SelectList(project, "ProjectName", "ProjectName", safety.ProjectTitle);

            return View(safety);
        }

        // POST: Safety/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SafetyMasterViewModel eng_safety_master, List<int> hazardList, List<int> ppeList, List<int> workerList)
        {
            if (ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(eng_safety_master.RepDate))
                    eng_safety_master.RepDate = DateTime.ParseExact(eng_safety_master.RepDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_safety_master.RepTime))
                    eng_safety_master.RepTime = DateTime.ParseExact(eng_safety_master.RepTime, "h:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");

                eng_safety_master.UpdatedBy = AppSession.GetCurrentUserId();
                eng_safety_master.UpdatedDate = DateTime.Now;

                var result = safetyService.SaveSafety(eng_safety_master, hazardList, ppeList, workerList);

                if (result > 0)
                    return getSuccessfulOperation();
                else
                    return getFailedOperation();

            }
            return View(eng_safety_master);
        }

        // GET: Safety/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_safety_master eng_safety_master = db.eng_safety_master.Find(id);
            if (eng_safety_master == null)
            {
                return HttpNotFound();
            }
            return View(eng_safety_master);
        }

        // POST: Safety/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = safetyService.DeleteSafety(id);

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
        public ActionResult SICreate()
        {
            var returnObject = new SafetyInspectionMasterViewModel();

            returnObject.Items = safetyService.getAllItems();       

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

            var mng = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 2).ToList();
            mng.Insert(0, new EmployeeViewModel() { FullName = "Select" });
            ViewBag.MNGList = new SelectList(mng, "FullName", "FullName");


            return View(returnObject);
        }


        [HttpPost]
        public ActionResult SICreate(SafetyInspectionMasterViewModel si)
        {
            int result = 0;

            if (!string.IsNullOrEmpty(si.SIDate))
                si.SIDate = DateTime.ParseExact(si.SIDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            if (!string.IsNullOrEmpty(si.ACDate1))
                si.ACDate1 = DateTime.ParseExact(si.ACDate1, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            if (!string.IsNullOrEmpty(si.ACDate2))
                si.ACDate2 = DateTime.ParseExact(si.ACDate2, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            if (!string.IsNullOrEmpty(si.ACDate3))
                si.ACDate3 = DateTime.ParseExact(si.ACDate3, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            if (!string.IsNullOrEmpty(si.ACDate4))
                si.ACDate4 = DateTime.ParseExact(si.ACDate4, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            if (!string.IsNullOrEmpty(si.ACDate5))
                si.ACDate5 = DateTime.ParseExact(si.ACDate5, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            if (!string.IsNullOrEmpty(si.ACDate6))
                si.ACDate6 = DateTime.ParseExact(si.ACDate6, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            if (!string.IsNullOrEmpty(si.ACDate7))
                si.ACDate7 = DateTime.ParseExact(si.ACDate7, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            if (!string.IsNullOrEmpty(si.ACDate8))
                si.ACDate8 = DateTime.ParseExact(si.ACDate8, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            if (!string.IsNullOrEmpty(si.ACDate9))
                si.ACDate9 = DateTime.ParseExact(si.ACDate9, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            if (!string.IsNullOrEmpty(si.ACDate10))
                si.ACDate10 = DateTime.ParseExact(si.ACDate10, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


            si.CreatedBy = AppSession.GetCurrentUserId();
            if (si.SAFINSID == 0)
            {
                si.CreatedBy = AppSession.GetCurrentUserId();
                result = safetyService.SICreate(si);
            }
            else
            {
                si.UpdatedBy = AppSession.GetCurrentUserId();
                result = safetyService.SIEdit(si);
            }


            return getSuccessfulOperation();
        }

        public ActionResult SIEdit(int? id)
        {
            var returnObject = safetyService.getSafetyInspectionMaster(id);

            returnObject.Items = safetyService.getAllItems();
                                   
            var project = projectService.getAllProjects().Where(a => a.Project_Status_ID == 1 || a.Project_Status_ID == 2 || a.Project_Status_ID == 4).ToList();

            if (project == null)
            {

                project.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName");
            }
            else
            {
                var projects = new SelectList(project, "ProjectID", "ProjectName");
                //projects.Where(a => a.Value == returnObject.ProjectID.ToString()).FirstOrDefault().Selected = true;

                //ViewBag.ProjectID = projects;
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName", returnObject.ProjectID);

            }            

            var mng = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 2).ToList();
            mng.Insert(0, new EmployeeViewModel() { FullName = "Select" });

            ViewBag.MNGList = new SelectList(mng, "FullName", "FullName");                      

            return View("~\\Views\\Safety\\SICreate.cshtml", returnObject);
        }


        [HttpGet]
        public ActionResult NewSICreate()
        {
            var returnObject = new NewSafetyInspectionViewModel();

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

            var mng = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 2).ToList();
            mng.Insert(0, new EmployeeViewModel() { FullName = "Select" });
            ViewBag.MNGList = new SelectList(mng, "FullName", "FullName");


            return View(returnObject);
        }

        [HttpGet]
        public ActionResult NewSIEdit(int? id)
        {
            //var returnObject = new NewSafetyInspectionViewModel();

            var safety = safetyService.NewSIEdit(id ?? default(int));

            if (safety == null)
            {
                return getFailedOperation();
            }

            var project = projectService.getAllProjects().Where(a => a.Project_Status_ID == 1 || a.Project_Status_ID == 2 || a.Project_Status_ID == 4).ToList();

            if (project == null)
            {

                project.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName");
            }
            else
            {
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName", safety.ProjectID);
            }

            var mng = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 2).ToList();
            mng.Insert(0, new EmployeeViewModel() { FullName = "Select" });
            ViewBag.MNGList = new SelectList(mng, "FullName", "FullName", safety.InspectedBy);


            return View(safety);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewSICreate(NewSafetyInspectionViewModel eng_safety_esh)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(eng_safety_esh.InspectionDate))
                    eng_safety_esh.InspectionDate = DateTime.ParseExact(eng_safety_esh.InspectionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                
               
                string halfPath = "/images/SIFiles/" + AppSession.GetCurrentUserId() + "/";
                var result = safetyService.NewSICreate(eng_safety_esh, Path.Combine(Server.MapPath("~/images/SIFiles/" + AppSession.GetCurrentUserId() + "/")), halfPath);
                if (result > 0)
                    return getSuccessfulOperation();
                else
                    return getFailedOperation();
            }

            
            return View(eng_safety_esh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewSIEdit(NewSafetyInspectionViewModel eng_safety_esh)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(eng_safety_esh.InspectionDate))
                    eng_safety_esh.InspectionDate = DateTime.ParseExact(eng_safety_esh.InspectionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");



                string halfPath = "/images/SIFiles/" + AppSession.GetCurrentUserId() + "/";
                var result = safetyService.NewSIEdit(eng_safety_esh, Path.Combine(Server.MapPath("~/images/SIFiles/" + AppSession.GetCurrentUserId() + "/")), halfPath);
                if (result > 0)
                    return getSuccessfulOperation();
                else
                    return getFailedOperation();
            }

            return View(eng_safety_esh);
        }

        public ActionResult NewSIView(int? pid, string dt, string location)
        {
            if (pid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  eng_quote_master eng_quote_master = db.eng_quote_master.Find(id);
            var safety = safetyService.getAllNewSafetyInspections().Where(a=>a.ProjectID == pid && a.ProjectLocation == location && a.InspectionDate == Convert.ToDateTime(dt).Date.ToString()).ToList();
            
            if (safety == null)
            {
                return HttpNotFound();
            }

            return View(safety);
        }


        // POST: ProjectReport/Delete/5
        [HttpPost, ActionName("DeleteSafetyFile")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSafetyFile(int id)
        {
            var result = safetyService.deleteSafetyFile(id);

            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
