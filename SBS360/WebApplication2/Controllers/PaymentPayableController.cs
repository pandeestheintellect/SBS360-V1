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
using Eng360Web.Controllers;
using Eng360Web.Models.Service.Interface;
using System.Globalization;
using Eng360Web.Models.Security;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class PaymentPayableController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly IPaymentServices paymentService;
        private readonly IPoServices poService;
        private readonly IEmployeeServices employeeService;
        private readonly IClaimServices claimService;

        public PaymentPayableController(IPaymentServices _paymentService, IPoServices _poService, IEmployeeServices _employeeService, IClaimServices _claimService)
        {
            paymentService = _paymentService;
            poService = _poService;
            employeeService = _employeeService;
            claimService = _claimService;

        }

        // GET: PaymentPayable
        public ActionResult Index()
        {
            var payable = paymentService.getAllPayables().ToList();
            return View(payable);
        }

        // GET: PaymentPayable/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //eng_pymt_payable eng_pymt_payable = db.eng_pymt_payable.Find(id);
            var payable = paymentService.getPayable(id ?? default(int));
            if (payable == null)
            {
                return HttpNotFound();
            }
            return View(payable);
        }

        // GET: PaymentPayable/Create
        public ActionResult Create()
        {
            var suppliers = poService.getAllPos().Where(a => a.isFullyPaid == 0).Select(b => new { b.SupplierID, b.eng_supplier_master.Company_Name }).Distinct().ToList();

            ViewBag.SupplierID = new SelectList(suppliers, "SupplierID", "Company_Name");

                List<PoViewModel> supno = new List<PoViewModel>();

            foreach (var supplier in suppliers)
            {

                supno.AddRange(poService.getAllPayPos(supplier.SupplierID ?? default(int)));

            }

            ViewBag.NPoID = new SelectList(supno, "SupplierPO", "PoRefNum");

            var dumEmps = claimService.getAllReportClaims().Where(a => a.isFullyPaid == 0 && a.PaymentSource == "Self").Select(b => new { b.UserID, b.eng_employee_profile.FirstName }).Distinct().ToList();

            ViewBag.DummyEmp = new SelectList(dumEmps, "UserID", "FirstName");


            List<ClaimViewModel> claimno = new List<ClaimViewModel>();

            foreach (var emp in dumEmps)
            {

                claimno.AddRange(claimService.getAllEmpClaims(emp.UserID ?? default(int)));

            }

            ViewBag.ClaimNo = new SelectList(claimno, "SVRemarks", "ClaimDisplayID");


            //ViewBag.SupplierID = new SelectList(supplierService.getAllSuppliers().Where(a=>a.IsActive == 1).ToList(), "SupplierID", "Company_name");

            ViewBag.EmpID = new SelectList(employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList(), "UserID", "FirstName");
            ViewBag.Tr_status = new SelectList(paymentService.getAllPaymentStatus(), "Id", "PymtStatus");
            ViewBag.TotAmt = 0;
            ViewBag.AlreadyPaid = 0;
            return View();
        }

        // POST: PaymentPayable/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PaymentPayableViewModel eng_pymt_payable)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(eng_pymt_payable.Tr_date))
                    eng_pymt_payable.Tr_date = DateTime.ParseExact(eng_pymt_payable.Tr_date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_pymt_payable.Due_date))
                    eng_pymt_payable.Due_date = DateTime.ParseExact(eng_pymt_payable.Due_date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                eng_pymt_payable.CreatedBy = AppSession.GetCurrentUserId();
                eng_pymt_payable.CreatedDate = DateTime.Now;

                var result = paymentService.CreatePayable(eng_pymt_payable);
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
            }

            
            return View(eng_pymt_payable);
        }

        public ActionResult GetAlreadyPaid(int? poid)
        {

            //   eng_product_master eng_product_master = db.eng_product_master.Find(id);
            decimal sumpaid = (decimal) paymentService.getAllPayables().Where(a=>a.PoID == poid).Sum(b=>b.Amount);

            return Json(new
            {
                value = "OK",
                sumpaid = sumpaid
            });
        }

        // GET: PaymentPayable/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return getFailedOperation();
            }
            var payable = paymentService.getPayable(id ?? default(int));
            if (payable == null)
            {
                return getFailedOperation();
            }
            //ViewBag.SupplierID = new SelectList(supplierService.getAllSuppliers().Where(a=>a.IsActive == 1).ToList(), "SupplierID", "Company_Name", payable.SupplierID);
            ViewBag.Tr_status = new SelectList(paymentService.getAllPaymentStatus(), "Id", "PymtStatus", payable.Tr_status);
            ViewBag.EmpID = new SelectList(employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList(), "UserID", "FirstName",payable.EmpID);
            return View(payable);
        }

        // POST: PaymentPayable/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PaymentPayableViewModel  eng_pymt_payable)
        {
            if (ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(eng_pymt_payable.Tr_date))
                    eng_pymt_payable.Tr_date = DateTime.ParseExact(eng_pymt_payable.Tr_date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_pymt_payable.Due_date))
                    eng_pymt_payable.Due_date = DateTime.ParseExact(eng_pymt_payable.Due_date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                eng_pymt_payable.UpdatedBy = AppSession.GetCurrentUserId();
                eng_pymt_payable.UpdatedDate = DateTime.Now;

                var result = paymentService.SavePayable(eng_pymt_payable);

                if (result > 0)
                {

                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
            }
            
            return View(eng_pymt_payable);
        }

        // GET: PaymentPayable/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_pymt_payable eng_pymt_payable = db.eng_pymt_payable.Find(id);
            if (eng_pymt_payable == null)
            {
                return HttpNotFound();
            }
            return View(eng_pymt_payable);
        }

        // POST: PaymentPayable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = paymentService.DeletePayable(id);

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
