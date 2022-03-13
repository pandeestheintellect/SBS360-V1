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
using System.IO;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.Utility;
using System.Globalization;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.Security;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class ProjectReportController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly IProjectReportService projectReportService;
        private readonly IClientServices clientService;
        private readonly IProjectServices projectService;
        private readonly IEmployeeServices employeeService;
        private readonly IUserServices userService;

        public ProjectReportController(IProjectReportService _projectReportService, IUserServices _userService,
            IClientServices _clientService, IProjectServices _projectService, IEmployeeServices _employeeService)
        {
            projectReportService = _projectReportService;
            clientService = _clientService;
            projectService = _projectService;
            employeeService = _employeeService;
            userService = _userService;

        }

        // GET: ProjectReport
        public ActionResult Index(int? id)
        {
           // var eng_project_report = db.eng_project_report.Include(e => e.eng_project_master).Include(e => e.eng_sys_task_status);

            var eng_project_report = projectReportService.getAllProjectReports().Where(a => a.ProjectID == id).ToList();
            if(eng_project_report == null)
            {
                return getFailedOperation();
            }
            var reports = eng_project_report.ToList();
            var output = Mapper.Map<List<ProjectReportViewModel>>(reports);

            foreach (var project in output)
            {

                project.ProjectName = project.eng_project_master.ProjectName;
                project.ClientName = project.eng_project_master.eng_quote_master.eng_client_master.Company_Name;

                var fn = userService.getAllUsers().Where(a=>a.UserID==project.CreatedBy).FirstOrDefault();
                project.SVName = fn.DisplayName;
                
            }



            return View(output);
        }

        public ActionResult ReportIndex()
        {
            var filter = new FilterViewModel();
            var projectreports = projectReportService.getAllProjectReports().ToList();

            foreach(var pjs in projectreports)
            {
                var fn = userService.getAllUsers().Where(a => a.UserID == pjs.CreatedBy).FirstOrDefault();
                pjs.SVName = fn.DisplayName;
            }

            var pnames = projectService.getAllProjects().ToList();
            pnames.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
            ViewBag.ProjectID = new SelectList(pnames, "ProjectID", "ProjectName");

            var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
            clients.Insert(0, new ClientViewModel() { ClientID = 0, Company_Name = "Select" });
            ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name");

            var svs = userService.getAllUsers().Where(a => a.IsActive == 1 && a.GroupID == 2 || a.GroupID == 7 || a.GroupID == 3).ToList();
            svs.Insert(0, new UserViewModel() { UserID = 0, DisplayName = "Select" });
            ViewBag.UserID = new SelectList(svs, "UserID", "DisplayName");

            filter.eng_project_report = projectreports;


            return View(filter);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportIndex(FilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                var fil = new FilterViewModel();
                var pnames = projectService.getAllProjects().ToList();
                pnames.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
                

                var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
                clients.Insert(0, new ClientViewModel() { ClientID = 0, Company_Name = "Select" });

                var svs = userService.getAllUsers().Where(a => a.IsActive == 1 && a.GroupID == 2 || a.GroupID == 7 || a.GroupID == 3).ToList();
                svs.Insert(0, new UserViewModel() { UserID = 0, DisplayName = "Select" });

                var prs = projectReportService.getFilterProjectReports(filter).ToList();

                foreach (var pjs in prs)
                {
                    var fn = userService.getAllUsers().Where(a => a.UserID == pjs.CreatedBy).FirstOrDefault();
                    pjs.SVName = fn.DisplayName;
                }

                fil.eng_project_report = prs;
                ViewBag.ProjectID = new SelectList(pnames, "ProjectID", "ProjectName", filter.ProjectID);
                ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name", filter.ClientID);
                ViewBag.UserID = new SelectList(svs, "UserID", "DisplayName", filter.UserID);
                fil.dateFrom = filter.dateFrom;
                fil.dateTo = filter.dateTo;
                fil.Month = filter.Month;
                                
                return View(fil);

            }
            return View();

        }



        // GET: ProjectReport/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_project_report eng_project_report = db.eng_project_report.Find(id);
            if (eng_project_report == null)
            {
                return HttpNotFound();
            }
            return View(eng_project_report);
        }

        // GET: ProjectReport/Create
        public ActionResult Create(int? id)
        {
            ProjectReportViewModel returnObject = new ProjectReportViewModel();

            if (returnObject == null)
            {

            }
            else
            {

                var project = projectReportService.getProject(id);
                returnObject.ProjectName = project.ProjectName;
                returnObject.ClientName = project.eng_quote_master.eng_client_master.Company_Name;
                returnObject.Location = project.eng_sys_location.LocationName;
                returnObject.ProjectID = project.ProjectID;
            }

            // ViewBag.ProjectID = new SelectList(db.eng_project_master, "ProjectID", "ProjectNo");
            ViewBag.TaskStatusID = new SelectList(db.eng_sys_task_status, "TaskStatusID", "TaskStatus");


            return View(returnObject);
        }

        // POST: ProjectReport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectReportViewModel eng_project_report)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(eng_project_report.ReportDate))
                    eng_project_report.ReportDate = DateTime.ParseExact(eng_project_report.ReportDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_project_report.Start_Date_Time))
                    eng_project_report.Start_Date_Time =
                      DateTime.ParseExact(eng_project_report.Start_Date_Time, "h:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");

                if (!string.IsNullOrEmpty(eng_project_report.End_Date_Time))
                    eng_project_report.End_Date_Time =
                      DateTime.ParseExact(eng_project_report.End_Date_Time, "h:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");

                eng_project_report.CreatedBy = AppSession.GetCurrentUserId();

                //eng_project_report.Start_Date_Time =
                //      DateTime.ParseExact(eng_project_report.Start_Date_Time, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");

                //if (!string.IsNullOrEmpty(eng_project_report.End_Date_Time))
                //    eng_project_report.End_Date_Time =
                //        DateTime.ParseExact(eng_project_report.End_Date_Time, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");

                string halfPath = "/images/UploadedFiles/" + AppSession.GetCurrentUserId() + "/";
                var result = projectReportService.CreateProjectReport(eng_project_report, Path.Combine(Server.MapPath("~/images/UploadedFiles/" + AppSession.GetCurrentUserId() + "/")), halfPath);
                if (result > 0)
                    return getSuccessfulOperation();
                else
                    return getFailedOperation();
            }

            ViewBag.ProjectID = new SelectList(db.eng_project_master, "ProjectID", "ProjectNo", eng_project_report.ProjectID);
            ViewBag.TaskStatusID = new SelectList(db.eng_sys_task_status, "TaskStatusID", "TaskStatus", eng_project_report.TaskStatusID);
            return View(eng_project_report);
        }

        // GET: ProjectReport/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectReportViewModel eng_project_report = projectReportService.getProrjectRPT(id);
            if (eng_project_report == null)
            {
                return HttpNotFound();
            }
            //  ViewBag.ProjectID = new SelectList(db.eng_project_master, "ProjectID", "ProjectNo", eng_project_report.ProjectID);
            ViewBag.TaskStatusID = new SelectList(projectReportService.getTaskStatus(), "TaskStatusID", "TaskStatus", eng_project_report.TaskStatusID);

            var project = projectReportService.getProject(eng_project_report.ProjectID);
            eng_project_report.ProjectName = project.ProjectName;
            eng_project_report.ClientName = project.eng_quote_master.eng_client_master.Company_Name;
            eng_project_report.Location = project.eng_sys_location.LocationName;
            

            return View(eng_project_report);
        }

        // POST: ProjectReport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectReportViewModel eng_project_report)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(eng_project_report).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");

                if (!string.IsNullOrEmpty(eng_project_report.Start_Date_Time))
                    eng_project_report.Start_Date_Time =
                      DateTime.ParseExact(eng_project_report.Start_Date_Time, "h:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");

                if (!string.IsNullOrEmpty(eng_project_report.End_Date_Time))
                    eng_project_report.End_Date_Time =
                      DateTime.ParseExact(eng_project_report.End_Date_Time, "h:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");


                //eng_project_report.Start_Date_Time =
                //         DateTime.ParseExact(eng_project_report.Start_Date_Time, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");

                //if (!string.IsNullOrEmpty(eng_project_report.End_Date_Time))
                //    eng_project_report.End_Date_Time =
                //        DateTime.ParseExact(eng_project_report.End_Date_Time, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");

                string halfPath = "/images/UploadedFiles/" + AppSession.GetCurrentUserId() + "/";
                var result = projectReportService.EditProjectReport(eng_project_report, Path.Combine(Server.MapPath("~/images/UploadedFiles/" + AppSession.GetCurrentUserId() + "/")), halfPath);
                if (result > 0)
                    return getSuccessfulOperation();
                else
                    return getFailedOperation();

            }
            return getFailedOperation();
            //ViewBag.ProjectID = new SelectList(db.eng_project_master, "ProjectID", "ProjectNo", eng_project_report.ProjectID);
            //ViewBag.TaskStatusID = new SelectList(db.eng_sys_task_status, "TaskStatusID", "TaskStatus", eng_project_report.TaskStatusID);
            //return View(eng_project_report);
        }

        // GET: ProjectReport/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_project_report eng_project_report = db.eng_project_report.Find(id);
            if (eng_project_report == null)
            {
                return HttpNotFound();
            }
            return View(eng_project_report);
        }

        // POST: ProjectReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = projectReportService.deleteFile(id);

            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }
        }

        // POST: ProjectReport/Delete/5
        [HttpPost, ActionName("DeletePR")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePRConfirmed(int id)
        {
            var result = projectReportService.DeleteProjectReport(id);

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
