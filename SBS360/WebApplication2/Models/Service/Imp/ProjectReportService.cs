using Eng360Web.Models.Domain;
using Eng360Web.Models.Repository.Interface;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Imp
{
    public class ProjectReportService : IProjectReportService
    {
        private readonly IProjectReportRepository projectReportRepository;
        public ProjectReportService(IProjectReportRepository _projectReportRepository)
        {
            projectReportRepository = _projectReportRepository;
        }
        public int CreateProjectReport(ProjectReportViewModel eng_project_report, string path,string halfPath)
        {
            return projectReportRepository.CreateProjectReport(eng_project_report, path, halfPath);
  
        }
        public int CreateProjectReport(MobileProjectReportViewModel eng_project_report, string path, string halfPath)
        {
            return projectReportRepository.CreateProjectReport(eng_project_report, path, halfPath);
        }

        public ProjectReportViewModel getProrjectRPT(int? id)
        {
            return projectReportRepository.getProrjectRPT(id);
        }

        public ProjectViewModel getProject(int? id)
        {
            return projectReportRepository.getProject(id);
        }

        public List<ProjectReportTaskStatusViewModel> getTaskStatus()
        {
            return projectReportRepository.getTaskStatus();
        }

        public int EditProjectReport(ProjectReportViewModel eng_project_report, string path, string halfPath)
        {
            return projectReportRepository.EditProjectReport(eng_project_report, path, halfPath);
        }

        public List<ProjectReportViewModel> getAllProjectReports()
        {
            return projectReportRepository.getAllProjectReports();
        }
        public int deleteFile(int? id)
        {
            return projectReportRepository.deleteFile(id);
        }
        public int DeleteProjectReport(int prID)
        {
            return projectReportRepository.DeleteProjectReport(prID);
        }
        public List<ProjectReportViewModel> getFilterProjectReports(FilterViewModel filter)
        {
            return projectReportRepository.getFilterProjectReports(filter);
        }

        public List<MobileProjectReportViewModel> getAllProjectReports(int userID)
        {
            return projectReportRepository.getAllProjectReports(userID);
        }

        public List<getProjectReport_Result> GetAllProjectManagementReports()
        {
            return projectReportRepository.GetAllProjectManagementReports();
        }


        public List<getTimeReport_Result> GetAllTimeManagementReports()
        {
            return projectReportRepository.GetAllTimeManagementReports();
        }

    }
}