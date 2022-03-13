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
using System.IO;
using System.Globalization;
using Eng360Web.Models.Security;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class ClaimController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly IClaimServices claimService;
        private readonly IEmployeeServices employeeService;
        private readonly IProjectServices projectService;
        private readonly IUserServices userService;

        public ClaimController(IClaimServices _claimService, IEmployeeServices _employeeService, IProjectServices _projectService, IUserServices _userService)
        {
            claimService = _claimService;
            employeeService = _employeeService;
            projectService = _projectService;
            userService = _userService;
        }

        // GET: Claim
        public ActionResult Index()
        {
            var userid = AppSession.GetCurrentUserId();
            var groupid = AppSession.GetCurrentUserGroup();

            var claims = claimService.getAllClaims(userid,groupid).ToList();

            foreach (var claim in claims)
            {
                double clamt = 0;

                foreach (var desc in claim.eng_claim_description)
                {
                    if(desc.GST == "YES")
                    clamt = clamt + (double)desc.RecpAmount + (double)desc.RecpAmount * (double) AppSession.GetCompanyDetail().GST *7 /100;
                    else
                    clamt = clamt + (double)desc.RecpAmount;

                }
                claim.ClaimAmount = (decimal)clamt;
            }

            return View(claims);
        }

        public ActionResult ReportIndex()
        {
            var filter = new FilterViewModel();

            var pnames = projectService.getAllProjects().ToList();
            pnames.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
            ViewBag.ProjectID = new SelectList(pnames, "ProjectID", "ProjectName");


            var fn = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
            fn.Insert(0, new EmployeeViewModel() { UserID = 0, FirstName = "Select" });
            ViewBag.UserID = new SelectList(fn, "UserID", "FullName");

            var claim = claimService.getAllReportClaims();
            
            filter.eng_claim = claim;
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

                var fn = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
                fn.Insert(0, new EmployeeViewModel() { UserID = 0, FullName = "Select" });

                

                var claims = claimService.getFilterClaims(filter).ToList();

                fil.eng_claim = claims;
                ViewBag.ProjectID = new SelectList(pnames, "ProjectID", "ProjectName", filter.ProjectID);
                ViewBag.UserID = new SelectList(fn, "UserID", "FullName", filter.UserID);
                               
                fil.dateFrom = filter.dateFrom;
                fil.dateTo = filter.dateTo;
                fil.Month = filter.Month;

                
                return View(fil);

            }
            return View();

        }



        // GET: Claim/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_claim eng_claim = db.eng_claim.Find(id);
            if (eng_claim == null)
            {
                return HttpNotFound();
            }
            return View(eng_claim);
        }

        // GET: Claim/Create
        public ActionResult Create()
        {
            

            var users = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
            ViewBag.UserID = new SelectList(users, "UserID", "FullName");

            var approvers = userService.getAllUsers().Where(a => a.IsActive == 1 && a.GroupID == 2).ToList();
            ViewBag.ApprovedBy = new SelectList(approvers, "UserID", "DisplayName");

            var project = projectService.getAllProjects().ToList();
            ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName");

            List<ClientContactViewModel> attention = new List<ClientContactViewModel>();

            ClaimViewModel claim = new ClaimViewModel();

           
            return View(claim);
        }

        // POST: Claim/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClaimViewModel eng_claim)
        {
           
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(eng_claim.SubmittedDate))
                    eng_claim.SubmittedDate = DateTime.ParseExact(eng_claim.SubmittedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_claim.ApprovedDate))
                    eng_claim.ApprovedDate = DateTime.ParseExact(eng_claim.ApprovedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                foreach (var desc in eng_claim.eng_claim_description)
                {
                    desc.RecpDate = DateTime.ParseExact(desc.RecpDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }

                string halfPath = "/images/ClaimFiles/" + AppSession.GetCurrentUserId() + "/";
                eng_claim.SubmittedBy = AppSession.GetCurrentUserId();
                eng_claim.Status = 0;

                var result = claimService.CreateClaim(eng_claim, Path.Combine(Server.MapPath("~/images/ClaimFiles/" + AppSession.GetCurrentUserId() + "/")), halfPath);
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }



            }

            
            return View(eng_claim);
        }

        // GET: Claim/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {

                return getFailedOperation();
            }

            var claim = claimService.getClaim(id ?? default(int));
            if (claim == null)
            {
                return getFailedOperation();
            }
            if (claim.ProjectID == null)
            {
                claim.ProjectID = 1;
            }
            ViewBag.UserID = new SelectList(employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList(), "UserID", "FullName", claim.UserID);
            ViewBag.ApprovedBy = new SelectList(userService.getAllUsers().Where(a => a.IsActive == 1 && a.GroupID == 2).ToList(), "UserID", "DisplayName", claim.ApprovedBy);
            ViewBag.ProjectID = new SelectList(projectService.getAllProjects().ToList(), "ProjectID", "ProjectName", claim.ProjectID);

            return View(claim);


        
        }

        // POST: Claim/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClaimViewModel eng_claim)
        {
            if (ModelState.IsValid)
            {
               
                foreach (var desc in eng_claim.eng_claim_description)
                {
                    desc.ClaimID = eng_claim.ClaimID;
                    desc.RecpDate = DateTime.ParseExact(desc.RecpDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }


                string halfPath = "/images/ClaimFiles/" + AppSession.GetCurrentUserId() + "/";

                if (AppSession.GetCurrentUserGroup() == 3)
                {
                    eng_claim.Status = 0;
                }
                var result = claimService.SaveClaim(eng_claim, Path.Combine(Server.MapPath("~/images/ClaimFiles/" + AppSession.GetCurrentUserId() + "/")), halfPath);
                if (result > 0)
                    return getSuccessfulOperation();
                else
                    return getFailedOperation();
            }

            return View(eng_claim);
        }


        // GET: Claim/CreateClaimDesc
        public ActionResult CreateClaimDesc(int? id)
        {
            if (id == null)
            {
                var types = claimService.getClaimType().ToList();
                ViewBag.ClaimTypeID = new SelectList(types, "ClaimTypeID", "ClaimType");
                return View();
            }
            else
            {
                var claimdesc = claimService.getClaimDesc(id);
                ViewBag.ClaimTypeID = new SelectList(claimService.getClaimType().ToList(), "ClaimTypeID", "ClaimType", claimdesc.ClaimTypeID);

                return View(claimdesc);
                
            }
        }



        // GET: Claim/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_claim eng_claim = db.eng_claim.Find(id);
            if (eng_claim == null)
            {
                return HttpNotFound();
            }
            return View(eng_claim);
        }

        // POST: Claim/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            eng_claim eng_claim = db.eng_claim.Find(id);
            db.eng_claim.Remove(eng_claim);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: ClaimFile/Delete/5
        [HttpPost, ActionName("DeleteFile")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteClaimFileConfirmed(int id)
        {
            var result = claimService.deleteClaimFile(id);

            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }
        }

        // POST: ClaimDescription/Delete/5
        [HttpPost, ActionName("DeleteClaimDesc")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteClaimDescConfirmed(int id)
        {
            var result = claimService.deleteClaimDesc(id);

            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }
        }

        // POST: ClaimDescription/Delete/5
        [HttpPost, ActionName("ApproveOrRejectClaim")]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveOrRejectClaim(int id, string remarks, int flag)
        {
            var claim = claimService.getClaim(id);

            if (flag==1)
            {
                claim.ApprovalRemarks = remarks;
                claim.ApprovedBy = AppSession.GetCurrentUserId();
                //claim.ApprovedDate = DateTime.Now;
                claim.ApprovedDate = DateTime.Now.ToString("MM/dd/yyyy");
                claim.Status = 1;
                claim.isFullyPaid = 0;
                    
            }

            if (flag == 2)
            {
                claim.RejectRemarks = remarks;
                claim.ApprovedBy = AppSession.GetCurrentUserId();
                //claim.ApprovedDate = DateTime.Now;;
                claim.ApprovedDate = DateTime.Now.ToString("MM/dd/yyyy");
                claim.Status = 2;

            }
            var result = claimService.ApproveRejectClaim(claim);

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
