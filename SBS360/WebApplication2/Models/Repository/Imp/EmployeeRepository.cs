using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eng360Web.Models.Domain;
using AutoMapper;
using System.Data.Entity;
using Eng360Web.Models.Utility;
using System.Data.Entity.Infrastructure;

namespace Eng360Web.Models.Repository.Interface
{
    public class EmployeeRepository : IEmployeeRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
         Logger logger = LogManager.GetCurrentClassLogger();

       
       public int CreateEmployee(EmployeeViewModel employee)
        {
            eng_address_master address = new eng_address_master() {
                Address1 = employee.eng_address_master.Address1,
                Address2 = employee.eng_address_master.Address2,
                 City = employee.eng_address_master.City,
                  Country = employee.eng_address_master.Country,
                   Fax1 = employee.eng_address_master.Fax1,
                    Email = employee.eng_address_master.Email,
                     Web= employee.eng_address_master.Web,
                      Tel = employee.eng_address_master.Tel,
                       Mobile = employee.eng_address_master.Mobile,
                        Postal_Code = employee.eng_address_master.Postal_Code
            };
            employee.AddressID = address.AddressID;
            
            var _db_employee = Mapper.Map<eng_employee_profile>(employee);

           Eng360DB.eng_employee_profile.Add(_db_employee);
           
            return Eng360DB.SaveChanges(); 
        }

        public int SaveEmployee(EmployeeViewModel employee)
        {

            var _db_employee = Mapper.Map<eng_employee_profile>(employee);
            Eng360DB.Entry(_db_employee).State = EntityState.Modified;
           
            var domainAddress = Eng360DB.eng_address_master.First(a => a.AddressID==employee.AddressID);

            Eng360DB.Entry(domainAddress).State = EntityState.Modified;

            return Eng360DB.SaveChanges();

            


        }

        public int DeleteEmployee(int empID)
        {
            

            var _db_employee = Eng360DB.eng_employee_profile.First(a => a.UserID == empID);

            _db_employee.UpdatedBy = AppSession.GetCurrentUserId();
            _db_employee.UpdatedDate = DateTime.Now;
            _db_employee.IsActive = 0;
            

            Eng360DB.Entry(_db_employee).State = EntityState.Modified;
            return Eng360DB.SaveChanges();
        }

        public EmployeeViewModel getEmployee(int empID)
        {
            eng_employee_profile eng_employee_profile = Eng360DB.eng_employee_profile.Find(empID);
            
            return Mapper.Map<EmployeeViewModel>(eng_employee_profile);
        }

        public List<EmployeeViewModel> getAllEmployees()
        {
            var eng_employee_profile = Eng360DB.eng_employee_profile.OrderBy(a=>a.FirstName).ToList();
            
            var lstEmployeeView = Mapper.Map<List<EmployeeViewModel>>(eng_employee_profile);

            foreach (var emps in lstEmployeeView)
            {
                emps.FullName = emps.FirstName + ' ' + emps.LastName;

            }

            return lstEmployeeView;
        }

        public List<EmployeeViewModel> getFilterEmployees(FilterViewModel filter)
        {
            var sql="select * from eng_employee_profile ";
            var where = "";
            if (filter.UserID > 0)
                where = "UserID = " + filter.UserID ;

            if (filter.dateFrom != null)
                if (where == "")
                    where += " DoJ >='" + filter.dateFrom + "'";
                else
                    where += " and DoJ >='" + filter.dateFrom + "'";

            if (filter.dateTo != null)
                if (where == "")
                    where += " DoJ <='" + filter.dateTo + "'";
                else
                    where += " and DoJ <='" + filter.dateTo + "'";

            if (filter.OpBranch != "Select")
                if (where == "")
                    where += " OpBranch ='" + filter.OpBranch + "'";
                else
                    where += " and OpBranch ='" + filter.OpBranch + "'";

            if (where != "")
                sql = sql + "Where " + where;

            

            // DbSqlQuery<eng_employee_profile> data = Eng360DB.eng_employee_profile.SqlQuery(sql);
            var data = Eng360DB.eng_employee_profile.SqlQuery(sql).ToList();
            
            var lstEmployeeView = Mapper.Map<List<EmployeeViewModel>>(data);
            return lstEmployeeView;
        }


        public List<GroupViewModel> getUsergroups()
        {
            var eng_usergroup = Eng360DB.eng_usergroup.ToList();
            var lstGroupView = Mapper.Map<List<GroupViewModel>>(eng_usergroup);
            return lstGroupView;
        }

        public List<AddressViewModel> getAddresses()
        {
            var eng_address_master = Eng360DB.eng_address_master.ToList();
            var lstAddressView = Mapper.Map<List<AddressViewModel>>(eng_address_master);
            return lstAddressView;
        }

        public List<DashBoardHREmpViewModel> getHREmpDashBoardResults()
        {
            var hrdashboard = Eng360DB.DB_Emp(1).ToList();
            var lstDashBoardView = Mapper.Map<List<DashBoardHREmpViewModel>>(hrdashboard);
            return lstDashBoardView;
        }

        public List<DashBoardCompanyViewModel> getCompanyDashBoardResults()
        {
            var cpydashboard = Eng360DB.DB_Company(1).ToList();
            var lstDashBoardView = Mapper.Map<List<DashBoardCompanyViewModel>>(cpydashboard);
            return lstDashBoardView;
        }
        
        public List<DashBoardVehicleViewModel> getVehicleDashBoardResults()
        {
            var vehdashboard = Eng360DB.DB_Vehicle(1).ToList();
            var lstDashBoardView = Mapper.Map<List<DashBoardVehicleViewModel>>(vehdashboard);
            return lstDashBoardView;
        }

        public List<DashBoardPayableViewModel> getPayableDashBoardResults()
        {
            var paydashboard = Eng360DB.DB_Payable(1).ToList();
            var lstDashBoardView = Mapper.Map<List<DashBoardPayableViewModel>>(paydashboard);
            return lstDashBoardView;
        }
       
        public List<DashBoardReceivableViewModel> getReceivableDashBoardResults()
        {
            var recdashboard = Eng360DB.DB_Receivable(1).ToList();
            var lstDashBoardView = Mapper.Map<List<DashBoardReceivableViewModel>>(recdashboard);
            return lstDashBoardView;
        }

        public List<DashBoardOperationViewModel> getOperationDashBoardResults(int groupid, int userid)
        {
            var opdashboard = Eng360DB.DB_Operation(groupid, userid).ToList();
            var lstDashBoardView = Mapper.Map<List<DashBoardOperationViewModel>>(opdashboard);
            return lstDashBoardView;
        }

        public List<DashBoardDirectorProjectViewModel> getDirectorProjectDashBoardResults()
        {
            var dirdashboard = Eng360DB.DB_Director_Project(1).ToList();
            var lstDashBoardView = Mapper.Map<List<DashBoardDirectorProjectViewModel>>(dirdashboard);
            return lstDashBoardView;
        }

        public List<DashBoardSVManProjectViewModel> getSVManProjectDashBoardResults(int groupid, int userid)
        {
            var svmandashboard = Eng360DB.DB_SVMan_Project(groupid, userid).ToList();
            var lstDashBoardView = Mapper.Map<List<DashBoardSVManProjectViewModel>>(svmandashboard);
            return lstDashBoardView;
        }

        public List<DashBoardSVManDTTRViewModel> getSVManDTTRDashBoardResults(int groupid, int userid)
        {
            var dttrdashboard = Eng360DB.DB_SVMan_DTTR(groupid, userid).ToList();
            var lstDashBoardView = Mapper.Map<List<DashBoardSVManDTTRViewModel>>(dttrdashboard);
            return lstDashBoardView;
        }

        public List<DashBoardSVManQualityViewModel> getSVManQualityDashBoardResults(int groupid, int userid)
        {
            var qlydashboard = Eng360DB.DB_SVMan_Quality(groupid, userid).ToList();
            var lstDashBoardView = Mapper.Map<List<DashBoardSVManQualityViewModel>>(qlydashboard);
            return lstDashBoardView;
        }

    }
}