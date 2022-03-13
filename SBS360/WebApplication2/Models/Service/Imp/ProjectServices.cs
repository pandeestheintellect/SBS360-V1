using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Interface
{
    public class ProjectServices : IProjectServices
    {
        private readonly IProjectRepository projectRepository;
        public ProjectServices(IProjectRepository _projectRepository)
        {
            projectRepository = _projectRepository;
        }


        public int CreateProject(ProjectViewModel project)
        {
            return projectRepository.CreateProject(project);
        }
        public int SaveProject(ProjectViewModel project)
        {
            return projectRepository.SaveProject(project);
        }

        public List<ProjectViewModel> getAllProjects()
        {
            return projectRepository.getAllProjects();
        }

        public ProjectViewModel getProject(int projectID)
        {

            return projectRepository.getProject(projectID);
        }

        public List<ProjectStatusViewModel> getProjectStatus()
        {
            return projectRepository.getProjectStatus();

        }

        public List<LocationViewModel> getAllLocations()
        {
            return projectRepository.getAllLocations();
        }

        public int UpdateInvDoDate(string invDate, string doDate, int invReleased, int Projectid, string typeChk)
        {
            return projectRepository.UpdateInvDoDate(invDate, doDate, invReleased, Projectid, typeChk);
        }

        public int CreateProjectInvoice(int ProjectID, string InvoiceDate, int QuotationID, List<QuoteDescriptionViewModel> desc, List<int> deleted, decimal finalAmount)
        {
            return projectRepository.CreateProjectInvoice(ProjectID, InvoiceDate, QuotationID, desc, deleted, finalAmount);
        }

        public List<CustomInvoiceViewModel> getAllCustomInvoice(int? id)
        {
            return projectRepository.getAllCustomInvoice(id);
        }

        public CustomInvoiceViewModel getCustomInvoice(int? id)
        {
            return projectRepository.getCustomInvoice(id);
        }
        public int DeleteCustomInvoice(int invoiceID)
        {
            return projectRepository.DeleteCustomInvoice(invoiceID);
        }
        public List<ProjectViewModel> getFilterProjects(FilterViewModel filter)
        {
            return projectRepository.getFilterProjects(filter);
        }
        public List<CustomInvoiceViewModel> getAllCustomQuoteInvoice(int? id)
        {
            return projectRepository.getAllCustomQuoteInvoice(id);
        }
    }
}