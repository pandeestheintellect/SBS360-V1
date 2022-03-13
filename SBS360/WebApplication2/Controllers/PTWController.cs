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
using System.IO;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.Utility;
using System.Globalization;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.Security;
using System.Data.Entity.Core.Objects;
using NLog;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class PTWController : SuperBaseController
    {

        private readonly IPTWServices ptwServices;
        private readonly IUserServices userService;
        private readonly IProjectServices projectService;
        private readonly IEmployeeServices employeeService;
        Logger logger = LogManager.GetCurrentClassLogger();

        public PTWController(IUserServices _userService,
            IProjectServices _projectService,
            IEmployeeServices _employeeService,
            IPTWServices _ptwServices)
        {
            ptwServices = _ptwServices;
            userService = _userService;
            projectService = _projectService;
            employeeService = _employeeService;
        }
        // GET: PTW
        public ActionResult Index()
        {
            var ptws = ptwServices.getALLPTWMasterWeb();
            foreach (var val in ptws)
            {
                val.ProjectTitle = projectService.getProject((int)val.ProjectID).ProjectName;
            }

            var ptwcons = ptwServices.getALLPTWConspcMasterWeb();

            

            foreach (var val in ptwcons)
            {
                var xnew = new PTWMasterViewModel();
                //val.ProjectTitle = projectService.getProject((int)val.ProjectID).ProjectName;
                xnew.PTW_master_ID = val.PTW_master_ID;
                xnew.PTW_type = val.PTW_type;
                xnew.ProjectTitle = projectService.getProject((int)val.ProjectID).ProjectName;
                xnew.Start_Date_Time = val.Start_Date_Time;
                xnew.End_Date_Time = val.End_Date_Time;
                xnew.NameOfApplicant = val.Applicant_Name;
                xnew.CompletedStage = val.CompletedStage;

                ptws.Add(xnew);
            }

            return View(ptws);
        }

        public ActionResult Index_View(int id)
        {
            var returnObject = ptwServices.getPTWMaster(id);
            returnObject.ProjectTitle = projectService.getProject((int)returnObject.ProjectID).ProjectName;
            return View(returnObject);
        }

        public ActionResult IndexConspc()
        {
            var ptws = ptwServices.getALLPTWConspcMasterWeb();
            foreach (var val in ptws)
            {
                val.ProjectTitle = projectService.getProject((int)val.ProjectID).ProjectName;
            }

            return View(ptws);
        }

        public ActionResult Index_Conspc_View(int id)
        {
            var returnObject = ptwServices.getPTWConspcMaster(id);
            returnObject.ProjectTitle = projectService.getProject((int)returnObject.ProjectID).ProjectName;
            returnObject.eng_PTW_Conspc_Detail_Stage5 = returnObject.eng_PTW_Conspc_Detail_Stage5.GroupBy(a => a.Stage5_Date_Time.Value.Date).Select(g=> g.First()).ToList();
            return View(returnObject);
        }

        [HttpPost]
        public ActionResult Create(PTWMasterViewModel ptw)
        {
            int result = 0;

            if (!string.IsNullOrEmpty(ptw.Date_Time))
             ptw.Date_Time = DateTime.ParseExact(ptw.Date_Time, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            
            if (!string.IsNullOrEmpty(ptw.Start_Date_Time))
                ptw.Start_Date_Time = DateTime.ParseExact(ptw.Start_Date_Time, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            
            if (!string.IsNullOrEmpty(ptw.End_Date_Time))
                ptw.End_Date_Time = DateTime.ParseExact(ptw.End_Date_Time, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            
            if (!string.IsNullOrEmpty(ptw.Stage1_Date_Time))
                ptw.Stage1_Date_Time = DateTime.ParseExact(ptw.Stage1_Date_Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");

            if (!string.IsNullOrEmpty(ptw.Stage2_Date_Time))
                ptw.Stage2_Date_Time = DateTime.ParseExact(ptw.Stage2_Date_Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");

            if (!string.IsNullOrEmpty(ptw.Stage3_Date_Time))
                ptw.Stage3_Date_Time = DateTime.ParseExact(ptw.Stage3_Date_Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");

            if (!string.IsNullOrEmpty(ptw.Stage5_Sup_Date_Time))
                ptw.Stage5_Sup_Date_Time = DateTime.ParseExact(ptw.Stage5_Sup_Date_Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");

            if (!string.IsNullOrEmpty(ptw.Stage5_Mng_Date_Time))
                ptw.Stage5_Mng_Date_Time = DateTime.ParseExact(ptw.Stage5_Mng_Date_Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");




            ptw.Created_By = AppSession.GetCurrentUserId();
            if (ptw.PTW_master_ID == 0)
            {
                ptw.Created_By = AppSession.GetCurrentUserId();
                result = ptwServices.createPtw(ptw);
            }
            else
            {
                ptw.Updated_By = AppSession.GetCurrentUserId();
                result = ptwServices.EditPtw(ptw);
            }


            return getSuccessfulOperation();
        }

        [HttpPost]
        public ResponseViewModel EditPtw(MobilePTWMaster engPTW)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                int result;

                result = ptwServices.EditPtw(engPTW);

                if (result > 0)
                {
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                }

            }

            catch (Exception ex)
            {
                logger.Debug("EditPTW Error:");
                logger.Debug(ex.Message);
                logger.Debug(ex.StackTrace);
            }
            return response;
        }

        public ActionResult Edit(int? id)
        {
            var ptwdesc = "";
            var returnObject = ptwServices.getPTWMaster(id);

            if (returnObject.PTW_type == "Hot work Permit")
                ptwdesc = "PTWHOT";
            if (returnObject.PTW_type == "Lifting work Permit")
                ptwdesc = "PTWLOPT";
            if (returnObject.PTW_type == "Working at Height Permit")
                ptwdesc = "PTWWAH";
            if (returnObject.PTW_type == "Excavation Permit")
                ptwdesc = "PTWWFEX";

            //returnObject.stages = ptwServices.getPTWStages(returnObject.PTW_type);
            //returnObject.stage1Config = ptwServices.getPTWStageOneConfigWeb(returnObject.PTW_type);

            returnObject.stages = ptwServices.getPTWStages(ptwdesc);

            returnObject.stage1Config = ptwServices.getPTWStageOneConfigWeb(ptwdesc);

            var acnt = returnObject.stage1Config.Count;
            var dcnt = returnObject.eng_PTW_Detail_Satge1.Count;

            if (acnt != dcnt)
            {

                foreach (var item in returnObject.stage1Config)
                {
                    var flag = 0;
                    foreach (var chk in returnObject.eng_PTW_Detail_Satge1)
                    {
                        if (chk.PTW_Stage_One_ID == item.PTW_Stage_One_ID)
                        {
                            flag = 1;
                            chk.item = item.Item;
                        }
                    }
                    if (flag == 0)
                    {
                        var newvm = new PTWStage1ViewModel();
                        newvm.PTW_Master_ID = returnObject.PTW_master_ID;
                        newvm.PTW_Stage_One_ID = item.PTW_Stage_One_ID;
                        newvm.item = item.Item;
                        newvm.Is_Applicable = 0;
                        returnObject.eng_PTW_Detail_Satge1.Add(newvm);
                    }

                }

            }

            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID != 4 && a.GroupID != 5 && a.GroupID != 7).ToList();

            List<SelectListItem> emplist = new List<SelectListItem>();
            foreach (var emp in emps)
            {
                emplist.Add(new SelectListItem { Text = (emp.FirstName + " " + emp.LastName), Value = emp.UserID.ToString() });
            }

            ViewBag.WorkerList = emplist;

            var project = projectService.getAllProjects().Where(a => a.Project_Status_ID == 1 || a.Project_Status_ID == 2 || a.Project_Status_ID == 4).ToList();

            if (project == null)
            {

                project.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName");
            }
            else
            {
                var projects = new SelectList(project, "ProjectID", "ProjectName");
                //projects.Where(a => a.Value == returnObject.ProjectID.ToString()).FirstOrDefault().Selected = true;

                //ViewBag.ProjectID = projects;
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName", returnObject.ProjectID);

            }

            var svs = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 3).ToList();
            svs.Insert(0, new EmployeeViewModel() { FullName = "Select" });
            ViewBag.SVList = new SelectList(svs, "FullName", "FullName");

            var mng = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 2).ToList();
            mng.Insert(0, new EmployeeViewModel() { FullName = "Select" });

            ViewBag.MNGList = new SelectList(mng, "FullName", "FullName");


            if (returnObject.eng_PTW_Detail_Satge4 != null)
                returnObject.is_already_created = returnObject.eng_PTW_Detail_Satge4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy")).FirstOrDefault() == null ? false : true;
            else
                returnObject.is_already_created = false;

            if (returnObject.is_already_created)
            {
              var obj=  returnObject.eng_PTW_Detail_Satge4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy")).FirstOrDefault();

                returnObject.Stage4_Sup_Name = obj.Sup_Signature;
                returnObject.Stage4_WSH_Name = obj.Mng_Signature;
            }
            returnObject.daysPtwCreated = getDayList(DateTime.Today.ToString("dddd").Substring(0, 3), returnObject.eng_PTW_Detail_Satge4);
            return View("~\\Views\\PTW\\Create.cshtml", returnObject);
        }

        // GET: PTW/View/
        public ActionResult ViewPTW(int? id, string dt)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  eng_quote_master eng_quote_master = db.eng_quote_master.Find(id);
            var ptw = ptwServices.getPTWMaster(id ?? default(int));
            if (ptw == null)
            {
                return HttpNotFound();
            }
            var ptwdesc = "";
            if (ptw.PTW_type == "Hot work Permit")
                ptwdesc = "PTWHOT";
            if (ptw.PTW_type == "Lifting work Permit")
                ptwdesc = "PTWLOPT";
            if (ptw.PTW_type == "Working at Height Permit")
                ptwdesc = "PTWWAH";
            if (ptw.PTW_type == "Excavation Permit")
                ptwdesc = "PTWWFEX";
            if (ptw.PTW_type == "Confined Space Permit")
                ptwdesc = "PTWCONSPC";

            //ptw.stage1details = ptwServices.getPTWStageOneConfigWeb().Where(a => a.PTW_Master_ID == ptw.PTW_master_ID).ToList();
            //var typedetails = ptwServices.getPTWStageOneConfig().Where(a => a.PTW_Type == ptw.PTW_type).ToList();

            var typedetails = ptwServices.getPTWStageOneConfig().Where(a => a.PTW_Type == ptwdesc).ToList();
            //foreach (var item in ptw.eng_PTW_Detail_Satge1)
            //{
            //    item.item = typedetails.Where(a => a.PTW_Stage_One_ID == item.PTW_Stage_One_ID).Select(b => b.Item).FirstOrDefault();
            //}

            foreach (var item in typedetails)
            {
                var flag = 0;
                foreach (var chk in ptw.eng_PTW_Detail_Satge1)
                {
                    if (chk.PTW_Stage_One_ID == item.PTW_Stage_One_ID)
                    {
                        flag = 1;
                        chk.item = item.Item;
                    }
                }
                if (flag == 0)
                {
                    var newvm = new PTWStage1ViewModel();
                    newvm.PTW_Master_ID = ptw.PTW_master_ID;
                    newvm.PTW_Stage_One_ID = item.PTW_Stage_One_ID;
                    newvm.item = item.Item;
                    newvm.Is_Applicable = 0;
                    ptw.eng_PTW_Detail_Satge1.Add(newvm);
                }
                                   
            }

            ptw.eng_PTW_Detail_Satge1.OrderBy(a=>a.PTW_Stage_One_ID);

            foreach (var emp in ptw.eng_PTW_Employee_Details)
            {
                var empdet = employeeService.getEmployee((int)emp.EmployeeID);
                emp.EmpName = empdet.FirstName + " " + empdet.LastName;
                emp.Desig = empdet.Designation;
            }

            if (dt != "check")
            {
                ptw.Stage4_Sup_Name = ptw.eng_PTW_Detail_Satge4.Where(a => a.DayDate == Convert.ToDateTime(dt)).FirstOrDefault().Sup_Signature;
                ptw.Stage4_WSH_Name = ptw.eng_PTW_Detail_Satge4.Where(a => a.DayDate == Convert.ToDateTime(dt)).FirstOrDefault().Mng_Signature;
                ViewBag.St4Day = ptw.eng_PTW_Detail_Satge4.Where(a => a.DayDate == Convert.ToDateTime(dt)).FirstOrDefault().Day;
            }
            ptw.ProjectTitle = projectService.getProject((int)ptw.ProjectID).ProjectName;

            return View(ptw);
        }


        public ActionResult RevokePTW(int? id)
        {
            var returnObject = ptwServices.getPTWMaster(id);

            //returnObject.stages = ptwServices.getPTWStages(returnObject.PTW_type);
            //returnObject.stage1Config = ptwServices.getPTWStageOneConfigWeb(returnObject.PTW_type);
            var ptwdesc = "";
            if (returnObject.PTW_type == "Hot work Permit")
                ptwdesc = "PTWHOT";
            if (returnObject.PTW_type == "Lifting work Permit")
                ptwdesc = "PTWLOPT";
            if (returnObject.PTW_type == "Working at Height Permit")
                ptwdesc = "PTWWAH";
            if (returnObject.PTW_type == "Excavation Permit")
                ptwdesc = "PTWWFEX";
            if (returnObject.PTW_type == "Confined Space Permit")
                ptwdesc = "PTWCONSPC";

            returnObject.stages = ptwServices.getPTWStages(ptwdesc);
            returnObject.stage1Config = ptwServices.getPTWStageOneConfigWeb(ptwdesc);

            var acnt = returnObject.stage1Config.Count;
            var dcnt = returnObject.eng_PTW_Detail_Satge1.Count;

            if(acnt != dcnt)
            {

                foreach (var item in returnObject.stage1Config)
                {
                    var flag = 0;
                    foreach (var chk in returnObject.eng_PTW_Detail_Satge1)
                    {
                        if (chk.PTW_Stage_One_ID == item.PTW_Stage_One_ID)
                        {
                            flag = 1;
                            chk.item = item.Item;
                        }
                    }
                    if (flag == 0)
                    {
                        var newvm = new PTWStage1ViewModel();
                        newvm.PTW_Master_ID = returnObject.PTW_master_ID;
                        newvm.PTW_Stage_One_ID = item.PTW_Stage_One_ID;
                        newvm.item = item.Item;
                        newvm.Is_Applicable = 0;
                        returnObject.eng_PTW_Detail_Satge1.Add(newvm);
                    }

                }

            }

            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID != 4 && a.GroupID != 5 && a.GroupID != 7).ToList();

            List<SelectListItem> emplist = new List<SelectListItem>();
            foreach (var emp in emps)
            {
                emplist.Add(new SelectListItem { Text = (emp.FirstName + " " + emp.LastName), Value = emp.UserID.ToString() });
            }

            ViewBag.WorkerList = emplist;

            var project = projectService.getAllProjects().Where(a => a.Project_Status_ID == 1 || a.Project_Status_ID == 2 || a.Project_Status_ID == 4).ToList();

            if (project == null)
            {

                project.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName");
            }
            else
            {
                var projects = new SelectList(project, "ProjectID", "ProjectName");
                //projects.Where(a => a.Value == returnObject.ProjectID.ToString()).FirstOrDefault().Selected = true;

                //ViewBag.ProjectID = projects;
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName", returnObject.ProjectID);

            }

            var svs = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 3).ToList();
            svs.Insert(0, new EmployeeViewModel() { FullName = "Select" });

            ViewBag.SVList = new SelectList(svs, "FullName", "FullName");

            returnObject.PTW_master_ID = 0;
            returnObject.CompletedStage = 0;
            returnObject.Start_Date_Time = null;
            returnObject.End_Date_Time = null;
            returnObject.Stage1_Date_Time = null;
            returnObject.Stage2_Date_Time = null;
            returnObject.Stage3_Date_Time = null;
            returnObject.Stage2_Person_Name = null;
            returnObject.Stage3_Person_Name = null;


            return View("~\\Views\\PTW\\Create.cshtml", returnObject);
        }




        [HttpGet]
        public ActionResult Create(string id)
        {
            var ptwdesc = "";
            var returnObject = new PTWMasterViewModel();
            returnObject.stages = ptwServices.getPTWStages(id);
            if (id == "PTWHOT")
                ptwdesc = "Hot work Permit";
            if (id == "PTWLOPT")
                ptwdesc = "Lifting work Permit";
            if (id == "PTWWAH")
                ptwdesc = "Working at Height Permit";
            if (id == "PTWWFEX")
                ptwdesc = "Excavation Permit";

            returnObject.PTW_type = ptwdesc;
            returnObject.is_already_created = false;
            returnObject.stage1Config = ptwServices.getPTWStageOneConfigWeb(id);
            returnObject.CompletedStage = 0;
            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID != 4 && a.GroupID != 5 && a.GroupID != 7).ToList();
            //inds.Insert(0, new SafetyHazardListViewModel() { Id = 0, Industry_Title = "Select" });

            List<SelectListItem> emplist = new List<SelectListItem>();
            foreach (var emp in emps)
            {
                emplist.Add(new SelectListItem { Text = (emp.FirstName + " " + emp.LastName), Value = emp.UserID.ToString() });
            }

            ViewBag.WorkerList = emplist;

            var project = projectService.getAllProjects().Where(a => a.Project_Status_ID == 1 || a.Project_Status_ID == 2 || a.Project_Status_ID == 4).ToList();

            if (project == null)
            {

                project.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName");
            }
            else
            {
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName");
            }

            var svs = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 3).ToList();
            svs.Insert(0, new EmployeeViewModel() { FullName = "Select" });            

            ViewBag.SVList = new SelectList(svs, "FullName", "FullName");
            
            return View(returnObject);
        }


        [HttpGet]
        public ActionResult CreateConspc()
        {
            var returnObject = new PTWConspcMasterViewModel();
            returnObject.stages = ptwServices.getPTWStages("PTWCONSPC");
            returnObject.PTW_type = "Confined Space Permit";
            returnObject.is_already_created = false;
            returnObject.stage1Config = ptwServices.getPTWStageOneConfigWeb("PTWCONSPC");
            returnObject.CompletedStage = 0;
            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID != 4 && a.GroupID != 5 && a.GroupID != 7).ToList();
            //inds.Insert(0, new SafetyHazardListViewModel() { Id = 0, Industry_Title = "Select" });

            List<SelectListItem> emplist = new List<SelectListItem>();
            foreach (var emp in emps)
            {
                emplist.Add(new SelectListItem { Text = (emp.FirstName + " " + emp.LastName), Value = emp.UserID.ToString() });
            }

            ViewBag.WorkerList = emplist;

            var project = projectService.getAllProjects().Where(a => a.Project_Status_ID == 1 || a.Project_Status_ID == 2 || a.Project_Status_ID == 4).ToList();

            if (project == null)
            {

                project.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName");
            }
            else
            {
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName");
            }

            var svs = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 3).ToList();
            svs.Insert(0, new EmployeeViewModel() { FullName = "Select" });
            ViewBag.SVList = new SelectList(svs, "FullName", "FullName");


            return View(returnObject);
        }

        [HttpPost]
        public ActionResult CreateConspc(PTWConspcMasterViewModel ptw)
        {
            int result = 0;

            if (!string.IsNullOrEmpty(ptw.Applicant_Date_Time))
                ptw.Applicant_Date_Time = DateTime.ParseExact(ptw.Applicant_Date_Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");

            if (!string.IsNullOrEmpty(ptw.Start_Date_Time))
                ptw.Start_Date_Time = DateTime.ParseExact(ptw.Start_Date_Time, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            if (!string.IsNullOrEmpty(ptw.End_Date_Time))
                ptw.End_Date_Time = DateTime.ParseExact(ptw.End_Date_Time, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            //if (!string.IsNullOrEmpty(ptw.Stage1_Date_Time))
            //    ptw.Stage1_Date_Time = DateTime.ParseExact(ptw.Stage1_Date_Time, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            if (!string.IsNullOrEmpty(ptw.Stage2_Assessor_Date_Time))
                ptw.Stage2_Assessor_Date_Time = DateTime.ParseExact(ptw.Stage2_Assessor_Date_Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");

            if (!string.IsNullOrEmpty(ptw.Stage3_WSH_Date_Time))
                ptw.Stage3_WSH_Date_Time = DateTime.ParseExact(ptw.Stage3_WSH_Date_Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");

            if (!string.IsNullOrEmpty(ptw.Stage4_Date_Time))
                ptw.Stage4_Date_Time = DateTime.ParseExact(ptw.Stage4_Date_Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");

            if (!string.IsNullOrEmpty(ptw.Stage5_Date_Time))
                ptw.Stage5_Date_Time = DateTime.ParseExact(ptw.Stage5_Date_Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");

            if (!string.IsNullOrEmpty(ptw.Stage6_Date_Time))
                ptw.Stage6_Date_Time = DateTime.ParseExact(ptw.Stage6_Date_Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");


            ptw.Created_By = AppSession.GetCurrentUserId();
            if (ptw.PTW_master_ID == 0)
            {
                ptw.Created_By = AppSession.GetCurrentUserId();
                result = ptwServices.createPtwConspc(ptw);
            }
            else
            {
                ptw.Updated_By = AppSession.GetCurrentUserId();
               result = ptwServices.EditPtwConspc(ptw);
            }


            return getSuccessfulOperation();
        }

        public ActionResult EditConspc(int? id)
        {
            var returnObject = ptwServices.getPTWConspcMaster(id);
            
            //returnObject.stages = ptwServices.getPTWStages(returnObject.PTW_type);
            //returnObject.stage1Config = ptwServices.getPTWStageOneConfigWeb(returnObject.PTW_type);

            returnObject.stages = ptwServices.getPTWStages("PTWCONSPC");
            returnObject.stage1Config = ptwServices.getPTWStageOneConfigWeb("PTWCONSPC");

           
            var acnt = returnObject.stage1Config.Count;
            var dcnt = returnObject.eng_PTW_Conspc_Detail_Stage1.Count;

            if (acnt != dcnt)
            {

                foreach (var item in returnObject.stage1Config)
                {
                    var flag = 0;
                    foreach (var chk in returnObject.eng_PTW_Conspc_Detail_Stage1)
                    {
                        if (chk.PTW_Stage_One_ID == item.PTW_Stage_One_ID)
                        {
                            flag = 1;
                            chk.item = item.Item;
                        }
                    }
                    if (flag == 0)
                    {
                        var newvm = new PTWConspcStage1ViewModel();
                        newvm.PTW_Master_ID = returnObject.PTW_master_ID;
                        newvm.PTW_Stage_One_ID = item.PTW_Stage_One_ID;
                        newvm.item = item.Item;
                        newvm.Is_Applicable_Applicant = 0;
                        newvm.Is_Applicable_Assessor = 0;
                        returnObject.eng_PTW_Conspc_Detail_Stage1.Add(newvm);
                    }

                }

            }

            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID != 4 && a.GroupID != 5 && a.GroupID != 7).ToList();

            List<SelectListItem> emplist = new List<SelectListItem>();
            foreach (var emp in emps)
            {
                emplist.Add(new SelectListItem { Text = (emp.FirstName + " " + emp.LastName), Value = emp.UserID.ToString() });
            }

            ViewBag.WorkerList = emplist;

            var project = projectService.getAllProjects().Where(a => a.Project_Status_ID == 1 || a.Project_Status_ID == 2 || a.Project_Status_ID == 4).ToList();

            if (project == null)
            {

                project.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName");
            }
            else
            {
                var projects = new SelectList(project, "ProjectID", "ProjectName");
                //projects.Where(a => a.Value == returnObject.ProjectID.ToString()).FirstOrDefault().Selected = true;

                //ViewBag.ProjectID = projects;
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName", returnObject.ProjectID);

            }

            var svs = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 3).ToList();
            svs.Insert(0, new EmployeeViewModel() { FullName = "Select" });
            ViewBag.SVList = new SelectList(svs, "FullName", "FullName");

            var mng = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 2).ToList();
            mng.Insert(0, new EmployeeViewModel() { FullName = "Select" });

            ViewBag.MNGList = new SelectList(mng, "FullName", "FullName");


            if (returnObject.eng_PTW_Conspc_Detail_Stage5 != null)
                returnObject.is_already_created = returnObject.eng_PTW_Conspc_Detail_Stage5.Where(a => a.Stage5_Date_Time.Value.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy")).FirstOrDefault() == null ? false : true;
            else
                returnObject.is_already_created = false;

            returnObject.eng_PTW_Conspc_Detail_Stage5 = returnObject.eng_PTW_Conspc_Detail_Stage5.Where(a => a.Stage5_Date_Time.Value.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy")).ToList();

            //if (returnObject.is_already_created)
            //{
            //    var obj = returnObject.eng_PTW_Detail_Satge4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy")).FirstOrDefault();

            //    returnObject.Stage4_Sup_Name = obj.Sup_Signature;
            //    returnObject.Stage4_WSH_Name = obj.Mng_Signature;
            //}
            //returnObject.daysPtwCreated = getDayList(DateTime.Today.ToString("dddd").Substring(0, 3), returnObject.eng_PTW_Detail_Satge4);

            
            return View("~\\Views\\PTW\\CreateConspc.cshtml", returnObject);
        }

        public ActionResult ViewPTWConspc(int? id, string dt)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  eng_quote_master eng_quote_master = db.eng_quote_master.Find(id);
            var ptw = ptwServices.getPTWConspcMaster(id ?? default(int));
            if (ptw == null)
            {
                return HttpNotFound();
            }

            //ptw.stage1details = ptwServices.getPTWStageOneConfigWeb().Where(a => a.PTW_Master_ID == ptw.PTW_master_ID).ToList();
            
            //var typedetails = ptwServices.getPTWStageOneConfig().Where(a => a.PTW_Type == ptw.PTW_type).ToList();

            var typedetails = ptwServices.getPTWStageOneConfig().Where(a => a.PTW_Type == "PTWCONSPC").ToList();
            foreach (var item in ptw.eng_PTW_Conspc_Detail_Stage1)
            {
                item.item = typedetails.Where(a => a.PTW_Stage_One_ID == item.PTW_Stage_One_ID).Select(b => b.Item).FirstOrDefault();
            }

            foreach (var emp in ptw.eng_PTW_Conspc_Employee_Details)
            {
                var empdet = employeeService.getEmployee((int)emp.EmployeeID);
                emp.EmpName = empdet.FirstName + " " + empdet.LastName;
                emp.Desig = empdet.Designation;
            }

            if (dt != "check")
            {
                ptw.eng_PTW_Conspc_Detail_Stage5 = ptw.eng_PTW_Conspc_Detail_Stage5.Where(a => a.Stage5_Date_Time.Value.Date == Convert.ToDateTime(dt).Date).ToList();
                //ptw.Stage4_WSH_Name = ptw.eng_PTW_Detail_Satge4.Where(a => a.DayDate == Convert.ToDateTime(dt)).FirstOrDefault().Mng_Signature;
                //ViewBag.St4Day = ptw.eng_PTW_Detail_Satge4.Where(a => a.DayDate == Convert.ToDateTime(dt)).FirstOrDefault().Day;
            }
            ptw.ProjectTitle = projectService.getProject((int)ptw.ProjectID).ProjectName;

            return View(ptw);
        }

        public ActionResult RevokePTWConspc(int? id)
        {
            var returnObject = ptwServices.getPTWConspcMaster(id);

            //returnObject.stages = ptwServices.getPTWStages(returnObject.PTW_type);
            //returnObject.stage1Config = ptwServices.getPTWStageOneConfigWeb(returnObject.PTW_type);

            returnObject.stages = ptwServices.getPTWStages("PTWCONSPC");
            returnObject.stage1Config = ptwServices.getPTWStageOneConfigWeb("PTWCONSPC");

            var acnt = returnObject.stage1Config.Count;
            var dcnt = returnObject.eng_PTW_Conspc_Detail_Stage1.Count;

            if (acnt != dcnt)
            {

                foreach (var item in returnObject.stage1Config)
                {
                    var flag = 0;
                    foreach (var chk in returnObject.eng_PTW_Conspc_Detail_Stage1)
                    {
                        if (chk.PTW_Stage_One_ID == item.PTW_Stage_One_ID)
                        {
                            flag = 1;
                            chk.item = item.Item;
                        }
                    }
                    if (flag == 0)
                    {
                        var newvm = new PTWConspcStage1ViewModel();
                        newvm.PTW_Master_ID = returnObject.PTW_master_ID;
                        newvm.PTW_Stage_One_ID = item.PTW_Stage_One_ID;
                        newvm.item = item.Item;
                        newvm.Is_Applicable_Applicant = 0;
                        newvm.Is_Applicable_Assessor = 0;
                        returnObject.eng_PTW_Conspc_Detail_Stage1.Add(newvm);
                    }

                }

            }

            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID != 4 && a.GroupID != 5 && a.GroupID != 7).ToList();

            List<SelectListItem> emplist = new List<SelectListItem>();
            foreach (var emp in emps)
            {
                emplist.Add(new SelectListItem { Text = (emp.FirstName + " " + emp.LastName), Value = emp.UserID.ToString() });
            }

            ViewBag.WorkerList = emplist;

            var project = projectService.getAllProjects().Where(a => a.Project_Status_ID == 1 || a.Project_Status_ID == 2 || a.Project_Status_ID == 4).ToList();

            if (project == null)
            {

                project.Insert(0, new ProjectViewModel() { ProjectID = 0, ProjectName = "Select" });
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName");
            }
            else
            {
                var projects = new SelectList(project, "ProjectID", "ProjectName");
                //projects.Where(a => a.Value == returnObject.ProjectID.ToString()).FirstOrDefault().Selected = true;

                //ViewBag.ProjectID = projects;
                ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName", returnObject.ProjectID);

            }

            var svs = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 3).ToList();
            svs.Insert(0, new EmployeeViewModel() { FullName = "Select" });

            ViewBag.SVList = new SelectList(svs, "FullName", "FullName");

            returnObject.PTW_master_ID = 0;
            returnObject.CompletedStage = 0;
            returnObject.Start_Date_Time = null;
            returnObject.End_Date_Time = null;
            returnObject.Applicant_Date_Time = null;
            returnObject.Stage2_O2 = null;
            returnObject.Stage2_CO2 = null;
            returnObject.Stage2_LEL = null;
            returnObject.Stage2_H2S = null;
            returnObject.Stage2_Comments = null;
            returnObject.Stage2_Assessor_Name = null;
            returnObject.Stage2_Assessor_Desig = null;
            returnObject.Stage2_Assessor_Date_Time = null;

            returnObject.Stage3_WSH_Name = null;
            returnObject.Stage3_WSH_Desig = null;
            returnObject.Stage3_Comments = null;
            returnObject.Stage3_WSH_Date_Time = null;

            returnObject.Stage4_Mng_Name = null;
            returnObject.Stage4_Mng_Desig = null;
            returnObject.Stage4_Comments = null;
            returnObject.Stage4_Date_Time = null;
            
            return View("~\\Views\\PTW\\CreateConspc.cshtml", returnObject);
        }



        List<string> getDayList(string strDay, List<PTWStage4ViewModel> stage4)
        {
            List<string> days = new List<string>();
            if (strDay == "Mon")
                return days;

            if (strDay == "Tue")
            {
                //Monday only
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Mon");
                }
                return days;
            }
            if (strDay == "Wen")
            {
                //Monday only
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Tue");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Mon");
                }
                return days;
            }
            if (strDay == "Thu")
            {
                //Monday only
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Wed");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Thu");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Mon");
                }
                return days;
            }
            if (strDay == "Fri")
            {
                //Monday only
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Thu");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Wed");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Tue");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-4).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Mon");
                }
                return days;
            }
            if (strDay == "Sat")
            {
                //Monday only
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Fri");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Thu");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Wed");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-4).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Thu");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-5).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Mon");
                }
                return days;
            }
            if (strDay == "Sun")
            {
                //Monday only
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Sat");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Fri");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Thu");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-4).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Wed");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-5).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Tue");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Mon");
                }
                return days;
            }

            return days;
        }


    }
}