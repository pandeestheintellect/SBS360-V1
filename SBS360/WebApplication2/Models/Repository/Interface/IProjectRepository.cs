using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Repository.Imp
{
    public interface IProjectRepository
    {

        List<ProjectViewModel> getAllProjects();
        int CreateProject(ProjectViewModel project);
        int SaveProject(ProjectViewModel project);

        ProjectViewModel getProject(int projectID);

        List<ProjectStatusViewModel> getProjectStatus();
        List<LocationViewModel> getAllLocations();
        int UpdateInvDoDate(string invDate, string doDate, int invReleased, int Projectid, string typeChk);
        int CreateProjectInvoice(int ProjectID, string InvoiceDate, int QuotationID, List<QuoteDescriptionViewModel> desc, List<int> deleted, decimal finalAmount);
        List<CustomInvoiceViewModel> getAllCustomInvoice(int? id);

        CustomInvoiceViewModel getCustomInvoice(int? id);
        int DeleteCustomInvoice(int invoiceID);
        List<ProjectViewModel> getFilterProjects(FilterViewModel filter);
        List<CustomInvoiceViewModel> getAllCustomQuoteInvoice(int? id);

    }
}