using AutoMapper;
using Eng360Web.Models.Domain;
using Eng360Web.Models.Repository.Interface;
using Eng360Web.Models.Security;
using Eng360Web.Models.Utility;
using Eng360Web.Models.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Repository.Imp
{
    
    public class InvoiceRepository : IInvoiceRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
        Logger logger = LogManager.GetCurrentClassLogger();

        public List<InvoiceMasterViewModel> getAllInvoices()
        {           
                var eng_invoice_master = Eng360DB.eng_invoice_master.ToList();
                var lstInvoiceView = Mapper.Map<List<InvoiceMasterViewModel>>(eng_invoice_master);
                return lstInvoiceView;          
            
        }

        public int CreateInvoice(InvoiceMasterViewModel eng_invoice_master, List<QuoteDescriptionViewModel> quoteDescription)
        {
            try
            {
                    eng_invoice_master domainInv = Mapper.Map<eng_invoice_master>(eng_invoice_master);
                    domainInv.isFullyPaid = 0;
                    Eng360DB.eng_invoice_master.Add(domainInv);

                foreach (var des in quoteDescription)
                {
                    //insert 
                    var domainInvDesc = new eng_invoice_details();
                        
                        domainInvDesc.InvoiceID = domainInv.InvoiceID;
                        domainInvDesc.Quantity = des.Quantity;
                        domainInvDesc.Description = des.QuoteDescription;
                        domainInvDesc.UnitOfMeasure = des.UnitOfMeasure;
                        domainInvDesc.Price = des.QuotePrice;
                        Eng360DB.eng_invoice_details.Add(domainInvDesc);
                    }
                if (eng_invoice_master.InvoiceType == "Quotation")
                {
                    eng_quote_master eng_quote_master = Eng360DB.eng_quote_master.Find(eng_invoice_master.QuoteID);

                    eng_quote_master.InvoiceFlag = eng_invoice_master.Qflag;
                    Eng360DB.Entry(eng_quote_master).State = EntityState.Modified;
                }

                return Eng360DB.SaveChanges();
            }
            catch (Exception ex)
            {
                //logger
                return -1;
            }
        }

        public int SaveInvoice(InvoiceMasterViewModel eng_invoice_master, List<QuoteDescriptionViewModel> quoteDescription)
        {
            try
            {
                eng_invoice_master domainInv = Mapper.Map<eng_invoice_master>(eng_invoice_master);

                domainInv.isFullyPaid = 0;
                Eng360DB.Entry(domainInv).State = EntityState.Modified;

                var detOne = Eng360DB.eng_invoice_details.Where(a => a.InvoiceID == domainInv.InvoiceID).ToList();
                if (detOne != null)
                    foreach (var lst in detOne)
                    {
                        var dellist = Eng360DB.eng_invoice_details.First(a => a.InvoiceDescID == lst.InvoiceDescID);
                        Eng360DB.eng_invoice_details.Remove(dellist);

                    }

                foreach (var des in quoteDescription)
                {
                    //insert 
                    var domainInvDesc = new eng_invoice_details();

                    domainInvDesc.InvoiceID = domainInv.InvoiceID;
                    domainInvDesc.Quantity = des.Quantity;
                    domainInvDesc.Description = des.QuoteDescription;
                    domainInvDesc.UnitOfMeasure = des.UnitOfMeasure;
                    domainInvDesc.Price = des.QuotePrice;
                    Eng360DB.eng_invoice_details.Add(domainInvDesc);
                }
                if (eng_invoice_master.InvoiceType == "Quotation")
                {
                    eng_quote_master eng_quote_master = Eng360DB.eng_quote_master.Find(eng_invoice_master.QuoteID);

                    eng_quote_master.InvoiceFlag = eng_invoice_master.Qflag;
                    Eng360DB.Entry(eng_quote_master).State = EntityState.Modified;
                }

                return Eng360DB.SaveChanges();
            }
            catch (Exception ex)
            {
                //logger
                return -1;
            }
        }


        public InvoiceMasterViewModel getInvoice(int invID)
        {
            eng_invoice_master eng_invoice_master = Eng360DB.eng_invoice_master.Find(invID);

            return Mapper.Map<InvoiceMasterViewModel>(eng_invoice_master);
        }

        public List<InvoiceMasterViewModel> getAllClientInvoices(int id)
        {

            var listInvs = Eng360DB.eng_invoice_master.Where(a => a.ClientID == id && a.isFullyPaid == 0).ToList();

            List<InvoiceMasterViewModel> invoice = new List<InvoiceMasterViewModel>();

            foreach (var inv in listInvs)
            {
                    invoice.Add(new InvoiceMasterViewModel() { InvoiceNum = inv.InvoiceNum + "_" + "Quotation",    InvoiceType = id.ToString() + "_" + inv.InvoiceID.ToString() });
                
            }

            return invoice;
        }

        public List<InvoiceMasterViewModel> getAllOtherInvoices()
        {

            var listInvs = Eng360DB.eng_invoice_master.Where(a => a.ClientID == 0 && a.isFullyPaid == 0).ToList();

            List<InvoiceMasterViewModel> invoice = new List<InvoiceMasterViewModel>();

            foreach (var inv in listInvs)
            {
                invoice.Add(new InvoiceMasterViewModel() { InvoiceNum = inv.InvoiceNum + "_" + "Others", InvoiceType = inv.ClientID.ToString() + "_" + inv.InvoiceID.ToString() });

            }

            return invoice;
        }

        public List<InvoiceMasterViewModel> getFilterInvoices(FilterViewModel filter)
        {
            var dCurrentDayofThisMonth = DateTime.Today.ToString("yyyy-MM-dd");
            var dFirstDayOfThisMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            var dLastDayOfLastMonth = dFirstDayOfThisMonth.AddDays(-1).ToString("yyyy-MM-dd");
            var dFirstDayOfLastMonth = dFirstDayOfThisMonth.AddMonths(-1).ToString("yyyy-MM-dd"); ;
            var dFirstDayOfCurrentMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).ToString("yyyy-MM-dd");

            var sql = "select * from eng_invoice_master ";
            var where = "";
            if (filter.InvoiceNo != "Select")
                where = "InvoiceNum = '" + filter.InvoiceNo + "'";

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
                    where += " isFullyPaid =" + (filter.QuoteStatusID-1);
                else
                    where += " and isFullyPaid =" + (filter.QuoteStatusID-1);

            if (where != "")
                sql = sql + "Where " + where;


            // DbSqlQuery<eng_employee_profile> data = Eng360DB.eng_employee_profile.SqlQuery(sql);
            var data = Eng360DB.eng_invoice_master.SqlQuery(sql).ToList();

            var lstInvView = Mapper.Map<List<InvoiceMasterViewModel>>(data);
            return lstInvView;
        }



    }
}