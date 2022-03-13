using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eng360Web.Models.Domain;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.ViewModel;
using Eng360Web.Models.Utility;
using System.Globalization;
using Eng360Web.Models.Security;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class InvoiceController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly IInvoiceServices invoiceService;
        private readonly IQuoteServices quoteService;        
        private readonly IPaymentServices paymentService;
        private readonly IClientServices clientService;

        // GET: Invoice
        public InvoiceController(IInvoiceServices _invoiceService, IQuoteServices _quoteService,  IPaymentServices _paymentService, IClientServices _clientService)
        {
            quoteService = _quoteService;
            invoiceService = _invoiceService;
            paymentService = _paymentService;
            clientService = _clientService;

        }

        // GET: Invoice
        public ActionResult Index()
        {
            var invs = invoiceService.getAllInvoices().ToList();

            foreach (var inv in invs)
            {
                if (inv.ClientID > 0)
                {
                    var fn = clientService.getAllClients().Where(a => a.ClientID == inv.ClientID).FirstOrDefault();
                    inv.ClientName = fn.Company_Name;
                }
            }

            return View(invs);
        }

        // GET: Invoice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var inv = invoiceService.getInvoice(id ?? default(int));
            if (inv == null)
            {
                return HttpNotFound();
            }
            inv.eng_invoice_details = inv.eng_invoice_details.OrderBy(a => a.InvoiceDescID).ToList();
            if(inv.Discount == null)
            inv.Discount = 0;

            return View(inv);
        }

        // GET: Invoice/Create
        public ActionResult Create()
        {

            var clients = quoteService.getAllQuotes().Where(a => a.InvoiceFlag == 0 || a.InvoiceFlag == 1).Select(b => new { b.ClientID, b.eng_client_master.Company_Name }).Distinct().ToList();

            //var clientlist = quoteService.getAllQuotes().Where(a => a.Is_invoice_released == 1 && a.isFullyPaid != 1).ToList().Select(b => new { b.ClientID, b.eng_client_master.Company_Name }).Distinct().ToList();

            
            ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name");

            List<QuoteViewModel> quoteno = new List<QuoteViewModel>();

            foreach (var client in clients)
            {

                quoteno.AddRange(quoteService.getAllClientQuotes(client.ClientID ?? default(int)));

            }

            //var quotes = quoteService.getAllQuotes().Where(a => a.InvoiceFlag == 0 || a.InvoiceFlag == 1).Select(b => new { b.QuoteID, b.QuoteRefNum }).Distinct().ToList();

            quoteno.Insert(0, new QuoteViewModel(){ ClientQuote = "0", QuoteRefNum = "Select" } );

            ViewBag.dummyQuoteID = new SelectList(quoteno, "ClientQuote", "QuoteRefNum");

            

            InvoiceMasterViewModel invoice = new InvoiceMasterViewModel();

            
            return View(invoice);
        }

        // POST: Invoice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InvoiceMasterViewModel eng_invoice_master, List<QuoteDescriptionViewModel> quoteDescription)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(eng_invoice_master.InvoiceDate))
                    eng_invoice_master.InvoiceDate = DateTime.ParseExact(eng_invoice_master.InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                    eng_invoice_master.CreatedBy = AppSession.GetCurrentUserId();
                    eng_invoice_master.CreatedDate = DateTime.Now;

                    var result = invoiceService.CreateInvoice(eng_invoice_master, quoteDescription);
                    if (result > 0)
                    {
                        return getSuccessfulOperation();
                    }
                    else
                    {
                        return getFailedOperation();
                    }
            }

            return View(eng_invoice_master);
        }

        // GET: Invoice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return getFailedOperation();
            }
            var invoice = invoiceService.getInvoice(id ?? default(int));
            if (invoice == null)
            {
                return getFailedOperation();
            }
            var clients = quoteService.getAllQuotes().Where(a => a.ClientID == invoice.ClientID).Select(b => new { b.ClientID, b.eng_client_master.Company_Name }).Distinct().ToList();           

            ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name", invoice.ClientID);

            decimal invraised = (decimal)invoiceService.getAllInvoices().Where(a => a.QuoteID == invoice.QuoteID).Sum(b => b.TotalInvoiceAmount);

            invoice.invAmt = invraised - (decimal)invoice.TotalInvoiceAmount;

            if (invoice.InvoiceType == "Quotation")
            {
                ViewBag.Discount = invoice.eng_quote_master.Discount;
                ViewBag.FinalAmt = invoice.eng_quote_master.FinalAmount;

                invoice.prjAmt = (decimal)invoice.eng_quote_master.FinalAmount - invraised;
            }
            return View(invoice);
        }

        // POST: Invoice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InvoiceMasterViewModel eng_invoice_master, List<QuoteDescriptionViewModel> quoteDescription)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(eng_invoice_master.InvoiceDate))
                    eng_invoice_master.InvoiceDate = DateTime.ParseExact(eng_invoice_master.InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                eng_invoice_master.UpdatedBy = AppSession.GetCurrentUserId();
                eng_invoice_master.UpdatedDate = DateTime.Now;

                var result = invoiceService.SaveInvoice(eng_invoice_master, quoteDescription);

                if (result > 0)
                {

                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
            }

            return View(eng_invoice_master);
        }

        // GET: Invoice/CreateInvoiceDesc
        public ActionResult CreateInvoiceDesc(int? id)
        {
            if (id == null)
            {
                return View(new QuoteDescriptionViewModel());
            }
            else
            {
                return View(quoteService.getDesc(id));
            }
        }

        // GET: Invoice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_invoice_master eng_invoice_master = db.eng_invoice_master.Find(id);
            if (eng_invoice_master == null)
            {
                return HttpNotFound();
            }
            return View(eng_invoice_master);
        }


        public ActionResult GetQuoteDetails(int? qid)
        {

            //   eng_product_master eng_product_master = db.eng_product_master.Find(id);
            var quote = quoteService.getQuote(qid ?? default(int));
            decimal sumpaid = (decimal)invoiceService.getAllInvoices().Where(a => a.QuoteID == qid).Sum(b => b.TotalInvoiceAmount);

            return Json(new
            {
                value = "OK",
                disc = quote.Discount,
                qamt = quote.FinalAmount,
                invraised = sumpaid,
                result = quote.eng_quote_description
            });
        }

        // GET: Quote/ViewPayment
        public ActionResult
        ViewInvPayment(int? id)
        {
            if (id == null)
            {
                return View();
            }
            var payInfo = new PaymentInfoViewModel();
            var invoice = invoiceService.getInvoice(id ?? default(int));

            //payInfo.QuotationNum = quote.QuoteRefNum;
            payInfo.InvoiceNum = invoice.InvoiceNum;
            // payInfo.InvoiceDate = quote.InvoiceDate.ToString();
            payInfo.InvAmount = (decimal)invoice.TotalInvoiceAmount;

            decimal InvAmt = 0;
            InvAmt = (decimal)invoice.TotalInvoiceAmount;

            var pymt = new List<PaymentReceivableViewModel>();             
                pymt.AddRange(paymentService.getAllReceivables().Where(a => a.InvoiceID == invoice.InvoiceID).ToList());
            

            var invs = new InvoiceInfoViewModel();
            var recds = new PaymentReceivableViewModel();

            payInfo.ExcessReceived = 0;
            if (pymt.Count == 0)
            {
                payInfo.ReceivedAmount = 0;
                payInfo.PymtStatus = "Billed";
                payInfo.PendingAmount = InvAmt;
            }

            decimal RcdAmt = 0;
            if (pymt.Count > 0)
            {
                foreach (var rcd in pymt)
                {
                    RcdAmt = RcdAmt + (decimal)rcd.Amount;
                }
                if (RcdAmt < InvAmt)
                {
                    payInfo.PymtStatus = "Partially Received";
                }
                payInfo.ReceivedAmount = RcdAmt;
                payInfo.PendingAmount = InvAmt - RcdAmt;

                if (RcdAmt == InvAmt)
                {
                    payInfo.PymtStatus = "Fully Received";
                    payInfo.PendingAmount = 0;
                }
            }   
            
            payInfo.eng_pymt_receivable = pymt;
            return View(payInfo);
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            eng_invoice_master eng_invoice_master = db.eng_invoice_master.Find(id);
            db.eng_invoice_master.Remove(eng_invoice_master);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult InvReportIndex()
        {
            var filter = new FilterViewModel();
            var quotes = invoiceService.getAllInvoices().ToList();

            foreach (var quote in quotes)
            {
                if (quote.QuoteID != null)
                {
                    var qt = quoteService.getQuote(quote.QuoteID ?? default(int));
                    quote.QuoteFinalAmount = qt.FinalAmount;
                    quote.QuoteRefNum = qt.QuoteRefNum;
                    quote.QuoteDate = qt.QuoteDate;
                    quote.ServicesFor = qt.QuoteTitle;
                }


                if (quote.ClientID >  0)
                    quote.ClientName = clientService.getClient(quote.ClientID ?? default(int)).Company_Name;
                else
                    quote.ClientName = quote.ClientOthers;

                double ivamt = 0;
                
                foreach (var iv in quote.eng_invoice_details)
                {
                    ivamt = ivamt + (double)iv.Quantity * (double)iv.Price;
                }
                quote.INVValue = (decimal)ivamt;
                if(quote.GST == "0")
                {
                    quote.Percentage = (decimal) ivamt * 7 / 100;
                }
                else
                {
                    quote.Percentage = 0;
                }

               
            }

            var invno = invoiceService.getAllInvoices().Select(a => a.InvoiceNum).ToList();
            invno.Insert(0, "Select");

            //int[] statusIds = { 1, 2, 3, 4, 5 };
            //var status = quoteService.getStatus().Where(a => statusIds.Contains(a.StatusID)).ToList();
            //status.Insert(0, new QuoteStatusViewModel() { StatusID = 0, QuoteStatus = "Select" });
            //ViewBag.QuoteStatusID = new SelectList(status, "StatusID", "QuoteStatus");

            var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
            clients.Insert(0, new ClientViewModel() { ClientID = 0, Company_Name = "Select" });
            ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name");

            ViewBag.InvoiceNo = new SelectList(invno);

            filter.eng_invoice_master = quotes;


            return View(filter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InvReportIndex(FilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                var fil = new FilterViewModel();
                var invno = invoiceService.getAllInvoices().Select(a => a.InvoiceNum).ToList();
                invno.Insert(0, "Select");

               
                var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
                clients.Insert(0, new ClientViewModel() { ClientID = 0, Company_Name = "Select" });

                var quotes = invoiceService.getFilterInvoices(filter).ToList();

                foreach (var quote in quotes)
                {
                    if (quote.QuoteID != null)
                    {
                        var qt = quoteService.getQuote(quote.QuoteID ?? default(int));
                        quote.QuoteFinalAmount = qt.FinalAmount;
                        quote.QuoteRefNum = qt.QuoteRefNum;
                        quote.QuoteDate = qt.QuoteDate;
                        quote.ServicesFor = qt.QuoteTitle;
                    }


                    if (quote.ClientID > 0)
                        quote.ClientName = clientService.getClient(quote.ClientID ?? default(int)).Company_Name;
                    else
                        quote.ClientName = quote.ClientOthers;

                    double ivamt = 0;

                    foreach (var iv in quote.eng_invoice_details)
                    {
                        ivamt = ivamt + (double)iv.Quantity * (double)iv.Price;
                    }
                    quote.INVValue = (decimal)ivamt;
                    if (quote.GST == "0")
                    {
                        quote.Percentage = (decimal)ivamt * 7 / 100;
                    }
                    else
                    {
                        quote.Percentage = 0;
                    }

                }

                fil.eng_invoice_master = quotes;
                ViewBag.QuoteStatusID = filter.QuoteStatusID;
                ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name", filter.ClientID);
                fil.dateFrom = filter.dateFrom;
                fil.dateTo = filter.dateTo;
                fil.Month = filter.Month;

                ViewBag.InvoiceNo = new SelectList(invno, filter.DoNo);
                return View(fil);

            }
            return View();

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
