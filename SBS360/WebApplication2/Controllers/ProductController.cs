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

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class ProductController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly IProductServices productService;

        public ProductController(IProductServices _productService)
        {
            productService = _productService;
        }

        // GET: Product
        public ActionResult Index()
        {
            return View(productService.getAllProducts().Where(a => a.IsActive == 1).ToList());
        }


        [ValidateAntiForgeryToken]
        public ActionResult getAllProducts()
        {
            var products = productService.getAllProducts().Where(a => a.IsActive == 1).ToList();
            return Json(products);

        }


        // GET: Product
        public ActionResult ReportIndex()
        {
            var filter = new FilterViewModel();
            var product = productService.getAllProducts().ToList();

            var pn = productService.getAllProducts().Select(a => a.Product_Name).Distinct().ToList();
            pn.Insert(0, "Select");

            var pcn = productService.getAllProducts().Select(a => a.Product_Company_Name).Distinct().ToList();
            pcn.Insert(0, "Select");

            var pc = productService.getAllProducts().Select(a => a.Product_Code).Distinct().ToList();
            pc.Insert(0, "Select");


            ViewBag.Product_Name = new SelectList(pn);
            ViewBag.Product_Company_Name = new SelectList(pcn);
            ViewBag.Product_Code = new SelectList(pc);
           
            filter.eng_product_master = product;

            return View(filter);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportIndex(FilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                var fil = new FilterViewModel();
                var pn = productService.getAllProducts().Select(a => a.Product_Name).Distinct().ToList();
                pn.Insert(0, "Select");

                var pcn = productService.getAllProducts().Select(a => a.Product_Company_Name).Distinct().ToList();
                pcn.Insert(0, "Select");

                var pc = productService.getAllProducts().Select(a => a.Product_Code).Distinct().ToList();
                pc.Insert(0, "Select");

                var product = productService.getFilterProducts(filter).ToList();
                fil.eng_product_master = product;
                ViewBag.Product_Name = new SelectList(pn, fil.Product_Name);
                ViewBag.Product_Company_Name = new SelectList(pcn, fil.Product_Company_Name);
                ViewBag.Product_Code = new SelectList(pc, fil.Product_Code);


                return View(fil);

            }
            return View();
        }



        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // eng_product_master eng_product_master = db.eng_product_master.Find(id);
            var product = productService.getProduct(id ?? default(int));
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel eng_product_master)
        {
            if (ModelState.IsValid)
            {
                //db.eng_product_master.Add(eng_product_master);
                //db.SaveChanges();
                eng_product_master.CreatedBy = AppSession.GetCurrentUserId();
                eng_product_master.CreatedDate = DateTime.Now;
                eng_product_master.IsActive = 1;

                var result = productService.CreateProduct(eng_product_master);
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }

              }

            return View(eng_product_master);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
         //   eng_product_master eng_product_master = db.eng_product_master.Find(id);
            var product = productService.getProduct(id ?? default(int));
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel eng_product_master)
        {
            if (ModelState.IsValid)
            {
                eng_product_master.UpdatedBy = AppSession.GetCurrentUserId();
                eng_product_master.UpdatedDate = DateTime.Now;

                var result = productService.SaveProduct(eng_product_master);

                if (result > 0)
                {

                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }

                //db.Entry(eng_product_master).State = EntityState.Modified;
                //db.SaveChanges();

                
            }
            return View(eng_product_master);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_product_master eng_product_master = db.eng_product_master.Find(id);
            if (eng_product_master == null)
            {
                return HttpNotFound();
            }
            return View(eng_product_master);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            var result = productService.DeleteProduct(id);

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
