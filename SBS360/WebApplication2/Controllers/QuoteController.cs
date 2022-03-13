using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eng360Web.Models.Domain;
using AutoMapper;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.ViewModel;
using Eng360Web.Models.Utility;
using System.Globalization;
using Eng360Web.Models.Security;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class QuoteController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly IQuoteServices quoteService;
        private readonly IClientServices clientService;
        private readonly IPaymentServices paymentService;

        public QuoteController(IQuoteServices _quoteService, IClientServices _clientService, IPaymentServices _paymentService)
        {
            quoteService = _quoteService;
            clientService = _clientService;
            paymentService = _paymentService;

        }

        // GET: Quote
        public ActionResult Index()
        {
            //  var eng_quote_master = db.eng_quote_master.Include(e => e.eng_client_master);
            //  return View(eng_quote_master.ToList());
            var quotes = quoteService.getAllQuotes().ToList();

            foreach (var quote in quotes)
            {
                double qtamt = 0;

                foreach (var desc in quote.eng_quote_description)
                {
                    qtamt = qtamt + (double)desc.Quantity * (double)desc.QuotePrice;
                }
                if (quote.GTAX == "0")
                {
                    quote.QuoteTotValue = (decimal)qtamt - quote.Discount + ((decimal)qtamt - quote.Discount) * 7 /100;
                }
                else
                {
                    quote.QuoteTotValue = (decimal)qtamt - quote.Discount;
                }
            }



            return View(quotes);
        }


        public ActionResult ReportIndex()
        {
            var filter = new FilterViewModel();
            var quotes = quoteService.getAllQuotes().ToList();

            foreach (var quote in quotes)
            {
                double qtamt = 0;

                foreach (var desc in quote.eng_quote_description)
                {
                    qtamt = qtamt + (double)desc.Quantity * (double)desc.QuotePrice;
                }
                quote.QuoteTotValue = (decimal)qtamt - quote.Discount;
            }

            var qnum = quoteService.getAllQuotes().Select(a => a.QuoteRefNum).Distinct().ToList();
            qnum.Insert(0, "Select");

            int[] statusIds = { 1, 2, 3, 4, 5 };
            var status = quoteService.getStatus().Where(a => statusIds.Contains(a.StatusID)).ToList();
            status.Insert(0, new QuoteStatusViewModel() { StatusID = 0, QuoteStatus = "Select" });
            ViewBag.QuoteStatusID = new SelectList(status, "StatusID", "QuoteStatus");

            var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
            clients.Insert(0, new ClientViewModel() { ClientID = 0, Company_Name = "Select" });
            ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name");

            ViewBag.QuoteRefNum = new SelectList(qnum);

            filter.eng_quote_master = quotes;


            return View(filter);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportIndex(FilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                var fil = new FilterViewModel();
                var qnum = quoteService.getAllQuotes().Select(a => a.QuoteRefNum).Distinct().ToList();
                qnum.Insert(0, "Select");

                int[] statusIds = { 1, 2, 3, 4, 5 };
                var status = quoteService.getStatus().Where(a => statusIds.Contains(a.StatusID)).ToList();
                status.Insert(0, new QuoteStatusViewModel() { StatusID = 0, QuoteStatus = "Select" });

                var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
                clients.Insert(0, new ClientViewModel() { ClientID = 0, Company_Name = "Select" });

                var quotes = quoteService.getFilterQuotes(filter).ToList();

                foreach (var quote in quotes)
                {
                    double qtamt = 0;

                    foreach (var desc in quote.eng_quote_description)
                    {
                        qtamt = qtamt + (double)desc.Quantity * (double)desc.QuotePrice;
                    }
                    quote.QuoteTotValue = (decimal)qtamt - quote.Discount;
                }



                fil.eng_quote_master = quotes;
                ViewBag.QuoteStatusID = new SelectList(status, "StatusID", "QuoteStatus", filter.QuoteStatusID);
                ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name", filter.ClientID);
                fil.dateFrom = filter.dateFrom;
                fil.dateTo = filter.dateTo;
                fil.Month = filter.Month;

                ViewBag.QuoteRefNum = new SelectList(qnum, filter.QuoteRefNum);
                return View(fil);

            }
            return View();

        }


        public ActionResult DOReportIndex()
        {
            var filter = new FilterViewModel();
            var quotes = quoteService.getAllQuotes().Where(a => a.QuoteStatusID != 1).ToList();

            foreach (var quote in quotes)
            {
                double qtamt = 0;

                foreach (var desc in quote.eng_quote_description)
                {
                    qtamt = qtamt + (double)desc.Quantity * (double)desc.QuotePrice;
                }
                quote.QuoteTotValue = (decimal)qtamt - quote.Discount;
            }

            var dono = quoteService.getAllQuotes().Select(a => a.DoNo).Distinct().ToList();
            dono.Insert(0, "Select");

            int[] statusIds = { 1, 2, 3, 4, 5 };
            var status = quoteService.getStatus().Where(a => statusIds.Contains(a.StatusID)).ToList();
            status.Insert(0, new QuoteStatusViewModel() { StatusID = 0, QuoteStatus = "Select" });
            ViewBag.QuoteStatusID = new SelectList(status, "StatusID", "QuoteStatus");

            var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
            clients.Insert(0, new ClientViewModel() { ClientID = 0, Company_Name = "Select" });
            ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name");

            ViewBag.DoNo = new SelectList(dono);

            filter.eng_quote_master = quotes;


            return View(filter);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DOReportIndex(FilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                var fil = new FilterViewModel();
                var dono = quoteService.getAllQuotes().Select(a => a.DoNo).Distinct().ToList();
                dono.Insert(0, "Select");

                int[] statusIds = { 1, 2, 3, 4, 5 };
                var status = quoteService.getStatus().Where(a => statusIds.Contains(a.StatusID)).ToList();
                status.Insert(0, new QuoteStatusViewModel() { StatusID = 0, QuoteStatus = "Select" });

                var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
                clients.Insert(0, new ClientViewModel() { ClientID = 0, Company_Name = "Select" });

                var quotes = quoteService.getFilterDOs(filter).Where(a => a.QuoteStatusID != 1).ToList();

                foreach (var quote in quotes)
                {
                    double qtamt = 0;

                    foreach (var desc in quote.eng_quote_description)
                    {
                        qtamt = qtamt + (double)desc.Quantity * (double)desc.QuotePrice;
                    }
                    quote.QuoteTotValue = (decimal)qtamt - quote.Discount;
                }



                fil.eng_quote_master = quotes;
                ViewBag.QuoteStatusID = new SelectList(status, "StatusID", "QuoteStatus", filter.QuoteStatusID);
                ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name", filter.ClientID);
                fil.dateFrom = filter.dateFrom;
                fil.dateTo = filter.dateTo;
                fil.Month = filter.Month;

                ViewBag.DoNo = new SelectList(dono, filter.DoNo);
                return View(fil);

            }
            return View();

        }


        public ActionResult InvReportIndex()
        {
            var filter = new FilterViewModel();
            var quotes = quoteService.getAllQuotes().Where(a => a.QuoteStatusID != 1).ToList();

            foreach (var quote in quotes)
            {
                double qtamt = 0;
                double gst = 0;
                double tot = 0;

                foreach (var desc in quote.eng_quote_description)
                {
                    qtamt = qtamt + (double)desc.Quantity * (double)desc.QuotePrice;
                }
                quote.QuoteAmount = (decimal)qtamt - quote.Discount;
                if (quote.GTAX == "0")
                {
                    gst = (qtamt - (double)quote.Discount) * 7 / 100;
                }
                tot = qtamt - (double)quote.Discount + gst;
                quote.QuoteTotValue = (decimal)tot;
                quote.GTAX = gst.ToString();
            }

            var invno = quoteService.getAllQuotes().Select(a => a.InvoiceNo).Distinct().ToList();
            invno.Insert(0, "Select");

            int[] statusIds = { 1, 2, 3, 4, 5 };
            var status = quoteService.getStatus().Where(a => statusIds.Contains(a.StatusID)).ToList();
            status.Insert(0, new QuoteStatusViewModel() { StatusID = 0, QuoteStatus = "Select" });
            ViewBag.QuoteStatusID = new SelectList(status, "StatusID", "QuoteStatus");

            var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
            clients.Insert(0, new ClientViewModel() { ClientID = 0, Company_Name = "Select" });
            ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name");

            ViewBag.InvoiceNo = new SelectList(invno);

            filter.eng_quote_master = quotes;


            return View(filter);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InvReportIndex(FilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                var fil = new FilterViewModel();
                var invno = quoteService.getAllQuotes().Select(a => a.InvoiceNo).Distinct().ToList();
                invno.Insert(0, "Select");

                int[] statusIds = { 1, 2, 3, 4, 5 };
                var status = quoteService.getStatus().Where(a => statusIds.Contains(a.StatusID)).ToList();
                status.Insert(0, new QuoteStatusViewModel() { StatusID = 0, QuoteStatus = "Select" });

                var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();
                clients.Insert(0, new ClientViewModel() { ClientID = 0, Company_Name = "Select" });

                var quotes = quoteService.getFilterInvoices(filter).Where(a => a.QuoteStatusID != 1).ToList();

                foreach (var quote in quotes)
                {
                    double qtamt = 0;
                    double gst = 0;
                    double tot = 0;

                    foreach (var desc in quote.eng_quote_description)
                    {
                        qtamt = qtamt + (double)desc.Quantity * (double)desc.QuotePrice;

                    }
                    quote.QuoteAmount = (decimal)qtamt - quote.Discount;
                    if (quote.GTAX == "0")
                    {
                        gst = (qtamt - (double)quote.Discount) * 7 / 100;
                    }
                    tot = qtamt- (double) quote.Discount + gst;
                    quote.QuoteTotValue = (decimal)tot;
                    quote.GTAX = gst.ToString();

                }



                fil.eng_quote_master = quotes;
                ViewBag.QuoteStatusID = new SelectList(status, "StatusID", "QuoteStatus", filter.QuoteStatusID);
                ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name", filter.ClientID);
                fil.dateFrom = filter.dateFrom;
                fil.dateTo = filter.dateTo;
                fil.Month = filter.Month;

                ViewBag.InvoiceNo = new SelectList(invno, filter.DoNo);
                return View(fil);

            }
            return View();

        }




        // GET: Quote/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  eng_quote_master eng_quote_master = db.eng_quote_master.Find(id);
            var quote = quoteService.getQuote(id ?? default(int));
            if (quote == null)
            {
                return HttpNotFound();
            }
            quote.eng_quote_description = quote.eng_quote_description.OrderBy(a => a.QDID).ToList();
            return View(quote);
        }

        // GET: Quote/Details/5
        public ActionResult DetailsWOH(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  eng_quote_master eng_quote_master = db.eng_quote_master.Find(id);
            var quote = quoteService.getQuote(id ?? default(int));
            if (quote == null)
            {
                return HttpNotFound();
            }
            quote.eng_quote_description = quote.eng_quote_description.OrderBy(a => a.QDID).ToList();
            return View(quote);
        }


        public ActionResult DetailsDo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  eng_quote_master eng_quote_master = db.eng_quote_master.Find(id);
            var quote = quoteService.getQuote(id ?? default(int));
            if (quote == null)
            {
                return HttpNotFound();
            }
            quote.eng_quote_description = quote.eng_quote_description.OrderBy(a => a.QDID).ToList();
            return View(quote);
        }


        public ActionResult DetailsDoWOH(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  eng_quote_master eng_quote_master = db.eng_quote_master.Find(id);
            var quote = quoteService.getQuote(id ?? default(int));
            if (quote == null)
            {
                return HttpNotFound();
            }
            quote.eng_quote_description = quote.eng_quote_description.OrderBy(a => a.QDID).ToList();
            return View(quote);
        }




        public ActionResult DetailsInv(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  eng_quote_master eng_quote_master = db.eng_quote_master.Find(id);
            var quote = quoteService.getQuote(id ?? default(int));
            if (quote == null)
            {
                return HttpNotFound();
            }
            quote.eng_quote_description = quote.eng_quote_description.OrderBy(a => a.QDID).ToList();
            return View(quote);
        }


        public ActionResult DetailsInvWOH(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  eng_quote_master eng_quote_master = db.eng_quote_master.Find(id);
            var quote = quoteService.getQuote(id ?? default(int));
            if (quote == null)
            {
                return HttpNotFound();
            }
            quote.eng_quote_description = quote.eng_quote_description.OrderBy(a => a.QDID).ToList();
            return View(quote);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInvDoDate(string invDate, string doDate, int invReleased, int QuoteID, string typeChk)
        {
            var result = quoteService.UpdateInvDoDate(invDate, doDate, invReleased, QuoteID, typeChk);

            if (result > 0)

                return getSuccessfulOperation();
            else
                return getFailedOperation();
        }

        // GET: Quote/Create
        public ActionResult Create()
        {
            int[] statusIds = { 1 };
            var status = quoteService.getStatus().Where(a => statusIds.Contains(a.StatusID)).ToList();
            ViewBag.QuoteStatusID = new SelectList(status, "StatusID", "QuoteStatus");

            var clients = clientService.getAllClients().Where(a => a.IsActive == 1).ToList();

            ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name");

            List<ClientContactViewModel> attention = new List<ClientContactViewModel>();

            foreach (var client in clients)
            {
                attention.AddRange(client.eng_client_contact);

            }
            attention.Insert(0, new ClientContactViewModel() { CCID = 0, SPOCName = "Select", eng_client_master = new ClientViewModel() { ClientID = 0 } });
            QuoteViewModel quote = new QuoteViewModel();

            quote.attention = attention;
            return View(quote);
        }

        // POST: Quote/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuoteViewModel eng_quote_master, List<QuoteDescriptionViewModel> quoteDescription)
        {
            if (ModelState.IsValid)
            {
                //db.eng_quote_master.Add(eng_quote_master);
                //db.SaveChanges();
                //return RedirectToAction("Index");\

                 if (!string.IsNullOrEmpty(eng_quote_master.QuoteDate))
                    eng_quote_master.QuoteDate = DateTime.ParseExact(eng_quote_master.QuoteDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


                eng_quote_master.CreatedBy = AppSession.GetCurrentUserId();
                eng_quote_master.CreatedDate = DateTime.Now;
                eng_quote_master.InvoiceDate = DateTime.Now;
                eng_quote_master.DODate = DateTime.Now;
                eng_quote_master.RvFlag = 0;

                int flag = 0;

                var result = quoteService.CreateQuote(eng_quote_master, quoteDescription, flag);
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
            }

            ViewBag.ClientID = new SelectList(db.eng_client_master, "ClientID", "ClientDisplayID", eng_quote_master.ClientID);
            return View(eng_quote_master);
        }

        [HttpGet]
        public ActionResult CreateProjectSelection(int? id)
        {
            var quote = quoteService.getQuote(id ?? default(int));
            var locations = quoteService.getLocations();
            locations.Insert(0, new LocationViewModel() { LocationId = 0, LocationName = "Select" });
            ViewBag.LocationId = new SelectList(locations, "LocationId", "LocationName");

            return View(quote.eng_quote_description.Where(a => a.ProjectID == null).ToList());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProjectSelection(List<int> ids, int locationID, string startDate, string endDate, string ProjectName)
        {
            var result = quoteService.projectCreationSelection(ids, locationID, startDate, endDate, ProjectName);
            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }
        }

        // GET: Quote/Edit/5
        public ActionResult Edit(int? id, int? flag)
        {
            if (id == null)
            {
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return getFailedOperation();
            }
            //eng_quote_master eng_quote_master = db.eng_quote_master.Find(id);
            var quote = quoteService.getQuote(id ?? default(int));

            if (quote == null)
            {
                return getFailedOperation();
            }
            int[] statusIds = { 1, 2, 4 };
            var status = quoteService.getStatus().Where(a => statusIds.Contains(a.StatusID)).ToList();
            ViewBag.QuoteStatusID = new SelectList(status, "StatusID", "QuoteStatus", quote.QuoteStatusID);

            var clients = clientService.getAllClients();

            ViewBag.ClientID = new SelectList(clients, "ClientID", "Company_Name", quote.ClientID);

            List<ClientContactViewModel> attention = new List<ClientContactViewModel>();

            foreach (var client in clients)
            {
                attention.AddRange(client.eng_client_contact);

            }
            attention.Insert(0, new ClientContactViewModel() { CCID = 0, SPOCName = "Select", eng_client_master = new ClientViewModel() { ClientID = 0 } });

            quote.attention = attention;
            quote.RevFlag = flag ?? default(int);
            return View(quote);
        }

        // POST: Quote/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuoteViewModel eng_quote_master, List<QuoteDescriptionViewModel> quoteDescription, List<int> deleted)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(eng_quote_master.QuoteDate))
                    eng_quote_master.QuoteDate = DateTime.ParseExact(eng_quote_master.QuoteDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (eng_quote_master.RevFlag == 0)
                {
                    eng_quote_master.UpdatedBy = AppSession.GetCurrentUserId();
                    eng_quote_master.UpdatedDate = DateTime.Now;
                    var result = quoteService.saveQuote(eng_quote_master, quoteDescription, deleted);

                    if (result == 0)
                        return getSuccessfulOperation();
                    else
                        return getFailedOperation();
                }

                if (eng_quote_master.RevFlag == 1)
                {

                    var oldquote = eng_quote_master.QuoteID;

                    eng_quote_master.CreatedBy = AppSession.GetCurrentUserId();
                    eng_quote_master.CreatedDate = DateTime.Now;
                    eng_quote_master.InvoiceDate = DateTime.Now;
                    eng_quote_master.DODate = DateTime.Now;
                    eng_quote_master.RvFlag = 0;

                    int flag = 1;

                    var result = quoteService.CreateQuote(eng_quote_master, quoteDescription, flag);

                    if (result >= 0)
                    {
                        var newres = quoteService.ReviseQuoteFlag(oldquote);
                        return getSuccessfulOperation();
                    }
                    else
                        return getFailedOperation();

                }



            }
            return getFailedOperation();
            //ViewBag.ClientID = new SelectList(db.eng_client_master, "ClientID", "ClientDisplayID", eng_quote_master.ClientID);
            //return View(eng_quote_master);
        }

        // GET: Quote/CreateQuoteDesc
        public ActionResult CreateQuoteDesc(int? id)
        {
            if (id == null)
            {
                return View();
            }
            else
            {
                return View(quoteService.getDesc(id));
            }
        }



        // GET: Quote/ViewPayment
        public ActionResult 
            ViewQLPayment(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var payInfo = new PaymentInfoViewModel(); 

            var quote = quoteService.getQuote(id ?? default(int));

            payInfo.QuotationNum = quote.QuoteRefNum;
            payInfo.InvoiceNum = quote.InvoiceNo;
            payInfo.InvoiceDate = quote.InvoiceDate.ToString();
            payInfo.InvAmount = (decimal) quote.FinalAmount;

            double qtamt = 0;
            decimal InvAmt = 0;
            //foreach (var desc in quote.eng_quote_description)
            //{
            //    qtamt = qtamt + (double)desc.Quantity * (double)desc.QuotePrice;
            //}
            
            //payInfo.InvAmount = (decimal) qtamt - (decimal) quote.Discount;

            InvAmt = (decimal)quote.FinalAmount;

            var pymt = paymentService.getAllReceivables().Where(a => a.QuoteID == id).ToList();
            var invoices = quoteService.getAllPaymentInvoices(id ?? default(int));
            

            var invs = new InvoiceInfoViewModel();
            var recds = new PaymentReceivableViewModel();

            payInfo.ExcessReceived = 0;
            if (pymt.Count == 0)
            {
                payInfo.ReceivedAmount = 0;
                payInfo.PymtStatus = "Billed";
            }

            decimal RcdAmt = 0;
            if(pymt.Count >0)
            {
                foreach(var rcd in pymt)
                {
                    RcdAmt = RcdAmt + (decimal) rcd.Amount;
                }
                if (RcdAmt < InvAmt)
                {
                    payInfo.PymtStatus = "Partially Received";
                }
                payInfo.ReceivedAmount = RcdAmt;
                payInfo.PendingAmount = InvAmt - RcdAmt;
            }

            
           
            if(quote.isFullyPaid == 1)
            {
                payInfo.PymtStatus = "Fully Received";
                if((RcdAmt - InvAmt) > 0)
                payInfo.ExcessReceived = RcdAmt - InvAmt;

                payInfo.PendingAmount = 0;
            }


            payInfo.eng_invoice_info = invoices;
            payInfo.eng_pymt_receivable = pymt;

            return View(payInfo);

           

        }



        // GET: Quote/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_quote_master eng_quote_master = db.eng_quote_master.Find(id);
            if (eng_quote_master == null)
            {
                return HttpNotFound();
            }
            return View(eng_quote_master);
        }

        // POST: Quote/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = quoteService.DeleteQuote(id);

            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getInvoiceDates(int id)
        {
            var cInv = quoteService.getQuote(id);

            return Json(new
            {
                value = "OK",
                invDate = Convert.ToDateTime(cInv.InvoiceDate).ToString(AppSettings.GetDateFormat().Replace("mm", "MM")),
                doDate = Convert.ToDateTime(cInv.DODate).ToString(AppSettings.GetDateFormat().Replace("mm", "MM")),
                doReleased = cInv.Is_do_released,
                invReleased = cInv.Is_invoice_released
            });
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
