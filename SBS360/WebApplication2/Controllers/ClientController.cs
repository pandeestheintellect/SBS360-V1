using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eng360Web.Models.Domain;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.ViewModel;
using AutoMapper;
using Eng360Web.Models.Utility;
using Eng360Web.Models.Security;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class ClientController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();
        private readonly IClientServices clientService;
        private readonly IEmployeeServices employeeService;
        

        public ClientController(IClientServices _clientService, IEmployeeServices _employeeService)
        {
            clientService = _clientService;
            employeeService = _employeeService;
            
        }

       
        // GET: Client
        public ActionResult Index()
        {
            //var eng_client_master = db.eng_client_master.Include(e => e.eng_address_master).Include(e => e.eng_sys_function).Include(e => e.eng_sys_industry);
            //return View(eng_client_master.ToList());
            return View(clientService.getAllClients().Where(a=>a.IsActive==1).ToList());
        }


        // GET: Client
        public ActionResult ReportIndex()
        {
            var filter = new FilterViewModel();
            var client = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();

            var clientname = clientService.getAllClients().Where(a=>a.IsActive == 1).Select(a => a.Company_Name).Distinct().ToList();
            clientname.Insert(0, "Select");

            ViewBag.Company_Name = new SelectList(clientname);

            filter.eng_client_master = client;

            return View(filter);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportIndex(FilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                var fil = new FilterViewModel();

                var clientname = clientService.getAllClients().Where(a => a.IsActive == 1).Select(a => a.Company_Name).Distinct().ToList();
                clientname.Insert(0, "Select");

                if (filter.Company_Name != "Select")
                {
                    var client = clientService.getAllClients().Where(a => a.IsActive == 1 && a.Company_Name == filter.Company_Name).ToList();
                    fil.eng_client_master = client;
                    ViewBag.Company_Name = new SelectList(clientname,filter.Company_Name);
                }
                else
                {
                    var client = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
                    fil.eng_client_master = client;
                    ViewBag.Company_Name = new SelectList(clientname);
                }

                return View(fil);

            }
            return View();

        }

        // GET: Client/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //eng_client_master eng_client_master = db.eng_client_master.Find(id);
            var client = clientService.getClient(id ?? default(int));
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Client/Create
        public ActionResult Create()
        {
            // ViewBag.AddressID = new SelectList(db.eng_address_master, "AddressID", "Email");

            var fns = clientService.getAllFunctions().ToList();
            fns.Insert(0, new FunctionViewModel() { Id = 0, Fn_Title = "Select" });
            
            var inds = clientService.getAllIndustries().ToList();
            inds.Insert(0, new IndustryViewModel() { Id = 0, Industry_Title = "Select" });
            
            ViewBag.FunctionalityID = new SelectList(fns, "Id", "Fn_Title");
            ViewBag.IndustryID = new SelectList(inds, "Id", "Industry_Title");
            return View();
        }

        // GET: Client/CreateContact
        public ActionResult CreateContact(int? id)
        {
            if (id == null)
            {
                ViewBag.Check = 0;
                return View();
            }
            else
            {
                ViewBag.Check = 1;
                return View(clientService.getContact(id));
            }
           
        }

        // POST: Client/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientViewModel eng_client_master, List<ClientContactViewModel> clientContact)
        {

            if (ModelState.IsValid)
            {
                //db.eng_quote_master.Add(eng_quote_master);
                //db.SaveChanges();
                //return RedirectToAction("Index");\
                eng_client_master.CreatedBy = AppSession.GetCurrentUserId();
                eng_client_master.CreatedDate = DateTime.Now;
                eng_client_master.IsActive = 1;

                if(eng_client_master.FunctionalityID == 0)
                {
                    eng_client_master.FunctionalityID = null;
                }
                if (eng_client_master.IndustryID == 0)
                {
                    eng_client_master.IndustryID = null;
                }

                var result = clientService.CreateClient(eng_client_master, clientContact);
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
            }
            ViewBag.AddressID = new SelectList(employeeService.getAddresses(), "AddressID", "Email", eng_client_master.AddressID);
            ViewBag.FunctionalityID = new SelectList(clientService.getAllFunctions(), "Id", "Fn_code", eng_client_master.FunctionalityID);
            ViewBag.IndustryID = new SelectList(clientService.getAllIndustries(), "Id", "Industry_Code", eng_client_master.IndustryID);
            return View(eng_client_master);
            
        }

        // GET: Client/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return getFailedOperation();
            }
            // eng_client_master eng_client_master = db.eng_client_master.Find(id);

            var fns = clientService.getAllFunctions().ToList();
            fns.Insert(0, new FunctionViewModel() { Id = 0, Fn_Title = "Select" });

            var inds = clientService.getAllIndustries().ToList();
            inds.Insert(0, new IndustryViewModel() { Id = 0, Industry_Title = "Select" });
           

            var client = clientService.getClient(id ?? default(int));
            if (client == null)
            {
                return getFailedOperation();
            }
            ViewBag.AddressID = new SelectList(employeeService.getAddresses(), "AddressID", "Email", client.AddressID);
            ViewBag.FunctionalityID = new SelectList(fns, "Id", "Fn_Title", client.FunctionalityID);
            ViewBag.IndustryID = new SelectList(inds, "Id", "Industry_Title", client.IndustryID);
            
            return View(client);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientViewModel eng_client_master, List<ClientContactViewModel> clientContact, List<int> deleted)
        {
            if (ModelState.IsValid)
            {
                eng_client_master.UpdatedBy = AppSession.GetCurrentUserId();
                eng_client_master.UpdatedDate = DateTime.Now;
                if (eng_client_master.FunctionalityID == 0)
                {
                    eng_client_master.FunctionalityID = null;
                }
                if (eng_client_master.IndustryID == 0)
                {
                    eng_client_master.IndustryID = null;
                }
                var result = clientService.SaveClient(eng_client_master, clientContact, deleted);

                if (result > 0)
                    return getSuccessfulOperation();
                else
                    return getFailedOperation();
            }
            return getFailedOperation();
            //ViewBag.AddressID = new SelectList(employeeService.getAddresses(), "AddressID", "Email", eng_client_master.AddressID);
            //ViewBag.FunctionalityID = new SelectList(clientService.getAllFunctions(), "Id", "Fn_code", eng_client_master.FunctionalityID);
            //ViewBag.IndustryID = new SelectList(clientService.getAllIndustries(), "Id", "Industry_Code", eng_client_master.IndustryID);
            //return View(eng_client_master);
        }

        // GET: Client/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_client_master eng_client_master = db.eng_client_master.Find(id);
            if (eng_client_master == null)
            {
                return HttpNotFound();
            }
            return View(eng_client_master);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            var result = clientService.DeleteClient(id);

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
