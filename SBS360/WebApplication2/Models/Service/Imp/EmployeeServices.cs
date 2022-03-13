using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.ViewModel;
using System.Collections.Generic;

namespace Eng360Web.Models.Service.Interface
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IEmployeeRepository userRepository;
        public EmployeeServices(IEmployeeRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        
        public int CreateEmployee(EmployeeViewModel employee)
        {
            return userRepository.CreateEmployee(employee);
        }
        public int SaveEmployee(EmployeeViewModel employee)
        {
            return userRepository.SaveEmployee(employee);
        }
        public int DeleteEmployee(int empID)
        {
            return userRepository.DeleteEmployee(empID);
        }
        public List<EmployeeViewModel> getAllEmployees()
        {
            return userRepository.getAllEmployees();
        }
        public List<GroupViewModel> getUsergroups()
        {
            return userRepository.getUsergroups();
        }
        public List<AddressViewModel> getAddresses()
        {
            return userRepository.getAddresses();
        }
        public EmployeeViewModel getEmployee(int empID)
        {

            return userRepository.getEmployee(empID);
        }

        public List<EmployeeViewModel> getFilterEmployees(FilterViewModel filter)
        {
            return userRepository.getFilterEmployees(filter);
        }
        public List<DashBoardHREmpViewModel> getHREmpDashBoardResults()
        {
            return userRepository.getHREmpDashBoardResults();
        }
        public List<DashBoardCompanyViewModel> getCompanyDashBoardResults()
        {
            return userRepository.getCompanyDashBoardResults();
        }
        public List<DashBoardVehicleViewModel> getVehicleDashBoardResults()
        {
            return userRepository.getVehicleDashBoardResults();
        }
        public List<DashBoardPayableViewModel> getPayableDashBoardResults()
        {
            return userRepository.getPayableDashBoardResults();
        }
        public List<DashBoardReceivableViewModel> getReceivableDashBoardResults()
        {
            return userRepository.getReceivableDashBoardResults();
        }

        public List<DashBoardOperationViewModel> getOperationDashBoardResults(int groupid, int userid)
        {
            return userRepository.getOperationDashBoardResults(groupid, userid);
        }
        public List<DashBoardDirectorProjectViewModel> getDirectorProjectDashBoardResults()
        {
            return userRepository.getDirectorProjectDashBoardResults();
        }
        public List<DashBoardSVManProjectViewModel> getSVManProjectDashBoardResults(int groupid, int userid)
        {
            return userRepository.getSVManProjectDashBoardResults(groupid, userid);
        }
        public List<DashBoardSVManDTTRViewModel> getSVManDTTRDashBoardResults(int groupid, int userid)
        {
            return userRepository.getSVManDTTRDashBoardResults(groupid, userid);
        }
        public List<DashBoardSVManQualityViewModel> getSVManQualityDashBoardResults(int groupid, int userid)
        {
            return userRepository.getSVManQualityDashBoardResults(groupid, userid);
        }
    }
}