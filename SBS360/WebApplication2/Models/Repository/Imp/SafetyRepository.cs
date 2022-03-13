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
using System.Globalization;
using System.IO;

namespace Eng360Web.Models.Repository.Interface
{
    public class SafetyRepository : ISafetyRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
         Logger logger = LogManager.GetCurrentClassLogger();

       public int CreateSafety(SafetyMasterViewModel safety, List<int> hazardList, List<int> ppeList, List<int> workerList)
        {

            eng_safety_master domainSafety = Mapper.Map<eng_safety_master>(safety);
            Eng360DB.eng_safety_master.Add(domainSafety);

            foreach (var haz in hazardList)
            {
                eng_safety_hazard_list domainHaz = new eng_safety_hazard_list();
                domainHaz.SafetyID = domainSafety.SafetyID;
                domainHaz.HazardID = haz;
                Eng360DB.eng_safety_hazard_list.Add(domainHaz);
            }

            foreach (var ppe in ppeList)
            {
                eng_safety_ppe_list domainPPE = new eng_safety_ppe_list();
                domainPPE.SafetyID = domainSafety.SafetyID;
                domainPPE.PPEID = ppe;
                Eng360DB.eng_safety_ppe_list.Add(domainPPE);
            }

            foreach (var emp in workerList)
            {
                eng_safety_worker_list domainEmp = new eng_safety_worker_list();
                domainEmp.SafetyID = domainSafety.SafetyID;
                domainEmp.EmpID = emp;
                Eng360DB.eng_safety_worker_list.Add(domainEmp);
            }



            return Eng360DB.SaveChanges();
        }

        public int SaveSafety(SafetyMasterViewModel safety, List<int> hazardList, List<int> ppeList, List<int> workerList)
        {
            try
            {
                var _db_safety = Mapper.Map<eng_safety_master>(safety);
                Eng360DB.Entry(_db_safety).State = EntityState.Modified;

                var hazards = Eng360DB.eng_safety_hazard_list.Where(a => a.SafetyID == _db_safety.SafetyID).ToList();

                foreach (var haz in hazards)
                {
                    var delhaz = Eng360DB.eng_safety_hazard_list.First(a => a.HLID == haz.HLID);
                    Eng360DB.eng_safety_hazard_list.Remove(delhaz);

                }

                var ppes = Eng360DB.eng_safety_ppe_list.Where(a => a.SafetyID == _db_safety.SafetyID).ToList();

                foreach (var ppe in ppes)
                {
                    var delppe = Eng360DB.eng_safety_ppe_list.First(a => a.PPELID == ppe.PPELID);
                    Eng360DB.eng_safety_ppe_list.Remove(delppe);

                }

                var emps = Eng360DB.eng_safety_worker_list.Where(a => a.SafetyID == _db_safety.SafetyID).ToList();

                foreach (var emp in emps)
                {
                    var delemp = Eng360DB.eng_safety_worker_list.First(a => a.WLID == emp.WLID);
                    Eng360DB.eng_safety_worker_list.Remove(delemp);

                }

                foreach (var haz in hazardList)
                {
                    eng_safety_hazard_list domainHaz = new eng_safety_hazard_list();
                    domainHaz.SafetyID = _db_safety.SafetyID;
                    domainHaz.HazardID = haz;
                    Eng360DB.eng_safety_hazard_list.Add(domainHaz);
                }

                foreach (var ppe in ppeList)
                {
                    eng_safety_ppe_list domainPPE = new eng_safety_ppe_list();
                    domainPPE.SafetyID = _db_safety.SafetyID;
                    domainPPE.PPEID = ppe;
                    Eng360DB.eng_safety_ppe_list.Add(domainPPE);
                }

                foreach (var emp in workerList)
                {
                    eng_safety_worker_list domainEmp = new eng_safety_worker_list();
                    domainEmp.SafetyID = _db_safety.SafetyID;
                    domainEmp.EmpID = emp;
                    Eng360DB.eng_safety_worker_list.Add(domainEmp);
                }



                return Eng360DB.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public int DeleteSafety(int safetyID)
        {
            var _db_safety = Eng360DB.eng_safety_master.First(a => a.SafetyID == safetyID);

            Eng360DB.eng_safety_master.Remove(_db_safety);
           
            return Eng360DB.SaveChanges();
        }

        public SafetyMasterViewModel getSafety(int safetyID)
        {
            eng_safety_master eng_safety_master = Eng360DB.eng_safety_master.Find(safetyID);

            return Mapper.Map<SafetyMasterViewModel>(eng_safety_master);
        }

        public NewSafetyInspectionViewModel NewSIEdit(int safetyID)
        {
            eng_safety_esh eng_safety_esh = Eng360DB.eng_safety_esh.Find(safetyID);

            return Mapper.Map<NewSafetyInspectionViewModel>(eng_safety_esh);
        }


        public List<SafetyMasterViewModel> getAllSafetys()
        {            
                var eng_safety_master = Eng360DB.eng_safety_master.ToList();
                var lstSafetyView = Mapper.Map<List<SafetyMasterViewModel>>(eng_safety_master);
                return lstSafetyView;            
        }

        public List<SafetyHazardListViewModel> getAllHazards()
        {
            var eng_sys_safety_hazard = Eng360DB.eng_sys_safety_hazard.ToList();
            var lstHazardView = Mapper.Map<List<SafetyHazardListViewModel>>(eng_sys_safety_hazard);
            return lstHazardView;
        }
        
        public List<SafetyPPEListViewModel> getAllPPEs()
        {
            var eng_sys_safety_ppelist = Eng360DB.eng_sys_safety_ppelist.ToList();
            var lstPPEView = Mapper.Map<List<SafetyPPEListViewModel>>(eng_sys_safety_ppelist);
            return lstPPEView;
        }

        public List<SafetyInspectionMasterViewModel> getAllSafetyInspections()
        {
            var eng_safety_master = Eng360DB.eng_safety_insp_master.ToList();
            var lstSafetyView = Mapper.Map<List<SafetyInspectionMasterViewModel>>(eng_safety_master);
            return lstSafetyView;
        }

        public List<NewSafetyInspectionViewModel> getAllNewSafetyInspections()
        {
            var eng_safety_master = Eng360DB.eng_safety_esh.ToList();
            var lstSafetyView = Mapper.Map<List<NewSafetyInspectionViewModel>>(eng_safety_master);
            return lstSafetyView;
        }


        public List<SafetyInspectionItemsViewModel> getAllItems()
        {
            var eng_safety_master = Eng360DB.eng_sys_safety_insp_items.ToList();
            var lstSafetyView = Mapper.Map<List<SafetyInspectionItemsViewModel>>(eng_safety_master);
            return lstSafetyView;
        }

        public int SICreate(SafetyInspectionMasterViewModel eng_Si)
        {

            try
            {
                eng_safety_insp_master domainSI = Mapper.Map<eng_safety_insp_master>(eng_Si);

                domainSI.eng_safety_insp_detail = new List<eng_safety_insp_detail>();
                
                var stageOneList = eng_Si.stage1String.Split(',');

                //1=yes , 2 = no , 3 = Na
                foreach (var stageOne in stageOneList)
                {
                    var tempId = stageOne.Split('*');
                    eng_safety_insp_detail domainSIDetail = new eng_safety_insp_detail();


                    domainSIDetail.SIItemID = Int32.Parse(tempId[0]);
                    domainSIDetail.Is_Applicable = Int32.Parse(tempId[1]);
                    
                    domainSIDetail.Recommendation = tempId[2];
                    domainSIDetail.ResponsiblePerson = tempId[3];

                    if (!string.IsNullOrEmpty(tempId[4]))
                    {
                        tempId[4] = DateTime.ParseExact(tempId[4], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                        domainSIDetail.ACDate = Convert.ToDateTime(tempId[4]);
                    }


                    domainSIDetail.SAFINSID = domainSI.SAFINSID;

                    domainSI.eng_safety_insp_detail.Add(domainSIDetail);
                }


                domainSI.CreatedDate = DateTime.Now;

                Eng360DB.eng_safety_insp_master.Add(domainSI);
                var result = Eng360DB.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }

        public SafetyInspectionMasterViewModel getSafetyInspectionMaster(int? id)
        {
            var result = Eng360DB.eng_safety_insp_master.Find(id);

            var si = Mapper.Map<SafetyInspectionMasterViewModel>(result);
            si.SIDate = Convert.ToDateTime(si.SIDate).ToString("dd/MM/yyyy");
            
            if (si.ACDate1 != null) 
                si.ACDate1 = Convert.ToDateTime(si.ACDate1).ToString("dd/MM/yyyy");
            if (si.ACDate2 != null)
                si.ACDate2 = Convert.ToDateTime(si.ACDate2).ToString("dd/MM/yyyy");
            if (si.ACDate3 != null)
                si.ACDate3 = Convert.ToDateTime(si.ACDate3).ToString("dd/MM/yyyy");
            if (si.ACDate4 != null)
                si.ACDate4 = Convert.ToDateTime(si.ACDate4).ToString("dd/MM/yyyy");
            if (si.ACDate5 != null)
                si.ACDate5 = Convert.ToDateTime(si.ACDate5).ToString("dd/MM/yyyy");
            if (si.ACDate6 != null)
                si.ACDate6 = Convert.ToDateTime(si.ACDate6).ToString("dd/MM/yyyy");
            if (si.ACDate7 != null)
                si.ACDate7 = Convert.ToDateTime(si.ACDate7).ToString("dd/MM/yyyy");
            if (si.ACDate8 != null)
                si.ACDate8 = Convert.ToDateTime(si.ACDate8).ToString("dd/MM/yyyy");
            if (si.ACDate9 != null)
                si.ACDate9 = Convert.ToDateTime(si.ACDate9).ToString("dd/MM/yyyy");
            if (si.ACDate10 != null)
                si.ACDate10 = Convert.ToDateTime(si.ACDate10).ToString("dd/MM/yyyy");

            return si;
        }

        public int SIEdit(SafetyInspectionMasterViewModel eng_Si)
        {
            try
            {
                eng_safety_insp_master domainSI = Mapper.Map<eng_safety_insp_master>(eng_Si);
                domainSI.UpdatedDate = DateTime.Now;

               
                    Eng360DB.Entry(domainSI).State = System.Data.Entity.EntityState.Modified;

                    var sistg1det = Eng360DB.eng_safety_insp_detail.Where(a => a.SAFINSID == eng_Si.SAFINSID).ToList();

                    foreach (var si in sistg1det)
                    {
                        var deldet = Eng360DB.eng_safety_insp_detail.First(a => a.SIDetailID == si.SIDetailID);
                        Eng360DB.eng_safety_insp_detail.Remove(deldet);

                    }

                domainSI.eng_safety_insp_detail = new List<eng_safety_insp_detail>();

                var stageOneList = eng_Si.stage1String.Split(',');

                //1=yes , 2 = no , 3 = Na
                foreach (var stageOne in stageOneList)
                {
                    var tempId = stageOne.Split('*');
                    eng_safety_insp_detail domainSIDetail = new eng_safety_insp_detail();


                    domainSIDetail.SIItemID = Int32.Parse(tempId[0]);
                    domainSIDetail.Is_Applicable = Int32.Parse(tempId[1]);

                    domainSIDetail.Recommendation = tempId[2];
                    domainSIDetail.ResponsiblePerson = tempId[3];

                    if (!string.IsNullOrEmpty(tempId[4]))
                    {
                        tempId[4] = DateTime.ParseExact(tempId[4], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                        domainSIDetail.ACDate = Convert.ToDateTime(tempId[4]);
                    }


                    domainSIDetail.SAFINSID = domainSI.SAFINSID;

                    domainSI.eng_safety_insp_detail.Add(domainSIDetail);
                }
                return Eng360DB.SaveChanges();
                               
            }
            catch (Exception ex)
            {

            }

            return -1;
        }


        public int NewSICreate(NewSafetyInspectionViewModel eng_safety_esh, string path, string halfPath)
        {
            try
            {
                eng_safety_esh domainSI = Mapper.Map<eng_safety_esh>(eng_safety_esh);

                //domainProject.ReportDate = DateTime.Now;
                domainSI.CreatedBy = AppSession.GetCurrentUserId();
                domainSI.CreatedDate = DateTime.Now;
                             
                    if (eng_safety_esh.files != null && eng_safety_esh.files.Count > 0)
                    {
                        int i = 0;
                        foreach (var file in eng_safety_esh.files)
                        {
                            if (file != null)
                            {
                                var filename = Path.GetFileName(file.FileName);
                                var rn = DateTime.Now.ToString("yyMMddhhmmss");

                                Directory.CreateDirectory(path);
                                var finalPath = path + "SI_" + domainSI.NSIID + "PR_" + domainSI.ProjectID +  "_" + rn + "." + getFileExtention(filename);
                                var halfPathEach = halfPath + "SI_" + domainSI.NSIID + "PR_" + domainSI.ProjectID +  "_" + rn + "." + getFileExtention(filename);
                                file.SaveAs(finalPath);

                                
                            domainSI.FileCaption = finalPath;

                            domainSI.FilePath = halfPathEach;
                            domainSI.FileName = file.FileName;
                               
                                i++;
                            }
                        }

                    Eng360DB.eng_safety_esh.Add(domainSI);
                    }
                var result = Eng360DB.SaveChanges();
                return result;               
            }
            catch (Exception ex)
            {
                return -1;
            }
            return -1;
        }

        string getFileExtention(string filename)
        {

            var result = filename.Split(new string[] { }, StringSplitOptions.None);

            return result[result.Length - 1];
        }

        public int deleteSafetyFile(int? id)
        {
            try
            {
                var rptFile = Eng360DB.eng_safety_esh.Find(id);
                if (rptFile != null)
                {
                    File.Delete(rptFile.FileCaption);
                    rptFile.FileCaption = null;
                    rptFile.FileName = null;
                    rptFile.FilePath = null;
                    Eng360DB.Entry(rptFile).State = System.Data.Entity.EntityState.Modified;
                }

                return Eng360DB.SaveChanges();

            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public int NewSIEdit(NewSafetyInspectionViewModel eng_safety_esh, string path, string halfPath)
        {
            try
            {
                eng_safety_esh domainSI = Mapper.Map<eng_safety_esh>(eng_safety_esh);
                domainSI.UpdatedBy = AppSession.GetCurrentUserId();
                domainSI.UpdatedDate = DateTime.Now;
                
                if (eng_safety_esh.files != null && eng_safety_esh.files.Count > 0)
                {
                    int i = 0;
                    foreach (var file in eng_safety_esh.files)
                    {
                        if (file != null)
                        {
                            var filename = Path.GetFileName(file.FileName);
                            var rn = DateTime.Now.ToString("yyMMddhhmmss");

                            Directory.CreateDirectory(path);
                            var finalPath = path + "SI_" + domainSI.NSIID + "PR_" + domainSI.ProjectID + "_" + rn + "." + getFileExtention(filename);
                            var halfPathEach = halfPath + "SI_" + domainSI.NSIID + "PR_" + domainSI.ProjectID + "_" + rn + "." + getFileExtention(filename);
                            file.SaveAs(finalPath);

                            domainSI.FileCaption = finalPath;
                            domainSI.FilePath = halfPathEach;
                            domainSI.FileName = file.FileName;

                            i++;
                        }
                    }
                    
                }

                Eng360DB.Entry(domainSI).State = System.Data.Entity.EntityState.Modified;

                var result = Eng360DB.SaveChanges();
                return result;
            }

            catch (Exception ex)
            {
                return -1;
            }
            return -1;
        }

        public int NewSICreate(MobileNewSafetyInspectionViewModel eng_safety_esh, string path, string halfPath)
        {
            try
            {
                eng_safety_esh domainSI = Mapper.Map<eng_safety_esh>(eng_safety_esh);
                //domainProject.ReportDate = DateTime.Now;
                // domainProject.CreatedBy = AppSession.GetCurrentUserId();
                domainSI.CreatedDate = DateTime.Now;
                Eng360DB.eng_safety_esh.Add(domainSI);

                var result = Eng360DB.SaveChanges();

                if (result > 0)
                    if (eng_safety_esh.files != null && eng_safety_esh.files.Count > 0)
                    {
                        int i = 0;
                        foreach (var file in eng_safety_esh.files)
                        {
                            if (file != null)
                            {
                                var data = Convert.FromBase64String(file.data);
                                var filename = file.filename;
                                var rn = DateTime.Now.ToString("yyMMddhhmmss");

                                Directory.CreateDirectory(path);
                                var finalPath = path + "SI_" + domainSI.NSIID + "PR_" + domainSI.ProjectID + "_" + rn + "." + getFileExtention(filename);
                                var halfPathEach = halfPath + "SI_" + domainSI.NSIID + "PR_" + domainSI.ProjectID + "_" + rn + "." + getFileExtention(filename);
                                //var finalPath = path + "P_" + domainProject.ProjectID + "PR_" + domainProject.ProjectReportID + "_" + i.ToString() + "_" + rn + "." + "jpg";
                                //var halfPathEach = halfPath + "P_" + domainProject.ProjectID + "PR_" + domainProject.ProjectReportID + "_" + i.ToString() + "_" + rn + "." + "jpg";
                                File.WriteAllBytes(finalPath, data);

                                //eng_project_report_files rptFiles = new eng_project_report_files();
                                //rptFiles.ProjectReportID = domainProject.ProjectReportID;
                                //rptFiles.FIleCaption = finalPath;

                                //rptFiles.FilePath = halfPathEach;
                                //rptFiles.FileName = file.filename;

                                domainSI.FileCaption = finalPath;
                                domainSI.FilePath = halfPathEach;
                                domainSI.FileName = file.filename;

                                Eng360DB.Entry(domainSI).State = System.Data.Entity.EntityState.Modified;

                                //   Eng360DB.eng_project_report_files.Add(rptFiles);
                                i++;
                            }
                        }
                        Eng360DB.SaveChanges();
                        return result;
                    }
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
            return -1;
        }
        public List<MobileNewSafetyInspectionViewModel> getAllSafetyInspections(int userID)
        {
            //DateTime from = DateTime.Parse(DateTime.Now.ToShortDateString());
            //DateTime to = DateTime.Parse(DateTime.Now.ToShortDateString()).AddDays(-30);

            var eng_safety_esh = Eng360DB.eng_safety_esh.Where(a => a.CreatedBy == userID).OrderByDescending(od => od.CreatedDate).ToList();
            var lstsafetyView = Mapper.Map<List<MobileNewSafetyInspectionViewModel>>(eng_safety_esh);

            foreach (var item in lstsafetyView)
            {
                var domianItem = eng_safety_esh.Where(a => a.NSIID == item.NSIID).FirstOrDefault();
                item.ProjectName = domianItem.eng_project_master.ProjectNo + " - " + domianItem.eng_project_master.ProjectName;
                item.InspectionDate = Convert.ToDateTime(domianItem.InspectionDate).ToString("dd/MM/yyyy");
                var filepaths = domianItem.FilePath;
                List<FileUploadViewModel> fileUpload = new List<FileUploadViewModel>();

                fileUpload.Add(new FileUploadViewModel() { filename = domianItem.FilePath });

                item.files = fileUpload;
            }

            return lstsafetyView;
        }



        public List<SafetyMasterViewModel> getFilterSafety(FilterViewModel filter)
        {
            var sql = "select * from eng_safety_master ";
            var where = "";

            //if (filter.Safety_Name != "Select")
            //    if (where == "")
            //        where += " Safety_Name ='" + filter.Safety_Name + "'";
            //    else
            //        where += " and Safety_Name ='" + filter.Safety_Name + "'";

            //if (filter.Safety_Company_Name != "Select")
            //    if (where == "")
            //        where += " Safety_Company_Name ='" + filter.Safety_Company_Name + "'";
            //    else
            //        where += " and Safety_Company_Name ='" + filter.Safety_Company_Name + "'";

            //if (filter.Safety_Code != "Select")
            //    if (where == "")
            //        where += " Safety_Code ='" + filter.Safety_Code + "'";
            //    else
            //        where += " and Safety_Code ='" + filter.Safety_Code + "'";

            //if (where != "")
            //    sql = sql + "Where " + where;

            //// DbSqlQuery<eng_employee_profile> data = Eng360DB.eng_employee_profile.SqlQuery(sql);
            var data = Eng360DB.eng_safety_master.SqlQuery(sql).ToList();

            var pdtView = Mapper.Map<List<SafetyMasterViewModel>>(data);
            return pdtView;
            
        }

    }

}