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
using System.Globalization;

namespace Eng360Web.Models.Repository.Interface
{
    public class ClaimRepository : IClaimRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
         Logger logger = LogManager.GetCurrentClassLogger();

        public int CreateClaim(ClaimViewModel eng_claim, string path, string halfPath)
        {
            try
            {
                eng_claim domainClaim = Mapper.Map<eng_claim>(eng_claim);
                Eng360DB.eng_claim.Add(domainClaim);

                //foreach (var desc in eng_claim.eng_claim_description)
                //{

                //    eng_claim_description domainDesc = Mapper.Map<eng_claim_description>(desc);
                //    domainDesc.ClaimID = domainClaim.ClaimID;
                //    Eng360DB.eng_claim_description.Add(domainDesc);


                //}
                var result = Eng360DB.SaveChanges();


                if (result > 0)
                {
                    if (eng_claim.files != null && eng_claim.files.Count > 0)
                    {
                        int i = 0;
                        foreach (var file in eng_claim.files)
                        {
                            if (file != null)
                            {
                                var filename = Path.GetFileName(file.FileName);
                                var rn = DateTime.Now.ToString("yyMMddhhmmss");

                                Directory.CreateDirectory(path);
                                var finalPath = path + "CC_" + domainClaim.ClaimID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                var halfPathEach = halfPath + "CC_" + domainClaim.ClaimID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                file.SaveAs(finalPath);

                                eng_claim_files rptFiles = new eng_claim_files();
                                rptFiles.ClaimID = domainClaim.ClaimID;
                                rptFiles.FIleCaption = finalPath;

                                rptFiles.FilePath = halfPathEach;
                                rptFiles.FileName = file.FileName;

                                Eng360DB.eng_claim_files.Add(rptFiles);
                                i++;
                            }
                        }
                        Eng360DB.SaveChanges();
                        return result;
                    }

                    if (eng_claim.Mobilefiles != null && eng_claim.Mobilefiles.Count > 0)
                    {
                        int i = 0;
                        foreach (var file in eng_claim.Mobilefiles)
                        {
                            if (file != null)
                            {

                                var data = Convert.FromBase64String(file.data);


                                var filename = file.filename;
                                var rn = DateTime.Now.ToString("yyMMddhhmmss");

                                Directory.CreateDirectory(path);
                                var finalPath = path + "CC_" + domainClaim.ClaimID + "_" + i.ToString() + "_" + rn + "." + "." + "jpg";
                                var halfPathEach = halfPath + "CC_" + domainClaim.ClaimID + "_" + i.ToString() + "_" + rn + "." + "." + "jpg";
                                File.WriteAllBytes(finalPath, data);

                                eng_claim_files rptFiles = new eng_claim_files();
                                rptFiles.ClaimID = domainClaim.ClaimID;
                                rptFiles.FIleCaption = finalPath;

                                rptFiles.FilePath = halfPathEach;
                                rptFiles.FileName = file.filename;

                                Eng360DB.eng_claim_files.Add(rptFiles);
                                i++;
                            }
                        }
                        Eng360DB.SaveChanges();
                        return result;
                    }
                }
                return result;


            }
            catch (Exception ex)
            {
                //logger
                return -1;
            }
        }


        public int SaveClaim(ClaimViewModel eng_claim, string path, string halfPath)
        {
            try
            {
                var new_eng_claim_desc = eng_claim.eng_claim_description;

                eng_claim.eng_claim_description = null;

                var domainClaim = Mapper.Map<eng_claim>(eng_claim);
                Eng360DB.Entry(domainClaim).State = EntityState.Modified;
                //foreach (var desc in eng_claim.eng_claim_description)
                    foreach (var desc in new_eng_claim_desc)
                    {
                    //insert 
                    if (desc.ClaimDescID == 0)
                    {
                        
                        desc.ClaimID = domainClaim.ClaimID;
                        eng_claim_description domainDesc = Mapper.Map<eng_claim_description>(desc);
                        Eng360DB.eng_claim_description.Add(domainDesc);
                    }
                    if (desc.ClaimDescID > 0)
                    {
                        
                        desc.ClaimID = domainClaim.ClaimID;
                        eng_claim_description domainDesc = Mapper.Map<eng_claim_description>(desc);
                        Eng360DB.Entry(domainDesc).State = EntityState.Modified;
                    }
                    //edit
                }

                var result = Eng360DB.SaveChanges();

                if (result > 0)
                    if (eng_claim.files != null && eng_claim.files.Count > 0)
                    {
                        int i = 0;
                        foreach (var file in eng_claim.files)
                        {
                            if (file != null)
                            {
                                var filename = Path.GetFileName(file.FileName);
                                var rn = DateTime.Now.ToString("yyMMddhhmmss");

                                Directory.CreateDirectory(path);
                                var finalPath = path  + "CC_" + domainClaim.ClaimID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                var halfPathEach = halfPath + "CC_" + domainClaim.ClaimID + "_" + i.ToString() + "_" + rn + "." + getFileExtention(filename);
                                file.SaveAs(finalPath);

                                eng_claim_files rptFiles = new eng_claim_files();
                                rptFiles.ClaimID = domainClaim.ClaimID;
                                rptFiles.FilePath = halfPathEach;
                                rptFiles.FIleCaption = finalPath;
                                rptFiles.FileName = file.FileName;

                                Eng360DB.eng_claim_files.Add(rptFiles);
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

        public int DeleteClaim(int claimID)
        {
            var _db_claim = Eng360DB.eng_claim.First(a => a.ClaimID == claimID);

            Eng360DB.Entry(_db_claim).State = EntityState.Modified;
            return Eng360DB.SaveChanges();
        }

        public ClaimViewModel getClaim(int claimID)
        {
            eng_claim eng_claim = Eng360DB.eng_claim.AsNoTracking().Where(a => a.ClaimID == claimID).FirstOrDefault();

            return Mapper.Map<ClaimViewModel>(eng_claim);
        }

        public int ApproveRejectClaim(ClaimViewModel claim)
        {

            try
            {
                eng_claim eng_claim = Eng360DB.eng_claim.AsNoTracking().Where(a => a.ClaimID == claim.ClaimID).FirstOrDefault();

                eng_claim.ApprovalRemarks = claim.ApprovalRemarks;
                eng_claim.ApprovedBy = claim.ApprovedBy;
                eng_claim.ApprovedDate = DateTime.Now;
                eng_claim.Status = claim.Status;
                eng_claim.isFullyPaid = claim.isFullyPaid;
                eng_claim.RejectRemarks = claim.RejectRemarks;

                Eng360DB.Entry(eng_claim).State = EntityState.Modified;

                return Eng360DB.SaveChanges();

            }
            catch(Exception ex)
            {
                logger.Info("Claim Approval Error:");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
                
                
                return -1;
            }

           // return Eng360DB.SaveChanges();

        }

        public List<ClaimViewModel> getAllClaims(int userid, int groupid)
        {
            List<eng_claim> claimList;

            if (groupid == 2)
            {
                 //claimList = Eng360DB.eng_claim.Where(a => a.ApprovedBy == userid).ToList();

                claimList = Eng360DB.eng_claim.ToList();
            }
            else
            {
                 claimList = Eng360DB.eng_claim.Where(a => a.SubmittedBy == userid).ToList();
            }

            
            var lstClaimView = Mapper.Map<List<ClaimViewModel>>(claimList);
            return lstClaimView;
        }

        public ClaimDescriptionViewModel getClaimDesc(int? id)
        {

            var domainDesc = Eng360DB.eng_claim_description.Find(id);
            return Mapper.Map<ClaimDescriptionViewModel>(domainDesc);
        }

        public List<ClaimTypeViewModel> getClaimType()
        {

            var obStatus = Eng360DB.eng_sys_claimtype.ToList();

            return Mapper.Map<List<ClaimTypeViewModel>>(obStatus);
        }

        string getFileExtention(string filename)
        {

            var result = filename.Split(new string[] { }, StringSplitOptions.None);

            return result[result.Length - 1];
        }
        
        public int deleteClaimFile(int? id)
        {
            try
            {
                var rptFile = Eng360DB.eng_claim_files.Find(id);
                if (rptFile != null)
                {

                    Eng360DB.eng_claim_files.Remove(rptFile);

                    File.Delete(rptFile.FIleCaption);

                }

                return Eng360DB.SaveChanges();

            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public int deleteClaimDesc(int? id)
        {
            try
            {
                var claimDescID = Eng360DB.eng_claim_description.Find(id);
                if (claimDescID != null)
                {
                    Eng360DB.eng_claim_description.Remove(claimDescID);

                }
                return Eng360DB.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public List<ClaimViewModel> getAllReportClaims()
        {
                List<eng_claim> claimList;
            
                claimList = Eng360DB.eng_claim.ToList();
            
            var lstClaimView = Mapper.Map<List<ClaimViewModel>>(claimList);
            return lstClaimView;
        }

        public List<ClaimViewModel> getAllEmpClaims(int id)
        {

            var listClaims = Eng360DB.eng_claim.Where(a => a.UserID == id && a.isFullyPaid == 0 && a.PaymentSource == "Self").ToList();

            List<ClaimViewModel> clvm = new List<ClaimViewModel>();

            foreach (var clm in listClaims)
            {

                clvm.Add(new ClaimViewModel() {  ClaimDisplayID = clm.ClaimDisplayID + "_(" + clm.TotalClaim + ")", SVRemarks = id.ToString() + "_" +clm.ClaimID.ToString() });

            }

            return clvm;
        }



        public List<ClaimViewModel> getFilterClaims(FilterViewModel filter)
        {
            var dCurrentDayofThisMonth = DateTime.Today.ToString("yyyy-MM-dd");
            var dFirstDayOfThisMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            var dLastDayOfLastMonth = dFirstDayOfThisMonth.AddDays(-1).ToString("yyyy-MM-dd");
            var dFirstDayOfLastMonth = dFirstDayOfThisMonth.AddMonths(-1).ToString("yyyy-MM-dd"); ;
            var dFirstDayOfCurrentMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).ToString("yyyy-MM-dd");

            var sql = "select * from eng_claim ";
            var where = "";
            if (filter.ProjectID != 0)
                where = "ProjectID = " + filter.ProjectID;

            if (filter.UserID > 0)
                if (where == "")
                    where += " UserID =" + filter.UserID;
                else
                    where += " and UserID =" + filter.UserID;

            if (filter.Month == 0)
            {
                if (filter.dateFrom != null)
                {
                    filter.dateFrom = DateTime.ParseExact(filter.dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " SubmittedDate >='" + filter.dateFrom + "'";
                    else
                        where += " and SubmittedDate >='" + filter.dateFrom + "'";
                }
                if (filter.dateTo != null)
                {
                    filter.dateTo = DateTime.ParseExact(filter.dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    if (where == "")
                        where += " SubmittedDate <='" + filter.dateTo + "'";
                    else
                        where += " and SubmittedDate <='" + filter.dateTo + "'";
                }
            }

            if (filter.Month == 1)
            {

                if (where == "")
                    where += " SubmittedDate >= '" + dFirstDayOfCurrentMonth + "' and SubmittedDate <= '" + dCurrentDayofThisMonth + "'";
                else
                    where += " and SubmittedDate >='" + dFirstDayOfCurrentMonth + "' and SubmittedDate <='" + dCurrentDayofThisMonth + "'";

            }

            if (filter.Month == 2)
            {

                if (where == "")
                    where += " SubmittedDate >='" + dFirstDayOfLastMonth + "' and SubmittedDate <='" + dLastDayOfLastMonth + "'";
                else
                    where += " and SubmittedDate >='" + dFirstDayOfLastMonth + "' and SubmittedDate <='" + dLastDayOfLastMonth + "'";

            }

            if (filter.QuoteStatusID > 0)
                if (where == "")
                {
                    var val = filter.QuoteStatusID - 1;
                    where += " Status =" + val;
                }
                else
                {
                    var val = filter.QuoteStatusID - 1;
                    where += " and Status =" + val;
                }


            if (where != "")
                sql = sql + "Where " + where;


            // DbSqlQuery<eng_employee_profile> data = Eng360DB.eng_employee_profile.SqlQuery(sql);
            var data = Eng360DB.eng_claim.SqlQuery(sql).ToList();

            var lstClaimView = Mapper.Map<List<ClaimViewModel>>(data);
            return lstClaimView;
        }



    }

}