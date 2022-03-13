using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eng360Web.Models.Domain;

namespace Eng360Web.Controllers
{
    public class AddressController : Controller
    {
        private Eng360Entities1 db = new Eng360Entities1();

        // GET: Address
        public ActionResult Index()
        {
            return View(db.eng_address_master.ToList());
        }

        // GET: Address/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_address_master eng_address_master = db.eng_address_master.Find(id);
            if (eng_address_master == null)
            {
                return HttpNotFound();
            }
            return View(eng_address_master);
        }

        // GET: Address/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Address/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AddressID,Email,Mobile,Tel,Web,Address1,Address2,City,Country,Postal_Code,Fax1,SkypeID,Remarks")] eng_address_master eng_address_master)
        {
            if (ModelState.IsValid)
            {
                db.eng_address_master.Add(eng_address_master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eng_address_master);
        }

        // GET: Address/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_address_master eng_address_master = db.eng_address_master.Find(id);
            if (eng_address_master == null)
            {
                return HttpNotFound();
            }
            return View(eng_address_master);
        }

        // POST: Address/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AddressID,Email,Mobile,Tel,Web,Address1,Address2,City,Country,Postal_Code,Fax1,SkypeID,Remarks")] eng_address_master eng_address_master)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eng_address_master).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eng_address_master);
        }

        // GET: Address/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_address_master eng_address_master = db.eng_address_master.Find(id);
            if (eng_address_master == null)
            {
                return HttpNotFound();
            }
            return View(eng_address_master);
        }

        // POST: Address/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            eng_address_master eng_address_master = db.eng_address_master.Find(id);
            db.eng_address_master.Remove(eng_address_master);
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
