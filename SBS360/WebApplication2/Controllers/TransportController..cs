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
using System.Globalization;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class TransportController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly ITransportServices transportService;

        public TransportController(ITransportServices _transportService)
        {
            transportService = _transportService;
        }

        // GET: Transport
        public ActionResult Index()
        {
            return View(transportService.getAllTransports().Where(a => a.IsActive == 1).ToList());
        }


        [ValidateAntiForgeryToken]
        public ActionResult getAllTransports()
        {
            var transports = transportService.getAllTransports().Where(a => a.IsActive == 1).ToList();
            return Json(transports);

        }


        // GET: Transport
        public ActionResult ReportIndex()
        {
            var filter = new FilterViewModel();
            var transport = transportService.getAllTransports().ToList();

            //var pn = transportService.getAllTransports().Select(a => a.Transport_Name).Distinct().ToList();
            //pn.Insert(0, "Select");

            //var pcn = transportService.getAllTransports().Select(a => a.Transport_Company_Name).Distinct().ToList();
            //pcn.Insert(0, "Select");

            //var pc = transportService.getAllTransports().Select(a => a.Transport_Code).Distinct().ToList();
            //pc.Insert(0, "Select");


            //ViewBag.Transport_Name = new SelectList(pn);
            //ViewBag.Transport_Company_Name = new SelectList(pcn);
            //ViewBag.Transport_Code = new SelectList(pc);
           
            //filter.eng_transport_master = transport;

            return View(filter);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportIndex(FilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                var fil = new FilterViewModel();
                //var pn = transportService.getAllTransports().Select(a => a.Transport_Name).Distinct().ToList();
                //pn.Insert(0, "Select");

                //var pcn = transportService.getAllTransports().Select(a => a.Transport_Company_Name).Distinct().ToList();
                //pcn.Insert(0, "Select");

                //var pc = transportService.getAllTransports().Select(a => a.Transport_Code).Distinct().ToList();
                //pc.Insert(0, "Select");

                //var transport = transportService.getFilterTransports(filter).ToList();
                //fil.eng_transport_master = transport;
                //ViewBag.Transport_Name = new SelectList(pn, fil.Transport_Name);
                //ViewBag.Transport_Company_Name = new SelectList(pcn, fil.Transport_Company_Name);
                //ViewBag.Transport_Code = new SelectList(pc, fil.Transport_Code);


                return View(fil);

            }
            return View();
        }



        // GET: Transport/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // eng_transport_master eng_transport_master = db.eng_transport_master.Find(id);
            var transport = transportService.getTransport(id ?? default(int));
            if (transport == null)
            {
                return HttpNotFound();
            }
            return View(transport);
        }

        // GET: Transport/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TransportViewModel eng_transport_master)
        {
            if (ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(eng_transport_master.COE_Issue_Date))
                    eng_transport_master.COE_Issue_Date = DateTime.ParseExact(eng_transport_master.COE_Issue_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_transport_master.COE_Expiry_Date))
                    eng_transport_master.COE_Expiry_Date = DateTime.ParseExact(eng_transport_master.COE_Expiry_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_transport_master.RoadTax_Issue_Date))
                    eng_transport_master.RoadTax_Issue_Date = DateTime.ParseExact(eng_transport_master.RoadTax_Issue_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_transport_master.RoadTax_Expiry_Date))
                    eng_transport_master.RoadTax_Expiry_Date = DateTime.ParseExact(eng_transport_master.RoadTax_Expiry_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_transport_master.Insurance_Issue_Date))
                    eng_transport_master.Insurance_Issue_Date = DateTime.ParseExact(eng_transport_master.Insurance_Issue_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_transport_master.Insurance_Expiry_Date))
                    eng_transport_master.Insurance_Expiry_Date = DateTime.ParseExact(eng_transport_master.Insurance_Expiry_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_transport_master.Vehicle_Inspection_Date))
                    eng_transport_master.Vehicle_Inspection_Date = DateTime.ParseExact(eng_transport_master.Vehicle_Inspection_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_transport_master.Inspection_Due_Date))
                    eng_transport_master.Inspection_Due_Date = DateTime.ParseExact(eng_transport_master.Inspection_Due_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                
                eng_transport_master.CreatedBy = AppSession.GetCurrentUserId();
                eng_transport_master.CreatedDate = DateTime.Now;
                eng_transport_master.IsActive = 1;

                var result = transportService.CreateTransport(eng_transport_master);
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }

              }

            return View(eng_transport_master);
        }

        // GET: Transport/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
         //   eng_transport_master eng_transport_master = db.eng_transport_master.Find(id);
            var transport = transportService.getTransport(id ?? default(int));
            if (transport == null)
            {
                return HttpNotFound();
            }
            return View(transport);
        }

        // POST: Transport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TransportViewModel eng_transport_master)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(eng_transport_master.COE_Issue_Date))
                    eng_transport_master.COE_Issue_Date = DateTime.ParseExact(eng_transport_master.COE_Issue_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_transport_master.COE_Expiry_Date))
                    eng_transport_master.COE_Expiry_Date = DateTime.ParseExact(eng_transport_master.COE_Expiry_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_transport_master.RoadTax_Issue_Date))
                    eng_transport_master.RoadTax_Issue_Date = DateTime.ParseExact(eng_transport_master.RoadTax_Issue_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_transport_master.RoadTax_Expiry_Date))
                    eng_transport_master.RoadTax_Expiry_Date = DateTime.ParseExact(eng_transport_master.RoadTax_Expiry_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_transport_master.Insurance_Issue_Date))
                    eng_transport_master.Insurance_Issue_Date = DateTime.ParseExact(eng_transport_master.Insurance_Issue_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_transport_master.Insurance_Expiry_Date))
                    eng_transport_master.Insurance_Expiry_Date = DateTime.ParseExact(eng_transport_master.Insurance_Expiry_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_transport_master.Vehicle_Inspection_Date))
                    eng_transport_master.Vehicle_Inspection_Date = DateTime.ParseExact(eng_transport_master.Vehicle_Inspection_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_transport_master.Inspection_Due_Date))
                    eng_transport_master.Inspection_Due_Date = DateTime.ParseExact(eng_transport_master.Inspection_Due_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                eng_transport_master.UpdatedBy = AppSession.GetCurrentUserId();
                eng_transport_master.UpdatedDate = DateTime.Now;

                var result = transportService.SaveTransport(eng_transport_master);

                if (result > 0)
                {

                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }

                
            }
            return View(eng_transport_master);
        }

        // GET: Transport/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_transport_master eng_transport_master = db.eng_transport_master.Find(id);
            if (eng_transport_master == null)
            {
                return HttpNotFound();
            }
            return View(eng_transport_master);
        }

        // POST: Transport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            var result = transportService.DeleteTransport(id);

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
