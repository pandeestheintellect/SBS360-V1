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
    public class RARepository : IRARepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
         Logger logger = LogManager.GetCurrentClassLogger();


        public List<RATransMasterViewModel> getAllRiskAssessments()
        {
            var eng_ra_detail = Eng360DB.eng_ra_trans_master.ToList();
            var lstView = Mapper.Map<List<RATransMasterViewModel>>(eng_ra_detail);
            return lstView;
        }


        public List<RAMastersViewModel> getAllWorkActivities()
        {
            var eng_ra_detail = Eng360DB.eng_ra_work_activity.ToList();
            var lstView = Mapper.Map<List<RAMastersViewModel>>(eng_ra_detail);
            return lstView;
        }

        public List<RAMastersViewModel> getAllRAProcesses()
        {
            var eng_ra_detail = Eng360DB.eng_ra_process.ToList();
            var lstView = Mapper.Map<List<RAMastersViewModel>>(eng_ra_detail);
            return lstView;
        }

        public List<RAMastersViewModel> getAllLocations()
        {
            var eng_ra_detail = Eng360DB.eng_ra_location.ToList();
            var lstView = Mapper.Map<List<RAMastersViewModel>>(eng_ra_detail);
            return lstView;
        }

        public List<RAMastersViewModel> getAllHazardList()
        {
            var eng_ra_detail = Eng360DB.eng_ra_hazardlist.ToList();
            var lstView = Mapper.Map<List<RAMastersViewModel>>(eng_ra_detail);
            return lstView;
        }

        public List<RAMastersViewModel> getAllPAHs()
        {
            var eng_ra_detail = Eng360DB.eng_ra_possible_accident_health.ToList();
            var lstView = Mapper.Map<List<RAMastersViewModel>>(eng_ra_detail);
            return lstView;
        }

        public List<RAMastersViewModel> getAllControlMeasures()
        {
            var eng_ra_detail = Eng360DB.eng_ra_control_measures.ToList();
            var lstView = Mapper.Map<List<RAMastersViewModel>>(eng_ra_detail);
            return lstView;
        }
        

        public RAMastersViewModel getRAItem(string typ, int itemID)
        {
            if (typ == "RAWA")
            {
                eng_ra_work_activity eng_ra_detail = Eng360DB.eng_ra_work_activity.Find(itemID);
                return Mapper.Map<RAMastersViewModel>(eng_ra_detail);
            }
            if (typ == "RAPS")
            {
                eng_ra_process eng_ra_detail = Eng360DB.eng_ra_process.Find(itemID);
                return Mapper.Map<RAMastersViewModel>(eng_ra_detail);
            }
            if (typ == "RALN")
            {
                eng_ra_location eng_ra_detail = Eng360DB.eng_ra_location.Find(itemID);
                return Mapper.Map<RAMastersViewModel>(eng_ra_detail);
            }
            if (typ == "RAHZ")
            {
                eng_ra_hazardlist eng_ra_detail = Eng360DB.eng_ra_hazardlist.Find(itemID);
                return Mapper.Map<RAMastersViewModel>(eng_ra_detail);
            }
            if (typ == "RAPA")
            {
                eng_ra_possible_accident_health eng_ra_detail = Eng360DB.eng_ra_possible_accident_health.Find(itemID);
                return Mapper.Map<RAMastersViewModel>(eng_ra_detail);
            }
            if (typ == "RACM")
            {
                eng_ra_control_measures eng_ra_detail = Eng360DB.eng_ra_control_measures.Find(itemID);
                return Mapper.Map<RAMastersViewModel>(eng_ra_detail);
            }

            return null;
            
        }
        
        public int CreateRA(RAMastersViewModel ra)
        {
            if (ra.ramastertype == "RAWA")
            {
                var _db_ra = Mapper.Map<eng_ra_work_activity>(ra);
                Eng360DB.eng_ra_work_activity.Add(_db_ra);
            }
            if (ra.ramastertype == "RAPS")
            {
                var _db_ra = Mapper.Map<eng_ra_process>(ra);
                Eng360DB.eng_ra_process.Add(_db_ra);
            }
            if (ra.ramastertype == "RALN")
            {
                var _db_ra = Mapper.Map<eng_ra_location>(ra);
                Eng360DB.eng_ra_location.Add(_db_ra);
            }
            if (ra.ramastertype == "RAHZ")
            {
                var _db_ra = Mapper.Map<eng_ra_hazardlist>(ra);
                Eng360DB.eng_ra_hazardlist.Add(_db_ra);
            }
            if (ra.ramastertype == "RAPA")
            {
                var _db_ra = Mapper.Map<eng_ra_possible_accident_health>(ra);
                Eng360DB.eng_ra_possible_accident_health.Add(_db_ra);
            }
            if (ra.ramastertype == "RACM")
            {
                var _db_ra = Mapper.Map<eng_ra_control_measures>(ra);
                Eng360DB.eng_ra_control_measures.Add(_db_ra);
            }

            return Eng360DB.SaveChanges();
        }


        public int EditRA(RAMastersViewModel ra)
        {
            if (ra.ramastertype == "RAWA")
            {
                var _db_ra = Mapper.Map<eng_ra_work_activity>(ra);
                Eng360DB.Entry(_db_ra).State = EntityState.Modified;               
            }
            if (ra.ramastertype == "RAPS")
            {
                var _db_ra = Mapper.Map<eng_ra_process>(ra);
                Eng360DB.Entry(_db_ra).State = EntityState.Modified;                
            }
            if (ra.ramastertype == "RALN")
            {
                var _db_ra = Mapper.Map<eng_ra_location>(ra);
                Eng360DB.Entry(_db_ra).State = EntityState.Modified;               
            }
            if (ra.ramastertype == "RAHZ")
            {
                var _db_ra = Mapper.Map<eng_ra_hazardlist>(ra);
                Eng360DB.Entry(_db_ra).State = EntityState.Modified;                
            }
            if (ra.ramastertype == "RAPA")
            {
                var _db_ra = Mapper.Map<eng_ra_possible_accident_health>(ra);
                Eng360DB.Entry(_db_ra).State = EntityState.Modified;               
            }
            if (ra.ramastertype == "RACM")
            {
                var _db_ra = Mapper.Map<eng_ra_control_measures>(ra);
                Eng360DB.Entry(_db_ra).State = EntityState.Modified;               
            }           

            return Eng360DB.SaveChanges();
        }


        public int DeleteRA(string typ, int id)
        {
            if (typ == "RAWA")
            {
                var _db_ra = Eng360DB.eng_ra_work_activity.First(a => a.ItemID == id);
                Eng360DB.eng_ra_work_activity.Remove(_db_ra);
            }
            if (typ == "RAPS")
            {
                var _db_ra = Eng360DB.eng_ra_process.First(a => a.ItemID == id);
                Eng360DB.eng_ra_process.Remove(_db_ra);
            }
            if (typ == "RALN")
            {
                var _db_ra = Eng360DB.eng_ra_location.First(a => a.ItemID == id);
                Eng360DB.eng_ra_location.Remove(_db_ra);
            }
            if (typ == "RAHZ")
            {
                var _db_ra = Eng360DB.eng_ra_hazardlist.First(a => a.ItemID == id);
                Eng360DB.eng_ra_hazardlist.Remove(_db_ra);
            }
            if (typ == "RAPA")
            {
                var _db_ra = Eng360DB.eng_ra_possible_accident_health.First(a => a.ItemID == id);
                Eng360DB.eng_ra_possible_accident_health.Remove(_db_ra);
            }
            if (typ == "RACM")
            {
                var _db_ra = Eng360DB.eng_ra_control_measures.First(a => a.ItemID == id);
                Eng360DB.eng_ra_control_measures.Remove(_db_ra);
            }
            
            return Eng360DB.SaveChanges();
        }

        public int CreateRATrans(RATransMasterViewModel ra)
        {
                var _db_ra = Mapper.Map<eng_ra_trans_master>(ra);
                Eng360DB.eng_ra_trans_master.Add(_db_ra);

                return Eng360DB.SaveChanges();
        }

        public RATransMasterViewModel getRATransMaster(int raID)
        {
                eng_ra_trans_master eng_ra_detail = Eng360DB.eng_ra_trans_master.Find(raID);
                return Mapper.Map<RATransMasterViewModel>(eng_ra_detail);
        }

        public int EditRA(RATransMasterViewModel ra)
        {
            var _db_ra = Mapper.Map<eng_ra_trans_master>(ra);
            Eng360DB.Entry(_db_ra).State = EntityState.Modified;
            return Eng360DB.SaveChanges();
        }

        public int AddRADetailsOne(RATransMasterViewModel ra, List<RATransDetail1ViewModel> raDetails)
        {

            var detOne = Eng360DB.eng_ra_trans_detail1.Where(a => a.RAID == ra.RAID).ToList();
            if(detOne != null)
            foreach (var lst in detOne)
            {
                var dellist = Eng360DB.eng_ra_trans_detail1.First(a => a.RAWADetID == lst.RAWADetID);
                Eng360DB.eng_ra_trans_detail1.Remove(dellist);

            }

            foreach (var desc in raDetails)
            {
                
                eng_ra_trans_detail1 domainDesc = Mapper.Map<eng_ra_trans_detail1>(desc);
                domainDesc.RAID = ra.RAID;
                Eng360DB.eng_ra_trans_detail1.Add(domainDesc);
            }


            return Eng360DB.SaveChanges();
        }

        public List<RALikelihoodViewModel> getAllLikelihoods()
        {
            var eng_ra_detail = Eng360DB.eng_sys_rm_likelihood.ToList();
            var lstView = Mapper.Map<List<RALikelihoodViewModel>>(eng_ra_detail);
            return lstView;
        }

        public List<RASeverityViewModel> getAllSeverities()
        {
            var eng_ra_detail = Eng360DB.eng_sys_rm_severity.ToList();
            var lstView = Mapper.Map<List<RASeverityViewModel>>(eng_ra_detail);
            return lstView;
        }

        public List<RATransDetail1ViewModel> getAllProjectWorkActivities(int id)
        {
            var eng_ra_detail = Eng360DB.eng_ra_trans_detail1.Where(a=>a.RAID == id).ToList();
            var lstView = Mapper.Map<List<RATransDetail1ViewModel>>(eng_ra_detail);
            return lstView;
        }

        public int AddRACM(RATransMasterViewModel ra, List<RATransRACMViewModel> raDetails)
        {

            var racm = Eng360DB.eng_ra_trans_racm.Where(a => a.RAID == ra.RAID).ToList();
            if (racm != null)
                foreach (var lst in racm)
                {
                    var dellist = Eng360DB.eng_ra_trans_racm.First(a => a.RACMID == lst.RACMID);
                    Eng360DB.eng_ra_trans_racm.Remove(dellist);

                }

            foreach (var desc in raDetails)
            {
                if (!string.IsNullOrEmpty(desc.DueDate))
                    desc.DueDate = DateTime.ParseExact(desc.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                eng_ra_trans_racm domainDesc = Mapper.Map<eng_ra_trans_racm>(desc);
                domainDesc.RAID = ra.RAID;
                domainDesc.PreparedBy = AppSession.GetCurrentUserId().ToString();
                domainDesc.UpdatedDate = DateTime.Now;
                Eng360DB.eng_ra_trans_racm.Add(domainDesc);
            }


            return Eng360DB.SaveChanges();
        }

        public int AddRACMMobile(RATransRACMViewModel raDetails)
        {
            try
            {

                //if (!string.IsNullOrEmpty(raDetails.DueDate))
                //raDetails.DueDate = DateTime.ParseExact(raDetails.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                eng_ra_trans_racm domainDesc = Mapper.Map<eng_ra_trans_racm>(raDetails);
                domainDesc.RAID = raDetails.RAID;
                // domainDesc.PreparedBy = //AppSession.GetCurrentUserId().ToString();
                domainDesc.UpdatedDate = DateTime.Now;
                Eng360DB.eng_ra_trans_racm.Add(domainDesc);



                return Eng360DB.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
                return -1;
            }
        }

        public List<MobileRATransRACMViewModel> GetRaDetails(int id)
        {
            var obj = Eng360DB.eng_ra_trans_racm.Where(a => a.RAID == id).ToList();
            var reternObj = Mapper.Map<List<MobileRATransRACMViewModel>>(obj);
            if (reternObj != null)
            {
                foreach (var ra in reternObj)
                {
                    ra.DueDate = DateTime.Parse(ra.DueDate).ToString("dd/MM/yyyy");
                }
            }

            return reternObj;
        }



    }

}