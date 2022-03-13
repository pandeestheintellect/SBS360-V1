using AutoMapper;
using Eng360Web.Models.Domain;
using Eng360Web.Models.Repository.Interface;
using Eng360Web.Models.Utility;
using Eng360Web.Models.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Globalization;

namespace Eng360Web.Models.Repository.Imp
{
    public class QuoteRepository : IQuoteRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
        Logger logger = LogManager.GetCurrentClassLogger();
        public List<QuoteViewModel> getAllQuotes()
        {
            var eng_quote_master = Eng360DB.eng_quote_master.ToList();
            var lstQuoteView = Mapper.Map<List<QuoteViewModel>>(eng_quote_master);
            return lstQuoteView;
        }

        public List<QuoteDescriptionViewModel> getAllDescriptions()
        {
            var eng_quote_description = Eng360DB.eng_quote_description.ToList();
            var lstDescView = Mapper.Map<List<QuoteDescriptionViewModel>>(eng_quote_description);
            return lstDescView;
        }

        public QuoteViewModel getQuote(int quoteID)
        {
            eng_quote_master eng_quote_master = Eng360DB.eng_quote_master.Find(quoteID);

            eng_company eng_company = Eng360DB.eng_company.First();
            var output = Mapper.Map<QuoteViewModel>(eng_quote_master);

            output.CompanyName = eng_company.CompanyName;
            output.Auth_InvoiceName = eng_company.Auth_InvoiceName;
            output.InvoiceTerms = eng_company.InvoiceTerms;
            return output;
        }

        public List<QuoteStatusViewModel> getStatus()
        {

            var obStatus = Eng360DB.eng_sys_quotestatus.ToList();

            return Mapper.Map<List<QuoteStatusViewModel>>(obStatus);
        }

        public List<QuoteViewModel> getAllClientQuotes(int id)
        {

            var listQuotes = Eng360DB.eng_quote_master.Where(a => a.ClientID == id && (a.InvoiceFlag == 0 || a.InvoiceFlag == 1)).ToList();

            List<QuoteViewModel> quvm = new List<QuoteViewModel>();

            foreach (var quote in listQuotes)
            {

                quvm.Add(new QuoteViewModel() { QuoteRefNum = quote.QuoteRefNum + "_(" + quote.FinalAmount + ")", ClientQuote = id.ToString() + "_" + quote.QuoteID.ToString() });

            }

            return quvm;
        }


        public int CreateQuote(QuoteViewModel eng_quote_master, List<QuoteDescriptionViewModel> quoteDescription, int revflag)
        {
            try
            {
                if (revflag == 0)
                {
                    eng_quote_master domainQuote = Mapper.Map<eng_quote_master>(eng_quote_master);
                    Eng360DB.eng_quote_master.Add(domainQuote);

                    foreach (var desc in quoteDescription)
                    {
                        desc.AddedBy = AppSession.GetCurrentUserId();
                        desc.AddedDate = DateTime.Now;
                        eng_quote_description domainDesc = Mapper.Map<eng_quote_description>(desc);
                        domainDesc.QuoteID = domainQuote.QuoteID;
                        Eng360DB.eng_quote_description.Add(domainDesc);
                    }
                }

                if (revflag == 1)
                {
                    eng_quote_master domainQuote = Mapper.Map<eng_quote_master>(eng_quote_master);
                    Eng360DB.eng_quote_master.Add(domainQuote);

                    foreach (var desc in quoteDescription)
                    {

                        desc.AddedBy = AppSession.GetCurrentUserId();
                        desc.AddedDate = DateTime.Now;
                        eng_quote_description domainDesc = Mapper.Map<eng_quote_description>(desc);
                        domainDesc.QuoteID = domainQuote.QuoteID;
                        Eng360DB.eng_quote_description.Add(domainDesc);

                    }
                }


                return Eng360DB.SaveChanges();
            }
            catch (Exception ex)
            {
                //logger
                return -1;
            }
        }

        public int projectCreationSelection(List<int> ids, int LocationID, string startDate, string endDate, string ProjectName)
        {

            try
            {
                eng_quote_master quote;
                if (ids.Count > 0)
                    quote = Eng360DB.eng_quote_description.Find(ids[0]).eng_quote_master;
                else
                    return -1;

                var projectDescs = quote.eng_quote_description.Where(a => ids.Contains(a.QDID)).ToList();
                var projectDescsPendins = quote.eng_quote_description.Where(a => a.ProjectID == null).ToList();

                if (!string.IsNullOrEmpty(startDate))
                    startDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(endDate))
                    endDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                decimal amt = 0;
                foreach(var calc in projectDescs)
                {
                    amt = amt + (decimal) calc.Quantity *  (decimal) calc.QuotePrice;

                }

                if(quote.GTAX == "0")
                {
                    amt = amt + amt * 7 / 100;
                }

                eng_project_master domainProject = new eng_project_master();

                //if (ProjectName == "auto")
                //    domainProject.ProjectName = projectDescs.FirstOrDefault().QuoteDescription;
                //else
                //    domainProject.ProjectName = ProjectName;

                domainProject.ProjectName = ProjectName;

                domainProject.LocationId = LocationID;
                domainProject.QuotationID = quote.QuoteID;
                domainProject.Start_Date = Convert.ToDateTime(startDate);
                domainProject.CreatedBy = AppSession.GetCurrentUserId();
                domainProject.CreatedDate = DateTime.Now;
                domainProject.InvoiceDate = DateTime.Now;
                domainProject.DoDate = DateTime.Now;

                domainProject.Project_Cost = amt;

                if (!string.IsNullOrEmpty(endDate))
                    domainProject.End_Date = Convert.ToDateTime(endDate);

                //when create its considerd as started
                domainProject.Project_Status_ID = 1;

                Eng360DB.eng_project_master.Add(domainProject);

                foreach (var projectDesc in projectDescs)
                {
                    projectDesc.ProjectID = domainProject.ProjectID;
                    Eng360DB.Entry(projectDesc).State = System.Data.Entity.EntityState.Modified;

                }

                if (projectDescsPendins.Count == ids.Count)
                {
                    quote.QuoteStatusID = 5;
                    Eng360DB.Entry(quote).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    quote.QuoteStatusID = 3;
                    Eng360DB.Entry(quote).State = System.Data.Entity.EntityState.Modified;
                }

                return Eng360DB.SaveChanges();
            }
            catch (Exception ex)
            {
                //Logger
                return -1;

            }
        }

        public List<LocationViewModel> getLocations()
        {
            var locations = Eng360DB.eng_sys_location.ToList();

            return Mapper.Map<List<LocationViewModel>>(locations);

        }

        public int saveQuote(QuoteViewModel eng_quote_master, List<QuoteDescriptionViewModel> quoteDescription, List<int> deleted)
        {
            try
            {
                if (eng_quote_master.QuoteStatusID == 2)
                    eng_quote_master.InvoiceFlag = 0;

                var domainQuote = Mapper.Map<eng_quote_master>(eng_quote_master);
                Eng360DB.Entry(domainQuote).State = EntityState.Modified;
                foreach (var desc in quoteDescription)
                {
                    //insert 
                    if (desc.QDID == 0)
                    {
                        desc.AddedBy = AppSession.GetCurrentUserId();
                        desc.AddedDate = DateTime.Now;
                        desc.QuoteID = domainQuote.QuoteID;
                        eng_quote_description domainDesc = Mapper.Map<eng_quote_description>(desc);
                        Eng360DB.eng_quote_description.Add(domainDesc);
                    }
                    if (desc.QDID > 0)
                    {
                        desc.UpdatedBy = AppSession.GetCurrentUserId();
                        desc.UpdatedDate = DateTime.Now;
                        desc.QuoteID = domainQuote.QuoteID;
                        eng_quote_description domainDesc = Mapper.Map<eng_quote_description>(desc);
                        Eng360DB.Entry(domainDesc).State = EntityState.Modified;
                    }
                    //edit
                }
                if (deleted != null)
                    foreach (var deleteDesc in deleted)
                    {
                        var willbeDeleted = Eng360DB.eng_quote_description.Find(deleteDesc);
                        Eng360DB.eng_quote_description.Remove(willbeDeleted);
                    }


                var result = Eng360DB.SaveChanges();
                if (eng_quote_master.QuoteStatusID == 2)
                {
                    if (eng_quote_master.isAutoApproved == true)
                    {
                        var quoteLineItems = Eng360DB.eng_quote_description.Where(a => a.QuoteID == eng_quote_master.QuoteID).ToList();
                        var ids = quoteLineItems.Select(a => a.QDID).ToList();

                        if (eng_quote_master.isProjectCreated == true)
                        {
                            foreach (var id in ids)
                            {
                                List<int> singleID = new List<int>();
                                singleID.Add(id);
                                //result = projectCreationSelection(singleID, 1, DateTime.Now.ToString("dd/MM/yyyy"), "", "auto");
                                result = projectCreationSelection(singleID, 1, DateTime.Now.ToString("dd/MM/yyyy"), "", eng_quote_master.ProjectTitle);
                            }
                        }
                        else
                        {

                            //result = projectCreationSelection(ids, 1, DateTime.Now.ToString("dd/MM/yyyy"), "", "auto");
                            result = projectCreationSelection(ids, 1, DateTime.Now.ToString("dd/MM/yyyy"), "", eng_quote_master.ProjectTitle);
                        }
                    }

                }
                if (result == -1)
                    return -1;
                else
                    return 0;

            }
            catch (Exception ex)
            {
                //logger
                return -1;
            }

        }

        public QuoteDescriptionViewModel getDesc(int? id)
        {

            var domainDesc = Eng360DB.eng_quote_description.Find(id);
            return Mapper.Map<QuoteDescriptionViewModel>(domainDesc);
        }


        public List<QuoteViewModel> getAllClientInvoices(int id)
        {

            var listQuotes =  Eng360DB.eng_quote_master.Where(a=>a.ClientID == id && a.Is_invoice_released == 1 && a.isFullyPaid == 0).ToList();

            List<QuoteViewModel> qvm = new List<QuoteViewModel>();

            foreach (var quote in listQuotes)
            {

                if(quote.Is_Quote_level_inv == 1)
                {
                    qvm.Add(new QuoteViewModel() { InvoiceNo = quote.InvoiceNo + "_" + quote.QuoteTitle + "_" + "Quotation", ClientQuote = id.ToString() + "_" + quote.QuoteID.ToString() });

                }
                if (quote.Is_Project_level_inv == 1)
                {
                    var listProjs = quote.eng_project_master.Where(p => p.isFullyPaid == 0 && p.Is_Project_level_inv == 1);
                    foreach(var proj in listProjs)
                    {
                        qvm.Add(new QuoteViewModel() { InvoiceNo = proj.InvoiceNo + "_" + proj.ProjectName + "_" + "Project", ClientQuote = id.ToString() + "_" + quote.QuoteID.ToString()  });

                    }                   

                }
                if (quote.Is_Custom_level_inv == 1)
                {
                    var listProjs = quote.eng_project_master.Where(p=>p.Is_Custom_level_inv == 1);
                    foreach (var proj in listProjs)
                    {
                        var cInvoices = proj.eng_custom_invoice.Where(c => c.isFullyPaid == 0);

                        foreach (var cinv in cInvoices)
                        {
                            qvm.Add(new QuoteViewModel() { InvoiceNo = cinv.InvoiceNo + "_" + proj.ProjectName + "_" + "Custom", ClientQuote = id.ToString() + "_" + quote.QuoteID.ToString() });
                        }
                    }

                }

            }

            return qvm;
        }


        public List<InvoiceInfoViewModel> getAllPaymentInvoices(int id)
        {

            var listQuotes = Eng360DB.eng_quote_master.Where(a => a.QuoteID == id).ToList();

            List<InvoiceInfoViewModel> pivm = new List<InvoiceInfoViewModel>();

            foreach (var quote in listQuotes)
            {

                if (quote.Is_Quote_level_inv == 1)
                {
                    pivm.Add(new InvoiceInfoViewModel() {
                        InvoiceNum = quote.InvoiceNo, 
                        InvAmount = (decimal) quote.FinalAmount,
                        InvoiceDate = quote.InvoiceDate.ToString()
                         
                    });

                }
                if (quote.Is_Project_level_inv == 1)
                {
                    var listProjs = quote.eng_project_master.Where(p => p.QuotationID == id && p.Is_Project_level_inv == 1);
                    foreach (var proj in listProjs)
                    {
                        pivm.Add(new InvoiceInfoViewModel() {
                            InvoiceNum = proj.InvoiceNo,
                            InvAmount = (decimal) proj.Project_Cost,
                            InvoiceDate = proj.InvoiceDate.ToString()
                        });

                    }

                }
                if (quote.Is_Custom_level_inv == 1)
                {
                    var listProjs = quote.eng_project_master.Where(p => p.QuotationID == id && p.Is_Custom_level_inv == 1);
                    foreach (var proj in listProjs)
                    {
                        var cInvoices = proj.eng_custom_invoice.ToList();

                        foreach (var cinv in cInvoices)
                        {
                            pivm.Add(new InvoiceInfoViewModel() {
                                InvoiceNum = cinv.InvoiceNo,
                                InvAmount = (decimal) cinv.FinalInvoiceAmount,
                                InvoiceDate = cinv.InvoiceDate.ToString()
                            });
                        }
                    }

                }

            }

            return pivm;
        }




        public int DeleteQuote(int quoteID)
        {
            var _db_quote = Eng360DB.eng_quote_master.First(a => a.QuoteID == quoteID);

            Eng360DB.eng_quote_master.Remove(_db_quote);

            return Eng360DB.SaveChanges();
        }
        public int UpdateInvDoDate(string invDate, string doDate, int invReleased, int QuoteID, string typeChk)
        {
            eng_quote_master eng_quote_master = Eng360DB.eng_quote_master.Find(QuoteID);
               

            if (!string.IsNullOrEmpty(invDate))
                invDate = DateTime.ParseExact(invDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            if (!string.IsNullOrEmpty(doDate))
                doDate = DateTime.ParseExact(doDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            if (typeChk == "WC")
            {
                eng_quote_master.DODate = Convert.ToDateTime(doDate);
                eng_quote_master.Is_do_released = invReleased;

                if (invReleased > 0)
                {
                    eng_quote_master.Is_do_released = invReleased;
                    eng_quote_master.Is_Quote_level_do = invReleased;
                }

                if (invReleased > 0)
                {
                    eng_do_seqnum doseqnum = new Domain.eng_do_seqnum();
                    doseqnum.DoNo = "CITI-CQ-WC-000";
                    doseqnum.DoType = "Q";
                    doseqnum.id = QuoteID;
                    Eng360DB.eng_do_seqnum.Add(doseqnum);
                }

            }

            if (typeChk == "IV" || typeChk == "IVwoDO")
            {
                eng_quote_master.InvoiceDate = Convert.ToDateTime(invDate);
                eng_quote_master.Is_invoice_released = invReleased;
                eng_quote_master.isFullyPaid = 0;

                if (invReleased > 0)
                    eng_quote_master.Is_Quote_level_inv = invReleased;

                Eng360DB.Entry(eng_quote_master).State = EntityState.Modified;

                if (invReleased > 0)
                {
                    eng_inv_seqnum invseqnum = new Domain.eng_inv_seqnum();
                    invseqnum.InvoiceNo = "CITI-CQ-INV-000";
                    if (typeChk == "IV")
                        invseqnum.InvoiceType = "Q";
                    else
                        invseqnum.InvoiceType = "QwoDO";

                    invseqnum.id = QuoteID;
                    Eng360DB.eng_inv_seqnum.Add(invseqnum);
                }
            }

            return Eng360DB.SaveChanges();

        }

        public List<QuoteViewModel> getFilterQuotes(FilterViewModel filter)
        {
            var dCurrentDayofThisMonth = DateTime.Today.ToString("yyyy-MM-dd");
            var dFirstDayOfThisMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            var dLastDayOfLastMonth = dFirstDayOfThisMonth.AddDays(-1).ToString("yyyy-MM-dd");
            var dFirstDayOfLastMonth = dFirstDayOfThisMonth.AddMonths(-1).ToString("yyyy-MM-dd"); ;
            var dFirstDayOfCurrentMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).ToString("yyyy-MM-dd");

            var sql = "select * from eng_quote_master ";
            var where = "";
            if (filter.QuoteRefNum != "Select")
                where = "QuoteRefNum = '" + filter.QuoteRefNum + "'";

            if (filter.ClientID > 0)
                if (where == "")
                    where += " ClientID =" + filter.ClientID;
                else
                    where += " and ClientID =" + filter.ClientID;

            if (filter.Month == 0)
            {
                if (filter.dateFrom != null)
                {
                    filter.dateFrom = DateTime.ParseExact(filter.dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " QuoteDate >='" + filter.dateFrom + "'";
                    else
                        where += " and QuoteDate >='" + filter.dateFrom + "'";
                }
                if (filter.dateTo != null)
                {
                    filter.dateTo = DateTime.ParseExact(filter.dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " QuoteDate <='" + filter.dateTo + "'";
                    else
                        where += " and QuoteDate <='" + filter.dateTo + "'";
                }
            }

            if (filter.Month == 1)
            {

                if (where == "")
                    where += " QuoteDate >= '" + dFirstDayOfCurrentMonth + "' and QuoteDate <= '" + dCurrentDayofThisMonth + "'";
                else
                    where += " and QuoteDate >='" + dFirstDayOfCurrentMonth + "' and QuoteDate <='" + dCurrentDayofThisMonth + "'";

            }

            if (filter.Month == 2)
            {

                if (where == "")
                    where += " QuoteDate >='" + dFirstDayOfLastMonth + "' and QuoteDate <='" + dLastDayOfLastMonth + "'";
                else
                    where += " and QuoteDate >='" + dFirstDayOfLastMonth + "' and QuoteDate <='" + dLastDayOfLastMonth + "'";

            }

            if (filter.QuoteStatusID > 0)
                if (where == "")
                    where += " QuoteStatusID =" + filter.QuoteStatusID;
                else
                    where += " and QuoteStatusID =" + filter.QuoteStatusID;

            if (where != "")
                sql = sql + "Where " + where;


            // DbSqlQuery<eng_employee_profile> data = Eng360DB.eng_employee_profile.SqlQuery(sql);
            var data = Eng360DB.eng_quote_master.SqlQuery(sql).ToList();

            var lstQuoteView = Mapper.Map<List<QuoteViewModel>>(data);
            return lstQuoteView;
        }

        public List<QuoteViewModel> getFilterDOs(FilterViewModel filter)
        {
            var dCurrentDayofThisMonth = DateTime.Today.ToString("yyyy-MM-dd");
            var dFirstDayOfThisMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            var dLastDayOfLastMonth = dFirstDayOfThisMonth.AddDays(-1).ToString("yyyy-MM-dd");
            var dFirstDayOfLastMonth = dFirstDayOfThisMonth.AddMonths(-1).ToString("yyyy-MM-dd"); ;
            var dFirstDayOfCurrentMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).ToString("yyyy-MM-dd");

            var sql = "select * from eng_quote_master ";
            var where = "";
            if (filter.DoNo != "Select")
                where = "DoNo = '" + filter.DoNo + "'";

            if (filter.ClientID > 0)
                if (where == "")
                    where += " ClientID =" + filter.ClientID;
                else
                    where += " and ClientID =" + filter.ClientID;

            if (filter.Month == 0)
            {
                if (filter.dateFrom != null)
                {
                    filter.dateFrom = DateTime.ParseExact(filter.dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " DODate >='" + filter.dateFrom + "'";
                    else
                        where += " and DODate >='" + filter.dateFrom + "'";
                }
                if (filter.dateTo != null)
                {
                    filter.dateTo = DateTime.ParseExact(filter.dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " DODate <='" + filter.dateTo + "'";
                    else
                        where += " and DODate <='" + filter.dateTo + "'";
                }
            }

            if (filter.Month == 1)
            {

                if (where == "")
                    where += " DODate >= '" + dFirstDayOfCurrentMonth + "' and DODate <= '" + dCurrentDayofThisMonth + "'";
                else
                    where += " and DODate >='" + dFirstDayOfCurrentMonth + "' and DODate <='" + dCurrentDayofThisMonth + "'";

            }

            if (filter.Month == 2)
            {

                if (where == "")
                    where += " DODate >='" + dFirstDayOfLastMonth + "' and DODate <='" + dLastDayOfLastMonth + "'";
                else
                    where += " and DODate >='" + dFirstDayOfLastMonth + "' and DODate <='" + dLastDayOfLastMonth + "'";

            }

            if (filter.QuoteStatusID > 0)
                if (where == "")
                    where += " QuoteStatusID =" + filter.QuoteStatusID;
                else
                    where += " and QuoteStatusID =" + filter.QuoteStatusID;

            if (where != "")
                sql = sql + "Where " + where;


            // DbSqlQuery<eng_employee_profile> data = Eng360DB.eng_employee_profile.SqlQuery(sql);
            var data = Eng360DB.eng_quote_master.SqlQuery(sql).ToList();

            var lstQuoteView = Mapper.Map<List<QuoteViewModel>>(data);
            return lstQuoteView;
        }

        public int ReviseQuoteFlag(int quoteid)
        {

            var quote = Eng360DB.eng_quote_master.AsNoTracking().Where(a => a.QuoteID == quoteid).FirstOrDefault();

            var _db_quote = Mapper.Map<eng_quote_master>(quote);
            _db_quote.RvFlag = 1;
            Eng360DB.Entry(_db_quote).State = EntityState.Modified;


            return Eng360DB.SaveChanges();


        }


        public List<QuoteViewModel> getFilterInvoices(FilterViewModel filter)
        {
            var dCurrentDayofThisMonth = DateTime.Today.ToString("yyyy-MM-dd");
            var dFirstDayOfThisMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            var dLastDayOfLastMonth = dFirstDayOfThisMonth.AddDays(-1).ToString("yyyy-MM-dd");
            var dFirstDayOfLastMonth = dFirstDayOfThisMonth.AddMonths(-1).ToString("yyyy-MM-dd"); ;
            var dFirstDayOfCurrentMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).ToString("yyyy-MM-dd");

            var sql = "select * from eng_quote_master ";
            var where = "";
            if (filter.InvoiceNo != "Select")
                where = "InvoiceNo = '" + filter.InvoiceNo + "'";

            if (filter.ClientID > 0)
                if (where == "")
                    where += " ClientID =" + filter.ClientID;
                else
                    where += " and ClientID =" + filter.ClientID;

            if (filter.Month == 0)
            {
                if (filter.dateFrom != null)
                {
                    filter.dateFrom = DateTime.ParseExact(filter.dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " InvoiceDate >='" + filter.dateFrom + "'";
                    else
                        where += " and InvoiceDate >='" + filter.dateFrom + "'";
                }
                if (filter.dateTo != null)
                {
                    filter.dateTo = DateTime.ParseExact(filter.dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " InvoiceDate <='" + filter.dateTo + "'";
                    else
                        where += " and InvoiceDate <='" + filter.dateTo + "'";
                }
            }

            if (filter.Month == 1)
            {

                if (where == "")
                    where += " InvoiceDate >= '" + dFirstDayOfCurrentMonth + "' and InvoiceDate <= '" + dCurrentDayofThisMonth + "'";
                else
                    where += " and InvoiceDate >='" + dFirstDayOfCurrentMonth + "' and InvoiceDate <='" + dCurrentDayofThisMonth + "'";

            }

            if (filter.Month == 2)
            {

                if (where == "")
                    where += " InvoiceDate >='" + dFirstDayOfLastMonth + "' and InvoiceDate <='" + dLastDayOfLastMonth + "'";
                else
                    where += " and InvoiceDate >='" + dFirstDayOfLastMonth + "' and InvoiceDate <='" + dLastDayOfLastMonth + "'";

            }

            if (filter.QuoteStatusID > 0)
                if (where == "")
                    where += " QuoteStatusID =" + filter.QuoteStatusID;
                else
                    where += " and QuoteStatusID =" + filter.QuoteStatusID;

            if (where != "")
                sql = sql + "Where " + where;


            // DbSqlQuery<eng_employee_profile> data = Eng360DB.eng_employee_profile.SqlQuery(sql);
            var data = Eng360DB.eng_quote_master.SqlQuery(sql).ToList();

            var lstQuoteView = Mapper.Map<List<QuoteViewModel>>(data);
            return lstQuoteView;
        }


    }
}