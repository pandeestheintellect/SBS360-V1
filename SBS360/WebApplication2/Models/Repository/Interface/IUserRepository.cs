using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Repository.Imp
{
    public interface IUserRepository
    {

        UserViewModel ValidateUserLogin(string username, string password);
        List<ModuleViewModel> GetModuleNames(int userID);
        int CreateUser(UserViewModel user);
        int SaveUser(UserViewModel user);
        int DeleteUser(int userID);
        UserViewModel getUser(int userID);
        List<UserViewModel> getAllUsers();
        List<GroupViewModel> getUsergroups();
        bool chkUser(string username);
        List<EmployeeViewModel> getEmployees();
        List<PermissionViewModel> getAllPermissions();
        List<ModuleViewModel> GetAllModules();
        int CreateMenu(int groupid, List<int> moduleNames);
        int DeleteMenu(int permID);


    }
}