using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Eng360Web.Models.Domain;
using Eng360Web.Models.Service.Imp;

using AutoMapper;
using Eng360Web.Models.Utility;
using System.Threading.Tasks;
using System.Web;
using System.Globalization;
using Eng360Web.Models.Service.Interface;
using System.IO;
using NLog;

namespace Eng360Web.Controllers
{
    public class MobileLoginController : ApiController
    {
        private readonly IUserServices userService;
        private readonly IProjectServices projectService;
        private readonly IEmployeeServices employeeService;
        private readonly IProjectReportService projectReportService;
        private readonly ITimeEntryServices timeentryService;
        private readonly IClaimServices claimService;
        private readonly ICompanyServices companyService;
        private readonly ISafetyServices safetyService;
        private readonly IQualityDefectServices qualitydefectService;
        private readonly ISupplierServices supplierService;
        private readonly IPTWServices ptwServices;
        Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IRAServices raService;


        public MobileLoginController(IUserServices _userService, IProjectServices _projectService, IEmployeeServices _employeeService,
            IProjectReportService _projectReportService, ITimeEntryServices _timeentryService,
            IClaimServices _claimService, ICompanyServices _companyService,
            ISafetyServices _safetyService, IQualityDefectServices _qualitydefectService,
            ISupplierServices _supplierService, IPTWServices _ptwServices, IRAServices _raService)
        {
            userService = _userService;
            projectService = _projectService;
            employeeService = _employeeService;
            projectReportService = _projectReportService;
            timeentryService = _timeentryService;
            claimService = _claimService;
            companyService = _companyService;
            safetyService = _safetyService;
            qualitydefectService = _qualitydefectService;
            supplierService = _supplierService;
            ptwServices = _ptwServices;
            raService = _raService;
        }
        [HttpGet]
        public LoginResponseViewModel EngLogin(string username, string password)
        {
            //logger.Debug(string.Format("Login Username:{0}", username));

            //SecureString secPwd = new SecureString();
            try
            {

                var returnUser = userService.ValidateUserLogin(username, password);
                if (returnUser != null)
                {
                    if (returnUser.IsActive == 1)
                    {
                        LoginResponseViewModel returnResponse = new Models.ViewModel.LoginResponseViewModel();
                        returnResponse.Credentials = returnUser.Password;
                        returnResponse.Success = true;
                        returnResponse.User = new EngLiteUserViewModel() { groupID = returnUser.GroupID.ToString(), userId = returnUser.UserID.ToString(), userFullName = returnUser.DisplayName, userName = returnUser.UserName };

                        return returnResponse;
                    }
                    else
                    {

                    }

                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                //secPwd.Dispose();
            }
            return new LoginResponseViewModel();
            //return authService.SingleLogin(username, password);
        }

        [HttpPost]
        public ResponseViewModel CreateTimeEntry(MobileTimeEntryViewModel eng_time_entry)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                eng_time_entry.SubmittedDate = DateTime.Now;
                eng_time_entry.WEHflag = eng_time_entry.ispublicholiday == 1 ? true : false;
                eng_time_entry.LBflag = eng_time_entry.islunchtime == 1 ? true : false;

                //if (!string.IsNullOrEmpty(eng_time_entry.Work_Start_Time) && !string.IsNullOrEmpty(eng_time_entry.Work_End_Time))
                //{
                //    TimeSpan workhours = Convert.ToDateTime(eng_time_entry.Work_End_Time) - Convert.ToDateTime(eng_time_entry.Work_Start_Time);
                //    eng_time_entry.No_of_WorkHours = Convert.ToDecimal(workhours.TotalHours);
                //}

                //TimeSpan othours;
                //if (!string.IsNullOrEmpty(eng_time_entry.Ot_Start_Time) && !string.IsNullOrEmpty(eng_time_entry.Ot_End_Time))
                //{
                //    TimeSpan othours = Convert.ToDateTime(eng_time_entry.Ot_End_Time) - Convert.ToDateTime(eng_time_entry.Ot_Start_Time);
                //    eng_time_entry.No_of_OtHours = Convert.ToDecimal(othours.TotalHours);
                //}


                eng_time_entry.ReportEndDate = DateTime.ParseExact(eng_time_entry.Work_End_Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


                if (!string.IsNullOrEmpty(eng_time_entry.ReportDate))
                    eng_time_entry.Work_Start_Time =
                      DateTime.ParseExact(eng_time_entry.ReportDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("HH:mm");

                if (!string.IsNullOrEmpty(eng_time_entry.Work_End_Time))
                    eng_time_entry.Work_End_Time =
                      DateTime.ParseExact(eng_time_entry.Work_End_Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("HH:mm");

                eng_time_entry.ReportDate = DateTime.ParseExact(eng_time_entry.ReportDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                //if (!string.IsNullOrEmpty(eng_time_entry.Ot_Start_Time))
                //    eng_time_entry.Ot_Start_Time =
                //        DateTime.ParseExact(eng_time_entry.Ot_Start_Time, "HH:mm", CultureInfo.InvariantCulture).ToString("HH:mm");

                //if (!string.IsNullOrEmpty(eng_time_entry.Ot_End_Time))
                //    eng_time_entry.Ot_End_Time =
                //        DateTime.ParseExact(eng_time_entry.Ot_End_Time, "HH:mm", CultureInfo.InvariantCulture).ToString("HH:mm");

                if (eng_time_entry.ReportDate.ToString().Contains("-"))
                {
                    eng_time_entry.ReportDate = DateTime.ParseExact(eng_time_entry.ReportDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                else
                {
                    eng_time_entry.ReportDate = DateTime.ParseExact(eng_time_entry.ReportDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }

                if (eng_time_entry.ReportEndDate.ToString().Contains("-"))
                {
                    eng_time_entry.ReportEndDate = DateTime.ParseExact(eng_time_entry.ReportEndDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                else
                {
                    eng_time_entry.ReportEndDate = DateTime.ParseExact(eng_time_entry.ReportEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                //eng_time_entry.ReportDate = DateTime.ParseExact(eng_time_entry.ReportDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                var eng = Mapper.Map<TimeEntryViewModel>(eng_time_entry);
                int result = 0;
                //var result = timeentryService.CreateTimeEntry(eng);
                var emparr = eng_time_entry.SelectedEmpList.Split(',');
                foreach (var emp in emparr)
                {
                    eng.EmpID = Convert.ToInt32(emp);
                    result = timeentryService.CreateTimeEntry(eng);
                }
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
                logger.Debug("Claim Report Error:");
                logger.Debug(ex.Message);
                logger.Debug(ex.StackTrace);
            }
            return response;
        }


        [HttpPost]
        public ResponseViewModel CreateSafetyDTTR(MobileSafetyMasterViewModel input)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var eng_safety_master = Mapper.Map<SafetyMasterViewModel>(input);

                if (!string.IsNullOrEmpty(eng_safety_master.RepDate))
                    if (eng_safety_master.RepDate.ToString().Contains("-"))
                    {
                        eng_safety_master.RepDate = DateTime.ParseExact(eng_safety_master.RepDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        eng_safety_master.RepDate = DateTime.ParseExact(eng_safety_master.RepDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    }

                //eng_safety_master.RepDate = DateTime.ParseExact(eng_safety_master.RepDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (!string.IsNullOrEmpty(eng_safety_master.RepTime))
                    eng_safety_master.RepTime = DateTime.ParseExact(eng_safety_master.RepTime, "HH:mm", CultureInfo.InvariantCulture).ToString("HH:mm");

                //eng_safety_master.SubmittedBy = input.SubmittedBy;
                eng_safety_master.CreatedDate = DateTime.Now;

                var result = safetyService.CreateSafety(eng_safety_master, input.hazardList1, input.ppeList1, input.workerList1);
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
                logger.Debug("Safty DTTR creation Error:");
                logger.Debug(ex.Message);
                logger.Debug(ex.StackTrace);
            }
            return response;
        }
        [HttpPost]
        public ResponseViewModel EditDT(MobileQualityDefectCPAViewModel eng_mob_cpa)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {

                string halfPath = "/images/QualityFiles/" + eng_mob_cpa.UpdatedBy + "/";


                eng_mob_cpa.UpdatedDate = DateTime.Now;

                //  var eng_cpa = Mapper.Map<QualityDefectCPAViewModel>(eng_mob_cpa);

                var result = qualitydefectService.AddEditCpa(eng_mob_cpa, Path.Combine(HttpContext.Current.Server.MapPath("~/images/QualityFiles/" + eng_mob_cpa.UpdatedBy + "/")), halfPath);
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
                logger.Debug("UpdateDT Error:");
                logger.Debug(ex.Message);
                logger.Debug(ex.StackTrace);
            }
            return response;
        }

        [HttpPost]
        public ResponseViewModel createQIP(MobileQualityDefectViewModel eng_qip)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {

                eng_qip.InspectedDate = DateTime.ParseExact(eng_qip.InspectedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                if (eng_qip.SupplierFlag == false)
                    eng_qip.SupplierID = null;

                string halfPath = "/images/QualityFiles/" + eng_qip.CreatedBy + "/";
                var result = qualitydefectService.CreateQualityDefect(eng_qip, Path.Combine(HttpContext.Current.Server.MapPath("~/images/QualityFiles/" + eng_qip.CreatedBy + "/")), halfPath);
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
                logger.Debug("createQIP Error:");
                logger.Debug(ex.Message);
                logger.Debug(ex.StackTrace);
            }
            return response;
        }


        [HttpGet]
        public List<MobileQualityDefectDetailViewModel> getDefectTracking()
        {
            var list = qualitydefectService.getAllDefectDetails().Where(a => a.DefectStatus != "Pending").ToList();
            var retunList = Mapper.Map<List<MobileQualityDefectDetailViewModel>>(list);

            foreach (var a in retunList)
            {
                var domainDefect = list.Where(b => b.DefectDetailID == a.DefectDetailID).First();

                a.InspectionNO = domainDefect.eng_qa_defect.DefectDisplayID;
                a.ProjectName = domainDefect.eng_qa_defect.eng_project_master.ProjectName;
            }

            return retunList;
        }

        [HttpGet]
        public MobileQualityDefectCPAViewModel getDefectCPA(int id)
        {
            var cpa = qualitydefectService.getQualityCPADetail(id);


            var defect = qualitydefectService.getQualityDefectDetail(id);

            if (cpa == null)
            {

                var cpaview = new MobileQualityDefectCPAViewModel();
                cpaview.DefectDetailID = defect.DefectDetailID;
                cpaview.ProjectName = defect.eng_qa_defect.eng_project_master.ProjectName;
                cpaview.DefectTrackNum = defect.DefectTrackNum;
                cpaview.Inspected_Date = defect.eng_qa_defect.InspectedDate;
                cpaview.DefectTitle = defect.DefectTitle;
                cpaview.DefectImpactDetails = defect.DefectImpactDetails;
                cpaview.SupplierName = defect.eng_qa_defect.eng_supplier_master == null ? "" : defect.eng_qa_defect.eng_supplier_master.Company_Name;



                return cpaview;
            }
            var MobileCPA = Mapper.Map<MobileQualityDefectCPAViewModel>(cpa);
            MobileCPA.DefectDetailID = cpa.eng_qa_defect_detail.DefectDetailID;
            MobileCPA.ProjectName = cpa.eng_qa_defect_detail.eng_qa_defect.eng_project_master.ProjectName;
            MobileCPA.DefectTrackNum = cpa.eng_qa_defect_detail.DefectTrackNum;
            MobileCPA.Inspected_Date = cpa.eng_qa_defect_detail.eng_qa_defect.InspectedDate;
            MobileCPA.DefectTitle = cpa.eng_qa_defect_detail.DefectTitle;
            MobileCPA.DefectImpactDetails = cpa.eng_qa_defect_detail.DefectImpactDetails;
            MobileCPA.SupplierName = cpa.eng_qa_defect_detail.eng_qa_defect.eng_supplier_master == null ? "" : cpa.eng_qa_defect_detail.eng_qa_defect.eng_supplier_master.Company_Name;


            return MobileCPA;
        }



        [HttpPost]
        public ResponseViewModel CreateProjectReport(MobileProjectReportViewModel eng_project_report)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                if (eng_project_report.ReportDate.ToString().Contains("-"))
                {
                    eng_project_report.ReportDate = DateTime.ParseExact(eng_project_report.ReportDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                else
                {
                    eng_project_report.ReportDate = DateTime.ParseExact(eng_project_report.ReportDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                if (!string.IsNullOrEmpty(eng_project_report.Start_Date_Time))
                    eng_project_report.Start_Date_Time =
                      DateTime.ParseExact(eng_project_report.Start_Date_Time, "HH:mm", CultureInfo.InvariantCulture).ToString("HH:mm");

                if (!string.IsNullOrEmpty(eng_project_report.End_Date_Time))
                    eng_project_report.End_Date_Time =
                      DateTime.ParseExact(eng_project_report.End_Date_Time, "HH:mm", CultureInfo.InvariantCulture).ToString("HH:mm");

                string halfPath = "/images/UploadedFiles/" + eng_project_report.CreatedBy + "/";
                // eng_project_report.ProgressPercentage = "0%";
                var result = projectReportService.CreateProjectReport(eng_project_report, Path.Combine(HttpContext.Current.Server.MapPath("~/images/UploadedFiles/" + eng_project_report.CreatedBy + "/")), halfPath);
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
                logger.Debug("Project Report Error:");
                logger.Debug(ex.Message);
                logger.Debug(ex.StackTrace);
                
            }
            return response;
        }

        [HttpPost]
        public ResponseViewModel CreateSIReport(MobileNewSafetyInspectionViewModel eng_safety_esh)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                if (eng_safety_esh.InspectionDate.ToString().Contains("-"))
                {
                    eng_safety_esh.InspectionDate = DateTime.ParseExact(eng_safety_esh.InspectionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                else
                {
                    eng_safety_esh.InspectionDate = DateTime.ParseExact(eng_safety_esh.InspectionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }

                string halfPath = "/images/UploadedFiles/" + eng_safety_esh.CreatedBy + "/";
                // eng_project_report.ProgressPercentage = "0%";
                var result = safetyService.NewSICreate(eng_safety_esh, Path.Combine(HttpContext.Current.Server.MapPath("~/images/UploadedFiles/" + eng_safety_esh.CreatedBy + "/")), halfPath);
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
                logger.Debug("Claim Report Error:");
                logger.Debug(ex.Message);
                logger.Debug(ex.StackTrace);
            }
            return response;
        }

        [HttpGet]
        public List<ClaimTypeViewModel> getClaimType()
        {
            return claimService.getClaimType();
        }

        [HttpPost]
        public ResponseViewModel createCliam(MobileCliamViewModel eng_claim)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var recpDate = "";
                if (eng_claim.SubmittedDate.ToString().Contains("-"))
                {
                    eng_claim.SubmittedDate = DateTime.ParseExact(eng_claim.SubmittedDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                else
                {
                    eng_claim.SubmittedDate = DateTime.ParseExact(eng_claim.SubmittedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }

                if (eng_claim.eng_claim_description.FirstOrDefault().RecpDate.Contains("-"))
                {
                    recpDate = DateTime.ParseExact(eng_claim.eng_claim_description.FirstOrDefault().RecpDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                else
                {
                    recpDate = DateTime.ParseExact(eng_claim.eng_claim_description.FirstOrDefault().RecpDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }

                ClaimViewModel claimvm = new ClaimViewModel()
                {
                    Mobilefiles = eng_claim.files,
                    ProjectID = eng_claim.ProjectID,
                    SubmittedBy = Int32.Parse(eng_claim.SubmittedBy),
                    Status = 0,
                    SVRemarks = eng_claim.SVRemarks,
                    ClaimAgainst = "Project",
                    UserID = eng_claim.UserID,
                    //ApprovedBy = Int32.Parse( eng_claim.ApprovedBy),
                    //SubmittedDate = DateTime.Parse(DateTime.ParseExact(eng_claim.SubmittedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy"))
                    SubmittedDate = eng_claim.SubmittedDate
                };
                ClaimDescriptionViewModel claimDescVm = new ClaimDescriptionViewModel()
                {
                    ClaimDescription = eng_claim.eng_claim_description.FirstOrDefault().ClaimDescription,
                    GST = eng_claim.eng_claim_description.FirstOrDefault().GST,
                    RecpAmount = eng_claim.eng_claim_description.FirstOrDefault().RecpAmount,
                    //RecpDate = DateTime.Parse(DateTime.ParseExact(eng_claim.eng_claim_description.FirstOrDefault().RecpDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")),
                    //RecpDate = DateTime.ParseExact(eng_claim.eng_claim_description.FirstOrDefault().RecpDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy"),
                    RecpDate = recpDate,
                    ClaimTypeID = eng_claim.eng_claim_description.FirstOrDefault().ClaimTypeID
                };

                claimvm.eng_claim_description = new List<ClaimDescriptionViewModel>() { claimDescVm };

                string halfPath = "/images/ClaimFiles/" + eng_claim.SubmittedBy + "/";
                var result = claimService.CreateClaim(claimvm, Path.Combine(HttpContext.Current.Server.MapPath("~/images/ClaimFiles/" + eng_claim.SubmittedBy + "/")), halfPath);
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
                logger.Debug("Claim Report Error:");
                logger.Debug(ex.Message);
                logger.Debug(ex.StackTrace);
            }

            return response;
        }

        [HttpGet]
        public List<MobileProjectReportViewModel> getAllProjectReports(int id)
        {
            return projectReportService.getAllProjectReports(id);
        }
        [HttpGet]
        public List<MobileNewSafetyInspectionViewModel> getAllSafetyInspections(int id)
        {
            return safetyService.getAllSafetyInspections(id);
        }
        [HttpGet]
        public List<MobileSafetyMasterViewModel> getAllDTTRReports()
        {
            var safety = safetyService.getAllSafetys().ToList();

            var returnObject = Mapper.Map<List<MobileSafetyMasterViewModel>>(safety);

            foreach (var saf in safety)
            {
                var hazardIDList = saf.eng_safety_hazard_list.Select(a => a.HazardID).ToList();
                var hazardStrList = string.Join(",", safetyService.getAllHazards().Where(a => hazardIDList.Contains(a.HazardID)).Select(b => b.HazardDesc).ToList());

                var PPEIDList = saf.eng_safety_ppe_list.Select(a => a.PPEID).ToList();
                var PPEStrList = string.Join(",", safetyService.getAllPPEs().Where(a => hazardIDList.Contains(a.PPEID)).Select(b => b.PPE_Desc).ToList());

                // var WorkerIDList =;
                var WorkerStrList = string.Join(",", saf.eng_safety_worker_list.Select(a => a.eng_employee_profile.EmpID + "-" + a.eng_employee_profile.FirstName).ToList());


                var safObject = returnObject.Find(a => a.SafetyID == saf.SafetyID);
                safObject.supperviosrName = saf.eng_users.DisplayName;
                safObject.HazardList = hazardStrList;
                safObject.PPEList = PPEStrList;
                safObject.WorkerList = WorkerStrList;
                safObject.supperviosrName = saf.eng_users.DisplayName;
            }
            return returnObject;
        }

        [HttpGet]
        public List<MobileProjectReportViewModel> GetAllProjectManagementReports()
        {
            var listofSummary = projectReportService.GetAllProjectManagementReports();
            List<MobileProjectReportViewModel> returnobj = new List<MobileProjectReportViewModel>();

            foreach (var los in listofSummary)
            {
                var prj = projectReportService.getProject(los.ProjectID);
                MobileProjectReportViewModel mprv = new MobileProjectReportViewModel();
                mprv.ProjectReportID = 0;
                mprv.ProjectID = prj.ProjectID;
                mprv.ProjectName = prj.ProjectName;
                mprv.TaskStatusID = prj.Project_Status_ID;
                mprv.Start_Date_Time = prj.Start_Date;
                mprv.End_Date_Time = prj.End_Date;
                mprv.TotalHrsReported = los.WrkHrs;
                mprv.CreatedDate = prj.CreatedDate;
                mprv.UpdatedDate = DateTime.Now;
                returnobj.Add(mprv);
            }
            return returnobj;
        }
        [HttpGet]
        public List<MobileProjectReportViewModel> GetAllTimeManagementReports()
        {
            var listofSummary = projectReportService.GetAllTimeManagementReports();
            List<MobileProjectReportViewModel> returnobj = new List<MobileProjectReportViewModel>();

            foreach (var los in listofSummary)
            {
                var prj = projectReportService.getProject(los.ProjectID);
                MobileProjectReportViewModel mprv = new MobileProjectReportViewModel();
                mprv.ProjectReportID = 0;
                mprv.ProjectID = prj.ProjectID;
                mprv.ProjectName = prj.ProjectName;
                mprv.TaskStatusID = prj.Project_Status_ID;
                mprv.Start_Date_Time = prj.Start_Date;
                mprv.End_Date_Time = prj.End_Date;
                mprv.TotalHrsReported = los.TotalHRS.ToString();
                mprv.CreatedDate = prj.CreatedDate;
                mprv.UpdatedDate = DateTime.Now;
                mprv.TotalOTHrsReported = los.totalOtHrs.ToString();

                returnobj.Add(mprv);
            }
            return returnobj;
        }

        [HttpGet]
        public MobileCompanyViewModel getCompanyDetails()
        {
            var company = companyService.getCompany();
            return new MobileCompanyViewModel() { Lunch_Break_Hours = company.Lunch_Break_Hours, Normal_Work_Hours = company.Normal_Work_Hours, Weekend_Work_Hours = company.Weekend_Work_Hours, CompanyName = company.CompanyName };
        }

        [HttpGet]
        public List<MobileCliamViewModel> getClaims(int userid, int groupid)
        {
            var claims = claimService.getAllClaims(userid, groupid).ToList();
            if (claims != null && claims.Count > 0)
                claims = claims.Where(a => a.ClaimAgainst == "Project").ToList();

            var lstMobileClaims = Mapper.Map<List<MobileCliamViewModel>>(claims);

            foreach (var claim in lstMobileClaims)
            {
                double clamt = 0;

                var domainClaim = claims.Where(a => a.ClaimID == claim.ClaimID).FirstOrDefault();
                if (domainClaim.eng_claim_files != null && domainClaim.eng_claim_files.Count > 0)
                    claim.filepath = domainClaim.eng_claim_files.FirstOrDefault().FilePath;

                claim.SubmittedDate = Convert.ToDateTime(domainClaim.SubmittedDate).ToString("dd/MM/yyyy");

                claim.EmployeeName = domainClaim.eng_employee_profile.FirstName;
                //claim.ManagerName = domainClaim.eng_users.DisplayName;
                claim.ProjectName = domainClaim.eng_project_master.ProjectNo + " - " + domainClaim.eng_project_master.ProjectName;// domainClaim.eng_project_master.ProjectName;
                foreach (var desc in claim.eng_claim_description)
                {
                    desc.ClaimCategoryName = domainClaim.eng_claim_description.Where(d => d.ClaimDescID == desc.ClaimDescID).FirstOrDefault().eng_sys_claimtype.ClaimType;
                    //if(desc.GST == "Y")
                    //clamt = clamt + (double)desc.RecpAmount + (double)desc.RecpAmount * 7;
                    //else
                    clamt = clamt + (double)desc.RecpAmount;

                }
                claim.ClaimAmount = (decimal)clamt;
            }

            return lstMobileClaims;
        }



        [HttpGet]
        public List<MobileTimeEntryViewModel> getAllTimeEntry(int id)
        {
            return timeentryService.getAllTimeEntry(id);
        }

        [HttpGet]
        public List<ProjectMobileViewModel> getProjects()
        {
            List<ProjectViewModel> returnObject = new List<ProjectViewModel>();
            returnObject = projectService.getAllProjects();
            List<ProjectMobileViewModel> pmv = new List<Models.ViewModel.ProjectMobileViewModel>();
            foreach (var project in returnObject)
            {
                pmv.Add(new ProjectMobileViewModel() { ProjectID = project.ProjectID, ProjectName = project.ProjectName, ProjectNo = project.ProjectNo });
            }

            return pmv;
        }
        [HttpGet]
        public List<EmployeeMobileViewModel> getAllEmployees()
        {
            List<EmployeeViewModel> returnObject = new List<EmployeeViewModel>();
            returnObject = employeeService.getAllEmployees();
            List<EmployeeMobileViewModel> emv = new List<Models.ViewModel.EmployeeMobileViewModel>();
            foreach (var emp in returnObject)
            {
                emv.Add(new EmployeeMobileViewModel() { EmpID = emp.UserID.ToString(), FirstName = emp.FirstName, LastName = emp.LastName, EmpNo = emp.EmpID, Designation = emp.Designation, IsActive = Convert.ToInt32(emp.IsActive) });
            }

            return emv;
        }

        [HttpGet]
        public List<UserMobileViewModel> getAllUsers()
        {
            List<UserMobileViewModel> returnObject = new List<UserMobileViewModel>();
            var obj = userService.getAllUsers().Where(a => a.IsActive == 1 && a.GroupID == 2).ToList();

            foreach (var usr in obj)
            {
                returnObject.Add(new UserMobileViewModel() { UserID = usr.UserID, DisplayName = usr.DisplayName });
            }
            return returnObject;

        }

        [HttpGet]
        public List<MobileSupplierViewModel> getAllSuppliers()
        {
            var returnSupplier = supplierService.getAllSuppliers().Where(a => a.IsActive == 1).ToList();
            var mappedReturn = Mapper.Map<List<MobileSupplierViewModel>>(returnSupplier);

            return mappedReturn;
        }

        //[Route("user/PostUserImage")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> PostUserImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {



                            var filePath = HttpContext.Current.Server.MapPath("~/images/ProfilePictures/" + postedFile.FileName);

                            postedFile.SaveAs(filePath);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

        [HttpGet]
        public List<SafetyHazardListViewModel> getAllHazards()
        {
            var hazards = safetyService.getAllHazards().ToList();

            return hazards;
        }
        [HttpGet]
        public List<SafetyPPEListViewModel> getAllPPE()
        {
            var ppes = safetyService.getAllPPEs().ToList();

            return ppes;
        }

        [HttpGet]
        public List<MobileQualityDefectViewModel> getAllQIP()
        {
            var qualitydefects = qualitydefectService.getAllQualityDefects().ToList();
            var returnObject = Mapper.Map<List<MobileQualityDefectViewModel>>(qualitydefects);
            foreach (var ro in returnObject)
            {
                ro.SupplierID = ro.SupplierID == null ? 0 : Int32.Parse(ro.SupplierID.ToString());
                ro.ProjectName = qualitydefects.Where(a => a.DefectID == ro.DefectID).FirstOrDefault().eng_project_master.ProjectName;
            }


            return returnObject;
        }

        [HttpGet]
        public List<MobilePTWRConfigSatgeOne> getPTWStageOneConfig()
        {
            return ptwServices.getPTWStageOneConfig();
        }

        [HttpGet]
        public List<MobilePTWstages> getPTWStages()
        {
            return ptwServices.getPTWStages();
        }

        [HttpGet]
        public List<MobilePTWMaster> getAllPTW()
        {

            return ptwServices.getALLPTWMaster();
        }

        [HttpPost]
        public ResponseViewModel createPTWR(MobilePTWMaster engPTW)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {

                //eng_qip.InspectedDate = DateTime.ParseExact(eng_qip.InspectedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");




                var result = ptwServices.createPtw(engPTW);
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
                logger.Debug("createPTW Error:");
                logger.Debug(ex.Message);
                logger.Debug(ex.StackTrace);
            }
            return response;
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


        [HttpGet]
        public List<MobileRATransMasterViewModel> getAllRA()
        {
            var raList = raService.getAllRiskAssessments();

            var raMobileList = Mapper.Map<List<MobileRATransMasterViewModel>>(raList);

            foreach (var ra in raMobileList)
            {
                var project = projectService.getProject(ra.ProjectID);
                ra.ProjectTitle = project.ProjectName;
                //ra.DueDate = DateTime.Parse(ra.DueDate).ToShortDateString();
                if (ra.eng_ra_trans_racm != null)
                {
                    foreach (var racm in ra.eng_ra_trans_racm)
                    {
                        racm.DueDate = DateTime.Parse(racm.DueDate).ToString("dd/MM/yyyy");
                    }
                }

            }

            return raMobileList;
        }

        [HttpGet]
        public MobileRATransMasterViewModel getRA(int id)
        {
            var raList = raService.getAllRiskAssessments();

            var ra = raService.getAllRiskAssessments().Where(a => a.RAID == id).SingleOrDefault();

            var raMobileList = Mapper.Map<MobileRATransMasterViewModel>(ra);


            var project = projectService.getProject(raMobileList.ProjectID);
            raMobileList.ProjectTitle = project.ProjectName;
            //ra.DueDate = DateTime.Parse(ra.DueDate).ToShortDateString();
            if (ra.eng_ra_trans_racm != null)
            {
                foreach (var racm in ra.eng_ra_trans_racm)
                {
                    racm.DueDate = DateTime.Parse(racm.DueDate).ToString("dd/MM/yyyy");
                }
            }



            return raMobileList;
        }

        [HttpGet]
        public List<RAMastersViewModel> getAllHazardList()
        {
            var HazardAll = raService.getAllHazardList();

            HazardAll.Insert(0, new RAMastersViewModel() { ItemID = 0, ItemDescription = "Select" });

            return HazardAll;
        }

        [HttpGet]
        public List<RAMastersViewModel> getAllControlMeasures()
        {
            var ControlAll = raService.getAllControlMeasures();
            ControlAll.Insert(0, new RAMastersViewModel() { ItemID = 0, ItemDescription = "Select" });
            return ControlAll;
        }
        [HttpGet]
        public List<MobileRATransDetail1ViewModel> getAllProjectWorkActivities(int id)
        {
            List<MobileRATransDetail1ViewModel> returnObject = new List<MobileRATransDetail1ViewModel>();
            var WorkAll = raService.getAllProjectWorkActivities(id);

            foreach (var work in WorkAll)
            {
                returnObject.Add(new MobileRATransDetail1ViewModel() { RAWADetID = work.RAWADetID, WorkActivities = work.WorkActivities });
            }
            returnObject.Insert(0, new MobileRATransDetail1ViewModel() { RAWADetID = 0, WorkActivities = "Select" });
            return returnObject;
        }
        [HttpGet]
        public List<RALikelihoodViewModel> getAllLikelihoods()
        {
            var LhAll = raService.getAllLikelihoods();
            LhAll.Insert(0, new RALikelihoodViewModel() { RMLHID = 0, Likelihood_Value = 0 });
            return LhAll;
        }
        [HttpGet]
        public List<RASeverityViewModel> getAllSeverities()
        {
            var SvAll = raService.getAllSeverities();
            SvAll.Insert(0, new RASeverityViewModel() { RMSVID = 0, Severity_Value = 0 });
            return SvAll;
        }
        [HttpGet]
        public List<RAMastersViewModel> getAllPAHs()
        {
            var PahAll = raService.getAllPAHs();
            PahAll.Insert(0, new RAMastersViewModel() { ItemID = 0, ItemDescription = "Select" });
            return PahAll;
        }
        [HttpGet]
        public List<EmployeeMobileViewModel> ImpOfficers()
        {
            var emps = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 2).ToList();
            //   emps.Insert(0, new EmployeeViewModel() { FullName = "Select" });

            List<EmployeeMobileViewModel> emv = new List<Models.ViewModel.EmployeeMobileViewModel>();
            foreach (var emp in emps)
            {
                emv.Add(new EmployeeMobileViewModel() { EmpID = emp.UserID.ToString(), FirstName = emp.FirstName, LastName = emp.LastName, EmpNo = emp.EmpID, Designation = emp.Designation, IsActive = Convert.ToInt32(emp.IsActive) });
            }

            return emv;
            // return emps;
        }

        [HttpGet]
        public List<MobileRATransRACMViewModel> GetRADetils(int id)
        {
            return raService.GetRaDetails(id);
        }

        [HttpPost]
        public ResponseViewModel createRACM(RATransRACMViewModel racm)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {

                if (racm.DueDate.ToString().Contains("-"))
                {
                    racm.DueDate = DateTime.ParseExact(racm.DueDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                else
                {
                    racm.DueDate = DateTime.ParseExact(racm.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                //eng_qip.InspectedDate = DateTime.ParseExact(eng_qip.InspectedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                var result = raService.AddRACMMobile(racm);
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
                logger.Debug("createRACM Error:");
                logger.Debug(ex.Message);
                logger.Debug(ex.StackTrace);
            }
            return response;
        }


    }
}
