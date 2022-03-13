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
using iTextSharp.text;
using Eng360Web.Models.Security;

namespace Eng360Web
{
    [AccessDeniedAuthorize]
    public class PaymentReceivableController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly IPaymentServices paymentService;
        private readonly IClientServices clientService;
        private readonly IQuoteServices quoteService;
        private readonly IInvoiceServices invoiceService;

        public PaymentReceivableController(IPaymentServices _paymentService, IClientServices _clientService, IQuoteServices _quoteService, IInvoiceServices _invoiceService)
        {
            paymentService = _paymentService;
            clientService = _clientService;
            quoteService = _quoteService;
            invoiceService = _invoiceService;

        }

        // GET: Payment
        public ActionResult Index()
        {
            // var eng_pymt_receivable = db.eng_pymt_receivable.Include(e => e.eng_client_master);

            var receivables = paymentService.getAllReceivables().ToList();

            foreach (var rec in receivables)
            {
                if (rec.InvoiceID != null)
                    rec.ClientName = invoiceService.getInvoice(rec.InvoiceID ?? default(int)).ClientOthers;
            }

            return View(receivables);

        }

        // GET: Payment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  eng_quote_master eng_quote_master = db.eng_quote_master.Find(id);
            var receivable = paymentService.getReceivable(id ?? default(int));
            if (receivable == null)
            {
                return HttpNotFound();
            }
            return View(receivable);
        }

        // GET: Payment/Create
        public ActionResult Create()
        {

            var clients = invoiceService.getAllInvoices().Where(a => a.isFullyPaid == 0 && a.ClientID != 0).Select(b => new { b.ClientID, b.eng_quote_master.eng_client_master.Company_Name }).Distinct().ToList();

            //var clientlist = quoteService.getAllQuotes().Where(a => a.Is_invoice_released == 1 && a.isFullyPaid != 1).ToList().Select(b => new { b.ClientID, b.eng_client_master.Company_Name }).Distinct().ToList();
            ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name");


            List<InvoiceMasterViewModel> invno = new List<InvoiceMasterViewModel>();
            foreach (var client in clients)
            {
                invno.AddRange(invoiceService.getAllClientInvoices(client.ClientID ?? default(int)));
            }

            invno.AddRange(invoiceService.getAllOtherInvoices());

            ViewBag.DummyNo = new SelectList(invno, "InvoiceType", "InvoiceNum");

            //List<QuoteViewModel> invno = new List<QuoteViewModel>();
            //foreach (var client in clients)
            //{
            //    invno.AddRange(quoteService.getAllClientInvoices(client.ClientID ?? default(int)));
            //}
            //ViewBag.DummyNo = new SelectList(invno, "ClientQuote", "InvoiceNo");




            // ViewBag.ClientID = new SelectList(clientService.getAllClients().Where(a=>a.IsActive == 1).ToList(), "ClientID", "Company_Name");
            ViewBag.Tr_status = new SelectList(paymentService.getAllPaymentStatus(), "Id", "PymtStatus");
            return View();
        }

        // POST: Payment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PaymentReceivableViewModel eng_pymt_receivable)
        {
            if (ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(eng_pymt_receivable.Tr_date))
                    eng_pymt_receivable.Tr_date = DateTime.ParseExact(eng_pymt_receivable.Tr_date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_pymt_receivable.Due_date))
                    eng_pymt_receivable.Due_date = DateTime.ParseExact(eng_pymt_receivable.Due_date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                eng_pymt_receivable.CreatedBy = AppSession.GetCurrentUserId();
                eng_pymt_receivable.CreatedDate = DateTime.Now;

                var result = paymentService.CreateReceivable(eng_pymt_receivable);
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
            }


            return View(eng_pymt_receivable);
        }

        // GET: Payment/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return getFailedOperation();
            }
            // eng_client_master eng_client_master = db.eng_client_master.Find(id);
            var receivable = paymentService.getReceivable(id ?? default(int));
            if (receivable == null)
            {
                return getFailedOperation();
            }
            ViewBag.ClientID = new SelectList(clientService.getAllClients().Where(a => a.IsActive == 1).ToList(), "ClientID", "Company_Name", receivable.ClientID);
            ViewBag.Tr_status = new SelectList(paymentService.getAllPaymentStatus(), "Id", "PymtStatus", receivable.Tr_status);

            //if (receivable.InvoiceType != null || receivable.InvoiceType != "")
            //{
            //    ViewBag.TotAmt = paymentService.GetInvoiceAmount(receivable.InvoiceNo, receivable.InvoiceType);
            //    ViewBag.AlreadyPaid = paymentService.getAllReceivables().Where(a => a.InvoiceNo == receivable.InvoiceNo && a.PymtID != receivable.PymtID).Sum(b => b.Amount);
            //}
            if (receivable.InvoiceID != null)
            {
                ViewBag.TotAmt = invoiceService.getInvoice(receivable.InvoiceID ?? default(int)).TotalInvoiceAmount;

                ViewBag.AlreadyPaid = paymentService.getAllReceivables().Where(a => a.InvoiceID == receivable.InvoiceID && a.PymtID != receivable.PymtID).Sum(b => b.Amount);
                if (receivable.ClientID == null)
                    receivable.ClientName = invoiceService.getInvoice(receivable.InvoiceID ?? default(int)).ClientOthers;
            }
            return View(receivable);


        }

        // POST: Payment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PaymentReceivableViewModel eng_pymt_receivable)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(eng_pymt_receivable.Tr_date))
                    eng_pymt_receivable.Tr_date = DateTime.ParseExact(eng_pymt_receivable.Tr_date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_pymt_receivable.Due_date))
                    eng_pymt_receivable.Due_date = DateTime.ParseExact(eng_pymt_receivable.Due_date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                eng_pymt_receivable.UpdatedBy = AppSession.GetCurrentUserId();
                eng_pymt_receivable.UpdatedDate = DateTime.Now;

                var result = paymentService.SaveReceivable(eng_pymt_receivable);

                if (result > 0)
                {

                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
            }

            return View(eng_pymt_receivable);
        }

        public ActionResult GetAlreadyPaid(string inv, string invfrom)
        {
            decimal invamount = (decimal)paymentService.GetInvoiceAmount(inv, invfrom);
            //   eng_product_master eng_product_master = db.eng_product_master.Find(id);
            decimal sumpaid = (decimal)paymentService.getAllReceivables().Where(a => a.InvoiceNo == inv).Sum(b => b.Amount);

            return Json(new
            {
                value = "OK",
                sumpaid = sumpaid,
                invamount = invamount
            });
        }


        public ActionResult GetInvoiceDetails(int inv)
        {
            decimal invamount = (decimal)invoiceService.getInvoice(inv).TotalInvoiceAmount;
            //   eng_product_master eng_product_master = db.eng_product_master.Find(id);
            decimal sumpaid = (decimal)paymentService.getAllReceivables().Where(a => a.InvoiceID == inv).Sum(b => b.Amount);

            return Json(new
            {
                value = "OK",
                sumpaid = sumpaid,
                invamount = invamount
            });
        }

        // GET: Payment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_pymt_receivable eng_pymt_receivable = db.eng_pymt_receivable.Find(id);
            if (eng_pymt_receivable == null)
            {
                return HttpNotFound();
            }
            return View(eng_pymt_receivable);
        }

        // POST: Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = paymentService.DeleteReceivable(id);

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
