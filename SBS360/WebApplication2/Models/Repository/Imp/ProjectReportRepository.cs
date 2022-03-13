using AutoMapper;
using Eng360Web.Models.Domain;
using Eng360Web.Models.Repository.Interface;
using Eng360Web.Models.Utility;
using Eng360Web.Models.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Repository.Imp
{
    public class ProjectReportRepository: IProjectReportRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
        Logger logger = LogManager.GetCurrentClassLogger();
        public int CreateProjectReport(ProjectReportViewModel eng_project_report,string path,string halfPath)
        {
            try
            {
                eng_project_report domainProject = Mapper.Map<eng_project_report>(eng_project_report);

                //domainProject.ReportDate = DateTime.Now;
                //domainProject.CreatedBy = AppSession.GetCurrentUserId();
                domainProject.CreatedDate = DateTime.Now;

                logger.Debug("WebProject Report:");
                logger.Debug(domainProject.ProjectID + "|" + domainProject.CreatedBy + "|" + domainProject.ReportDate);

                Eng360DB.eng_project_report.Add(domainProject);

          var result =      Eng360DB.SaveChanges();

                if(result>0)
                if (eng_project_report.files != null && eng_project_report.files.Count > 0)
                {
                    int i = 0;
                    foreach (var file in eng_project_report.files)
                    {
                        if (file != null)
                        {
                            var filename = Path.GetFileName(file.FileName);
                            var rn = DateTime.Now.ToString("yyMMddhhmmss");

                            Directory.CreateDirectory(path);
                            var finalPath = path + "P_" + domainProject.ProjectID + "PR_" + domainProject.ProjectReportID + "_" + i.ToString() + "_"+ rn + "." + getFileExtention(filename);
                            var halfPathEach = halfPath + "P_" + domainProject.ProjectID + "PR_" + domainProject.ProjectReportID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                            file.SaveAs(finalPath);

                            eng_project_report_files rptFiles = new eng_project_report_files();
                            rptFiles.ProjectReportID = domainProject.ProjectReportID;
                                rptFiles.FIleCaption = finalPath;

                            rptFiles.FilePath = halfPathEach;
                            rptFiles.FileName = file.FileName;

                            Eng360DB.eng_project_report_files.Add(rptFiles);
                            i++;
                        }
                    }
                    Eng360DB.SaveChanges();
                        

                        return result;
                }
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
            return -1;
        }


        public int CreateProjectReport(MobileProjectReportViewModel eng_project_report, string path, string halfPath)
        {
            try
            {
                eng_project_report domainProject = Mapper.Map<eng_project_report>(eng_project_report);

                //domainProject.ReportDate = DateTime.Now;
                // domainProject.CreatedBy = AppSession.GetCurrentUserId();
                domainProject.CreatedDate = DateTime.Now;



                Eng360DB.eng_project_report.Add(domainProject);

                var result = Eng360DB.SaveChanges();

                if (result > 0)
                    if (eng_project_report.files != null && eng_project_report.files.Count > 0)
                    {
                        int i = 0;
                        foreach (var file in eng_project_report.files)
                        {
                            if (file != null)
                            {

                                var data = Convert.FromBase64String(file.data);


                                var filename = file.filename;
                                var rn = DateTime.Now.ToString("yyMMddhhmmss");

                                Directory.CreateDirectory(path);
                                var finalPath = path + "P_" + domainProject.ProjectID + "PR_" + domainProject.ProjectReportID + "_" + i.ToString() + "_" + rn + "." + "jpg";
                                var halfPathEach = halfPath + "P_" + domainProject.ProjectID + "PR_" + domainProject.ProjectReportID + "_" + i.ToString() + "_" + rn + "." + "jpg";
                                File.WriteAllBytes(finalPath, data);

                                eng_project_report_files rptFiles = new eng_project_report_files();
                                rptFiles.ProjectReportID = domainProject.ProjectReportID;
                                rptFiles.FIleCaption = finalPath;

                                rptFiles.FilePath = halfPathEach;
                                rptFiles.FileName = file.filename;

                                Eng360DB.eng_project_report_files.Add(rptFiles);
                                i++;
                            }
                        }
                        Eng360DB.SaveChanges();
                        return result;
                    }
                return result;
            }
            catch (Exception ex)
            {
                logger.Debug("Project Report Error:");
                logger.Debug(ex.Message);
                logger.Debug(ex.StackTrace);
                if(ex.InnerException != null)
                {
                    logger.Debug(ex.InnerException.Message);
                    logger.Debug(ex.InnerException.StackTrace);

                }
                return -1;
            }
            return -1;
        }


        public int EditProjectReport(ProjectReportViewModel eng_project_report, string path, string halfPath)
        {
            try
            {
                eng_project_report domainProject = Mapper.Map<eng_project_report>(eng_project_report);
                domainProject.UpdatedBy = AppSession.GetCurrentUserId();
                domainProject.UpdatedDate = DateTime.Now;


                Eng360DB.Entry(domainProject).State = System.Data.Entity.EntityState.Modified;

                var result = Eng360DB.SaveChanges();

                if (result > 0)
                    if (eng_project_report.files != null && eng_project_report.files.Count > 0)
                    {
                        int i = 0;
                        foreach (var file in eng_project_report.files)
                        {
                            if (file != null)
                            {
                                var filename = Path.GetFileName(file.FileName);
                                var rn = DateTime.Now.ToString("yyMMddhhmmss");

                                Directory.CreateDirectory(path);
                                var finalPath = path + "P_" + domainProject.ProjectID + "PR_" + domainProject.ProjectReportID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                var halfPathEach = halfPath + "P_" + domainProject.ProjectID + "PR_" + domainProject.ProjectReportID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                file.SaveAs(finalPath);

                                eng_project_report_files rptFiles = new eng_project_report_files();
                                rptFiles.ProjectReportID = domainProject.ProjectReportID;
                                rptFiles.FilePath = halfPathEach;
                                rptFiles.FIleCaption = finalPath;
                                rptFiles.FileName = file.FileName;

                                Eng360DB.eng_project_report_files.Add(rptFiles);
                                i++;
                            }
                        }
                        Eng360DB.SaveChanges();
                        return result;
                    }
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
            return -1;
        }

        public int deleteFile(int? id)
        {
            try
            {
                var rptFile = Eng360DB.eng_project_report_files.Find(id);
                if (rptFile != null)
                {

                    Eng360DB.eng_project_report_files.Remove(rptFile);

                    File.Delete(rptFile.FIleCaption);

                }

                return Eng360DB.SaveChanges();
                 
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        string getFileExtention(string filename)
        {

            var result = filename.Split(new string[] { }, StringSplitOptions.None);

            return result[result.Length - 1];
        }

        public ProjectReportViewModel getProrjectRPT(int? id)
        {
            var projectRpt = Eng360DB.eng_project_report.Where(a => a.ProjectReportID == id).SingleOrDefault();
            return Mapper.Map<ProjectReportViewModel>(projectRpt);
        }

        public ProjectViewModel getProject(int? id)
        {
            var project = Eng360DB.eng_project_master.Where(a => a.ProjectID == id).SingleOrDefault();
           var Returnobject = Mapper.Map<ProjectViewModel>(project);

            if(project.Start_Date!=null)
            Returnobject.Start_Date =Convert.ToDateTime(  project.Start_Date).ToString("dd/MM/yyyy");
            if (project.End_Date != null)
                Returnobject.End_Date = Convert.ToDateTime(project.End_Date).ToString("dd/MM/yyyy");
            return Returnobject;
        }

        public List<ProjectReportTaskStatusViewModel> getTaskStatus()
        {
            var status = Eng360DB.eng_sys_task_status.ToList();

            return Mapper.Map<List<ProjectReportTaskStatusViewModel>>(status);
           
        }

        public List<ProjectReportViewModel> getAllProjectReports()
        {
            var eng_project_report = Eng360DB.eng_project_report.ToList();
            var lstProjectReportView = Mapper.Map<List<ProjectReportViewModel>>(eng_project_report);
            return lstProjectReportView;
        }

        public int DeleteProjectReport(int prID)
        {
            var _db_pr = Eng360DB.eng_project_report.First(a => a.ProjectReportID == prID);

            Eng360DB.eng_project_report.Remove(_db_pr);

            return Eng360DB.SaveChanges();
        }

        public List<ProjectReportViewModel> getFilterProjectReports(FilterViewModel filter)
        {
            var dCurrentDayofThisMonth = DateTime.Today.ToString("yyyy-MM-dd");
            var dFirstDayOfThisMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            var dLastDayOfLastMonth = dFirstDayOfThisMonth.AddDays(-1).ToString("yyyy-MM-dd");
            var dFirstDayOfLastMonth = dFirstDayOfThisMonth.AddMonths(-1).ToString("yyyy-MM-dd"); ;
            var dFirstDayOfCurrentMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).ToString("yyyy-MM-dd");

            var sql = "select * from eng_project_report ";
            var where = "";
            var newsql = "(select ProjectID from eng_project_master where QuotationID in (select QuoteID from eng_quote_master inner join eng_client_master on eng_quote_master.ClientID = eng_client_master.ClientID where eng_quote_master.ClientID = " + filter.ClientID + "))";
            if (filter.ProjectID > 0)
                where = "ProjectID = " + filter.ProjectID ;

            if (filter.ClientID > 0)
                if (where == "")
                    where += " ProjectID in " + newsql;
                else
                    where += " and ProjectID =" + newsql;

            if (filter.UserID > 0)
                if (where == "")
                    where += " CreatedBy =" + filter.UserID;
                else
                    where += " and CreatedBy =" + filter.UserID;

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

            

            if (where != "")
                sql = sql + "Where " + where;


            // DbSqlQuery<eng_employee_profile> data = Eng360DB.eng_employee_profile.SqlQuery(sql);
            var data = Eng360DB.eng_project_report.SqlQuery(sql).ToList();

            var lstprView = Mapper.Map<List<ProjectReportViewModel>>(data);
            return lstprView;
        }

        public List<getProjectReport_Result> GetAllProjectManagementReports()
        {
            return Eng360DB.getProjectReport().ToList();
        }
        public List<getTimeReport_Result> GetAllTimeManagementReports()
        {
            return Eng360DB.getTimeReport().ToList();
        }

        public List<MobileProjectReportViewModel> getAllProjectReports(int userID)
        {
            DateTime from = DateTime.Parse(DateTime.Now.ToShortDateString());
            DateTime to = DateTime.Parse(DateTime.Now.ToShortDateString()).AddDays(-30);

            var eng_project_report = Eng360DB.eng_project_report.Where(a => a.CreatedBy == userID && a.CreatedDate <= from && a.CreatedDate >= to).OrderByDescending(od => od.CreatedDate).ToList();
            var lstProjectReportView = Mapper.Map<List<MobileProjectReportViewModel>>(eng_project_report);

            foreach (var item in lstProjectReportView)
            {
                var domianItem = eng_project_report.Where(a => a.ProjectReportID == item.ProjectReportID).FirstOrDefault();
                item.ProjectName = domianItem.eng_project_master.ProjectNo + " - " + domianItem.eng_project_master.ProjectName;
                item.ReportDate = Convert.ToDateTime(domianItem.ReportDate).ToString("dd/MM/yyyy");
                var filepaths = domianItem.eng_project_report_files.Select(a => a.FilePath).ToArray();
                List<FileUploadViewModel> fileUpload = new List<FileUploadViewModel>();
                foreach (var file in filepaths)
                {
                    fileUpload.Add(new FileUploadViewModel() { filename = file });
                }
                item.files = fileUpload;
            }

            return lstProjectReportView;
        }


    }
}