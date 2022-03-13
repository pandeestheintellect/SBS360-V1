using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Interface
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository userRepository;
        
        public UserServices(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        public UserViewModel ValidateUserLogin(string username, string password)
        {
            return userRepository.ValidateUserLogin(username, password);
        }

        public List<ModuleViewModel> GetModuleNames(int userID)
        {
            return userRepository.GetModuleNames(userID);
        }

        public List<UserViewModel> getAllUsers()
        {
            return userRepository.getAllUsers();
        }


        public int CreateUser(UserViewModel user)
        {
            return userRepository.CreateUser(user);
        }

        public int SaveUser(UserViewModel user)
        {
            return userRepository.SaveUser(user);
        }
        public int DeleteUser(int userID)
        {
            return userRepository.DeleteUser(userID);
        }

        public UserViewModel getUser(int userID)
        {

            return userRepository.getUser(userID);
        }
        public List<GroupViewModel> getUsergroups()
        {
            return userRepository.getUsergroups();
        }
        public bool chkUser(string username)
        {
            return userRepository.chkUser(username);
        }
        public List<EmployeeViewModel> getEmployees()
        {
            return userRepository.getEmployees();
        }
        public List<PermissionViewModel> getAllPermissions()
        {
            return userRepository.getAllPermissions();
        }
        public List<ModuleViewModel> GetAllModules()
        {
            return userRepository.GetAllModules();
        }
        public int CreateMenu(int groupid, List<int> moduleNames)
        {
            return userRepository.CreateMenu(groupid, moduleNames);
        }
        public int DeleteMenu(int permID)
        {
            return userRepository.DeleteMenu(permID);
        }
    }
}