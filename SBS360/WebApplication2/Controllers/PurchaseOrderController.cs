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
using System.Globalization;
using Eng360Web.Models.Security;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class PurchaseOrderController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly IPoServices poService;
        private readonly ISupplierServices supplierService;


        public PurchaseOrderController(IPoServices _poService, ISupplierServices _supplierService)
        {
            poService = _poService;
            supplierService = _supplierService;
        }

        // GET: Po
        public ActionResult Index()
        {
            //return View(poService.getAllPos().ToList());

            var pos = poService.getAllPos().ToList();

            foreach (var po in pos)
            {
                double poamt = 0;

                foreach (var desc in po.eng_po_description)
                {
                    poamt = poamt + (double)desc.Quantity * (double)desc.PoPrice;
                }
                po.PoValue = (decimal)poamt;
            }



            return View(pos);

        }


        public ActionResult ReportIndex()
        {
            var filter = new FilterViewModel();
            var pos = poService.getAllPos().ToList();

            foreach (var po in pos)
            {
                double poamt = 0;

                foreach (var desc in po.eng_po_description)
                {
                    poamt = poamt + (double)desc.Quantity * (double)desc.PoPrice;
                }
                po.PoValue = (decimal)poamt;
            }

            var pnum = poService.getAllPos().Select(a => a.PoRefNum).Distinct().ToList();
            pnum.Insert(0, "Select");

            
            var supplier = supplierService.getAllSuppliers().Where(a => a.IsActive == 1).ToList();
            supplier.Insert(0, new SupplierViewModel() { SupplierID = 0, Company_Name = "Select" });
            ViewBag.SupplierID = new SelectList(supplier, "SupplierID", "Company_Name");

            ViewBag.PoRefNum = new SelectList(pnum);

            filter.eng_po_master = pos;

            return View(filter);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportIndex(FilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                var fil = new FilterViewModel();
                var ponum = poService.getAllPos().Select(a => a.PoRefNum).Distinct().ToList();
                ponum.Insert(0, "Select");



                var supplier = supplierService.getAllSuppliers().Where(a => a.IsActive == 1).ToList();
                supplier.Insert(0, new SupplierViewModel() { SupplierID = 0, Company_Name = "Select" });
                

                var pos = poService.getFilterPos(filter).ToList();

                foreach (var po in pos)
                {
                    double poamt = 0;

                    foreach (var desc in po.eng_po_description)
                    {
                        poamt = poamt + (double)desc.Quantity * (double)desc.PoPrice;
                    }
                    po.PoValue = (decimal)poamt;
                }



                fil.eng_po_master = pos;
                
                ViewBag.SupplierID = new SelectList(supplier, "SupplierID", "Company_Name", filter.SupplierID);
                fil.dateFrom = filter.dateFrom;
                fil.dateTo = filter.dateTo;
                fil.Month = filter.Month;

                ViewBag.PoRefNum = new SelectList(ponum, filter.PoRefNum);
                return View(fil);

            }
            return View();

        }



        // GET: Po/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // eng_po_master eng_po_master = db.eng_po_master.Find(id);
            var po = poService.getPo(id ?? default(int));
            if (po == null)
            {
                return HttpNotFound();
            }
            return View(po);



        }

        // GET: Po/Create
        public ActionResult Create()
        {
            ViewBag.OrderStatusID = new SelectList(poService.getStatus(), "StatusID", "QuoteStatus");

            var supplier = supplierService.getAllSuppliers().Where(a => a.IsActive == 1).ToList();

            ViewBag.SupplierID = new SelectList(supplier, "SupplierID", "Company_Name");
            PoViewModel po = new PoViewModel();
            return View(po);
        }

        // POST: Po/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PoViewModel eng_po_master, List<PoDescriptionViewModel> poDescription)
        {
            if (ModelState.IsValid)
            {
                //db.eng_po_master.Add(eng_po_master);
                //db.SaveChanges();
                if (!string.IsNullOrEmpty(eng_po_master.PoDate))
                    eng_po_master.PoDate = DateTime.ParseExact(eng_po_master.PoDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                eng_po_master.CreatedBy = AppSession.GetCurrentUserId();
                eng_po_master.CreatedDate = DateTime.Now;
                eng_po_master.isFullyPaid = 0;
               

                var result = poService.CreatePo(eng_po_master, poDescription);
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }

              }

            return View(eng_po_master);
        }

        // GET: Po/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return getFailedOperation();
            }
            //eng_quote_master eng_quote_master = db.eng_quote_master.Find(id);
            var po = poService.getPo(id ?? default(int));

            if (po == null)
            {
                return getFailedOperation();
            }
            int[] statusIds = { 6, 7, 8 };
            var status = poService.getStatus().Where(a => statusIds.Contains(a.StatusID)).ToList();
            ViewBag.OrderStatusID = new SelectList(status, "StatusID", "QuoteStatus", po.OrderStatusID);

            var suppliers = supplierService.getAllSuppliers();

            ViewBag.SupplierID = new SelectList(suppliers, "SupplierID", "Company_Name", po.SupplierID);

            

           
            return View(po);
        }

        // POST: Po/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PoViewModel eng_po_master, List<PoDescriptionViewModel> poDescription, List<int> deleted)
        {
            if (ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(eng_po_master.PoDate))
                    eng_po_master.PoDate = DateTime.ParseExact(eng_po_master.PoDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                eng_po_master.UpdatedBy = AppSession.GetCurrentUserId();
                eng_po_master.UpdatedDate = DateTime.Now;
                var result = poService.SavePo(eng_po_master, poDescription, deleted);

                if (result >= 0)
                    return getSuccessfulOperation();
                else
                    return getFailedOperation();

            }
            return getFailedOperation();
        }

        // GET: Po/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_po_master eng_po_master = db.eng_po_master.Find(id);
            if (eng_po_master == null)
            {
                return HttpNotFound();
            }
            return View(eng_po_master);
        }

        // POST: Po/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            var result = poService.DeletePo(id);

            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }
            // return RedirectToAction("Index");

          
        }

        // GET: PO/CreatePoDesc
        public ActionResult CreatePoDesc(int? id)
        {
            if (id == null)
            {
                return View();
            }
            else
            {
                return View(poService.getDesc(id));
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
