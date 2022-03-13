using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Eng360Web.Models.Domain;
using AutoMapper;
using System.Data.Entity;
using Eng360Web.Models.Utility;
using Eng360Web.Models.Repository.Interface;


namespace Eng360Web.Models.Repository.Interface
{
    public class QualityDefectRepository : IQualityDefectRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
        Logger logger = LogManager.GetCurrentClassLogger();

        public int CreateQualityDefect(QualityDefectViewModel eng_qa_defect, string path, string halfPath)
        {
            try
            {
                eng_qa_defect domainQualityDefect = Mapper.Map<eng_qa_defect>(eng_qa_defect);
                Eng360DB.eng_qa_defect.Add(domainQualityDefect);

                //foreach (var desc in eng_qa_defect.eng_qa_defect_detail)
                //{

                //    eng_qa_defect_detail domainDesc = Mapper.Map<eng_qa_defect_detail>(desc);
                //    domainDesc.DefectID = domainQualityDefect.DefectID;
                //    Eng360DB.eng_qa_defect_detail.Add(domainDesc);


                //}
                var result = Eng360DB.SaveChanges();


                if (result > 0)
                    if (eng_qa_defect.files != null && eng_qa_defect.files.Count > 0)
                    {
                        int i = 0;
                        foreach (var file in eng_qa_defect.files)
                        {
                            if (file != null)
                            {
                                var filename = Path.GetFileName(file.FileName);
                                var rn = DateTime.Now.ToString("yyMMddhhmmss");

                                Directory.CreateDirectory(path);
                                var finalPath = path + "CCQD_" + domainQualityDefect.DefectID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                var halfPathEach = halfPath + "CCQD_" + domainQualityDefect.DefectID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                file.SaveAs(finalPath);

                                eng_qa_defect_files rptFiles = new eng_qa_defect_files();
                                rptFiles.DefectID = domainQualityDefect.DefectID;
                                rptFiles.FIleCaption = finalPath;

                                rptFiles.FilePath = halfPathEach;
                                rptFiles.FileName = file.FileName;
                                rptFiles.DefectType = 0;

                                Eng360DB.eng_qa_defect_files.Add(rptFiles);
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
                //logger
                return -1;
            }
        }
        public int CreateQualityDefect(MobileQualityDefectViewModel eng_qa_defect, string path, string halfPath)
        {
            try
            {

                var eng_qa_defect_VM = Mapper.Map<QualityDefectViewModel>(eng_qa_defect);
                eng_qa_defect_VM.eng_qa_defect_detail = new List<QualityDefectDetailViewModel>();
                eng_qa_defect_VM.eng_qa_defect_detail.Add(new QualityDefectDetailViewModel() { DefectTitle = eng_qa_defect.DefectTitle, DefectImpactDetails = eng_qa_defect.DefectImpactDetails, DefectStatus = "InProcess" });

                eng_qa_defect domainQualityDefect = Mapper.Map<eng_qa_defect>(eng_qa_defect_VM);

                domainQualityDefect.DefStatus = "Completed";
                Eng360DB.eng_qa_defect.Add(domainQualityDefect);

                //foreach (var desc in eng_qa_defect.eng_qa_defect_detail)
                //{

                //    eng_qa_defect_detail domainDesc = Mapper.Map<eng_qa_defect_detail>(desc);
                //    domainDesc.DefectID = domainQualityDefect.DefectID;
                //    Eng360DB.eng_qa_defect_detail.Add(domainDesc);


                //}
                var result = Eng360DB.SaveChanges();


                if (result > 0)
                    if (eng_qa_defect.mobileFiles != null && eng_qa_defect.mobileFiles.Count > 0)
                    {
                        int i = 0;
                        foreach (var file in eng_qa_defect.mobileFiles)
                        {
                            if (file != null)
                            {

                                var data = Convert.FromBase64String(file.data);


                                var filename = file.filename;
                                var rn = DateTime.Now.ToString("yyMMddhhmmss");

                                Directory.CreateDirectory(path);
                                //var finalPath = path + "P_" + domainQualityDefect.DefectID + "PR_" + domainProject.ProjectReportID + "_" + i.ToString() + "_" + rn + "." + "jpg";
                                //var halfPathEach = halfPath + "P_" + domainProject.ProjectID + "PR_" + domainProject.ProjectReportID + "_" + i.ToString() + "_" + rn + "." + "jpg";
                                var finalPath = path + "CCQD_" + domainQualityDefect.DefectID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                var halfPathEach = halfPath + "CCQD_" + domainQualityDefect.DefectID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                File.WriteAllBytes(finalPath, data);

                                eng_qa_defect_files rptFiles = new eng_qa_defect_files();
                                rptFiles.DefectID = domainQualityDefect.DefectID;
                                rptFiles.FIleCaption = finalPath;

                                rptFiles.FilePath = halfPathEach;
                                rptFiles.FileName = file.filename;
                                rptFiles.DefectType = 0;

                                Eng360DB.eng_qa_defect_files.Add(rptFiles);
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
                //logger
                return -1;
            }
        }

        public int SaveQualityDefect(QualityDefectViewModel eng_qa_defect, string path, string halfPath)
        {
            try
            {
                var new_eng_qa_defect_desc = eng_qa_defect.eng_qa_defect_detail;

                eng_qa_defect.eng_qa_defect_detail = null;

                var domainQualityDefect = Mapper.Map<eng_qa_defect>(eng_qa_defect);
                Eng360DB.Entry(domainQualityDefect).State = EntityState.Modified;
                //foreach (var desc in eng_qa_defect.eng_qa_defect_detail)
                foreach (var desc in new_eng_qa_defect_desc)
                {
                    //insert 
                    if (desc.DefectDetailID == 0)
                    {
                        if (domainQualityDefect.DefStatus == "Completed")
                            desc.DefectStatus = "InProcess";

                        desc.DefectID = domainQualityDefect.DefectID;
                        eng_qa_defect_detail domainDesc = Mapper.Map<eng_qa_defect_detail>(desc);
                        Eng360DB.eng_qa_defect_detail.Add(domainDesc);
                    }
                    if (desc.DefectDetailID > 0)
                    {
                        if (domainQualityDefect.DefStatus == "Completed")
                            desc.DefectStatus = "InProcess";

                        desc.DefectID = domainQualityDefect.DefectID;
                        eng_qa_defect_detail domainDesc = Mapper.Map<eng_qa_defect_detail>(desc);
                        Eng360DB.Entry(domainDesc).State = EntityState.Modified;
                    }
                    if (desc.DefectDetailID < 0)
                    {
                        if (domainQualityDefect.DefStatus == "Completed")
                        {
                            desc.DefectStatus = "InProcess";
                            desc.DefectID = domainQualityDefect.DefectID;
                            eng_qa_defect_detail domainDesc = Mapper.Map<eng_qa_defect_detail>(desc);
                            Eng360DB.Entry(domainDesc).State = EntityState.Modified;
                        }
                    }
                    //edit
                }

                var result = Eng360DB.SaveChanges();

                if (result > 0)
                    if (eng_qa_defect.files != null && eng_qa_defect.files.Count > 0)
                    {
                        int i = 0;
                        foreach (var file in eng_qa_defect.files)
                        {
                            if (file != null)
                            {
                                var filename = Path.GetFileName(file.FileName);
                                var rn = DateTime.Now.ToString("yyMMddhhmmss");

                                Directory.CreateDirectory(path);
                                var finalPath = path + "CCQD_" + domainQualityDefect.DefectID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                var halfPathEach = halfPath + "CCQD_" + domainQualityDefect.DefectID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                file.SaveAs(finalPath);

                                eng_qa_defect_files rptFiles = new eng_qa_defect_files();
                                rptFiles.DefectID = domainQualityDefect.DefectID;
                                rptFiles.FilePath = halfPathEach;
                                rptFiles.FIleCaption = finalPath;
                                rptFiles.FileName = file.FileName;
                                rptFiles.DefectType = 0;

                                Eng360DB.eng_qa_defect_files.Add(rptFiles);
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
                //logger
                return -1;
            }

        }

        public int DeleteQualityDefect(int qualitydefectID)
        {
            var _db_qualitydefect = Eng360DB.eng_qa_defect.First(a => a.DefectID == qualitydefectID);


            Eng360DB.eng_qa_defect.Remove(_db_qualitydefect);


            return Eng360DB.SaveChanges();
        }


        public QualityDefectViewModel getQualityDefect(int qualitydefectID)
        {
            eng_qa_defect eng_qa_defect = Eng360DB.eng_qa_defect.AsNoTracking().Where(a => a.DefectID == qualitydefectID).FirstOrDefault();

            return Mapper.Map<QualityDefectViewModel>(eng_qa_defect);
        }

        public int ApproveRejectQualityDefect(QualityDefectViewModel qualitydefect)
        {

            var _db_qualitydefect = Mapper.Map<eng_qa_defect>(qualitydefect);
            Eng360DB.Entry(_db_qualitydefect).State = EntityState.Modified;


            return Eng360DB.SaveChanges();


        }

        public List<QualityDefectViewModel> getAllQualityDefects()
        {
            List<eng_qa_defect> qualitydefectList;


            qualitydefectList = Eng360DB.eng_qa_defect.ToList();


            var lstQualityDefectView = Mapper.Map<List<QualityDefectViewModel>>(qualitydefectList);
            return lstQualityDefectView;
        }

        public QualityDefectDetailViewModel getQualityDefectDetail(int? id)
        {

            var domainDesc = Eng360DB.eng_qa_defect_detail.Find(id);
            return Mapper.Map<QualityDefectDetailViewModel>(domainDesc);
        }



        string getFileExtention(string filename)
        {

            var result = filename.Split(new string[] { }, StringSplitOptions.None);

            return result[result.Length - 1];
        }

        public int deleteQualityDefectFile(int? id)
        {
            try
            {
                var rptFile = Eng360DB.eng_qa_defect_files.Find(id);
                if (rptFile != null)
                {

                    Eng360DB.eng_qa_defect_files.Remove(rptFile);

                    File.Delete(rptFile.FIleCaption);

                }

                return Eng360DB.SaveChanges();

            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public int deleteQualityDefectDetail(int? id)
        {
            try
            {
                var qualitydefectDescID = Eng360DB.eng_qa_defect_detail.Find(id);
                if (qualitydefectDescID != null)
                {
                    Eng360DB.eng_qa_defect_detail.Remove(qualitydefectDescID);

                }
                return Eng360DB.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public List<QualityDefectDetailViewModel> getAllDefectDetails()
        {
            List<eng_qa_defect_detail> defectdetailList;

            defectdetailList = Eng360DB.eng_qa_defect_detail.ToList();

            var lstDefectView = Mapper.Map<List<QualityDefectDetailViewModel>>(defectdetailList);
            return lstDefectView;
        }

        public QualityDefectCPAViewModel getQualityCPADetail(int? id)
        {

            var domainCPA = Eng360DB.eng_qa_defect_cpa.Where(a => a.DefectDetailID == id).FirstOrDefault();
            return Mapper.Map<QualityDefectCPAViewModel>(domainCPA);
        }


        public int AddEditCpa(QualityDefectCPAViewModel eng_qa_defect_cpa, string path, string halfPath)
        {
            try
            {
                var domainCpa = Mapper.Map<eng_qa_defect_cpa>(eng_qa_defect_cpa);

                var cpa = Eng360DB.eng_qa_defect_cpa.AsNoTracking().Where(a => a.DefectDetailID == eng_qa_defect_cpa.DefectDetailID).FirstOrDefault();
                if (cpa == null)
                {

                    Eng360DB.eng_qa_defect_cpa.Add(domainCpa);
                }
                else
                {
                    cpa = domainCpa;
                    Eng360DB.Entry(cpa).State = EntityState.Modified;
                }

                if (eng_qa_defect_cpa.TrackStatus == "Completed")
                {
                    var defectdet = Eng360DB.eng_qa_defect_detail.Where(a => a.DefectDetailID == eng_qa_defect_cpa.DefectDetailID).FirstOrDefault();
                    defectdet.DefectStatus = eng_qa_defect_cpa.TrackStatus;


                    Eng360DB.Entry(defectdet).State = EntityState.Modified;
                }

                var result = Eng360DB.SaveChanges();

                if (result > 0)
                    if (eng_qa_defect_cpa.files != null && eng_qa_defect_cpa.files.Count > 0)
                    {
                        int i = 0;
                        foreach (var file in eng_qa_defect_cpa.files)
                        {
                            if (file != null)
                            {
                                var filename = Path.GetFileName(file.FileName);
                                var rn = DateTime.Now.ToString("yyMMddhhmmss");

                                Directory.CreateDirectory(path);
                                var finalPath = path + "CCCPA_" + domainCpa.DPCID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                var halfPathEach = halfPath + "CCCPA_" + domainCpa.DPCID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                file.SaveAs(finalPath);

                                eng_qa_defect_cpa_files rptFiles = new eng_qa_defect_cpa_files();
                                rptFiles.DPCID = domainCpa.DPCID;
                                rptFiles.FilePath = halfPathEach;
                                rptFiles.FIleCaption = finalPath;
                                rptFiles.FileName = file.FileName;

                                Eng360DB.eng_qa_defect_cpa_files.Add(rptFiles);
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
                //logger
                return -1;
            }

        }


        public int AddEditCpa(MobileQualityDefectCPAViewModel eng_qa_defect_cpa, string path, string halfPath)
        {
            try
            {
                var domainCpa = Mapper.Map<eng_qa_defect_cpa>(eng_qa_defect_cpa);

                var cpa = Eng360DB.eng_qa_defect_cpa.AsNoTracking().Where(a => a.DefectDetailID == eng_qa_defect_cpa.DefectDetailID).FirstOrDefault();
                if (cpa == null)
                {

                    Eng360DB.eng_qa_defect_cpa.Add(domainCpa);
                }
                else
                {
                    cpa = domainCpa;
                    Eng360DB.Entry(cpa).State = EntityState.Modified;
                }

                if (eng_qa_defect_cpa.TrackStatus == "Completed")
                {
                    var defectdet = Eng360DB.eng_qa_defect_detail.Where(a => a.DefectDetailID == eng_qa_defect_cpa.DefectDetailID).FirstOrDefault();
                    defectdet.DefectStatus = eng_qa_defect_cpa.TrackStatus;


                    Eng360DB.Entry(defectdet).State = EntityState.Modified;
                }

                var result = Eng360DB.SaveChanges();

                if (result > 0)
                    if (eng_qa_defect_cpa.mobileFiles != null && eng_qa_defect_cpa.mobileFiles.Count > 0)
                    {
                        int i = 0;
                        foreach (var file in eng_qa_defect_cpa.mobileFiles)
                        {
                            if (file != null)
                            {

                                var data = Convert.FromBase64String(file.data);


                                var filename = file.filename;
                                var rn = DateTime.Now.ToString("yyMMddhhmmss");

                                Directory.CreateDirectory(path);
                                //var finalPath = path + "P_" + domainQualityDefect.DefectID + "PR_" + domainProject.ProjectReportID + "_" + i.ToString() + "_" + rn + "." + "jpg";
                                //var halfPathEach = halfPath + "P_" + domainProject.ProjectID + "PR_" + domainProject.ProjectReportID + "_" + i.ToString() + "_" + rn + "." + "jpg";
                                var finalPath = path + "CCCPA_" + domainCpa.DPCID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                var halfPathEach = halfPath + "CCCPA_" + domainCpa.DPCID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                File.WriteAllBytes(finalPath, data);

                                eng_qa_defect_cpa_files rptFiles = new eng_qa_defect_cpa_files();
                                rptFiles.DPCID = domainCpa.DPCID;
                                rptFiles.FIleCaption = finalPath;

                                rptFiles.FilePath = halfPathEach;
                                rptFiles.FileName = file.filename;

                                Eng360DB.eng_qa_defect_cpa_files.Add(rptFiles);
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
                //logger
                return -1;
            }

        }

        public int deleteQualityCPAFile(int? id)
        {
            try
            {
                var rptFile = Eng360DB.eng_qa_defect_cpa_files.Find(id);
                if (rptFile != null)
                {

                    Eng360DB.eng_qa_defect_cpa_files.Remove(rptFile);
                    File.Delete(rptFile.FIleCaption);

                }

                return Eng360DB.SaveChanges();

            }
            catch (Exception ex)
            {
                return -1;
            }

        }



    }

}