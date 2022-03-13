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
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.Utility;
using Eng360Web.Models.Security;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class SupplierController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly ISupplierServices supplierService;
        private readonly IEmployeeServices employeeService;
        private readonly IClientServices clientService;

        public SupplierController(ISupplierServices _supplierService, IEmployeeServices _employeeService, IClientServices _clientService)
        {
            supplierService = _supplierService;
            employeeService = _employeeService;
            clientService = _clientService;
        }

        // GET: Supplier
        public ActionResult Index()
        {
            return View(supplierService.getAllSuppliers().Where(a => a.IsActive == 1).ToList());
        }


        // GET: Supplier
        public ActionResult ReportIndex()
        {
            var filter = new FilterViewModel();
            var supplier = supplierService.getAllSuppliers().Where(a => a.IsActive == 1).ToList();

            var suppname = supplierService.getAllSuppliers().Where(a => a.IsActive == 1).Select(a => a.Company_Name).Distinct().ToList();
            suppname.Insert(0, "Select");

            ViewBag.Company_Name = new SelectList(suppname);

            filter.eng_supplier_master = supplier;

            return View(filter);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportIndex(FilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                var fil = new FilterViewModel();

                var suppname = supplierService.getAllSuppliers().Where(a => a.IsActive == 1).Select(a => a.Company_Name).Distinct().ToList();
                suppname.Insert(0, "Select");

                if (filter.Company_Name != "Select")
                {
                    var supplier = supplierService.getAllSuppliers().Where(a => a.IsActive == 1 && a.Company_Name == filter.Company_Name).ToList();
                    fil.eng_supplier_master = supplier;
                    ViewBag.Company_Name = new SelectList(suppname, filter.Company_Name);
                }
                else
                {
                    var supplier = supplierService.getAllSuppliers().Where(a => a.IsActive == 1).ToList();
                    fil.eng_supplier_master = supplier;
                    ViewBag.Company_Name = new SelectList(suppname);
                }

                return View(fil);

            }
            return View();

        }


        // GET: Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //eng_supplier_master eng_supplier_master = db.eng_supplier_master.Find(id);
            var supplier = supplierService.getSupplier(id ?? default(int));
            if (supplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.IndustryID = new SelectList(clientService.getAllIndustries(), "Id", "Fn_title", supplier.IndustryID);
            ViewBag.AddressID = new SelectList(employeeService.getAddresses(), "AddressID", "Email", supplier.AddressID);
            return View(supplier);
        }

        // GET: Supplier/Create
        public ActionResult Create()
        {
            //var supplier = new SupplierViewModel();
            ViewBag.IndustryID = new SelectList(clientService.getAllIndustries(), "Id", "Industry_Title");
            return View();
        }

        // POST: Supplier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SupplierViewModel eng_supplier_master)
        {
           
                if (ModelState.IsValid)
                {
                    eng_supplier_master.CreatedBy = AppSession.GetCurrentUserId();
                    eng_supplier_master.CreatedDate = DateTime.Now;
                    eng_supplier_master.IsActive = 1;

                    var result = supplierService.CreateSupplier(eng_supplier_master);

                    if (result > 0)
                    {
                        return getSuccessfulOperation();
                    }
                    else
                    {
                        return getFailedOperation();
                    }
  
                }
            

            return View(eng_supplier_master);
        }

        // GET: Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  eng_supplier_master eng_supplier_master = db.eng_supplier_master.Find(id);
            var supplier = supplierService.getSupplier(id ?? default(int));
            if (supplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddressID = new SelectList(employeeService.getAddresses(), "AddressID", "Email", supplier.AddressID);
            ViewBag.IndustryID = new SelectList(clientService.getAllIndustries(), "Id", "Industry_Title", supplier.IndustryID);
            return View(supplier);
        }

        // POST: Supplier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SupplierViewModel eng_supplier_master)
        {
            if (ModelState.IsValid)
            {
                eng_supplier_master.UpdatedBy = AppSession.GetCurrentUserId();
                eng_supplier_master.UpdatedDate = DateTime.Now;

                var result = supplierService.SaveSupplier(eng_supplier_master);
                if (result > 0)
                {

                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
                
            }
            return View(eng_supplier_master);
        }

        // GET: Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_supplier_master eng_supplier_master = db.eng_supplier_master.Find(id);
            if (eng_supplier_master == null)
            {
                return HttpNotFound();
            }
            return View(eng_supplier_master);
        }

        // POST: Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            var result = supplierService.DeleteSupplier(id);

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
