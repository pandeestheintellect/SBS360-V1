using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eng360Web.Models.Domain;
using Eng360Web.Models.ViewModel;
using AutoMapper;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.Utility;
using System.Globalization;
using Eng360Web.Models.Security;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class TimeEntryController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly ITimeEntryServices timeentryService;
        private readonly IEmployeeServices employeeService;
        private readonly IProjectServices projectService;
        private readonly IUserServices userService;

        public TimeEntryController(ITimeEntryServices _timeentryService, IEmployeeServices _employeeService,
            IUserServices _userService, IProjectServices _projectService)
        {
            timeentryService = _timeentryService;
            employeeService = _employeeService;
            projectService = _projectService;
            userService = _userService;

        }

        // GET: TimeEntry
        public ActionResult Index()
        {
            
            var te = timeentryService.getAllTimeEntries().ToList();
            return View(te);
        }


        public ActionResult ReportIndex()
        {
            var filter = new FilterViewModel();

            var pnames = projectService.getAllProjects().ToList();
            pnames.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
            ViewBag.ProjectID = new SelectList(pnames, "ProjectID", "ProjectName");


            var fn = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
            fn.Insert(0, new EmployeeViewModel() { UserID = 0, FirstName = "Select" });
            ViewBag.UserID = new SelectList(fn, "UserID", "FirstName");

            var svs = userService.getAllUsers().Where(a => a.IsActive == 1 && a.GroupID == 2 || a.GroupID == 7 || a.GroupID == 3).ToList();
            svs.Insert(0, new UserViewModel() { UserID = 0, DisplayName = "Select" });
            ViewBag.CreatedBy = new SelectList(svs, "UserID", "DisplayName");

            var tes = timeentryService.getFilterTEs(filter).ToList();

            foreach (var te in tes)
            {
                var fna = userService.getAllUsers().Where(a => a.UserID == te.SubmittedBy).FirstOrDefault();
                te.SVName = fna.DisplayName;
            }

            filter.eng_time_entry = tes;
            return View(filter);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportIndex(FilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                var fil = new FilterViewModel();
                var pnames = projectService.getAllProjects().ToList();
                pnames.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });

                var fn = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
                fn.Insert(0, new EmployeeViewModel() { UserID = 0, FirstName = "Select" });

                var svs = userService.getAllUsers().Where(a => a.IsActive == 1 && a.GroupID == 2 || a.GroupID == 7 || a.GroupID == 3).ToList();
                svs.Insert(0, new UserViewModel() { UserID = 0, DisplayName = "Select" });
                



                var tes = timeentryService.getFilterTEs(filter).ToList();

                foreach (var te in tes)
                {
                    var fna = userService.getAllUsers().Where(a => a.UserID == te.SubmittedBy).FirstOrDefault();
                    te.SVName = fna.DisplayName;
                }

                fil.eng_time_entry = tes;
                ViewBag.ProjectID = new SelectList(pnames, "ProjectID", "ProjectName", filter.ProjectID);
                ViewBag.UserID = new SelectList(fn, "UserID", "FirstName", filter.UserID);
                ViewBag.CreatedBy = new SelectList(svs, "UserID", "DisplayName", filter.CreatedBy);

                fil.dateFrom = filter.dateFrom;
                fil.dateTo = filter.dateTo;
                fil.Month = filter.Month;


                return View(fil);

            }
            return View();

        }


        // GET: TEIndex
        public ActionResult TeDetail(int? id, string repdate)
        {
            // var eng_project_report = db.eng_project_report.Include(e => e.eng_project_master).Include(e => e.eng_sys_task_status);

            var eng_time_entry = timeentryService.getAllTEs(id,repdate).ToList();
            if (eng_time_entry == null)
            {
                return getFailedOperation();
            }
            var tes = eng_time_entry.ToList();
            var output = Mapper.Map<List<TimeEntryViewModel>>(tes);

            //foreach (var timeentry in output)
            //{

            //    timeentry.FirstName = timeentry.eng_employee_profile.FirstName;
            //    timeentry.ProjectName = timeentry.eng_project_master.ProjectName;

            //}

            return View(output);
        }



        // GET: TimeEntry/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_time_entry eng_time_entry = db.eng_time_entry.Find(id);
            if (eng_time_entry == null)
            {
                return HttpNotFound();
            }
            return View(eng_time_entry);
        }

        // GET: TimeEntry/Create
        public ActionResult Create()
        {
            ViewBag.EmpID = new SelectList(employeeService.getAllEmployees().Where(a=>a.IsActive == 1).ToList(), "UserID", "FullName");
            ViewBag.ProjectID = new SelectList(projectService.getAllProjects().ToList(), "ProjectID", "ProjectName");
            return View();
        }

        // POST: TimeEntry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TimeEntryViewModel eng_time_entry)
        {
            if (ModelState.IsValid)
            {
                int result = 0;
                var normal_work_hours = AppSession.GetCompanyDetail().Normal_Work_Hours;
                var weekend_work_hours = AppSession.GetCompanyDetail().Weekend_Work_Hours;
                var lunch_break_hours = AppSession.GetCompanyDetail().Lunch_Break_Hours;

                //var normal_total_work_hours = normal_work_hours - lunch_break_hours;
                //var weekend_total_work_hours = weekend_work_hours - lunch_break_hours;
                //decimal totWorkHours;

                //if(eng_time_entry.LBflag)
                //{
                //    normal_total_work_hours = normal_total_work_hours + lunch_break_hours;
                //    weekend_total_work_hours = weekend_total_work_hours + lunch_break_hours;
                //}

                if (!string.IsNullOrEmpty(eng_time_entry.Work_Start_Time) && !string.IsNullOrEmpty(eng_time_entry.Work_End_Time))
                {
                    decimal workhours = 0;
                    decimal othours = 0;

                    AppSettings.getWorkTimeHours(normal_work_hours, weekend_work_hours, lunch_break_hours, eng_time_entry.Work_Start_Time, eng_time_entry.Work_End_Time,
                        eng_time_entry.WEHflag, eng_time_entry.LBflag, out workhours, out othours);

                    eng_time_entry.No_of_WorkHours = workhours;
                    eng_time_entry.No_of_OtHours = othours;

                }
                else
                {
                    eng_time_entry.No_of_WorkHours = 0;
                    eng_time_entry.No_of_OtHours = 0;
                }

                //TimeSpan othours;
                if (!string.IsNullOrEmpty(eng_time_entry.Ot_Start_Time) && !string.IsNullOrEmpty(eng_time_entry.Ot_End_Time))
                {
                    TimeSpan othours = Convert.ToDateTime(eng_time_entry.Ot_End_Time) - Convert.ToDateTime(eng_time_entry.Ot_Start_Time);
                    eng_time_entry.No_of_OtHours = Convert.ToDecimal(othours.TotalHours);
                }

                eng_time_entry.ReportDate = DateTime.ParseExact(eng_time_entry.Work_Start_Time, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                eng_time_entry.ReportEndDate = DateTime.ParseExact(eng_time_entry.Work_End_Time, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


                if (!string.IsNullOrEmpty(eng_time_entry.Work_Start_Time))
                    eng_time_entry.Work_Start_Time =
                      DateTime.ParseExact(eng_time_entry.Work_Start_Time, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture).ToString("HH:mm");

                if (!string.IsNullOrEmpty(eng_time_entry.Work_End_Time))
                    eng_time_entry.Work_End_Time =
                      DateTime.ParseExact(eng_time_entry.Work_End_Time, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture).ToString("HH:mm");


                if (!string.IsNullOrEmpty(eng_time_entry.Ot_Start_Time))
                    eng_time_entry.Ot_Start_Time =
                        DateTime.ParseExact(eng_time_entry.Ot_Start_Time, "h:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");

                if (!string.IsNullOrEmpty(eng_time_entry.Ot_End_Time))
                    eng_time_entry.Ot_End_Time =
                        DateTime.ParseExact(eng_time_entry.Ot_End_Time, "h:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");

                //TimeSpan workhours = DateTime.ParseExact(eng_time_entry.Work_End_Time, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture) - DateTime.ParseExact(eng_time_entry.Work_Start_Time, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);


                eng_time_entry.SubmittedBy = AppSession.GetCurrentUserId();
                eng_time_entry.SubmittedDate = DateTime.Now;

                if (eng_time_entry.Leave > 0)
                {
                    eng_time_entry.No_of_WorkHours = 0;
                    eng_time_entry.No_of_OtHours = 0;
                }

                var emparr = eng_time_entry.SelectedEmpList.Split(',');
                foreach (var emp in emparr)
                {
                    eng_time_entry.EmpID = Convert.ToInt32(emp);
                    result = timeentryService.CreateTimeEntry(eng_time_entry);
                }


                if (result > 0)
                    return getSuccessfulOperation();
                else
                    return getFailedOperation();
            }


            return View(eng_time_entry);
        }

        // GET: TimeEntry/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeEntryViewModel te = timeentryService.getTimeEntry(id);

            if (te == null)
            {
                return HttpNotFound();
            }

            ViewBag.EmpID = new SelectList(employeeService.getAllEmployees().Where(a=>a.IsActive == 1).ToList(), "UserID", "FirstName", te.EmpID);
            ViewBag.ProjectID = new SelectList(projectService.getAllProjects().ToList(), "ProjectID", "ProjectName", te.ProjectID);
           

            return View(te);
        }

        // POST: TimeEntry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TimeEntryViewModel eng_time_entry)
        {
            if (ModelState.IsValid)
            {
                var normal_work_hours = AppSession.GetCompanyDetail().Normal_Work_Hours;
                var weekend_work_hours = AppSession.GetCompanyDetail().Weekend_Work_Hours;
                var lunch_break_hours = AppSession.GetCompanyDetail().Lunch_Break_Hours;

                var wstime = eng_time_entry.ReportDate + " " + eng_time_entry.Work_Start_Time;
                var wetime = eng_time_entry.ReportEndDate + " " + eng_time_entry.Work_End_Time;

                //var normal_total_work_hours = normal_work_hours - lunch_break_hours;
                //var weekend_total_work_hours = weekend_work_hours - lunch_break_hours;
                //decimal totWorkHours;

                //if (eng_time_entry.LBflag)
                //{
                //    normal_total_work_hours = normal_total_work_hours + lunch_break_hours;
                //    weekend_total_work_hours = weekend_total_work_hours + lunch_break_hours;
                //}

                if (!string.IsNullOrEmpty(eng_time_entry.Work_Start_Time) && !string.IsNullOrEmpty(eng_time_entry.Work_End_Time))
                {
                    decimal workhours = 0;
                    decimal othours = 0;


                    AppSettings.getWorkTimeHours(normal_work_hours, weekend_work_hours, lunch_break_hours, wstime, wetime,
                        eng_time_entry.WEHflag, eng_time_entry.LBflag, out workhours, out othours);

                    eng_time_entry.No_of_WorkHours = workhours;
                    eng_time_entry.No_of_OtHours = othours;
                }
                else
                {
                    eng_time_entry.No_of_WorkHours = 0;
                    eng_time_entry.No_of_OtHours = 0;
                }




                //TimeSpan othours;
                if (!string.IsNullOrEmpty(eng_time_entry.Ot_Start_Time) && !string.IsNullOrEmpty(eng_time_entry.Ot_End_Time))
                {
                    TimeSpan othours = Convert.ToDateTime(eng_time_entry.Ot_End_Time) - Convert.ToDateTime(eng_time_entry.Ot_Start_Time);
                    eng_time_entry.No_of_OtHours = Convert.ToDecimal(othours.TotalHours);
                }

                if (!string.IsNullOrEmpty(eng_time_entry.Work_Start_Time))
                    eng_time_entry.Work_Start_Time =
                      DateTime.ParseExact(eng_time_entry.Work_Start_Time, "HH:mm", CultureInfo.InvariantCulture).ToString("HH:mm");

                if (!string.IsNullOrEmpty(eng_time_entry.Work_End_Time))
                    eng_time_entry.Work_End_Time =
                      DateTime.ParseExact(eng_time_entry.Work_End_Time, "HH:mm", CultureInfo.InvariantCulture).ToString("HH:mm");


                if (!string.IsNullOrEmpty(eng_time_entry.Ot_Start_Time))
                    eng_time_entry.Ot_Start_Time =
                        DateTime.ParseExact(eng_time_entry.Ot_Start_Time, "h:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");

                if (!string.IsNullOrEmpty(eng_time_entry.Ot_End_Time))
                    eng_time_entry.Ot_End_Time =
                        DateTime.ParseExact(eng_time_entry.Ot_End_Time, "h:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");

                //TimeSpan workhours = DateTime.ParseExact(eng_time_entry.Work_End_Time, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture) - DateTime.ParseExact(eng_time_entry.Work_Start_Time, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
                eng_time_entry.ReportDate = DateTime.ParseExact(eng_time_entry.ReportDate, "yyyy/MM/dd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                eng_time_entry.ReportEndDate = DateTime.ParseExact(eng_time_entry.ReportEndDate, "yyyy/MM/dd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


                eng_time_entry.UpdatedBy = AppSession.GetCurrentUserId();
                eng_time_entry.UpdatedDate = DateTime.Now;

                var result = timeentryService.SaveTimeEntry(eng_time_entry);
                if (result > 0)
                    return getSuccessfulOperation();
                else
                    return getFailedOperation();



            }

            return View(eng_time_entry);
        }

        // GET: TimeEntry/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_time_entry eng_time_entry = db.eng_time_entry.Find(id);
            if (eng_time_entry == null)
            {
                return HttpNotFound();
            }
            return View(eng_time_entry);
        }

        // POST: TimeEntry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            var result = timeentryService.DeleteTimeEntry(id);

            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }



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
