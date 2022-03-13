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

namespace Eng360Web.Models.Repository.Interface
{
    public class UserRepository : IUserRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
         Logger logger = LogManager.GetCurrentClassLogger();


        public UserViewModel ValidateUserLogin(string username, string password)
        {
            var user = Eng360DB.eng_users.Where(a => a.UserName == username.Trim() && a.Password == password.Trim()).FirstOrDefault();
            var userViewModel = Mapper.Map<UserViewModel>(user);
            return userViewModel;

        }

        public List<ModuleViewModel> GetModuleNames(int userID)
        {
            var user = Eng360DB.eng_users.Where(a => a.UserID == userID).FirstOrDefault();
            var activePermission = user.eng_usergroup.eng_permission.Where(p => p.Access > 0).ToList();
            var Modules = activePermission.Select(m => m.eng_module).ToList<eng_module>();
            var result = Mapper.Map<List<ModuleViewModel>>(Modules);
            return result;
        }

        public List<PermissionViewModel> getAllPermissions()
        {            
            var Permn = Eng360DB.eng_permission.ToList();
            var result = Mapper.Map<List<PermissionViewModel>>(Permn);
            return result;
        }

        public List<ModuleViewModel> GetAllModules()
        {
            var modules = Eng360DB.eng_module.ToList();
            var result = Mapper.Map<List<ModuleViewModel>>(modules);
            return result;
        }

        //public int CreateMenu(PermissionViewModel perm)
        //{
        //    var res = 0;
        //    var chk = Eng360DB.eng_permission.Where(a => a.GroupID == perm.GroupID && a.ModuleID == perm.ModuleID).FirstOrDefault();

        //    if (chk == null)
        //    {
        //        var db_perm = Mapper.Map<eng_permission>(perm);
        //        Eng360DB.eng_permission.Add(db_perm);
        //       res= Eng360DB.SaveChanges();
        //    }
        //    else
        //    {
        //        res = -1;
        //    }
        //    return res;
        //}

        public int DeleteMenu(int permID)
        {
            var _db_perm = Eng360DB.eng_permission.First(a => a.PermissionID == permID);
            Eng360DB.eng_permission.Remove(_db_perm);
            return Eng360DB.SaveChanges();
        }


        public int CreateUser(UserViewModel user)
        {
            
           var _db_user = Mapper.Map<eng_users>(user);

           Eng360DB.eng_users.Add(_db_user);
           return Eng360DB.SaveChanges();
        }

        public int SaveUser(UserViewModel user)
        {

            var _db_user = Mapper.Map<eng_users>(user);
            Eng360DB.Entry(_db_user).State = EntityState.Modified;

           return Eng360DB.SaveChanges();


        }

        public int DeleteUser(int userID)
        {
            var _db_user = Eng360DB.eng_users.First(a => a.UserID == userID);
            _db_user.UpdatedBy = AppSession.GetCurrentUserId();
            _db_user.UpdatedDate = DateTime.Now;
            
            _db_user.IsActive = 0;

            // Eng360DB.eng_users.Remove(_db_user);
            Eng360DB.Entry(_db_user).State = EntityState.Modified;

            return Eng360DB.SaveChanges();
        }

        public UserViewModel getUser(int userID)
        {
            eng_users eng_users = Eng360DB.eng_users.Find(userID);

            return Mapper.Map<UserViewModel>(eng_users);
        }

        public List<UserViewModel> getAllUsers()
        {
            var eng_users = Eng360DB.eng_users.ToList();
            var lstUserView = Mapper.Map<List<UserViewModel>>(eng_users);
            return lstUserView;
        }

        public List<GroupViewModel> getUsergroups()
        {
            var eng_usergroup = Eng360DB.eng_usergroup.ToList();
            var lstGroupView = Mapper.Map<List<GroupViewModel>>(eng_usergroup);
            return lstGroupView;
        }

        public List<EmployeeViewModel> getEmployees()
        {
            var eng_employee_profile = Eng360DB.eng_employee_profile.ToList();
            var lstEmployeeView = Mapper.Map<List<EmployeeViewModel>>(eng_employee_profile);
            foreach (var emps in lstEmployeeView)
            {
                emps.FullName = emps.FirstName + ' ' + emps.LastName;

            }
            return lstEmployeeView;
        }

        public bool chkUser(string username)
        {
            try
            {
                var user = Eng360DB.eng_users.Where(a => a.UserName == username).SingleOrDefault();
                if (user == null)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public int CreateMenu(int groupid, List<int> moduleNames)
        {
            var modules = Eng360DB.eng_permission.Where(a => a.GroupID == groupid).ToList();

            foreach (var mods in modules)
            {
                var delmod = Eng360DB.eng_permission.First(a => a.PermissionID == mods.PermissionID);
                Eng360DB.eng_permission.Remove(delmod);
            }

            foreach (var mod in moduleNames)
            {
                eng_permission domainPer = new eng_permission();
                domainPer.GroupID = groupid;
                domainPer.ModuleID = mod;
                domainPer.Access = 1;
                Eng360DB.eng_permission.Add(domainPer);
            }
            return Eng360DB.SaveChanges();
        }



    }

}