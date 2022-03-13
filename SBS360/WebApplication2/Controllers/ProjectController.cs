using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eng360Web.Models.Domain;
using AutoMapper;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.Utility;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.ViewModel;
using System.Globalization;
using Eng360Web.Models.Security;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class ProjectController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly IProjectServices projectService;
        private readonly IClientServices clientService;
        private readonly IQuoteServices quoteService;

        public ProjectController(IProjectServices _projectService, IClientServices _clientService, IQuoteServices _quoteService)
        {
            projectService = _projectService;
            clientService = _clientService;
            quoteService = _quoteService;

        }



        // GET: Project
        public ActionResult Index()
        {
            // var eng_project_master = db.eng_project_master.Include(e => e.eng_quote_master).Include(e => e.eng_sys_location).Include(e => e.eng_sys_project_status);
            return View(projectService.getAllProjects().ToList());
        }

        public ActionResult ReportIndex()
        {
            var filter = new FilterViewModel();
            var projects = projectService.getAllProjects().ToList();

            //foreach (var quote in projects)
            //{
            //    double qtamt = 0;

            //    foreach (var desc in quote.eng_quote_description)
            //    {
            //        qtamt = qtamt + (double)desc.Quantity * (double)desc.QuotePrice;
            //    }
            //    quote.QuoteTotValue = (decimal)qtamt;
            //}

            var pnames = projectService.getAllProjects().ToList();
            pnames.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
            ViewBag.ProjectID = new SelectList(pnames, "ProjectID", "ProjectName");


            var status = projectService.getProjectStatus().ToList();
            status.Insert(0, new ProjectStatusViewModel() { ProjectStatusID = 0, ProjectStatus = "Select" });
            ViewBag.Project_Status_ID = new SelectList(status, "ProjectStatusID", "ProjectStatus");

            var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
            clients.Insert(0, new ClientViewModel() { ClientID = 0, Company_Name = "Select" });
            ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name");


            filter.eng_project_master = projects;


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



                var status = projectService.getProjectStatus().ToList();
                status.Insert(0, new ProjectStatusViewModel() { ProjectStatusID = 0, ProjectStatus = "Select" });


                var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
                clients.Insert(0, new ClientViewModel() { ClientID = 0, Company_Name = "Select" });


                var projects = projectService.getFilterProjects(filter).ToList();

                //foreach (var quote in quotes)
                //{
                //    double qtamt = 0;

                //    foreach (var desc in quote.eng_quote_description)
                //    {
                //        qtamt = qtamt + (double)desc.Quantity * (double)desc.QuotePrice;
                //    }
                //    quote.QuoteTotValue = (decimal)qtamt;
                //}



                fil.eng_project_master = projects;
                ViewBag.Project_Status_ID = new SelectList(status, "ProjectStatusID", "ProjectStatus", filter.Project_Status_ID);

                ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name", filter.ClientID);
                fil.dateFrom = filter.dateFrom;
                fil.dateTo = filter.dateTo;
                fil.Month = filter.Year;
                ViewBag.ProjectID = new SelectList(pnames, "ProjectID", "ProjectName", filter.ProjectID);

                return View(fil);

            }
            return View();

        }




        //// GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return getFailedOperation();
            }
            //eng_project_master eng_project_master = db.eng_project_master.Find(id);
            var project = projectService.getProject(id ?? default(int));
            if (project == null)
            {
                return getFailedOperation();
            }

            ViewBag.QuotationID = new SelectList(quoteService.getAllQuotes(), "QuoteID", "QuoteRefNum", project.QuotationID);
            ViewBag.LocationId = new SelectList(projectService.getAllLocations(), "LocationId", "LocationName", project.LocationId);
            ViewBag.Project_Status_ID = new SelectList(projectService.getProjectStatus(), "ProjectStatusID", "ProjectStatus", project.Project_Status_ID);
            return View(project);
        }

        public ActionResult DetailsInv(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // eng_project_master eng_project_master = db.eng_project_master.Find(id);
            var project = projectService.getProject(id ?? default(int));
            var quote = quoteService.getQuote(project.QuotationID ?? default(int));
            if (quote == null)
            {
                return HttpNotFound();
            }
            quote.InvoiceNo = project.InvoiceNo;
            quote.InvoiceDate = project.InvoiceDate;
            quote.DoNo = project.DoNo;
            quote.DODate = project.DoDate;

            quote.eng_quote_description = quote.eng_quote_description.Where(a => a.ProjectID == id).ToList();
            return View("~/Views/Quote/DetailsInv.cshtml", quote);
        }

        public ActionResult DetailsDo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var project = projectService.getProject(id ?? default(int));
            var quote = quoteService.getQuote(project.QuotationID ?? default(int));
            if (quote == null)
            {
                return HttpNotFound();
            }

            quote.InvoiceNo = project.InvoiceNo;
            quote.InvoiceDate = project.InvoiceDate;
            quote.DoNo = project.DoNo;
            quote.DODate = project.DoDate;
            quote.eng_quote_description = quote.eng_quote_description.Where(a => a.ProjectID == id).ToList();
            return View("~/Views/Quote/DetailsDo.cshtml", quote);
        }
        // GET: Project/Create
        public ActionResult Create()
        {
            ViewBag.QutaionID = new SelectList(db.eng_quote_master, "QuoteID", "QuoteRefNum");
            ViewBag.LocationId = new SelectList(db.eng_sys_location, "LocationId", "LocationName");
            ViewBag.Project_Status_Id = new SelectList(db.eng_sys_project_status, "ProjectStatusID", "ProjectStatus");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,ProjectNo,ProjectName,LocationId,QuotationID,DoNo,Start_Date,End_Date,Key_Milestones,Service_Desc,Project_Status_ID,Payment_Status,Client_Acceptance_Status,Received_Amount,Pending_Amount")] eng_project_master eng_project_master)
        {
            if (ModelState.IsValid)
            {
                db.eng_project_master.Add(eng_project_master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QutaionID = new SelectList(db.eng_quote_master, "QuoteID", "QuoteRefNum", eng_project_master.QuotationID);
            ViewBag.LocationId = new SelectList(db.eng_sys_location, "LocationId", "LocationName", eng_project_master.LocationId);
            ViewBag.Project_Status_Id = new SelectList(db.eng_sys_project_status, "ProjectStatusID", "ProjectStatus", eng_project_master.Project_Status_ID);
            return View(eng_project_master);
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return getFailedOperation();
            }
            //eng_project_master eng_project_master = db.eng_project_master.Find(id);
            var project = projectService.getProject(id ?? default(int));
            if (project == null)
            {
                return getFailedOperation();
            }

            ViewBag.QuotationID = new SelectList(quoteService.getAllQuotes(), "QuoteID", "QuoteRefNum", project.QuotationID);
            ViewBag.LocationId = new SelectList(projectService.getAllLocations(), "LocationId", "LocationName", project.LocationId);
            ViewBag.Project_Status_ID = new SelectList(projectService.getProjectStatus(), "ProjectStatusID", "ProjectStatus", project.Project_Status_ID);
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectViewModel project)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(project.Start_Date))
                    project.Start_Date = DateTime.ParseExact(project.Start_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(project.End_Date))
                    project.End_Date = DateTime.ParseExact(project.End_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


                project.UpdatedBy = AppSession.GetCurrentUserId();
                project.UpdatedDate = DateTime.Now;
                var result = projectService.SaveProject(project);

                if (result > 0)
                    return getSuccessfulOperation();
                else
                    return getFailedOperation();

            }

            return getFailedOperation();
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_project_master eng_project_master = db.eng_project_master.Find(id);
            if (eng_project_master == null)
            {
                return HttpNotFound();
            }
            return View(eng_project_master);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            eng_project_master eng_project_master = db.eng_project_master.Find(id);
            db.eng_project_master.Remove(eng_project_master);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInvDoDate(string invDate, string doDate, int invReleased, int projectID, string typeChk)
        {
            var result = projectService.UpdateInvDoDate(invDate, doDate, invReleased, projectID, typeChk);

            if (result > 0)

                return getSuccessfulOperation();
            else
                return getFailedOperation();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getInvoiceDates(int id)
        {
            var cInv = projectService.getProject(id);

            return Json(new
            {
                value = "OK",
                invDate = Convert.ToDateTime(cInv.InvoiceDate).ToString(AppSettings.GetDateFormat().Replace("mm", "MM")),
                doDate = Convert.ToDateTime(cInv.DoDate).ToString(AppSettings.GetDateFormat().Replace("mm", "MM")),
                doReleased = cInv.eng_quote_master.Is_do_released,
                invReleased = cInv.eng_quote_master.Is_invoice_released
            });
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
