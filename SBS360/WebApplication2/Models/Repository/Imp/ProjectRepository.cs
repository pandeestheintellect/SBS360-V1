using AutoMapper;
using Eng360Web.Models.Domain;
using Eng360Web.Models.Repository.Interface;
using Eng360Web.Models.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using Eng360Web.Models.Utility;
using System.Globalization;

namespace Eng360Web.Models.Repository.Imp
{
    public class ProjectRepository : IProjectRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
        Logger logger = LogManager.GetCurrentClassLogger();


        public int CreateProject(ProjectViewModel eng_project_master)
        {

            var _db_Project = Mapper.Map<eng_project_master>(eng_project_master);

            Eng360DB.eng_project_master.Add(_db_Project);

            return Eng360DB.SaveChanges();


        }

        public int SaveProject(ProjectViewModel eng_project_master)
        {
            var _db_project = Mapper.Map<eng_project_master>(eng_project_master);
            Eng360DB.Entry(_db_project).State = EntityState.Modified;


            return Eng360DB.SaveChanges();
        }

        public ProjectViewModel getProject(int id)
        {
            var project = Eng360DB.eng_project_master.Where(a => a.ProjectID == id).SingleOrDefault();
            return Mapper.Map<ProjectViewModel>(project);
        }

        public List<ProjectStatusViewModel> getProjectStatus()
        {
            var status = Eng360DB.eng_sys_project_status.ToList();

            return Mapper.Map<List<ProjectStatusViewModel>>(status);

        }

        public List<LocationViewModel> getAllLocations()
        {
            var eng_sys_location = Eng360DB.eng_sys_location.ToList();
            var lstLocationView = Mapper.Map<List<LocationViewModel>>(eng_sys_location);
            return lstLocationView;
        }

        public List<ProjectViewModel> getAllProjects()
        {
            var eng_project_master = Eng360DB.eng_project_master.ToList();
            var lstProjectView = Mapper.Map<List<ProjectViewModel>>(eng_project_master);
            return lstProjectView;
        }

        public int UpdateInvDoDate(string invDate, string doDate, int invReleased, int Projectid, string typeChk)
        {
            eng_project_master eng_project_master = Eng360DB.eng_project_master.Find(Projectid);

            if (!string.IsNullOrEmpty(invDate))
                invDate = DateTime.ParseExact(invDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            if (!string.IsNullOrEmpty(doDate))
                doDate = DateTime.ParseExact(doDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            if (typeChk == "WC")
            {
                eng_project_master.DoDate = Convert.ToDateTime(doDate);

                if (invReleased > 0)
                {
                    eng_project_master.Is_Project_level_do = invReleased;
                   
                }

                if (invReleased > 0)
                {
                    eng_quote_master eng_quote_master = Eng360DB.eng_quote_master.Find(eng_project_master.QuotationID);

                    eng_quote_master.Is_do_released = 1;
                    eng_quote_master.Is_Quote_level_do = 0; 
                    Eng360DB.Entry(eng_quote_master).State = EntityState.Modified;
                    
                    eng_do_seqnum doseqnum = new Domain.eng_do_seqnum();
                    doseqnum.DoNo = "CITI-CQ-WC-000";
                    doseqnum.DoType = "P";
                    doseqnum.id = Projectid;
                    Eng360DB.eng_do_seqnum.Add(doseqnum);
                }

            }

            if (typeChk == "IV" || typeChk == "IVwoDO")
            {

                eng_project_master.InvoiceDate = Convert.ToDateTime(invDate);

                eng_project_master.isFullyPaid = 0;
                if (invReleased > 0)
                {
                    eng_project_master.Is_Project_level_inv = 1;
                }

                Eng360DB.Entry(eng_project_master).State = EntityState.Modified;

                if (invReleased > 0)
                {
                    eng_quote_master eng_quote_master = Eng360DB.eng_quote_master.Find(eng_project_master.QuotationID);

                    eng_quote_master.Is_invoice_released = 1;
                    eng_quote_master.Is_Project_level_inv = 1;
                    eng_quote_master.isFullyPaid = 0;

                    Eng360DB.Entry(eng_quote_master).State = EntityState.Modified;

                    eng_inv_seqnum invseqnum = new Domain.eng_inv_seqnum();
                    invseqnum.InvoiceNo = "CITI-CQ-INV-000";
                    if (typeChk == "IV")
                        invseqnum.InvoiceType = "P";
                    else
                        invseqnum.InvoiceType = "PwoDO";

                    invseqnum.id = Projectid;
                    Eng360DB.eng_inv_seqnum.Add(invseqnum);

                }
            }
           

            return Eng360DB.SaveChanges();
        }

        public int CreateProjectInvoice(int ProjectID, string InvoiceDate, int QuotationID, List<QuoteDescriptionViewModel> desc, List<int> deleted, decimal finalAmount)
        {

            try
            {
                if (!string.IsNullOrEmpty(InvoiceDate))
                    InvoiceDate = DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


                var domainInvoice = new eng_custom_invoice();
                domainInvoice.InvoiceDate = Convert.ToDateTime(InvoiceDate);
                domainInvoice.ProjectID = ProjectID;
                domainInvoice.QuotationID = QuotationID;
                domainInvoice.DODate = Convert.ToDateTime(InvoiceDate);
                domainInvoice.isFullyPaid = 0;
                domainInvoice.FinalInvoiceAmount = finalAmount;
                Eng360DB.eng_custom_invoice.Add(domainInvoice);
                               
                foreach (var des in desc)
                {
                    //insert 
                    if (des.QDID == 0)
                    {
                        var domainInvDesc = new eng_custom_invoice_details();

                        domainInvDesc.AddedBy = AppSession.GetCurrentUserId();
                        domainInvDesc.AddedDate = DateTime.Now;
                        domainInvDesc.InvoiceID = domainInvoice.InvoiceID;
                        domainInvDesc.Quantity = des.Quantity;
                        domainInvDesc.QuoteDescription = des.QuoteDescription;
                        domainInvDesc.UnitOfMeasure = des.UnitOfMeasure;
                        domainInvDesc.QuotePrice = des.QuotePrice;

                        Eng360DB.eng_custom_invoice_details.Add(domainInvDesc);
                    }
                    if (des.QDID > 0)
                    {

                        var domainInvDesc = Eng360DB.eng_custom_invoice_details.Find(des.QDID);

                        domainInvDesc.UpdatedBy = AppSession.GetCurrentUserId();
                        domainInvDesc.UpdatedDate = DateTime.Now;
                        domainInvDesc.Quantity = des.Quantity;
                        domainInvDesc.QuoteDescription = des.QuoteDescription;
                        domainInvDesc.UnitOfMeasure = des.UnitOfMeasure;
                        domainInvDesc.QuotePrice = des.QuotePrice;

                        Eng360DB.Entry(domainInvDesc).State = EntityState.Modified;
                    }
                    //edit
                }

                //if (deleted != null)
                //    foreach (var deleteDesc in deleted)
                //    {
                //        var willbeDeleted = Eng360DB.eng_custom_invoice_details.Find(deleteDesc);
                //        if (willbeDeleted != null)
                //            Eng360DB.eng_custom_invoice_details.Remove(willbeDeleted);
                //    }

                eng_project_master eng_project_master = Eng360DB.eng_project_master.Find(ProjectID);

                eng_quote_master eng_quote_master = eng_project_master.eng_quote_master;

                eng_project_master.Is_Custom_level_inv = 1;
                Eng360DB.Entry(eng_project_master).State = EntityState.Modified;


                eng_quote_master.Is_invoice_released = 1;
                eng_quote_master.Is_Custom_level_inv = 1;
                eng_quote_master.isFullyPaid = 0;
                Eng360DB.Entry(eng_quote_master).State = EntityState.Modified;

               

                var result = Eng360DB.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public List<CustomInvoiceViewModel> getAllCustomInvoice(int? id)
        {
            List<CustomInvoiceViewModel> returnList = new List<CustomInvoiceViewModel>();

            var invList = Eng360DB.eng_custom_invoice.Where(a => a.ProjectID == id).ToList();
            foreach (var inv in invList)
            {
                var project = Eng360DB.eng_project_master.Find(inv.ProjectID);
                returnList.Add(new CustomInvoiceViewModel()
                {
                    ProjectNo = project.ProjectNo,
                    QuoteNo = project.eng_quote_master.QuoteRefNum,

                    InvoiceNo = inv.InvoiceNo,
                    InvoiceDate = Convert.ToDateTime(inv.InvoiceDate).ToString("dd/MM/yyyy"),
                    InvoiceID = inv.InvoiceID
                });
            }

            return returnList;

        }

        public List<CustomInvoiceViewModel> getAllCustomQuoteInvoice(int? id)
        {
            List<CustomInvoiceViewModel> returnList = new List<CustomInvoiceViewModel>();

            var invList = Eng360DB.eng_custom_invoice.Where(a => a.QuotationID == id).ToList();
           
            foreach (var inv in invList)
            {
               
                returnList.Add(new CustomInvoiceViewModel()
                {
                    InvoiceNo = inv.InvoiceNo,
                    InvoiceDate = Convert.ToDateTime(inv.InvoiceDate).ToString("dd/MM/yyyy"),
                    InvoiceID = inv.InvoiceID
                });
            }

            return returnList;

        }


        public CustomInvoiceViewModel getCustomInvoice(int? id)
        {
            var domainInv = Eng360DB.eng_custom_invoice.Find(id);

            CustomInvoiceViewModel returnOnject = new CustomInvoiceViewModel()
            {
                ProjectID = domainInv.ProjectID,
                QuotationID = domainInv.QuotationID,

                InvoiceNo = domainInv.InvoiceNo,
                InvoiceDate = Convert.ToDateTime(domainInv.InvoiceDate).ToString("MM/dd/yyyy"),
                InvoiceID = domainInv.InvoiceID,
                DoNo = domainInv.DoNo
            };
            returnOnject.eng_quote_description = new List<QuoteDescriptionViewModel>();
            foreach (var des in domainInv.eng_custom_invoice_details)
            {
                returnOnject.eng_quote_description.Add(new QuoteDescriptionViewModel()
                {
                    Quantity = des.Quantity ?? default(int),
                    QuoteDescription = des.QuoteDescription,
                    UnitOfMeasure = des.UnitOfMeasure,
                    QuotePrice = des.QuotePrice
                });



            }
            return returnOnject;
        }

        public int DeleteCustomInvoice(int invoiceID)
        {
            var _db_custinv = Eng360DB.eng_custom_invoice.First(a => a.InvoiceID == invoiceID);

            Eng360DB.eng_custom_invoice.Remove(_db_custinv);
            return Eng360DB.SaveChanges();
        }

        public List<ProjectViewModel> getFilterProjects(FilterViewModel filter)
        {
            var dFirstDayOfThisYear = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM-dd");
            var dCurrentDayofThisYear = DateTime.Today.ToString("yyyy-MM-dd");
            var dFirstDayOfCurrentYear = new DateTime(DateTime.Now.Year, 1, 1);

            var dLastDayOfLastYear = dFirstDayOfCurrentYear.AddDays(-1).ToString("yyyy-MM-dd");
            var dFirstDayOfLastYear = dFirstDayOfCurrentYear.AddYears(-1).ToString("yyyy-MM-dd");


            var sql = "select * from eng_project_master ";
            var where = "";

            var newsql = "(select QuoteID from eng_quote_master inner join eng_client_master on eng_quote_master.ClientID = eng_client_master.ClientID where eng_quote_master.ClientID = " + filter.ClientID + ")";

            if (filter.ProjectID > 0)
                where = "ProjectID = " + filter.ProjectID;

            if (filter.ClientID > 0)
            {

                if (where == "")
                    where += " QuotationID in " + newsql;
                else
                    where += " and QuotationID in " + newsql;
            }

            if (filter.Year == 0)
            {
                if (filter.dateFrom != null)
                {
                    filter.dateFrom = DateTime.ParseExact(filter.dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " Start_Date >='" + filter.dateFrom + "'";
                    else
                        where += " and Start_Date >='" + filter.dateFrom + "'";
                }
                if (filter.dateTo != null)
                {
                    filter.dateTo = DateTime.ParseExact(filter.dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " Start_Date <='" + filter.dateTo + "'";
                    else
                        where += " and Start_Date <='" + filter.dateTo + "'";
                }
            }

            if (filter.Year == 1)
            {

                if (where == "")
                    where += " Start_Date >= '" + dFirstDayOfThisYear + "' and Start_Date <= '" + dCurrentDayofThisYear + "'";
                else
                    where += " and Start_Date >='" + dFirstDayOfThisYear + "' and Start_Date <='" + dCurrentDayofThisYear + "'";

            }

            if (filter.Year == 2)
            {

                if (where == "")
                    where += " Start_Date >='" + dFirstDayOfLastYear + "' and Start_Date <='" + dLastDayOfLastYear + "'";
                else
                    where += " and Start_Date >='" + dFirstDayOfLastYear + "' and Start_Date <='" + dLastDayOfLastYear + "'";

            }

            if (filter.Project_Status_ID > 0)
                if (where == "")
                    where += " Project_Status_ID =" + filter.Project_Status_ID;
                else
                    where += " and Project_Status_ID =" + filter.Project_Status_ID;

            if (where != "")
                sql = sql + "Where " + where;


            // DbSqlQuery<eng_employee_profile> data = Eng360DB.eng_employee_profile.SqlQuery(sql);
            var data = Eng360DB.eng_project_master.SqlQuery(sql).ToList();

            var lstProjectView = Mapper.Map<List<ProjectViewModel>>(data);
            return lstProjectView;
        }



    }
}