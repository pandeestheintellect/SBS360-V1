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
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.Security;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class CustomInvoiceController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();
        private readonly IProjectServices projectService;
        private readonly IClientServices clientService;
        private readonly IQuoteServices quoteService;

        public CustomInvoiceController(IProjectServices _projectService, IClientServices _clientService, IQuoteServices _quoteService)
        {
            projectService = _projectService;
            clientService = _clientService;
            quoteService = _quoteService;

        }


        // GET: CustomInvoice
        public ActionResult Index(int? id)
        {
            ViewBag.ProjectID = id;
            return View(projectService.getAllCustomInvoice(id));
        }

        // GET: CustomInvoice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_custom_invoice eng_custom_invoice = db.eng_custom_invoice.Find(id);
            if (eng_custom_invoice == null)
            {
                return HttpNotFound();
            }
            return View(eng_custom_invoice);
        }


        public ActionResult DetailsInv(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // eng_project_master eng_project_master = db.eng_project_master.Find(id);

            var inv = projectService.getCustomInvoice(id);

            var project = projectService.getProject(inv.ProjectID ?? default(int));
            var quote = quoteService.getQuote(project.QuotationID ?? default(int));
            // QuoteViewModel quote = new QuoteViewModel();
            if (quote == null)
            {
                return HttpNotFound();
            }
            quote.InvoiceNo = inv.InvoiceNo;
            quote.InvoiceDate = Convert.ToDateTime(inv.InvoiceDate);
            quote.DoNo = inv.DoNo;
            //quote.DODate = inv.DODate;

            quote.eng_quote_description = inv.eng_quote_description;
            return View("~/Views/Quote/DetailsInv.cshtml", quote);
        }


        public ActionResult DetailsDO(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // eng_project_master eng_project_master = db.eng_project_master.Find(id);

            var inv = projectService.getCustomInvoice(id);

            var project = projectService.getProject(inv.ProjectID ?? default(int));
            var quote = quoteService.getQuote(project.QuotationID ?? default(int));
            // QuoteViewModel quote = new QuoteViewModel();
            if (quote == null)
            {
                return HttpNotFound();
            }
            quote.InvoiceNo = inv.InvoiceNo;
            quote.InvoiceDate = Convert.ToDateTime(inv.InvoiceDate);
            quote.DoNo = inv.DoNo;
            //quote.DODate = inv.DODate;

            quote.eng_quote_description = inv.eng_quote_description;
            return View("~/Views/Quote/DetailsDo.cshtml", quote);
        }




        // GET: CustomInvoice/Create
        public ActionResult Create(int? id)
        {

            //id - project id 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var project = projectService.getProject(id ?? default(int)); //   db.eng_project_master.Find(id);

            var returnObject = new CustomInvoiceViewModel();

            returnObject.ProjectID = id;
            returnObject.ProjectNo = project.ProjectNo;
            returnObject.QuotationID = project.QuotationID;
            returnObject.QuoteNo = project.eng_quote_master.QuoteRefNum;
            returnObject.GTAX = project.eng_quote_master.GTAX;

            returnObject.Discount = project.eng_quote_master.Discount;
            returnObject.eng_quote_description = project.eng_quote_master.eng_quote_description.Where(a => a.ProjectID == id).ToList();
            decimal amt = 0;
            decimal invamt = 0;

            if (project.eng_quote_master.Discount != 0)
            {
                foreach (var desc in project.eng_quote_master.eng_quote_description.Where(a => a.QuoteID == project.QuotationID).ToList())
                {
                    amt = amt + (decimal)desc.Quantity * (decimal)desc.QuotePrice;
                }

                amt = amt - (decimal) project.eng_quote_master.Discount;

                // returnObject.QuoteFinalAmount = project.eng_quote_master.FinalAmount;

                returnObject.QuoteFinalAmount = amt;

                var quoteObj = projectService.getAllCustomQuoteInvoice(project.QuotationID);

                foreach (var inv in quoteObj)
                {
                    var custdesc = projectService.getCustomInvoice(inv.InvoiceID);
                    foreach (var custdet in custdesc.eng_quote_description)
                    {
                        invamt = invamt + (decimal)custdet.Quantity * (decimal)custdet.QuotePrice;
                    }
                }

                returnObject.invAmt = invamt;

            }
            else
            {
                
                foreach (var desc in project.eng_quote_master.eng_quote_description.Where(a => a.ProjectID == id).ToList())
                {
                    amt = amt + (decimal)desc.Quantity * (decimal)desc.QuotePrice;
                }

                returnObject.prjAmt = amt;

                var invObj = projectService.getAllCustomInvoice(id);

                foreach (var inv in invObj)
                {
                    var custdesc = projectService.getCustomInvoice(inv.InvoiceID);
                    foreach (var custdet in custdesc.eng_quote_description)
                    {
                        invamt = invamt + (decimal)custdet.Quantity * (decimal)custdet.QuotePrice;
                    }
                }

                returnObject.invAmt = invamt;
            }
            return View(returnObject);
        }

        // POST: CustomInvoice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int ProjectID, string InvoiceDate, int QuotationID, List<QuoteDescriptionViewModel> desc, List<int> deleted, decimal finalAmount)
        {
            if (ModelState.IsValid)
            {
                //db.eng_custom_invoice.Add(eng_custom_invoice);
                //db.SaveChanges();
                //return RedirectToAction("Index");

                var result = projectService.CreateProjectInvoice(ProjectID, InvoiceDate, QuotationID, desc, deleted, finalAmount);
                if (result > 0)
                    return getSuccessfulOperation();
                else
                    return getFailedOperation();
            }
            return getFailedOperation();

        }

        // GET: CustomInvoice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_custom_invoice eng_custom_invoice = db.eng_custom_invoice.Find(id);
            if (eng_custom_invoice == null)
            {
                return HttpNotFound();
            }
            return View(eng_custom_invoice);
        }

        // POST: CustomInvoice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceID,InvoiceDate,ProjectID,QuotationID,InvoiceNo,DoNo,DODate")] eng_custom_invoice eng_custom_invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eng_custom_invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eng_custom_invoice);
        }

        // GET: CustomInvoice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_custom_invoice eng_custom_invoice = db.eng_custom_invoice.Find(id);
            if (eng_custom_invoice == null)
            {
                return HttpNotFound();
            }
            return View(eng_custom_invoice);
        }

        // POST: CustomInvoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = projectService.DeleteCustomInvoice(id);

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
