using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eng360Web.Models.Domain;
using AutoMapper;
using System.Data.Entity;
using Eng360Web.Models.Utility;
using System.Globalization;

namespace Eng360Web.Models.Repository.Imp
{
    public class PoRepository : IPoRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
         Logger logger = LogManager.GetCurrentClassLogger();

       public int CreatePo(PoViewModel eng_po_master, List<PoDescriptionViewModel> poDescription)
        {
            try
            {
                eng_po_master domainPo = Mapper.Map<eng_po_master>(eng_po_master);
                Eng360DB.eng_po_master.Add(domainPo);

                foreach (var desc in poDescription)
                {
                    desc.AddedBy = AppSession.GetCurrentUserId();
                    desc.AddedDate = DateTime.Now;
                    eng_po_description domainDesc = Mapper.Map<eng_po_description>(desc);
                    domainDesc.PoID = domainPo.PoID;
                    Eng360DB.eng_po_description.Add(domainDesc);


                }


                return Eng360DB.SaveChanges();
            }
            catch (Exception ex)
            {
                //logger
                return -1;
            }
        }

        public int SavePo(PoViewModel eng_po_master, List<PoDescriptionViewModel> poDescription, List<int> deleted)
        {

            try
            {
                var domainPo = Mapper.Map<eng_po_master>(eng_po_master);
                domainPo.isFullyPaid = 0;
                Eng360DB.Entry(domainPo).State = EntityState.Modified;
                foreach (var desc in poDescription)
                {
                    //insert 
                    if (desc.PDID == 0)
                    {
                        desc.AddedBy = AppSession.GetCurrentUserId();
                        desc.AddedDate = DateTime.Now;
                        desc.PoID = domainPo.PoID;
                        eng_po_description domainDesc = Mapper.Map<eng_po_description>(desc);
                        Eng360DB.eng_po_description.Add(domainDesc);
                    }
                    if (desc.PDID > 0)
                    {
                        desc.UpdatedBy = AppSession.GetCurrentUserId();
                        desc.UpdatedDate = DateTime.Now;
                        desc.PoID = domainPo.PoID;
                        eng_po_description domainDesc = Mapper.Map<eng_po_description>(desc);
                        Eng360DB.Entry(domainDesc).State = EntityState.Modified;
                    }
                    //edit
                }
                if (deleted != null)
                    foreach (var deleteDesc in deleted)
                    {
                        var willbeDeleted = Eng360DB.eng_po_description.Find(deleteDesc);
                        Eng360DB.eng_po_description.Remove(willbeDeleted);
                    }


                return Eng360DB.SaveChanges();
                
                

            }
            catch (Exception ex)
            {
                //logger
                return -1;
            }


        }

        public int DeletePo(int poID)
        {
            var _db_po = Eng360DB.eng_po_master.First(a => a.PoID == poID);

            Eng360DB.eng_po_master.Remove(_db_po);

            return Eng360DB.SaveChanges();
        }

        public PoViewModel getPo(int poID)
        {
            eng_po_master eng_po_master = Eng360DB.eng_po_master.Find(poID);

            return Mapper.Map<PoViewModel>(eng_po_master);
        }

        public List<PoViewModel> getAllPos()
        {
            var eng_po_master = Eng360DB.eng_po_master.ToList();
            var lstPoView = Mapper.Map<List<PoViewModel>>(eng_po_master);
            return lstPoView;
        }

        public List<PoViewModel> getAllPayPos(int id)
        {

            var listPos = Eng360DB.eng_po_master.Where(a => a.SupplierID == id && a.isFullyPaid == 0).ToList();

            List<PoViewModel> povm = new List<PoViewModel>();

            foreach (var po in listPos)
            {

                povm.Add(new PoViewModel() {  PoRefNum = po.PoRefNum + "_(" + po.FinalAmount + ")", SupplierPO = id.ToString() + "_" + po.PoID.ToString() });

            }

            return povm;
        }


        public List<QuoteStatusViewModel> getStatus()
        {

            var obStatus = Eng360DB.eng_sys_quotestatus.Where(a=>a.Selection ==1).ToList();

            return Mapper.Map<List<QuoteStatusViewModel>>(obStatus);
        }

        public PoDescriptionViewModel getDesc(int? id)
        {

            var domainDesc = Eng360DB.eng_po_description.Find(id);
            return Mapper.Map<PoDescriptionViewModel>(domainDesc);
        }

        public List<PoViewModel> getFilterPos(FilterViewModel filter)
        {
            var dCurrentDayofThisMonth = DateTime.Today.ToString("yyyy-MM-dd");
            var dFirstDayOfThisMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            var dLastDayOfLastMonth = dFirstDayOfThisMonth.AddDays(-1).ToString("yyyy-MM-dd");
            var dFirstDayOfLastMonth = dFirstDayOfThisMonth.AddMonths(-1).ToString("yyyy-MM-dd"); ;
            var dFirstDayOfCurrentMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).ToString("yyyy-MM-dd");

            var sql = "select * from eng_po_master ";
            var where = "";
            if (filter.PoRefNum != "Select")
                where = "PoRefNum = '" + filter.PoRefNum + "'";

            if (filter.SupplierID > 0)
                if (where == "")
                    where += " SupplierID =" + filter.SupplierID;
                else
                    where += " and SupplierID =" + filter.SupplierID;

            if (filter.Month == 0)
            {
                if (filter.dateFrom != null)
                {
                    filter.dateFrom = DateTime.ParseExact(filter.dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " PoDate >='" + filter.dateFrom + "'";
                    else
                        where += " and PoDate >='" + filter.dateFrom + "'";
                }
                if (filter.dateTo != null)
                {
                    filter.dateTo = DateTime.ParseExact(filter.dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " PoDate <='" + filter.dateTo + "'";
                    else
                        where += " and PoDate <='" + filter.dateTo + "'";
                }
            }

            if (filter.Month == 1)
            {

                if (where == "")
                    where += " PoDate >= '" + dFirstDayOfCurrentMonth + "' and PoDate <= '" + dCurrentDayofThisMonth + "'";
                else
                    where += " and PoDate >='" + dFirstDayOfCurrentMonth + "' and PoDate <='" + dCurrentDayofThisMonth + "'";

            }

            if (filter.Month == 2)
            {

                if (where == "")
                    where += " PoDate >='" + dFirstDayOfLastMonth + "' and PoDate <='" + dLastDayOfLastMonth + "'";
                else
                    where += " and PoDate >='" + dFirstDayOfLastMonth + "' and PoDate <='" + dLastDayOfLastMonth + "'";

            }

            
            if (where != "")
                sql = sql + "Where " + where;


            // DbSqlQuery<eng_employee_profile> data = Eng360DB.eng_employee_profile.SqlQuery(sql);
            var data = Eng360DB.eng_po_master.SqlQuery(sql).ToList();

            var lstPoView = Mapper.Map<List<PoViewModel>>(data);
            return lstPoView;
        }


    }

}