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

namespace Eng360Web.Models.Repository.Interface
{
    public class MaterialManagementRepository : IMaterialManagementRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
         Logger logger = LogManager.GetCurrentClassLogger();

       public int CreateStore(StoreMasterViewModel store)
        {
            var _db_store = Mapper.Map<eng_store_master>(store);

           Eng360DB.eng_store_master.Add(_db_store);
           return Eng360DB.SaveChanges();
        }

        public int SaveStore(StoreMasterViewModel store)
        {

            var _db_store = Mapper.Map<eng_store_master>(store);
            Eng360DB.Entry(_db_store).State = EntityState.Modified;

           return Eng360DB.SaveChanges();


        }        

        public StoreMasterViewModel getStore(int storeID)
        {
            eng_store_master eng_store_master = Eng360DB.eng_store_master.Find(storeID);

            return Mapper.Map<StoreMasterViewModel>(eng_store_master);
        }

        public MMInwardViewModel getInward(int inwID)
        {
            eng_inward eng_inward = Eng360DB.eng_inward.Find(inwID);

            return Mapper.Map<MMInwardViewModel>(eng_inward);
        }

        public MMOutwardViewModel getOutward(int ouwID)
        {
            eng_outward eng_outward = Eng360DB.eng_outward.Find(ouwID);

            return Mapper.Map<MMOutwardViewModel>(eng_outward);
        }


        public MMStockAdjustViewModel getStockAdjust(int sajID)
        {
            eng_mm_stockadj_master eng_stockadj = Eng360DB.eng_mm_stockadj_master.Find(sajID);

            return Mapper.Map<MMStockAdjustViewModel>(eng_stockadj);
        }


        public List<StoreMasterViewModel> getAllStores()
        {
            var eng_store_master = Eng360DB.eng_store_master.ToList();
            var lstStoreView = Mapper.Map<List<StoreMasterViewModel>>(eng_store_master);
            return lstStoreView;
        }

        public List<MMInwardViewModel> getAllInwards()
        {
            var eng_inward = Eng360DB.eng_inward.ToList();
            var lstInwardView = Mapper.Map<List<MMInwardViewModel>>(eng_inward);
            return lstInwardView;
        }

        public List<MMOutwardViewModel> getAllOutwards()
        {
            var eng_outward = Eng360DB.eng_outward.ToList();
            var lstOutwardView = Mapper.Map<List<MMOutwardViewModel>>(eng_outward);
            return lstOutwardView;
        }

        public List<MMStockAdjustViewModel> getAllStockAdjusts()
        {
            var eng_stockadj = Eng360DB.eng_mm_stockadj_master.ToList();
            var lstStockAdjView = Mapper.Map<List<MMStockAdjustViewModel>>(eng_stockadj);
            return lstStockAdjView;
        }


        public int CreateInward(MMInwardViewModel eng_inward, List<MMInwardDescViewModel> inwardDescription)
        {
            try
            {
                
                    eng_inward domainInward = Mapper.Map<eng_inward>(eng_inward);
                    Eng360DB.eng_inward.Add(domainInward);
           
                foreach (var desc in inwardDescription)
                    {
                        
                        eng_mm_inwdesc domainDesc = Mapper.Map<eng_mm_inwdesc>(desc);
                        domainDesc.Inward_ID = domainInward.Inward_ID;
                        
                        Eng360DB.eng_mm_inwdesc.Add(domainDesc);

                     }
                if (eng_inward.DraftFlag == 1)
                {
                    Eng360DB.SaveChanges();
                    var inw = domainInward.Inward_ID;

                    foreach (var desc in inwardDescription)
                    {
                        var mmtrmas = new MMTransMasterViewModel();
                        mmtrmas.ProductID = desc.ProductID;
                        mmtrmas.Quantity = desc.Quantity;
                        mmtrmas.UoM = desc.UoM;
                        mmtrmas.inoutadj_ref = "INW-" + inw;

                        eng_mm_trmaster trmDesc = Mapper.Map<eng_mm_trmaster>(mmtrmas);
                        //trmDesc.inoutadj_ref = "INW-" + domainInward.Inward_ID;
                        trmDesc.StoreID = domainInward.StoreID;
                        trmDesc.Trn_Date = domainInward.Received_Date;

                        Eng360DB.eng_mm_trmaster.Add(trmDesc);

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


        public int SaveInward(MMInwardViewModel eng_inward, List<MMInwardDescViewModel> inwardDescription)
        {
            try
            {
                
                var domainInward = Mapper.Map<eng_inward>(eng_inward);
                Eng360DB.Entry(domainInward).State = EntityState.Modified;
  
                        List<eng_mm_inwdesc> list = Eng360DB.eng_mm_inwdesc.Where(s => s.Inward_ID == eng_inward.Inward_ID).ToList();
                        Eng360DB.eng_mm_inwdesc.RemoveRange(list);
           

                foreach (var desc in inwardDescription)
                {

                    eng_mm_inwdesc domainDesc = Mapper.Map<eng_mm_inwdesc>(desc);
                    domainDesc.Inward_ID = domainInward.Inward_ID;

                    Eng360DB.eng_mm_inwdesc.Add(domainDesc);

                }

                if (eng_inward.DraftFlag == 1)
                {
                   
                    var inw = eng_inward.Inward_ID;

                    foreach (var desc in inwardDescription)
                    {
                        var mmtrmas = new MMTransMasterViewModel();
                        mmtrmas.ProductID = desc.ProductID;
                        mmtrmas.Quantity = desc.Quantity;
                        mmtrmas.UoM = desc.UoM;
                        mmtrmas.inoutadj_ref = "INW-" + inw;

                        eng_mm_trmaster trmDesc = Mapper.Map<eng_mm_trmaster>(mmtrmas);
                        //trmDesc.inoutadj_ref = "INW-" + domainInward.Inward_ID;
                        trmDesc.StoreID = domainInward.StoreID;
                        trmDesc.Trn_Date = domainInward.Received_Date;

                        Eng360DB.eng_mm_trmaster.Add(trmDesc);

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



        public int CreateOutward(MMOutwardViewModel eng_outward, List<MMOutwardDescViewModel> outwardDescription)
        {
            try
            {

                eng_outward domainOutward = Mapper.Map<eng_outward>(eng_outward);
                Eng360DB.eng_outward.Add(domainOutward);

                foreach (var desc in outwardDescription)
                {

                    eng_mm_outdesc domainDesc = Mapper.Map<eng_mm_outdesc>(desc);
                    domainDesc.Outward_ID = domainOutward.Outward_ID;

                    Eng360DB.eng_mm_outdesc.Add(domainDesc);

                }

                if (eng_outward.DraftFlag == 1)
                {
                    Eng360DB.SaveChanges();
                    var ouw = domainOutward.Outward_ID;

                    foreach (var desc in outwardDescription)
                    {
                        var mmtrmas = new MMTransMasterViewModel();
                        mmtrmas.ProductID = desc.ProductID;
                        mmtrmas.Quantity = desc.Quantity * -1;
                        mmtrmas.UoM = desc.UoM;
                        mmtrmas.inoutadj_ref = "OUW-" + ouw;

                        eng_mm_trmaster trmDesc = Mapper.Map<eng_mm_trmaster>(mmtrmas);
                        //trmDesc.inoutadj_ref = "INW-" + domainInward.Inward_ID;
                        trmDesc.StoreID = domainOutward.StoreID;
                        trmDesc.Trn_Date = domainOutward.Delivery_Date;

                        Eng360DB.eng_mm_trmaster.Add(trmDesc);

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


        public int SaveOutward(MMOutwardViewModel eng_outward, List<MMOutwardDescViewModel> outwardDescription)
        {
            try
            {

                var domainOutward = Mapper.Map<eng_outward>(eng_outward);
                Eng360DB.Entry(domainOutward).State = EntityState.Modified;

                List<eng_mm_outdesc> list = Eng360DB.eng_mm_outdesc.Where(s => s.Outward_ID == eng_outward.Outward_ID).ToList();
                Eng360DB.eng_mm_outdesc.RemoveRange(list);
                

                foreach (var desc in outwardDescription)
                {

                    eng_mm_outdesc domainDesc = Mapper.Map<eng_mm_outdesc>(desc);
                    domainDesc.Outward_ID = domainOutward.Outward_ID;

                    Eng360DB.eng_mm_outdesc.Add(domainDesc);

                }

                if (eng_outward.DraftFlag == 1)
                {
                   
                    var ouw = eng_outward.Outward_ID;
                                        
                    foreach (var desc in outwardDescription)
                    {
                        var mmtrmas = new MMTransMasterViewModel();
                        mmtrmas.ProductID = desc.ProductID;
                        mmtrmas.Quantity = desc.Quantity * -1;
                        mmtrmas.UoM = desc.UoM;
                        mmtrmas.inoutadj_ref = "OUW-" + ouw;

                        eng_mm_trmaster trmDesc = Mapper.Map<eng_mm_trmaster>(mmtrmas);
                        //trmDesc.inoutadj_ref = "INW-" + domainInward.Inward_ID;
                        trmDesc.StoreID = domainOutward.StoreID;
                        trmDesc.Trn_Date = domainOutward.Delivery_Date;

                        Eng360DB.eng_mm_trmaster.Add(trmDesc);

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

        public int GetCurrentStock(int pid, int storeid)
        {
            int cnt = Eng360DB.eng_mm_trmaster.Where(a => a.ProductID == pid && a.StoreID == storeid).Sum(a=>a.Quantity) ?? default(int);

            return cnt;
        }

        public int CreateStockAdjust(MMStockAdjustViewModel eng_stockadj)
        {
            try
            {

                eng_mm_stockadj_master domainStockAdj = Mapper.Map<eng_mm_stockadj_master>(eng_stockadj);
                Eng360DB.eng_mm_stockadj_master.Add(domainStockAdj);
                Eng360DB.SaveChanges();

                var ouw = domainStockAdj.StockAdjID;
                         
                        var mmtrmas = new MMTransMasterViewModel();
                        mmtrmas.ProductID = eng_stockadj.ProductID;
                if (eng_stockadj.AdjType == 1)
                {
                    mmtrmas.Quantity = (int)eng_stockadj.Quantity * -1;
                }
                else
                {
                    mmtrmas.Quantity = (int) eng_stockadj.Quantity;
                }
                        mmtrmas.UoM = eng_stockadj.Measuring_Unit;
                        mmtrmas.inoutadj_ref = "SAJ-" + ouw;

                        eng_mm_trmaster trmDesc = Mapper.Map<eng_mm_trmaster>(mmtrmas);
                        
                        trmDesc.StoreID = eng_stockadj.StoreID;
                        trmDesc.Trn_Date = domainStockAdj.Stock_Taking_Date;

                        Eng360DB.eng_mm_trmaster.Add(trmDesc);

                    
                

                return Eng360DB.SaveChanges();
            }
            catch (Exception ex)
            {
                //logger
                return -1;
            }
        }




        public List<MMStockViewModel> getStock()
        {

            var eng_stock = Eng360DB.GetStock().ToList();

            var stockView  = Mapper.Map<List<MMStockViewModel>>(eng_stock);


            return stockView;
        }

        public List<MMInwardViewModel> getFilterInwardReports(FilterViewModel filter)
        {            
            var sql = "select * from eng_inward ";
            var where = "";
                       
                if (filter.dateFrom != null)
                {
                    filter.dateFrom = DateTime.ParseExact(filter.dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " Received_Date >='" + filter.dateFrom + "'";
                    else
                        where += " and Received_Date >='" + filter.dateFrom + "'";
                }
                if (filter.dateTo != null)
                {
                    filter.dateTo = DateTime.ParseExact(filter.dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " Received_Date <='" + filter.dateTo + "'";
                    else
                        where += " and Received_Date <='" + filter.dateTo + "'";
                }

            if (where != "")
                sql = sql + "Where " + where;

            var data = Eng360DB.eng_inward.SqlQuery(sql).ToList();

            var lstprView = Mapper.Map<List<MMInwardViewModel>>(data);
            return lstprView;
        }


        public List<MMOutwardViewModel> getFilterOutwardReports(FilterViewModel filter)
        {
            var sql = "select * from eng_outward ";
            var where = "";

            if (filter.dateFrom != null)
            {
                filter.dateFrom = DateTime.ParseExact(filter.dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                if (where == "")
                    where += " Delivery_Date >='" + filter.dateFrom + "'";
                else
                    where += " and Delivery_Date >='" + filter.dateFrom + "'";
            }
            if (filter.dateTo != null)
            {
                filter.dateTo = DateTime.ParseExact(filter.dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                if (where == "")
                    where += " Delivery_Date <='" + filter.dateTo + "'";
                else
                    where += " and Delivery_Date <='" + filter.dateTo + "'";
            }

            if (where != "")
                sql = sql + "Where " + where;

            var data = Eng360DB.eng_outward.SqlQuery(sql).ToList();

            var lstprView = Mapper.Map<List<MMOutwardViewModel>>(data);
            return lstprView;
        }



    }

}