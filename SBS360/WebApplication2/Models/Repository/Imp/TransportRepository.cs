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

namespace Eng360Web.Models.Repository.Interface
{
    public class TransportRepository : ITransportRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
         Logger logger = LogManager.GetCurrentClassLogger();

       public int CreateTransport(TransportViewModel transport)
        {
            var _db_transport = Mapper.Map<eng_transport_master>(transport);

           Eng360DB.eng_transport_master.Add(_db_transport);
           return Eng360DB.SaveChanges();
        }

        public int SaveTransport(TransportViewModel transport)
        {

            var _db_transport = Mapper.Map<eng_transport_master>(transport);
            Eng360DB.Entry(_db_transport).State = EntityState.Modified;

           return Eng360DB.SaveChanges();


        }

        public int DeleteTransport(int transportID)
        {
            var _db_transport = Eng360DB.eng_transport_master.First(a => a.TransportID == transportID);

            _db_transport.UpdatedBy = AppSession.GetCurrentUserId();
            _db_transport.UpdatedDate = DateTime.Now;
            _db_transport.IsActive = 0;

            Eng360DB.Entry(_db_transport).State = EntityState.Modified;
            return Eng360DB.SaveChanges();
        }

        public TransportViewModel getTransport(int transportID)
        {
            eng_transport_master eng_transport_master = Eng360DB.eng_transport_master.Find(transportID);

            return Mapper.Map<TransportViewModel>(eng_transport_master);
        }

        public List<TransportViewModel> getAllTransports()
        {
            var eng_transport_master = Eng360DB.eng_transport_master.ToList();
            var lstTransportView = Mapper.Map<List<TransportViewModel>>(eng_transport_master);
            return lstTransportView;
        }

        public List<TransportViewModel> getFilterTransports(FilterViewModel filter)
        {
            var sql = "select * from eng_transport_master ";
            var where = "";
            
            //if (filter.Transport_Name != "Select")
            //    if (where == "")
            //        where += " Transport_Name ='" + filter.Transport_Name + "'";
            //    else
            //        where += " and Transport_Name ='" + filter.Transport_Name + "'";

            //if (filter.Transport_Company_Name != "Select")
            //    if (where == "")
            //        where += " Transport_Company_Name ='" + filter.Transport_Company_Name + "'";
            //    else
            //        where += " and Transport_Company_Name ='" + filter.Transport_Company_Name + "'";

            //if (filter.Transport_Code != "Select")
            //    if (where == "")
            //        where += " Transport_Code ='" + filter.Transport_Code + "'";
            //    else
            //        where += " and Transport_Code ='" + filter.Transport_Code + "'";

            if (where != "")
                sql = sql + "Where " + where;

            // DbSqlQuery<eng_employee_profile> data = Eng360DB.eng_employee_profile.SqlQuery(sql);
            var data = Eng360DB.eng_transport_master.SqlQuery(sql).ToList();

            var pdtView = Mapper.Map<List<TransportViewModel>>(data);
            return pdtView;
        }

    }

}