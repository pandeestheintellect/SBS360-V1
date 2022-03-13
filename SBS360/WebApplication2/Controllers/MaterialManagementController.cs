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
using Eng360Web.Models.Service.Interface;
using System.Globalization;
using Eng360Web.Models.Security;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class MaterialManagementController : SuperBaseController
    {
       
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly IMaterialManagementServices mmService;
        private readonly IEmployeeServices employeeService;
        private readonly IProductServices productService;
        private readonly ISupplierServices supplierService;
        private readonly IClientServices clientService;


        public MaterialManagementController(IMaterialManagementServices _mmService, IEmployeeServices _employeeService, IProductServices _productService, ISupplierServices _supplierService, IClientServices _clientService)
        {
            mmService = _mmService;
            employeeService = _employeeService;
            productService = _productService;
            supplierService = _supplierService;
            clientService = _clientService;
        }


        // GET: Store Master
        public ActionResult Index()
        {
            return View(mmService.getAllStores().ToList());
        }


        // GET: Inward
        public ActionResult IndexInward()
        {
            return View(mmService.getAllInwards().ToList());
        }

        // GET: Outward
        public ActionResult IndexOutward()
        {
            return View(mmService.getAllOutwards().ToList());
        }

        // GET: Stock
        public ActionResult IndexStock()
        {
            return View(mmService.getStock().ToList());
        }

        // GET: Stock
        public ActionResult IndexStockAdjust()
        {
            return View(mmService.getAllStockAdjusts().ToList());
        }

        public ActionResult ReportIndexInw()
        {
            var filter = new FilterViewModel();
            var inward = new List<MMInwardViewModel>();
            filter.eng_inward = inward;

            return View(filter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportIndexInw(FilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                var fil = new FilterViewModel();             

                var inw = mmService.getFilterInwardReports(filter).ToList();                
                fil.eng_inward = inw;                
                fil.dateFrom = filter.dateFrom;
                fil.dateTo = filter.dateTo;
                fil.MMType = filter.MMType;

                return View(fil);
            }
            return View();
        }

        public ActionResult ReportIndexOut()
        {
            var filter = new FilterViewModel();
            var outward = new List<MMOutwardViewModel>();
            filter.eng_outward = outward;

            return View(filter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportIndexOut(FilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                var fil = new FilterViewModel();

                var outw = mmService.getFilterOutwardReports(filter).ToList();
                fil.eng_outward = outw;
                fil.dateFrom = filter.dateFrom;
                fil.dateTo = filter.dateTo;
                fil.MMType = filter.MMType;

                return View(fil);
            }
            return View();
        }



        // GET: MaterialManagement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_store_master eng_store_master = db.eng_store_master.Find(id);
            if (eng_store_master == null)
            {
                return HttpNotFound();
            }
            return View(eng_store_master);
        }

        // GET: MaterialManagement/Create
        public ActionResult CreateStore()
        {
            return View();
        }

        // POST: MaterialManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStore(StoreMasterViewModel eng_store_master)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(eng_store_master.Start_Date))
                    eng_store_master.Start_Date = DateTime.ParseExact(eng_store_master.Start_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                eng_store_master.CreatedBy = AppSession.GetCurrentUserId();
                eng_store_master.CreatedDate = DateTime.Now;
                
                var result = mmService.CreateStore(eng_store_master);
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
            }

            return View(eng_store_master);
        }

        public ActionResult CreateInward()
        {
            
            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
            ViewBag.ReceivedBy = new SelectList(emps, "UserID", "FirstName");

            var suppliers = supplierService.getAllSuppliers().Where(a => a.IsActive == 1).ToList();
            suppliers.Insert(0, new SupplierViewModel() { SupplierID = 0, Company_Name = "Select" });
            ViewBag.SupplierID = new SelectList(suppliers, "SupplierID", "Company_Name");

            var stores = mmService.getAllStores().ToList();
            ViewBag.StoreID = new SelectList(stores, "StoreID", "Store_Name");
            
            return View();
        }

        // GET: Inward/Edit/5
        public ActionResult EditInward(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //   eng_product_master eng_product_master = db.eng_product_master.Find(id);
            var inw = mmService.getInward(id ?? default(int));
            if (inw == null)
            {
                return HttpNotFound();
            }

            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
            ViewBag.ReceivedBy = new SelectList(emps, "UserID", "FirstName", inw.ReceivedBy);

            var suppliers = supplierService.getAllSuppliers().Where(a => a.IsActive == 1).ToList();
            suppliers.Insert(0, new SupplierViewModel() { SupplierID = 0, Company_Name = "Select" });
            ViewBag.SupplierID = new SelectList(suppliers, "SupplierID", "Company_Name", inw.SupplierID);

            var stores = mmService.getAllStores().ToList();
            ViewBag.StoreID = new SelectList(stores, "StoreID", "Store_Name", inw.StoreID);
            
            return View(inw);
        }


        // GET: Inward/Edit/5
        public ActionResult ViewInward(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //   eng_product_master eng_product_master = db.eng_product_master.Find(id);
            var inw = mmService.getInward(id ?? default(int));
            if (inw == null)
            {
                return HttpNotFound();
            }

            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
            ViewBag.ReceivedBy = new SelectList(emps, "UserID", "FirstName", inw.ReceivedBy);

            var suppliers = supplierService.getAllSuppliers().Where(a => a.IsActive == 1).ToList();
            suppliers.Insert(0, new SupplierViewModel() { SupplierID = 0, Company_Name = "Select" });
            ViewBag.SupplierID = new SelectList(suppliers, "SupplierID", "Company_Name", inw.SupplierID);

            var stores = mmService.getAllStores().ToList();
            ViewBag.StoreID = new SelectList(stores, "StoreID", "Store_Name", inw.StoreID);

            return View(inw);
        }


        public ActionResult CreateStockAdjust()
        {

            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
            ViewBag.Stock_Taken_By = new SelectList(emps, "UserID", "FirstName");

           
            var stores = mmService.getAllStores().ToList();
            stores.Insert(0, new StoreMasterViewModel() { StoreID = 0, Store_Name = "Select" });
            ViewBag.StoreID = new SelectList(stores, "StoreID", "Store_Name");

            return View();
        }

        // GET: Inward/Edit/5
        public ActionResult ViewStockAdjust(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //   eng_product_master eng_product_master = db.eng_product_master.Find(id);
            var saj = mmService.getStockAdjust(id ?? default(int));
            if (saj == null)
            {
                return HttpNotFound();
            }

            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
            ViewBag.Stock_Taken_By = new SelectList(emps, "UserID", "FirstName", saj.Stock_Taken_By);

            var stores = mmService.getAllStores().ToList();            
            ViewBag.StoreID = new SelectList(stores, "StoreID", "Store_Name", saj.StoreID);

            return View(saj);
        }





        // POST: MM/CreateInward
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInward(MMInwardViewModel eng_inward, List<MMInwardDescViewModel> inwardDescription)
        {
            if (ModelState.IsValid)
            {
               
                if (!string.IsNullOrEmpty(eng_inward.Received_Date))
                    eng_inward.Received_Date = DateTime.ParseExact(eng_inward.Received_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_inward.Invoice_or_DO_Date))
                    eng_inward.Invoice_or_DO_Date = DateTime.ParseExact(eng_inward.Invoice_or_DO_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


                eng_inward.CreatedBy = AppSession.GetCurrentUserId();
                eng_inward.CreatedDate = DateTime.Now;
                                
                var result = mmService.CreateInward(eng_inward, inwardDescription);
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
            }

            
            return View(eng_inward);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInward(MMInwardViewModel eng_inward, List<MMInwardDescViewModel> inwardDescription)
        {
            if (ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(eng_inward.Invoice_or_DO_Date))
                    eng_inward.Invoice_or_DO_Date = DateTime.ParseExact(eng_inward.Invoice_or_DO_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


                eng_inward.UpdatedBy = AppSession.GetCurrentUserId();
                eng_inward.UpdatedDate = DateTime.Now;

                var result = mmService.SaveInward(eng_inward, inwardDescription);
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
            }


            return View(eng_inward);
        }




        public ActionResult CreateOutward()
        {

            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
            ViewBag.DeliveredBy = new SelectList(emps, "UserID", "FirstName");

            var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
            clients.Insert(0, new ClientViewModel() { ClientID = 0, Company_Name = "Select" });
            ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name");

            var stores = mmService.getAllStores().ToList();
            ViewBag.StoreID = new SelectList(stores, "StoreID", "Store_Name");

            return View();
        }

        public ActionResult EditOutward(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //   eng_product_master eng_product_master = db.eng_product_master.Find(id);
            var ouw = mmService.getOutward(id ?? default(int));
            if (ouw == null)
            {
                return HttpNotFound();
            }

            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
            ViewBag.DeliveredBy = new SelectList(emps, "UserID", "FirstName", ouw.DeliveredBy);

            var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
            clients.Insert(0, new ClientViewModel() { ClientID = 0, Company_Name = "Select" });
            ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name", ouw.ClientID);

            var stores = mmService.getAllStores().ToList();
            ViewBag.StoreID = new SelectList(stores, "StoreID", "Store_Name", ouw.StoreID);

            return View(ouw);
        }


        public ActionResult ViewOutward(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //   eng_product_master eng_product_master = db.eng_product_master.Find(id);
            var ouw = mmService.getOutward(id ?? default(int));
            if (ouw == null)
            {
                return HttpNotFound();
            }

            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
            ViewBag.DeliveredBy = new SelectList(emps, "UserID", "FirstName", ouw.DeliveredBy);

            var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
            clients.Insert(0, new ClientViewModel() { ClientID = 0, Company_Name = "Select" });
            ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name", ouw.ClientID);

            var stores = mmService.getAllStores().ToList();
            ViewBag.StoreID = new SelectList(stores, "StoreID", "Store_Name", ouw.StoreID);

            return View(ouw);
        }



        // POST: MM/CreateInward
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOutward(MMOutwardViewModel eng_outward, List<MMOutwardDescViewModel> outwardDescription)
        {
            if (ModelState.IsValid)
            {
               

                if (!string.IsNullOrEmpty(eng_outward.Delivery_Date))
                    eng_outward.Delivery_Date = DateTime.ParseExact(eng_outward.Delivery_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_outward.DO_Date))
                    eng_outward.DO_Date = DateTime.ParseExact(eng_outward.DO_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


                eng_outward.CreatedBy = AppSession.GetCurrentUserId();
                eng_outward.CreatedDate = DateTime.Now;

                var result = mmService.CreateOutward(eng_outward, outwardDescription);
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
            }


            return View(eng_outward);
        }


        // POST: MM/CreateInward
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOutward(MMOutwardViewModel eng_outward, List<MMOutwardDescViewModel> outwardDescription)
        {
            if (ModelState.IsValid)
            {


                if (!string.IsNullOrEmpty(eng_outward.DO_Date))
                    eng_outward.DO_Date = DateTime.ParseExact(eng_outward.DO_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


                eng_outward.UpdatedBy = AppSession.GetCurrentUserId();
                eng_outward.UpdatedDate = DateTime.Now;

                var result = mmService.SaveOutward(eng_outward, outwardDescription);
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
            }


            return View(eng_outward);
        }


        // GET: MaterialManagement/Edit/5
        public ActionResult EditStore(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //   eng_product_master eng_product_master = db.eng_product_master.Find(id);
            var store = mmService.getStore(id ?? default(int));
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // POST: MaterialManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStore(StoreMasterViewModel eng_store_master)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(eng_store_master.Start_Date))
                    eng_store_master.Start_Date = DateTime.ParseExact(eng_store_master.Start_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                eng_store_master.UpdatedBy = AppSession.GetCurrentUserId();
                eng_store_master.UpdatedDate = DateTime.Now;

                var result = mmService.SaveStore(eng_store_master);

                if (result > 0)
                {

                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
            }
            return View(eng_store_master);
        }

        public ActionResult GetCurrentStock(int? pid, int? storeid)
        {
           
            //   eng_product_master eng_product_master = db.eng_product_master.Find(id);
            int storecnt = mmService.GetCurrentStock(pid ?? default(int), storeid ?? default(int));
            
            return Json(new
            {
                value = "OK",
                stockcnt = storecnt
            });
        }

        // POST: MM/CreateInward
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStockAdjust(MMStockAdjustViewModel eng_mm_stockadj_master)
        {
            if (ModelState.IsValid)
            {


                if (!string.IsNullOrEmpty(eng_mm_stockadj_master.Stock_Taking_Date))
                    eng_mm_stockadj_master.Stock_Taking_Date = DateTime.ParseExact(eng_mm_stockadj_master.Stock_Taking_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


                if (!string.IsNullOrEmpty(eng_mm_stockadj_master.Adj_Ref_Date))
                    eng_mm_stockadj_master.Adj_Ref_Date = DateTime.ParseExact(eng_mm_stockadj_master.Adj_Ref_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


                eng_mm_stockadj_master.CreatedBy = AppSession.GetCurrentUserId();
                eng_mm_stockadj_master.CreatedDate = DateTime.Now;

                var result = mmService.CreateStockAdjust(eng_mm_stockadj_master);
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
            }


            return View(eng_mm_stockadj_master);
        }



        // GET: MaterialManagement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_store_master eng_store_master = db.eng_store_master.Find(id);
            if (eng_store_master == null)
            {
                return HttpNotFound();
            }
            return View(eng_store_master);
        }

        // POST: MaterialManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            eng_store_master eng_store_master = db.eng_store_master.Find(id);
            db.eng_store_master.Remove(eng_store_master);
            db.SaveChanges();
            return RedirectToAction("Index");
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
