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
using System.IO;


using System.Web.Security;

using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.xml;
using iTextSharp.text.html.simpleparser;
using System.util;
using System.Text.RegularExpressions;
using NLog;
using System.Globalization;
using Eng360Web.Models.Security;

namespace Eng360Web.Controllers
{
    [AccessDeniedAuthorize]
    public class QualityDefectController : SuperBaseController
    {
        private Eng360Entities1 db = new Eng360Entities1();
        Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IQualityDefectServices qualitydefectService;
        private readonly IEmployeeServices employeeService;
        private readonly IProjectServices projectService;
        private readonly ISupplierServices supplierService;

        public QualityDefectController(IQualityDefectServices _qualitydefectService, IEmployeeServices _employeeService, IProjectServices _projectService, ISupplierServices _supplierService)
        {
            qualitydefectService = _qualitydefectService;
            employeeService = _employeeService;
            projectService = _projectService;
            supplierService = _supplierService;
        }

        // GET: QualityDefect
        public ActionResult Index()
        {
            
            var qualitydefects = qualitydefectService.getAllQualityDefects().ToList();

            foreach (var detail in qualitydefects)
            {
                int defcnt = 0;

                foreach (var def in detail.eng_qa_defect_detail)
                {
                    defcnt = defcnt + 1;
                }
                detail.IssueCount = defcnt;
            }

            return View(qualitydefects);
        }

        // GET: QualityDefectCPA
        public ActionResult DTIndex()
        {

            var defects = qualitydefectService.getAllDefectDetails().Where(a=>a.DefectStatus != "Pending").ToList();

            return View(defects);
        }


        // GET: QualityDefect/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            var defect = qualitydefectService.getQualityDefect(id ?? default(int));
            if (defect == null)
            {
                return HttpNotFound();
            }

                int defcnt = 0;

                foreach (var def in defect.eng_qa_defect_detail)
                {
                    defcnt = defcnt + 1;
                }

            defect.IssueCount = defcnt;
            return View(defect);
            
        }

        // GET: QualityDefect/Create
        public ActionResult Create()
        {


            var inspectors = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 3).ToList();
            ViewBag.InspectedBy = new SelectList(inspectors, "UserID", "FullName");

            var managers = employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 2).ToList();
            ViewBag.PM_Incharge = new SelectList(managers, "UserID", "FullName");

            var suppliers = supplierService.getAllSuppliers().Where(a => a.IsActive == 1).ToList();
            ViewBag.SupplierID = new SelectList(suppliers, "SupplierID", "Company_Name");

            var project = projectService.getAllProjects().ToList();
            ViewBag.ProjectID = new SelectList(project, "ProjectID", "ProjectName");

            QualityDefectViewModel qualitydefect = new QualityDefectViewModel();


            return View(qualitydefect);
        }

        // POST: QualityDefect/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QualityDefectViewModel eng_qa_defect)
        {

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(eng_qa_defect.InspectedDate))
                    eng_qa_defect.InspectedDate = DateTime.ParseExact(eng_qa_defect.InspectedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");


                string halfPath = "/images/QualityFiles/" + AppSession.GetCurrentUserId() + "/";
                eng_qa_defect.CreatedBy = AppSession.GetCurrentUserId();
                eng_qa_defect.CreatedDate = DateTime.Now;
                var result = qualitydefectService.CreateQualityDefect(eng_qa_defect, Path.Combine(Server.MapPath("~/images/QualityFiles/" + AppSession.GetCurrentUserId() + "/")), halfPath);
                if (result > 0)
                {
                    return getSuccessfulOperation();
                }
                else
                {
                    return getFailedOperation();
                }



            }


            return View(eng_qa_defect);
        }

        // GET: QualityDefect/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {

                return getFailedOperation();
            }

            var qualitydefect = qualitydefectService.getQualityDefect(id ?? default(int));
            if (qualitydefect == null)
            {
                return getFailedOperation();
            }
            if (qualitydefect.SupplierID == null)
            {
                qualitydefect.SupplierID = 1;
            }
            ViewBag.InspectedBy = new SelectList(employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 3).ToList(), "UserID", "FullName", qualitydefect.InspectedBy);
            ViewBag.PM_Incharge = new SelectList(employeeService.getAllEmployees().Where(a => a.IsActive == 1 && a.GroupID == 2).ToList(), "UserID", "FullName", qualitydefect.PM_Incharge);
            ViewBag.ProjectID = new SelectList(projectService.getAllProjects().ToList(), "ProjectID", "ProjectName", qualitydefect.ProjectID);
            ViewBag.SupplierID = new SelectList(supplierService.getAllSuppliers().Where(a=>a.IsActive == 1).ToList(), "SupplierID", "Company_Name", qualitydefect.ProjectID);

            return View(qualitydefect);



        }

        // POST: QualityDefect/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QualityDefectViewModel eng_qa_defect)
        {
            if (ModelState.IsValid)
            {
                
                string halfPath = "/images/QualityFiles/" + AppSession.GetCurrentUserId() + "/";

                eng_qa_defect.UpdatedBy = AppSession.GetCurrentUserId();
                eng_qa_defect.UpdatedDate = DateTime.Now;

                var result = qualitydefectService.SaveQualityDefect(eng_qa_defect, Path.Combine(Server.MapPath("~/images/QualityFiles/" + AppSession.GetCurrentUserId() + "/")), halfPath);
                if (result > 0)
                    return getSuccessfulOperation();
                else
                    return getFailedOperation();
            }

            return View(eng_qa_defect);
        }


        // GET: QualityDefect/CreateQualityDefectDesc
        public ActionResult CreateDefectDetail(int? id)
        {
            if (id == null)
            {
                
                return View();
            }
            else
            {
                var qualitydefectdesc = qualitydefectService.getQualityDefectDetail(id);
                

                return View(qualitydefectdesc);

            }
        }



        // GET: QualityDefect/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eng_qa_defect eng_qa_defect = db.eng_qa_defect.Find(id);
            if (eng_qa_defect == null)
            {
                return HttpNotFound();
            }
            return View(eng_qa_defect);
        }

        // POST: QualityDefect/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = qualitydefectService.DeleteQualityDefect(id);

            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }
        }

        // POST: QualityDefectFile/Delete/5
        [HttpPost, ActionName("DeleteFile")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteQualityDefectFileConfirmed(int id)
        {
            var result = qualitydefectService.deleteQualityDefectFile(id);

            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }
        }

        // POST: QualityDefectDescription/Delete/5
        [HttpPost, ActionName("DeleteDefectDetail")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDefectDetailConfirmed(int id)
        {
            var result = qualitydefectService.deleteQualityDefectDetail(id);

            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }
        }

        // GET: QualityDefect/Details/5
        public ActionResult DTEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cpa = qualitydefectService.getQualityCPADetail(id ?? default(int));

            var defect = qualitydefectService.getQualityDefectDetail(id ?? default(int));

            if (cpa == null)
            {

                var cpaview = new QualityDefectCPAViewModel();
                cpaview.DefectDetailID = defect.DefectDetailID;
                cpaview.eng_qa_defect_detail = defect;
                cpaview.eng_qa_defect_cpa_files = null;
                
                return View(cpaview);
            }
       
            return View(cpa);
        }


        // POST: QualityDefectCPA/AddEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditCpa(QualityDefectCPAViewModel eng_qa_defect_cpa)
        {
            if (ModelState.IsValid)
            {

                string halfPath = "/images/QualityFiles/" + AppSession.GetCurrentUserId() + "/";

                eng_qa_defect_cpa.UpdatedBy = AppSession.GetCurrentUserId();
                eng_qa_defect_cpa.UpdatedDate = DateTime.Now;

                var result = qualitydefectService.AddEditCpa(eng_qa_defect_cpa, Path.Combine(Server.MapPath("~/images/QualityFiles/" + AppSession.GetCurrentUserId() + "/")), halfPath);
                if (result > 0)
                    return getSuccessfulOperation();
                else
                    return getFailedOperation();
            }

            return View(eng_qa_defect_cpa);
        }

        // POST: QualityDefectFile/Delete/5
        [HttpPost, ActionName("DeleteCPAFile")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteQualityCPAFileConfirmed(int id)
        {
            var result = qualitydefectService.deleteQualityCPAFile(id);

            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }
        }


        // GET: QualityDefect/DTDetails/5
        public ActionResult DTDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cpa = qualitydefectService.getQualityCPADetail(id ?? default(int));

            
            return View(cpa);
        }


        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }



        // POST: QualityDefectFile/Delete/5
        [HttpPost, ActionName("MailDefect")]
        [ValidateAntiForgeryToken]
        public ActionResult SendQualityDefectMail(int id)
        {
            
            var defect = qualitydefectService.getQualityDefect(id);
            int defcnt = 0;

            foreach (var def in defect.eng_qa_defect_detail)
            {
                defcnt = defcnt + 1;
            }

            defect.IssueCount = defcnt;


            var htmlstring = RenderRazorViewToString("Detailsnew", defect);
            
            var logopath = Server.MapPath("~/images/CompanyLogo/");
            var logo1 = logopath + "logo.png";
            var logo2 = logopath + "biz3.png";

            htmlstring = htmlstring.Replace("/images/CompanyLogo/logo.png", logo1);
            htmlstring = htmlstring.Replace("/images/CompanyLogo/biz3.png", logo2);

            var pdfObj = AppSettings.ConvertHTMLToPDF(htmlstring);

            var sub = "Sub: Date";
            var body = "Dear Sir/Madam, \n\nPlease find attached the Quality Inspection report for the below.\n\n";
            var to = "";
            var cc = "";
            var att = "";

            sub = sub + defect.InspectedDate.Split(new string[] { " " }, StringSplitOptions.None)[0] + " Quality Inspection Report for the Project Desc:" + defect.eng_project_master.ProjectName;
            sub = sub + ", Location:" + defect.Location;

            body = body + "Project Id:" + defect.eng_project_master.ProjectNo + "\n";
            body = body + "Project Name:" + defect.eng_project_master.ProjectName + "\n";
            body = body + "Location:" + defect.Location + "\n";
            body = body + "Inspection Date:" + defect.InspectedDate.Split(new string[] { " " }, StringSplitOptions.None)[0] + "\n";
            body = body + "Inspected By:" + defect.eng_employee_profile.FirstName + "\n";
            body = body + "Inspection Report Reference:" + defect.DefectDisplayID + "\n\n";
                        
            body = body + "\nPlease help to look into it and do the necessary. \n\nPlease feel free to approach our manager if any clarification/concern.\n\n";
            body = body + "Thanks\n \n Quality Inspection Team \n Citi Construction Pte Ltd";

            //to = "pandees@theintellect.com.sg";
            if(defect.SupplierID==null)
            {
                to = defect.eng_employee_profile.eng_address_master.Email;
                cc = defect.eng_employee_profile1.eng_address_master.Email;
            }
            else
            {
                to = defect.eng_supplier_master.eng_address_master.Email;
                cc= defect.eng_employee_profile.eng_address_master.Email;
                cc = cc + "," + defect.eng_employee_profile1.eng_address_master.Email;
            }


            //cc = "smanandh@gmx.com";
            //cc = cc + "," + "prabhu.thil@gmail.com";
            //cc = cc + "," + "sowdambbikainfotechservices@gmail.com";
            //cc = cc + "," + "pandees@gmail.com";
            //cc = cc + "," + "sivatcbm@yahoo.com.sg";

            foreach (var file in defect.eng_qa_defect_files)
            {
                att = att + file.FIleCaption + ",";
            }
                var result = 1;

            AppSettings.sendEmail(sub, body, to, cc, att, pdfObj.ToArray());

            if (result > 0)
            {
                return getSuccessfulOperation();
            }
            else
            {
                return getFailedOperation();
            }
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

      


        void GetPDF(string pHTML, string pHTML1, string pHTML2, string pHTML3)
        {
            byte[] bPDF = null;
            string imageURL = System.Configuration.ConfigurationManager.AppSettings["logo"].ToString();
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
            //Resize image depend upon your need
            jpg.ScaleToFit(140f, 120f);
            //Give space before image
            jpg.SpacingBefore = 10f;
            //Give some space after the image
            jpg.SpacingAfter = 1f;
            jpg.Alignment = Element.ALIGN_LEFT;
            
            MemoryStream ms = new MemoryStream();
            TextReader txtReader = new StringReader(pHTML);
            TextReader txtReader1 = new StringReader(pHTML1);
            TextReader txtReader2 = new StringReader(pHTML2);
            TextReader txtReader3 = new StringReader(pHTML3);
            
            // 1: create object of a itextsharp document class

            Document doc = new Document(PageSize.A4);
            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file

            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);
            // 3: we create a worker parse the document
            HTMLWorker htmlWorker = new HTMLWorker(doc);
            // 4: we open document and start the worker on the document

            doc.Open();
            doc.NewPage();
            doc.Add(jpg);
            htmlWorker.StartDocument();
            // 5: parse the html into the document
            htmlWorker.Parse(txtReader);
            // 6: close the document and the worker
            htmlWorker.EndDocument();
            htmlWorker.Close();
            
            doc.Close();

            bPDF = ms.ToArray();            

            using (FileStream fs = System.IO.File.Create(System.Configuration.ConfigurationManager.AppSettings["pdfout"].ToString()))

            {
                fs.Write(bPDF.ToArray(), 0, (int)bPDF.Length);
            }

            //return bPDF;

        }



    }
}
