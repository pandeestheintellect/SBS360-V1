using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Repository.Imp
{
    public interface IEmployeeRepository
    {
        
        
        List<EmployeeViewModel> getAllEmployees();
        int CreateEmployee(EmployeeViewModel employee);
        int SaveEmployee(EmployeeViewModel employee);
        int DeleteEmployee(int empID);
        EmployeeViewModel getEmployee(int empID);
       
        List<GroupViewModel> getUsergroups();
        List<AddressViewModel> getAddresses();
        List<EmployeeViewModel> getFilterEmployees(FilterViewModel filter);

        List<DashBoardHREmpViewModel> getHREmpDashBoardResults();
        List<DashBoardCompanyViewModel> getCompanyDashBoardResults();
        List<DashBoardVehicleViewModel> getVehicleDashBoardResults();
        List<DashBoardPayableViewModel> getPayableDashBoardResults();
        List<DashBoardReceivableViewModel> getReceivableDashBoardResults();
        List<DashBoardOperationViewModel> getOperationDashBoardResults(int groupid, int userid);
        List<DashBoardDirectorProjectViewModel> getDirectorProjectDashBoardResults();
        List<DashBoardSVManProjectViewModel> getSVManProjectDashBoardResults(int groupid, int userid);
        List<DashBoardSVManDTTRViewModel> getSVManDTTRDashBoardResults(int groupid, int userid);
        List<DashBoardSVManQualityViewModel> getSVManQualityDashBoardResults(int groupid, int userid);


    }
}