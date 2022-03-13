using Eng360Web.Models.Domain;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eng360Web.Models.Repository.Interface
{
    public interface IProjectReportRepository
    {
        int CreateProjectReport(ProjectReportViewModel eng_project_report, string path,string halfPath);
        int EditProjectReport(ProjectReportViewModel eng_project_report, string path, string halfPath);
        ProjectReportViewModel getProrjectRPT(int? id);
        ProjectViewModel getProject(int? id);
        List<ProjectReportTaskStatusViewModel> getTaskStatus();
        List<ProjectReportViewModel> getAllProjectReports();
        
        int deleteFile(int? id);
        int DeleteProjectReport(int prID);
        List<ProjectReportViewModel> getFilterProjectReports(FilterViewModel filter);

        int CreateProjectReport(MobileProjectReportViewModel eng_project_report, string path, string halfPath);
        List<MobileProjectReportViewModel> getAllProjectReports(int userID);
        List<getProjectReport_Result> GetAllProjectManagementReports();
        List<getTimeReport_Result> GetAllTimeManagementReports();
    }
}
