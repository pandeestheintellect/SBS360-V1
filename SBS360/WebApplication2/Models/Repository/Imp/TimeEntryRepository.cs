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
    public class TimeEntryRepository : ITimeEntryRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
        Logger logger = LogManager.GetCurrentClassLogger();

        public int CreateTimeEntry(TimeEntryViewModel timeentry)
        {
            //var _db_timeentry = Mapper.Map<eng_time_entry>(timeentry);

            //Eng360DB.eng_time_entry.Add(_db_timeentry);
            //return Eng360DB.SaveChanges();

            DateTime dateOfEntry = DateTime.ParseExact(timeentry.ReportDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            var exsitingTimeEntry = Eng360DB.eng_time_entry.Where(te => te.ReportDate == dateOfEntry && te.EmpID == timeentry.EmpID).FirstOrDefault();

            //(from te in Eng360DB.eng_time_entry
            //           where
            //          te.ReportDate == dateOfEntry && te.EmpID == timeentry.EmpID
            //           select te).FirstOrDefault();
            if (exsitingTimeEntry == null)
            {
                var _db_timeentry = Mapper.Map<eng_time_entry>(timeentry);

                Eng360DB.eng_time_entry.Add(_db_timeentry);
            }
            else
            {
                exsitingTimeEntry.ProjectID = timeentry.ProjectID;
                exsitingTimeEntry.EmpID = timeentry.EmpID;
                exsitingTimeEntry.ReportEndDate = DateTime.ParseExact(timeentry.ReportEndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                if (!string.IsNullOrEmpty(timeentry.Work_Start_Time))
                    exsitingTimeEntry.Work_Start_Time = TimeSpan.Parse(timeentry.Work_Start_Time);

                if (!string.IsNullOrEmpty(timeentry.Work_End_Time))
                    exsitingTimeEntry.Work_End_Time = TimeSpan.Parse(timeentry.Work_End_Time);


                if (!string.IsNullOrEmpty(timeentry.Ot_Start_Time))
                    exsitingTimeEntry.Ot_Start_Time = TimeSpan.Parse(timeentry.Ot_Start_Time);

                if (!string.IsNullOrEmpty(timeentry.Ot_End_Time))
                    exsitingTimeEntry.Ot_End_Time = TimeSpan.Parse(timeentry.Ot_End_Time);

                exsitingTimeEntry.No_of_WorkHours = timeentry.No_of_WorkHours;
                exsitingTimeEntry.No_of_OtHours = timeentry.No_of_OtHours;

                exsitingTimeEntry.Remarks = timeentry.Remarks;
                exsitingTimeEntry.UpdatedBy = timeentry.SubmittedBy;
                exsitingTimeEntry.UpdatedDate = timeentry.UpdatedDate;
                exsitingTimeEntry.WEHflag = timeentry.WEHflag ? 1 : 0;
                exsitingTimeEntry.LBflag = timeentry.LBflag ? 1 : 0;

                exsitingTimeEntry.Leave = timeentry.Leave;

                exsitingTimeEntry.UpdatedBy = timeentry.SubmittedBy;
                exsitingTimeEntry.UpdatedDate = timeentry.SubmittedDate;

                //Eng360DB.Entry(exsitingTimeEntry).State = EntityState.Unchanged;
                //Eng360DB.SaveChanges();
                Eng360DB.Entry(exsitingTimeEntry).State = EntityState.Modified;

                //  Eng360DB.eng_time_entry.Attach(_db_timeentry);
            }
            return Eng360DB.SaveChanges();

        }

        public int SaveTimeEntry(TimeEntryViewModel timeentry)
        {

            var _db_timeentry = Mapper.Map<eng_time_entry>(timeentry);
            Eng360DB.Entry(_db_timeentry).State = EntityState.Modified;

            return Eng360DB.SaveChanges();


        }

        public int DeleteTimeEntry(int timeentryID)
        {
            var _db_timeentry = Eng360DB.eng_time_entry.First(a => a.TEID == timeentryID);

            Eng360DB.eng_time_entry.Remove(_db_timeentry);
            return Eng360DB.SaveChanges();
        }

        public TimeEntryViewModel getTimeEntry(int? timeentryID)
        {
            eng_time_entry eng_time_entry = Eng360DB.eng_time_entry.Find(timeentryID);

            return Mapper.Map<TimeEntryViewModel>(eng_time_entry);
        }

        public List<TimeEntryViewModel> getAllTimeEntries()
        {
            var eng_time_entry = Eng360DB.eng_time_entry.ToList();
            var lstTimeEntryView = Mapper.Map<List<TimeEntryViewModel>>(eng_time_entry);
            return lstTimeEntryView;

            //var eng_time_entry = Eng360DB.TimeEntry().ToList();
            //List<TimeEntryViewModel> tevmList = new List<TimeEntryViewModel>();
            //foreach (var te in eng_time_entry)
            //{
            //    TimeEntryViewModel tevm = new TimeEntryViewModel(

            //        )
            //    {
            //        EmpID = te.EmpID,
            //        ReportDate = te.ReportDate,
            //        No_of_WorkHours = te.No_of_WorkHours,
            //        No_of_OtHours = te.No_of_Othours,
            //        FirstName = te.FirstName

            //    };
            //    tevmList.Add(tevm);
            //}
            //return tevmList;
        }

        public List<TimeEntryViewModel> getAllTEs(int ? id, string repdate)
        {
            var repdt = Convert.ToDateTime(repdate);
            var eng_time_entry = Eng360DB.eng_time_entry.Where(a=>a.EmpID==id && a.ReportDate == repdt).ToList();
            var lstTimeEntryView = Mapper.Map<List<TimeEntryViewModel>>(eng_time_entry);
            return lstTimeEntryView;
        }

        public List<MobileTimeEntryViewModel> getAllTimeEntry(int userID)
        {
            DateTime from = DateTime.Parse(DateTime.Now.ToShortDateString());
            DateTime to = DateTime.Parse(DateTime.Now.ToShortDateString()).AddDays(-30);

            var eng_project_report = Eng360DB.eng_time_entry.Where(a => a.SubmittedBy == userID && a.SubmittedDate <= from && a.SubmittedDate >= to).OrderByDescending(od => od.SubmittedDate).ToList();
            var lstTimeENtryView = Mapper.Map<List<MobileTimeEntryViewModel>>(eng_project_report);

            foreach (var item in lstTimeENtryView)
            {
                var domianItem = eng_project_report.Where(a => a.TEID == item.TEID).FirstOrDefault();

                item.ProjectName = domianItem.eng_project_master.ProjectNo + " - " + domianItem.eng_project_master.ProjectName;
                item.ReportDate = Convert.ToDateTime(domianItem.ReportDate).ToString("dd/MM/yyyy");
                item.FirstName = domianItem.eng_employee_profile.FirstName;
                item.islunchtime = domianItem.LBflag ?? default(int);
                item.ispublicholiday = domianItem.WEHflag ?? default(int);


            }

            return lstTimeENtryView;
        }


        public List<TimeEntryViewModel> getFilterTEs(FilterViewModel filter)
        {
            try
            {
            

            var dCurrentDayofThisMonth = DateTime.Today.ToString("yyyy-MM-dd");
            var dFirstDayOfThisMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            var dLastDayOfLastMonth = dFirstDayOfThisMonth.AddDays(-1).ToString("yyyy-MM-dd");
            var dFirstDayOfLastMonth = dFirstDayOfThisMonth.AddMonths(-1).ToString("yyyy-MM-dd"); ;
            var dFirstDayOfCurrentMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).ToString("yyyy-MM-dd");

                //var sql = "select EmpID, ProjectID, ReportDate, SubmittedBy, 0 as TEID, null as Work_Start_Time, null as Work_End_Time, null as Ot_Start_Time, null as Ot_End_Time, '' as Remarks, null as SubmittedDate, null as UpdatedDate, null as UpdatedBy, null as WEHflag, null as LBflag, null as Leave, sum(No_of_WorkHours) as No_of_WorkHours, sum(No_of_OtHours) as No_of_Othours from eng_time_entry ";
                var sql = "select EmpID, ProjectID, ReportDate, SubmittedBy, 0 as TEID, null as Work_Start_Time, null as Work_End_Time, null as Ot_Start_Time, null as Ot_End_Time, '' as Remarks, null as SubmittedDate, null as UpdatedDate, null as UpdatedBy, null as WEHflag, null as LBflag, null as Leave, sum(No_of_WorkHours) as No_of_WorkHours, sum(No_of_OtHours) as No_of_Othours, null as ReportEndDate from eng_time_entry ";

                var where = "";
            var group = "";
            if (filter.ProjectID != 0)
                where = "ProjectID = " + filter.ProjectID;

            if (filter.UserID > 0)
                if (where == "")
                    where += " EmpID =" + filter.UserID;
                else
                    where += " and EmpID =" + filter.UserID;

            if (filter.CreatedBy > 0)
                if (where == "")
                    where += " SubmittedBy =" + filter.CreatedBy;
                else
                    where += " and SubmittedBy =" + filter.CreatedBy;

            if (filter.Month == 0)
            {
                if (filter.dateFrom != null)
                {
                    filter.dateFrom = DateTime.ParseExact(filter.dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " ReportDate >='" + filter.dateFrom + "'";
                    else
                        where += " and ReportDate >='" + filter.dateFrom + "'";
                }
                if (filter.dateTo != null)
                {
                    filter.dateTo = DateTime.ParseExact(filter.dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " ReportDate <='" + filter.dateTo + "'";
                    else
                        where += " and ReportDate <='" + filter.dateTo + "'";
                }
            }

            if (filter.Month == 1)
            {

                if (where == "")
                    where += " ReportDate >= '" + dFirstDayOfCurrentMonth + "' and ReportDate <= '" + dCurrentDayofThisMonth + "'";
                else
                    where += " and ReportDate >='" + dFirstDayOfCurrentMonth + "' and ReportDate <='" + dCurrentDayofThisMonth + "'";

            }

            if (filter.Month == 2)
            {

                if (where == "")
                    where += " ReportDate >='" + dFirstDayOfLastMonth + "' and ReportDate <='" + dLastDayOfLastMonth + "'";
                else
                    where += " and ReportDate >='" + dFirstDayOfLastMonth + "' and ReportDate <='" + dLastDayOfLastMonth + "'";

            }

            group = " group by EmpID, ProjectID, ReportDate, SubmittedBy;";

            if (where != "")
                sql = sql + "Where " + where + group;
            else
                sql = sql + group;


            // DbSqlQuery<eng_employee_profile> data = Eng360DB.eng_employee_profile.SqlQuery(sql);
            //var data = Eng360DB.eng_time_entry.SqlQuery(sql).ToList();

            var data1 = Eng360DB.Database.SqlQuery<eng_time_entry>(sql).ToList();

            foreach (var data in data1)
            {
                data.eng_employee_profile = Eng360DB.eng_employee_profile.Find(data.EmpID);
                data.eng_project_master = Eng360DB.eng_project_master.Find(data.ProjectID);
            }

            var lstTeView = Mapper.Map<List<TimeEntryViewModel>>(data1);
            return lstTeView;

        }
            catch (Exception ex)
            {
                var lstTeView = new List<TimeEntryViewModel>();
                return lstTeView;

            }



}


            


}
}