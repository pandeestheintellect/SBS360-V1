using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eng360Web.Models.Domain;
using Eng360Web.Models.ViewModel;
using AutoMapper;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.Utility;
using System.Globalization;
using Eng360Web.Models.Security;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class EmployeeController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();

        private readonly IEmployeeServices employeeService;

        public EmployeeController(IEmployeeServices _employeeService)
        {
            employeeService = _employeeService;

        }

        // GET: Employee
        public ActionResult Index()
        {
            //var eng_employee_profile = db.eng_employee_profile.Include(e => e.eng_address_master).Include(e => e.eng_usergroup);
            //return View(eng_employee_profile.ToList());
            return View(employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList());
        }


        // GET: Employee
        public ActionResult ReportIndex()
        {
            var filter = new FilterViewModel();
            var emp = employeeService.getAllEmployees().ToList();

            var fn = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
             fn.Insert(0, new EmployeeViewModel() {UserID = 0, FirstName="Select" });

            //var ob = employeeService.getAllEmployees().GroupBy(a => a.OpBranch).ToList();
            var ob = employeeService.getAllEmployees().Select(a => a.OpBranch).Distinct().ToList();
            ob.Insert(0, "Select" );


            ViewBag.UserID = new SelectList(fn, "UserID", "FirstName");
            ViewBag.OpBranch = new SelectList(ob);
           // ViewBag.OpBranch = new SelectList(employeeService.getAllEmployees().Select(a=>a.OpBranch).Distinct());
            //emplist.Insert(0, new ClientContactViewModel() { CCID = 0, SPOCName = "Select", eng_client_master = new ClientViewModel() { ClientID = 0 } });

            filter.eng_employee_master = emp;
            

            return View(filter);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportIndex(FilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                var fil = new FilterViewModel();
                var fn = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList();
                fn.Insert(0, new EmployeeViewModel() { UserID = 0, FirstName = "Select" });

                //var ob = employeeService.getAllEmployees().GroupBy(a => a.OpBranch).ToList();
                var ob = employeeService.getAllEmployees().Select(a => a.OpBranch).Distinct().ToList();
                ob.Insert(0, "Select");

                if (!string.IsNullOrEmpty(filter.dateFrom))
                    filter.dateFrom = DateTime.ParseExact(filter.dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(filter.dateTo))
                    filter.dateTo = DateTime.ParseExact(filter.dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


                var emp = employeeService.getFilterEmployees(filter).ToList();
                fil.eng_employee_master = emp;
                ViewBag.UserID = new SelectList(fn, "UserID", "FirstName", fil.UserID); 
                fil.dateFrom = filter.dateFrom;
                fil.dateTo = filter.dateTo;
                ViewBag.OpBranch = new SelectList(ob, filter.OpBranch);
                return View(fil);

            }
            return View();
            
        }




        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //eng_employee_profile eng_employee_profile = db.eng_employee_profile.Find(id);
            var employee = employeeService.getEmployee(id ?? default(int));
            if (employee == null)
            {
                return HttpNotFound();
            }
           
            ViewBag.ContactID = new SelectList(employeeService.getAddresses(), "ContactID", "Email1", employee.AddressID);
            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {

            //  ViewBag.ContactID = new SelectList(db.eng_contact_master, "ContactID", "Email1");
           
            ViewBag.GroupID = new SelectList(employeeService.getUsergroups().Where(a => a.GroupID != 1).ToList(), "GroupID", "GroupName");
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "UserID,UserName,Password,EmpID,OpBranch,GroupID,FirstName,MidName,LastName,DisplayName,ContactID,Job_Title,DoB,DoJ,DoR,Gender,Designation,ID_Type,ID_Number,Profile_Desc,Profile_Photo_Path,llevel,CreatedDate,DeletedDate,CreatedBy,DeletedBy,LastLogin,Passport_Number,Passport_Valid_Till,Permit_Number,Permit_Valid_From,Permit_Valid_To,Licence_Number,Licence_Valid_Till,Insurance_Number,Insurance_Valid_Till,IsActive")] EmployeeViewModel eng_employee_profile)
        public ActionResult Create( EmployeeViewModel eng_employee_profile)
        {
            if (ModelState.IsValid )
            {
                //   db.eng_employee_profile.Add(eng_employee_profile);
                //  db.SaveChanges();

                if (!string.IsNullOrEmpty(eng_employee_profile.DoB))
                    eng_employee_profile.DoB = DateTime.ParseExact(eng_employee_profile.DoB, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.DoJ))
                    eng_employee_profile.DoJ = DateTime.ParseExact(eng_employee_profile.DoJ, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.DoR))
                    eng_employee_profile.DoR = DateTime.ParseExact(eng_employee_profile.DoR, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.Passport_Valid_Till))
                    eng_employee_profile.Passport_Valid_Till = DateTime.ParseExact(eng_employee_profile.Passport_Valid_Till, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.Permit_Valid_From))
                    eng_employee_profile.Permit_Valid_From = DateTime.ParseExact(eng_employee_profile.Permit_Valid_From, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.Permit_Valid_To))
                    eng_employee_profile.Permit_Valid_To = DateTime.ParseExact(eng_employee_profile.Permit_Valid_To, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.Licence_Valid_Till))
                    eng_employee_profile.Licence_Valid_Till = DateTime.ParseExact(eng_employee_profile.Licence_Valid_Till, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.Insurance_Valid_Till))
                    eng_employee_profile.Insurance_Valid_Till = DateTime.ParseExact(eng_employee_profile.Insurance_Valid_Till, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.SOC_Issue_Date))
                    eng_employee_profile.SOC_Issue_Date = DateTime.ParseExact(eng_employee_profile.SOC_Issue_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.SOC_Expiry_Date))
                    eng_employee_profile.SOC_Expiry_Date = DateTime.ParseExact(eng_employee_profile.SOC_Expiry_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.License_Scissor_Lift_ExpiryDate))
                    eng_employee_profile.License_Scissor_Lift_ExpiryDate = DateTime.ParseExact(eng_employee_profile.License_Scissor_Lift_ExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.License_Boom_Lift_ExpiryDate))
                    eng_employee_profile.License_Boom_Lift_ExpiryDate = DateTime.ParseExact(eng_employee_profile.License_Boom_Lift_ExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.License_WorkatHeight_ExpiryDate))
                    eng_employee_profile.License_WorkatHeight_ExpiryDate = DateTime.ParseExact(eng_employee_profile.License_WorkatHeight_ExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.License_IslandPass_ExpiryDate))
                    eng_employee_profile.License_IslandPass_ExpiryDate = DateTime.ParseExact(eng_employee_profile.License_IslandPass_ExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.License_Course_Expiry_Date))
                    eng_employee_profile.License_Course_Expiry_Date = DateTime.ParseExact(eng_employee_profile.License_Course_Expiry_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


                eng_employee_profile.CreatedBy = AppSession.GetCurrentUserId();
                    eng_employee_profile.CreatedDate = DateTime.Now;
                    eng_employee_profile.IsActive = 1;

                     var reuslt = employeeService.CreateEmployee(eng_employee_profile);

                if (reuslt > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }
               
               
            }

            //ViewBag.ContactID = new SelectList(db.eng_address_master, "ContactID", "Email1", eng_employee_profile.AddressID);
            //ViewBag.Group_ID = new SelectList(employeeService.getUsergroups(), "GroupID", "GroupName", eng_employee_profile.GroupID);
           // eng_employee_profile.Group_ID = new SelectList(db.eng_usergroup, "GroupID", "GroupName", eng_employee_profile.GroupID);
          //  return View(eng_employee_profile);
            return RedirectToAction("Create");
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //eng_employee_profile eng_employee_profile = db.eng_employee_profile.Find(id);
            var employee = employeeService.getEmployee(id??default(int));
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContactID = new SelectList(employeeService.getAddresses(), "ContactID", "Email1", employee.AddressID);
            ViewBag.GroupID = new SelectList(employeeService.getUsergroups().Where(a => a.GroupID != 1).ToList(), "GroupID", "GroupName", employee.GroupID);
            //var employee = Mapper.Map<EmployeeViewModel>(eng_employee_profile);
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeViewModel eng_employee_profile)
        {
            if (ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(eng_employee_profile.DoB))
                    eng_employee_profile.DoB = DateTime.ParseExact(eng_employee_profile.DoB, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.DoJ))
                    eng_employee_profile.DoJ = DateTime.ParseExact(eng_employee_profile.DoJ, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.DoR))
                    eng_employee_profile.DoR = DateTime.ParseExact(eng_employee_profile.DoR, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.Passport_Valid_Till))
                    eng_employee_profile.Passport_Valid_Till = DateTime.ParseExact(eng_employee_profile.Passport_Valid_Till, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.Permit_Valid_From))
                    eng_employee_profile.Permit_Valid_From = DateTime.ParseExact(eng_employee_profile.Permit_Valid_From, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.Permit_Valid_To))
                    eng_employee_profile.Permit_Valid_To = DateTime.ParseExact(eng_employee_profile.Permit_Valid_To, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.Licence_Valid_Till))
                    eng_employee_profile.Licence_Valid_Till = DateTime.ParseExact(eng_employee_profile.Licence_Valid_Till, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.Insurance_Valid_Till))
                    eng_employee_profile.Insurance_Valid_Till = DateTime.ParseExact(eng_employee_profile.Insurance_Valid_Till, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.SOC_Issue_Date))
                    eng_employee_profile.SOC_Issue_Date = DateTime.ParseExact(eng_employee_profile.SOC_Issue_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.SOC_Expiry_Date))
                    eng_employee_profile.SOC_Expiry_Date = DateTime.ParseExact(eng_employee_profile.SOC_Expiry_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.License_Scissor_Lift_ExpiryDate))
                    eng_employee_profile.License_Scissor_Lift_ExpiryDate = DateTime.ParseExact(eng_employee_profile.License_Scissor_Lift_ExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.License_Boom_Lift_ExpiryDate))
                    eng_employee_profile.License_Boom_Lift_ExpiryDate = DateTime.ParseExact(eng_employee_profile.License_Boom_Lift_ExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.License_WorkatHeight_ExpiryDate))
                    eng_employee_profile.License_WorkatHeight_ExpiryDate = DateTime.ParseExact(eng_employee_profile.License_WorkatHeight_ExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.License_IslandPass_ExpiryDate))
                    eng_employee_profile.License_IslandPass_ExpiryDate = DateTime.ParseExact(eng_employee_profile.License_IslandPass_ExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(eng_employee_profile.License_Course_Expiry_Date))
                    eng_employee_profile.License_Course_Expiry_Date = DateTime.ParseExact(eng_employee_profile.License_Course_Expiry_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                eng_employee_profile.UpdatedBy = AppSession.GetCurrentUserId();
                eng_employee_profile.UpdatedDate = DateTime.Now;
               
                var result = employeeService.SaveEmployee(eng_employee_profile);
                if (result > 0)
                {
                    
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }

               // db.Entry(eng_employee_profile).State = EntityState.Modified;
               // db.SaveChanges();
               
            }

            ViewBag.ContactID = new SelectList(employeeService.getAddresses(), "ContactID", "Email1", eng_employee_profile.AddressID);
            //ViewBag.GroupID = new SelectList(employeeService.getUsergroups(), "GroupID", "GroupName", eng_employee_profile.GroupID);
            return View(eng_employee_profile);
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_employee_profile eng_employee_profile = db.eng_employee_profile.Find(id);
            if (eng_employee_profile == null)
            {
                return HttpNotFound();
            }
            return View(eng_employee_profile);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
                       

            var result = employeeService.DeleteEmployee(id);

            //eng_employee_profile eng_employee_profile = db.eng_employee_profile.Find(id);
            //db.eng_employee_profile.Remove(eng_employee_profile);
            // var result =     db.SaveChanges();

            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }
           // return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public ActionResult getAllEmployees()
        {
            var empNames = employeeService.getAllEmployees().Where(a => a.IsActive == 1).ToList().Select(e => e.FirstName + ' ' + e.LastName).ToList();
            return Json(empNames);

        }

        // GET: Employee/Dashboard
        //public ActionResult GetHRDashBoard()
        //{
        //    var EmpReminder = employeeService.getHREmpDashBoardResults().ToList();
        //    return View(EmpReminder);
        //}

        // GET: Employee/Dashboard
        //public ActionResult GetCompanyDashBoard()
        //{
        //    var CpyReminder = employeeService.getCompanyDashBoardResults().ToList();
        //    return View(CpyReminder);
        //}

        // GET: Employee/Dashboard
        //public ActionResult GetVehicleDashBoard()
        //{
        //    var VehReminder = employeeService.getVehicleDashBoardResults().ToList();
        //    return View(VehReminder);
        //}

        // GET: Payable/Dashboard
        //public ActionResult GetPayableDashBoard()
        //{
        //    var PayReminder = employeeService.getPayableDashBoardResults().ToList();
        //    return View(PayReminder);
        //}

        // GET: Receivable/Dashboard
        //public ActionResult GetReceivableDashBoard()
        //{
        //    var RecReminder = employeeService.getReceivableDashBoardResults().ToList();
        //    return View(RecReminder);
        //}

        // GET: Operation/Dashboard
        //public ActionResult GetOperationDashBoard()
        //{
        //    var OpReminder = employeeService.getOperationDashBoardResults().ToList();
        //    return View(OpReminder);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
