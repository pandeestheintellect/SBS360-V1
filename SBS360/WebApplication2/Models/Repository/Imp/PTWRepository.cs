using AutoMapper;
using Eng360Web.Models.Domain;
using Eng360Web.Models.Repository.Interface;
using Eng360Web.Models.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Repository.Imp
{
    public class PTWRepository : IPTWRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
        Logger logger = LogManager.GetCurrentClassLogger();



        public List<MobilePTWRConfigSatgeOne> getPTWStageOneConfig()
        {
            var result = Eng360DB.eng_sys_ptw_stage1_config.ToList();

            return Mapper.Map<List<MobilePTWRConfigSatgeOne>>(result);
        }

        public List<MobilePTWRConfigSatgeOne> getPTWStageOneConfigWeb(string ptwType)
        {
            var result = Eng360DB.eng_sys_ptw_stage1_config.Where(a => a.PTW_Type.Trim() == ptwType).ToList();

            return Mapper.Map<List<MobilePTWRConfigSatgeOne>>(result);
        }


        public List<MobilePTWstages> getPTWStages(string ptwType = "")
        {
            List<eng_sys_ptw_stages> result = new List<eng_sys_ptw_stages>();
            //if (ptwType == "")
            result = Eng360DB.eng_sys_ptw_stages.ToList();
            //else
            //    result = Eng360DB.eng_sys_ptw_stages.Where(a => a.Stage_Type.Trim() == ptwType).ToList();

            if (ptwType != "")
                result = result.Where(a => a.Stage_Type.Trim().TrimStart() == ptwType).ToList();

            return Mapper.Map<List<MobilePTWstages>>(result);
        }

        
        public List<PTWMasterViewModel> getALLPTWMasterWeb()
        {
            var result = Eng360DB.eng_ptw_master.ToList();

            var listOfPtws = Mapper.Map<List<PTWMasterViewModel>>(result);
            return listOfPtws;
        }

        public PTWMasterViewModel getPTWMaster(int? id)
        {
            var result = Eng360DB.eng_ptw_master.Find(id);
            
            var ptw = Mapper.Map<PTWMasterViewModel>(result);
            ptw.Date_Time = Convert.ToDateTime(ptw.Date_Time).ToString("dd/MM/yyyy");
            ptw.New_Start_Date_Time = ptw.Start_Date_Time;
            ptw.New_End_Date_Time = ptw.End_Date_Time;
            ptw.Start_Date_Time = Convert.ToDateTime(ptw.Start_Date_Time).ToString("dd/MM/yyyy");
            ptw.End_Date_Time = Convert.ToDateTime(ptw.End_Date_Time).ToString("dd/MM/yyyy");
            ptw.Stage1_Date_Time = Convert.ToDateTime(ptw.Stage1_Date_Time).ToString("dd/MM/yyyy HH:mm");
            if(ptw.Stage2_Date_Time != null)
            ptw.Stage2_Date_Time = Convert.ToDateTime(ptw.Stage2_Date_Time).ToString("dd/MM/yyyy HH:mm");
            if (ptw.Stage3_Date_Time != null)
                ptw.Stage3_Date_Time = Convert.ToDateTime(ptw.Stage3_Date_Time).ToString("dd/MM/yyyy HH:mm");


            return ptw;
        }

        public List<MobilePTWMaster> getAllPtwDetails(int? id)
        {
            var ptw = Eng360DB.eng_ptw_master.Where(a => a.PTW_master_ID == id).FirstOrDefault();

            var project = Eng360DB.eng_project_master.Find(ptw.ProjectID);
            var projectname = "";
            if (project != null)
                projectname = project.ProjectNo + " - " + project.ProjectName;
            var stage4Details = ptw.eng_PTW_Detail_Satge4.ToList();

            List<MobilePTWMaster> returnObject = new List<MobilePTWMaster>();

            foreach (var details in stage4Details)
            {

                returnObject.Add(new MobilePTWMaster() { ProjectTitle = projectname, Date_Time = Convert.ToDateTime(details.DayDate), Day = details.Day, PTW_master_ID = Convert.ToInt32(id), Stage4_Sup_Name = details.Sup_Signature, Stage4_WSH_Name = details.Mng_Signature });

            }

            return returnObject;
        }




        public List<MobilePTWMaster> getALLPTWMaster()
        {
            var result = Eng360DB.eng_ptw_master.ToList();

            var listOfPtws = Mapper.Map<List<MobilePTWMaster>>(result);

            foreach (var ptw in listOfPtws)
            {
                List<string> stageOne = new List<string>();
                var domainPTW = result.Where(a => a.PTW_master_ID == ptw.PTW_master_ID).First();
                var project = Eng360DB.eng_project_master.Find(domainPTW.ProjectID);
                if (project != null)
                    ptw.ProjectTitle = project.ProjectNo + " - " + project.ProjectName;

                foreach (var satage1 in domainPTW.eng_PTW_Detail_Satge1)
                {
                    string temp = "";
                    if (satage1.Is_Applicable == 1)
                        temp = "yes";
                    else if (satage1.Is_Applicable == 2)
                        temp = "no";
                    else if (satage1.Is_Applicable == 3)
                        temp = "na";
                    stageOne.Add(satage1.PTW_Stage_One_ID + "_" + temp);
                }


                ptw.Stage_One_ItemIds = string.Join(",", stageOne.ToArray());


                if (domainPTW.eng_PTW_Detail_Satge4 != null)

                    ptw.is_already_created = domainPTW.eng_PTW_Detail_Satge4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy")).FirstOrDefault() == null ? false : true;
                else
                    ptw.is_already_created = false;

                if (ptw.is_already_created)
                {
                    var obj = domainPTW.eng_PTW_Detail_Satge4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy")).FirstOrDefault();

                    ptw.Stage4_Sup_Name = obj.Sup_Signature;
                    ptw.Stage4_WSH_Name = obj.Mng_Signature;
                    ptw.Stage4_Sup_Date_Time = obj.Sup_Sig_Date;
                    ptw.Stage4_WSH_Date_Time = obj.Mng_Sig_Date;
                }
                ptw.daysPtwCreated = getDayList(DateTime.Today.ToString("dddd").Substring(0, 3), domainPTW.eng_PTW_Detail_Satge4);
                //ptw.Stage4_Sup_Days = (domainPTW.MonSup == 1 ? "true" : "false") + "," +
                //    (domainPTW.TueSup == 1 ? "true" : "false") + "," + (domainPTW.WedSup == 1 ? "true" : "false")
                //    + "," + (domainPTW.ThuSup == 1 ? "true" : "false") + "," + (domainPTW.FriSup == 1 ? "true" : "false")
                //     + "," + (domainPTW.SatSup == 1 ? "true" : "false") + "," + (domainPTW.SunSup == 1 ? "true" : "false");

                //ptw.Stage4_Mng_Days = (domainPTW.MonMng == 1 ? "true" : "false") + "," +
                //    (domainPTW.TueMng == 1 ? "true" : "false") + "," + (domainPTW.WedMng == 1 ? "true" : "false")
                //    + "," + (domainPTW.ThuMng == 1 ? "true" : "false") + "," + (domainPTW.FriMng == 1 ? "true" : "false")
                //     + "," + (domainPTW.SatMng == 1 ? "true" : "false") + "," + (domainPTW.SunMng == 1 ? "true" : "false");

                ptw.EmployeeIDs = string.Join(",", domainPTW.eng_PTW_Employee_Details.Select(a => a.EmployeeID.ToString()).ToArray());
            }

            return listOfPtws;

        }
        List<string> getDayList(string strDay, ICollection<eng_PTW_Detail_Satge4> stage4)
        {
            List<string> days = new List<string>();
            if (strDay == "Mon")
                return days;

            if (strDay == "Tue")
            {
                //Monday only
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Mon");
                }
                return days;
            }
            if (strDay == "Wed")
            {
                //Monday only
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Tue");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Mon");
                }
                return days;
            }
            if (strDay == "Thu")
            {
                //Monday only
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Wed");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Thu");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Mon");
                }
                return days;
            }
            if (strDay == "Fri")
            {
                //Monday only
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Thu");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Wed");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Tue");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-4).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Mon");
                }
                return days;
            }
            if (strDay == "Sat")
            {
                //Monday only
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Fri");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Thu");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Wed");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-4).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Thu");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-5).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Mon");
                }
                return days;
            }
            if (strDay == "Sun")
            {
                //Monday only
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Sat");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Fri");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Thu");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-4).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Wed");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-5).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Tue");
                }
                if (stage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy")).FirstOrDefault() != null)
                {
                    days.Add("Mon");
                }
                return days;
            }

            return days;
        }


        public int createPtw(MobilePTWMaster eng_Ptw)
        {

            try
            {
                eng_ptw_master domainPTW = Mapper.Map<eng_ptw_master>(eng_Ptw);

                domainPTW.eng_PTW_Detail_Satge1 = new List<eng_PTW_Detail_Satge1>();
                domainPTW.eng_PTW_Employee_Details = new List<eng_PTW_Employee_Details>();

                var stageOneList = eng_Ptw.Stage_One_ItemIds.Split(',');

                //1=yes , 2 = no , 3 = Na
                foreach (var stageOne in stageOneList)
                {
                    var tempId = stageOne.Split('_');
                    eng_PTW_Detail_Satge1 domainPtwDetail = new eng_PTW_Detail_Satge1();
                    if (tempId[1] == "yes")
                    {
                        domainPtwDetail.PTW_Stage_One_ID = Int32.Parse(tempId[0]);
                        domainPtwDetail.Is_Applicable = 1;
                        domainPtwDetail.PTW_Master_ID = domainPTW.PTW_master_ID;
                    }
                    else if (tempId[1] == "no")
                    {
                        domainPtwDetail.PTW_Stage_One_ID = Int32.Parse(tempId[0]);
                        domainPtwDetail.Is_Applicable = 2;
                        domainPtwDetail.PTW_Master_ID = domainPTW.PTW_master_ID;
                    }
                    else if (tempId[1] == "na")
                    {
                        domainPtwDetail.PTW_Stage_One_ID = Int32.Parse(tempId[0]);
                        domainPtwDetail.Is_Applicable = 3;
                        domainPtwDetail.PTW_Master_ID = domainPTW.PTW_master_ID;
                    }
                    domainPTW.eng_PTW_Detail_Satge1.Add(domainPtwDetail);
                }

                var workerList = eng_Ptw.EmployeeIDs.Split(',');
                foreach (string emp in workerList)
                {
                    var emp_ID = Convert.ToInt32(emp);
                    domainPTW.eng_PTW_Employee_Details.Add(new eng_PTW_Employee_Details() { PTW_Master_ID = domainPTW.PTW_master_ID, EmployeeID = emp_ID });
                }


                //var supSatg4 = eng_Ptw.Stage4_Sup_Days.Split(',');

                //domainPTW.MonSup = bool.Parse(supSatg4[0]) ? 1 : 0;
                //domainPTW.TueSup = bool.Parse(supSatg4[1]) ? 1 : 0;
                //domainPTW.WedSup = bool.Parse(supSatg4[2]) ? 1 : 0;
                //domainPTW.ThuSup = bool.Parse(supSatg4[3]) ? 1 : 0;
                //domainPTW.FriSup = bool.Parse(supSatg4[4]) ? 1 : 0;
                //domainPTW.SatSup = bool.Parse(supSatg4[5]) ? 1 : 0;
                //domainPTW.SunSup = bool.Parse(supSatg4[6]) ? 1 : 0;

                //var mngSatg4 = eng_Ptw.Stage4_Mng_Days.Split(',');

                //domainPTW.MonMng = bool.Parse(mngSatg4[0]) ? 1 : 0;
                //domainPTW.TueMng = bool.Parse(mngSatg4[1]) ? 1 : 0;
                //domainPTW.WedMng = bool.Parse(mngSatg4[2]) ? 1 : 0;
                //domainPTW.ThuMng = bool.Parse(mngSatg4[3]) ? 1 : 0;
                //domainPTW.FriMng = bool.Parse(mngSatg4[4]) ? 1 : 0;
                //domainPTW.SatMng = bool.Parse(mngSatg4[5]) ? 1 : 0;
                //domainPTW.SunMng = bool.Parse(mngSatg4[6]) ? 1 : 0;

                domainPTW.Created_Date = DateTime.Now;

                domainPTW.Created_By = eng_Ptw.UserId;

                domainPTW.CompletedStage = 1;
                Eng360DB.eng_ptw_master.Add(domainPTW);
                var result = Eng360DB.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        public int EditPtw(MobilePTWMaster eng_Ptw)
        {
            try
            {
                //  eng_ptw_master domainPTW = Mapper.Map<eng_ptw_master>(eng_Ptw);
                eng_ptw_master domainPTW = Eng360DB.eng_ptw_master.Where(a => a.PTW_master_ID == eng_Ptw.PTW_master_ID).SingleOrDefault();
                // domainPTW.Updated_Date = DateTime.Today;

                if (eng_Ptw.CompletedStage == 1)
                {

                    domainPTW.Stage2_Date_Time = eng_Ptw.Stage2_Date_Time;
                    domainPTW.Stage2_Person_Name = eng_Ptw.Stage2_Person_Name;

                    if (!string.IsNullOrEmpty(eng_Ptw.Stage2_Person_Name))
                        domainPTW.CompletedStage = 2;
                    //if (!string.IsNullOrEmpty(eng_Ptw.Stage3_Person_Name))
                    //    domainPTW.CompletedStage = 3;
                    Eng360DB.Entry(domainPTW).State = System.Data.Entity.EntityState.Modified;


                    Eng360DB.SaveChanges();

                }
                if (eng_Ptw.CompletedStage == 2)
                {

                    domainPTW.Stage3_Date_Time = eng_Ptw.Stage3_Date_Time;
                    domainPTW.Stage3_Person_Name = eng_Ptw.Stage3_Person_Name;

                    if (!string.IsNullOrEmpty(eng_Ptw.Stage3_Person_Name))
                        domainPTW.CompletedStage = 3;
                    //if (!string.IsNullOrEmpty(eng_Ptw.Stage3_Person_Name))
                    //    domainPTW.CompletedStage = 3;
                    Eng360DB.Entry(domainPTW).State = System.Data.Entity.EntityState.Modified;


                    Eng360DB.SaveChanges();

                }


                if (eng_Ptw.CompletedStage >= 3)
                {
                    // domainPTW.eng_PTW_Detail_Satge4 = new List< eng_PTW_Detail_Satge4>();

                    string strDay = DateTime.Today.ToString("dddd").Substring(0, 3);

                    var listOFStage4 = Mapper.Map<List<PTWStage4ViewModel>>(Eng360DB.eng_PTW_Detail_Satge4.Where(a => a.Day == strDay && a.PTW_Master_ID == eng_Ptw.PTW_master_ID).ToList());
                    if (listOFStage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy")).FirstOrDefault() == null)
                    {
                        Eng360DB.eng_PTW_Detail_Satge4.Add(new eng_PTW_Detail_Satge4()
                        {
                            Day = strDay,
                            DayDate = DateTime.Today,
                            PTW_Master_ID = eng_Ptw.PTW_master_ID,
                            Sup_Signature = eng_Ptw.Stage4_Sup_Name,
                            Mng_Signature = eng_Ptw.Stage4_WSH_Name,
                            Mng_Sig_Date = DateTime.Today,
                            Sup_Sig_Date = DateTime.Today

                        });
                        domainPTW.Stage4_Sup_Name = eng_Ptw.Stage4_Sup_Name;
                        domainPTW.Stage4_Sup_Date_Time = eng_Ptw.Stage4_Sup_Date_Time;

                        domainPTW.Stage4_WSH_Name = eng_Ptw.Stage4_WSH_Name;
                        domainPTW.Stage4_WSH_Date_Time = eng_Ptw.Stage4_WSH_Date_Time;

                    }

                    if (!string.IsNullOrEmpty(eng_Ptw.Stage5_Sup_Person_Name) || !string.IsNullOrEmpty(eng_Ptw.Revoke_Desc))
                    {
                        domainPTW.Stage5_Sup_Person_Name = eng_Ptw.Stage5_Sup_Person_Name;
                        domainPTW.Stage5_Sup_Date_Time = eng_Ptw.Stage5_Sup_Date_Time;

                        domainPTW.Stage5_Mng_Person_Name = eng_Ptw.Stage5_mng_Person_Name;
                        domainPTW.Stage5_Mng_Date_Time = eng_Ptw.Stage5_mng_Date_Time;

                        domainPTW.Revoke_Desc = eng_Ptw.Revoke_Desc;
                        domainPTW.Revoke_Mng_Name = eng_Ptw.Revoke_Mng_Name;
                        domainPTW.Revoke_Sup_Name = eng_Ptw.Revoke_Sup_Name;


                        if (!string.IsNullOrEmpty(domainPTW.Stage5_Sup_Person_Name))
                            domainPTW.CompletedStage = 5;
                        else if (!string.IsNullOrEmpty(domainPTW.Revoke_Desc))
                            domainPTW.CompletedStage = 6;
                    }
                    Eng360DB.Entry(domainPTW).State = System.Data.Entity.EntityState.Modified;
                    Eng360DB.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
                return -1;
            }
            return 1;

        }


        public int EditPtw(PTWMasterViewModel eng_Ptw)
        {
            try
            {
                eng_ptw_master domainPTW = Mapper.Map<eng_ptw_master>(eng_Ptw);
                //eng_ptw_master domainPTW = Eng360DB.eng_ptw_master.Find(eng_Ptw.PTW_master_ID);
                domainPTW.Updated_Date = DateTime.Today;

                if (eng_Ptw.CompletedStage < 3)
                {
                    
                    if (!string.IsNullOrEmpty(eng_Ptw.Stage2_Person_Name))
                        domainPTW.CompletedStage = 2;


                    if (!string.IsNullOrEmpty(eng_Ptw.Stage3_Person_Name))
                        domainPTW.CompletedStage = 3;

                    Eng360DB.Entry(domainPTW).State = System.Data.Entity.EntityState.Modified;

                    var ptwstg1det = Eng360DB.eng_PTW_Detail_Satge1.Where(a => a.PTW_Master_ID == eng_Ptw.PTW_master_ID).ToList();

                    foreach (var ptw in ptwstg1det)
                    {
                        var deldet = Eng360DB.eng_PTW_Detail_Satge1.First(a => a.PTW_Detail_Satge1_ID == ptw.PTW_Detail_Satge1_ID);
                        Eng360DB.eng_PTW_Detail_Satge1.Remove(deldet);

                    }

                    var emplist = Eng360DB.eng_PTW_Employee_Details.Where(a => a.PTW_Master_ID == eng_Ptw.PTW_master_ID).ToList();

                    foreach (var empl in emplist)
                    {
                        var delemp = Eng360DB.eng_PTW_Employee_Details.First(a => a.PTWEmployeeID == empl.PTWEmployeeID);
                        Eng360DB.eng_PTW_Employee_Details.Remove(delemp);

                    }

                    domainPTW.eng_PTW_Detail_Satge1 = new List<eng_PTW_Detail_Satge1>();
                    domainPTW.eng_PTW_Employee_Details = new List<eng_PTW_Employee_Details>();

                    var stageOneList = eng_Ptw.stage1String.Split(',');

                    //1=yes , 2 = no , 3 = Na
                    foreach (var stageOne in stageOneList)
                    {
                        var tempId = stageOne.Split(':');
                        eng_PTW_Detail_Satge1 domainPtwDetail = new eng_PTW_Detail_Satge1();


                        domainPtwDetail.PTW_Stage_One_ID = Int32.Parse(tempId[0]);
                        domainPtwDetail.Is_Applicable = Int32.Parse(tempId[1]);
                        domainPtwDetail.PTW_Master_ID = domainPTW.PTW_master_ID;

                        domainPTW.eng_PTW_Detail_Satge1.Add(domainPtwDetail);
                    }

                    var workerList = eng_Ptw.EmpString.Split(',');
                    foreach (string emp in workerList)
                    {
                        var emp_ID = Convert.ToInt32(emp);
                        domainPTW.eng_PTW_Employee_Details.Add(new eng_PTW_Employee_Details() { PTW_Master_ID = domainPTW.PTW_master_ID, EmployeeID = emp_ID });
                    }


                    Eng360DB.SaveChanges();

                }

                if (eng_Ptw.CompletedStage >= 3)
                {
                    // domainPTW.eng_PTW_Detail_Satge4 = new List< eng_PTW_Detail_Satge4>();

                    string strDay = DateTime.Today.ToString("dddd").Substring(0, 3);

                    var listOFStage4 = Mapper.Map<List<PTWStage4ViewModel>>(Eng360DB.eng_PTW_Detail_Satge4.Where(a => a.Day == strDay && a.PTW_Master_ID == eng_Ptw.PTW_master_ID).ToList());
                    if (listOFStage4.Where(a => a.DayDate.Value.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy")).FirstOrDefault() == null)
                    {
                        Eng360DB.eng_PTW_Detail_Satge4.Add(new eng_PTW_Detail_Satge4()
                        {
                            Day = strDay,
                            DayDate = DateTime.Today,
                            PTW_Master_ID = domainPTW.PTW_master_ID,
                            Sup_Signature = domainPTW.Stage4_Sup_Name,
                            Mng_Signature = domainPTW.Stage4_WSH_Name,
                            Mng_Sig_Date = DateTime.Today,
                            Sup_Sig_Date = DateTime.Today

                        });
                    }

                    if (!string.IsNullOrEmpty(domainPTW.Stage5_Sup_Person_Name) || !string.IsNullOrEmpty(domainPTW.Revoke_Desc))
                    {
                        Eng360DB.Entry(domainPTW).State = System.Data.Entity.EntityState.Modified;
                        if (!string.IsNullOrEmpty(domainPTW.Stage5_Sup_Person_Name))
                            domainPTW.CompletedStage = 5;
                        else if (!string.IsNullOrEmpty(domainPTW.Revoke_Desc))
                            domainPTW.CompletedStage = 6;
                    }

                    Eng360DB.SaveChanges();
                }
                

            }
            catch (Exception ex)
            {

            }

            return -1;
        }


        public int createPtw(PTWMasterViewModel eng_Ptw)
        {

            try
            {
                eng_ptw_master domainPTW = Mapper.Map<eng_ptw_master>(eng_Ptw);

                domainPTW.eng_PTW_Detail_Satge1 = new List<eng_PTW_Detail_Satge1>();
                domainPTW.eng_PTW_Employee_Details = new List<eng_PTW_Employee_Details>();

                var stageOneList = eng_Ptw.stage1String.Split(',');

                //1=yes , 2 = no , 3 = Na
                foreach (var stageOne in stageOneList)
                {
                    var tempId = stageOne.Split(':');
                    eng_PTW_Detail_Satge1 domainPtwDetail = new eng_PTW_Detail_Satge1();


                    domainPtwDetail.PTW_Stage_One_ID = Int32.Parse(tempId[0]);
                    domainPtwDetail.Is_Applicable = Int32.Parse(tempId[1]);
                    domainPtwDetail.PTW_Master_ID = domainPTW.PTW_master_ID;

                    domainPTW.eng_PTW_Detail_Satge1.Add(domainPtwDetail);
                }

                var workerList = eng_Ptw.EmpString.Split(',');
                foreach (string emp in workerList)
                {
                    var emp_ID = Convert.ToInt32(emp);
                    domainPTW.eng_PTW_Employee_Details.Add(new eng_PTW_Employee_Details() { PTW_Master_ID = domainPTW.PTW_master_ID, EmployeeID = emp_ID });
                }

                if (!string.IsNullOrEmpty(eng_Ptw.Stage1_Person_Name))
                    domainPTW.CompletedStage = 1;

                if (!string.IsNullOrEmpty(eng_Ptw.Stage2_Person_Name))
                    domainPTW.CompletedStage = 2;


                if (!string.IsNullOrEmpty(eng_Ptw.Stage3_Person_Name))
                    domainPTW.CompletedStage = 3;

                if (!string.IsNullOrEmpty(eng_Ptw.Stage4_Sup_Name))
                    domainPTW.CompletedStage = 4;

                domainPTW.Created_Date = DateTime.Now;




                Eng360DB.eng_ptw_master.Add(domainPTW);
                var result = Eng360DB.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }


        public List<PTWConspcMasterViewModel> getALLPTWConspcMasterWeb()
        {
            var result = Eng360DB.eng_ptw_conspc_master.ToList();

            var listOfPtws = Mapper.Map<List<PTWConspcMasterViewModel>>(result);
            return listOfPtws;
        }


        public int createPtwConspc(PTWConspcMasterViewModel eng_Ptw)
        {

            try
            {
                eng_ptw_conspc_master domainPTW = Mapper.Map<eng_ptw_conspc_master>(eng_Ptw);

                domainPTW.eng_PTW_Conspc_Detail_Stage1 = new List<eng_PTW_Conspc_Detail_Stage1>();
                domainPTW.eng_PTW_Conspc_Employee_Details = new List<eng_PTW_Conspc_Employee_Details>();

                var stageOneList = eng_Ptw.stage1String.Split(',');

                //1=yes , 2 = no , 3 = Na
                foreach (var stageOne in stageOneList)
                {
                    var tempId = stageOne.Split(':');
                    eng_PTW_Conspc_Detail_Stage1 domainPtwDetail = new eng_PTW_Conspc_Detail_Stage1();


                    domainPtwDetail.PTW_Stage_One_ID = Int32.Parse(tempId[0]);
                    domainPtwDetail.Is_Applicable_Applicant = Int32.Parse(tempId[1]);
                    domainPtwDetail.Is_Applicable_Assessor = Int32.Parse(tempId[2]);
                    domainPtwDetail.Assessor_Remarks = tempId[3];


                    domainPtwDetail.PTW_Master_ID = domainPTW.PTW_master_ID;

                    domainPTW.eng_PTW_Conspc_Detail_Stage1.Add(domainPtwDetail);
                }

                var workerList = eng_Ptw.EmpString.Split(',');
                foreach (string emp in workerList)
                {
                    var emp_ID = Convert.ToInt32(emp);
                    domainPTW.eng_PTW_Conspc_Employee_Details.Add(new eng_PTW_Conspc_Employee_Details() { PTW_Master_ID = domainPTW.PTW_master_ID, EmployeeID = emp_ID });
                }

                if (!string.IsNullOrEmpty(eng_Ptw.Applicant_Name))
                    domainPTW.CompletedStage = 1;

                //if (!string.IsNullOrEmpty(eng_Ptw.Stage2_Person_Name))
                //    domainPTW.CompletedStage = 2;


                //if (!string.IsNullOrEmpty(eng_Ptw.Stage3_Person_Name))
                //    domainPTW.CompletedStage = 3;

                //if (!string.IsNullOrEmpty(eng_Ptw.Stage4_Sup_Name))
                //    domainPTW.CompletedStage = 4;

                domainPTW.Created_Date = DateTime.Now;

                Eng360DB.eng_ptw_conspc_master.Add(domainPTW);
                var result = Eng360DB.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }


        public int EditPtwConspc(PTWConspcMasterViewModel eng_Ptw)
        {
            try
            {
                eng_ptw_conspc_master domainPTW = Mapper.Map<eng_ptw_conspc_master>(eng_Ptw);
                //eng_ptw_master domainPTW = Eng360DB.eng_ptw_master.Find(eng_Ptw.PTW_master_ID);
                domainPTW.Updated_Date = DateTime.Today;

                if (eng_Ptw.CompletedStage < 4)
                {

                    if (!string.IsNullOrEmpty(eng_Ptw.Stage2_Assessor_Name))
                        domainPTW.CompletedStage = 2;


                    if (!string.IsNullOrEmpty(eng_Ptw.Stage3_WSH_Name))
                        domainPTW.CompletedStage = 3;

                    if (!string.IsNullOrEmpty(eng_Ptw.Stage4_Mng_Name))
                        domainPTW.CompletedStage = 4;

                    Eng360DB.Entry(domainPTW).State = System.Data.Entity.EntityState.Modified;

                    var ptwstg1det = Eng360DB.eng_PTW_Conspc_Detail_Stage1.Where(a => a.PTW_Master_ID == eng_Ptw.PTW_master_ID).ToList();

                    foreach (var ptw in ptwstg1det)
                    {
                        var deldet = Eng360DB.eng_PTW_Conspc_Detail_Stage1.First(a => a.PTW_Conspc_Stage1_ID == ptw.PTW_Conspc_Stage1_ID);
                        Eng360DB.eng_PTW_Conspc_Detail_Stage1.Remove(deldet);

                    }

                    var emplist = Eng360DB.eng_PTW_Conspc_Employee_Details.Where(a => a.PTW_Master_ID == eng_Ptw.PTW_master_ID).ToList();

                    foreach (var empl in emplist)
                    {
                        var delemp = Eng360DB.eng_PTW_Conspc_Employee_Details.First(a => a.PTWEmployeeID == empl.PTWEmployeeID);
                        Eng360DB.eng_PTW_Conspc_Employee_Details.Remove(delemp);

                    }

                    domainPTW.eng_PTW_Conspc_Detail_Stage1 = new List<eng_PTW_Conspc_Detail_Stage1>();
                    domainPTW.eng_PTW_Conspc_Employee_Details = new List<eng_PTW_Conspc_Employee_Details>();

                    var stageOneList = eng_Ptw.stage1String.Split(',');

                    //1=yes , 2 = no , 3 = Na
                    foreach (var stageOne in stageOneList)
                    {
                        var tempId = stageOne.Split(':');
                        eng_PTW_Conspc_Detail_Stage1 domainPtwDetail = new eng_PTW_Conspc_Detail_Stage1();


                        domainPtwDetail.PTW_Stage_One_ID = Int32.Parse(tempId[0]);
                        domainPtwDetail.Is_Applicable_Applicant = Int32.Parse(tempId[1]);
                        domainPtwDetail.Is_Applicable_Assessor = Int32.Parse(tempId[2]);
                        domainPtwDetail.Assessor_Remarks = tempId[3];

                        domainPTW.eng_PTW_Conspc_Detail_Stage1.Add(domainPtwDetail);
                    }

                    var workerList = eng_Ptw.EmpString.Split(',');
                    foreach (string emp in workerList)
                    {
                        var emp_ID = Convert.ToInt32(emp);
                        domainPTW.eng_PTW_Conspc_Employee_Details.Add(new eng_PTW_Conspc_Employee_Details() { PTW_Master_ID = domainPTW.PTW_master_ID, EmployeeID = emp_ID });
                    }


                    Eng360DB.SaveChanges();

                }

                if (eng_Ptw.CompletedStage >= 4)
                {

                    if (eng_Ptw.Stage5_Assessor_Name != null)
                    {
                        Eng360DB.eng_PTW_Conspc_Detail_Stage5.Add(new eng_PTW_Conspc_Detail_Stage5()
                        {
                            PTW_Master_ID = domainPTW.PTW_master_ID,
                            Stage5_Date_Time = Convert.ToDateTime(eng_Ptw.Stage5_Date_Time),
                            O2 = eng_Ptw.Stage5_O2,
                            CO2 = eng_Ptw.Stage5_CO2,
                            LEL = eng_Ptw.Stage5_LEL,
                            H2S = eng_Ptw.Stage5_H2S,
                            Safe_for_Entry = eng_Ptw.Stage5_Safe_for_Entry,
                            Stage5_Assessor_Name = eng_Ptw.Stage5_Assessor_Name,
                            Assessor_Comments = eng_Ptw.Stage5_Comments

                        });
                    }

                    //domainPTW.CompletedStage = 5;
                

                if (!string.IsNullOrEmpty(domainPTW.Stage6_Person_Name) || !string.IsNullOrEmpty(domainPTW.Revoke_Desc))
                    {
                        Eng360DB.Entry(domainPTW).State = System.Data.Entity.EntityState.Modified;
                        if (!string.IsNullOrEmpty(domainPTW.Stage6_Person_Name))
                            domainPTW.CompletedStage = 6;
                        else if (!string.IsNullOrEmpty(domainPTW.Revoke_Desc))
                        {
                            domainPTW.CompletedStage = 7;
                            domainPTW.Revoke_Date_Time = DateTime.Now;
                        }
                    }
                    Eng360DB.Entry(domainPTW).State = System.Data.Entity.EntityState.Modified;
                    return Eng360DB.SaveChanges();
                }


            }
            catch (Exception ex)
            {

            }

            return -1;
        }

        public PTWConspcMasterViewModel getPTWConspcMaster(int? id)
        {
            var result = Eng360DB.eng_ptw_conspc_master.Find(id);

            var ptw = Mapper.Map<PTWConspcMasterViewModel>(result);
            ptw.Applicant_Date_Time = Convert.ToDateTime(ptw.Applicant_Date_Time).ToString("dd/MM/yyyy HH:mm");
            //ptw.New_Start_Date_Time = ptw.Start_Date_Time;
            //ptw.New_End_Date_Time = ptw.End_Date_Time;
            ptw.Start_Date_Time = Convert.ToDateTime(ptw.Start_Date_Time).ToString("dd/MM/yyyy");
            ptw.End_Date_Time = Convert.ToDateTime(ptw.End_Date_Time).ToString("dd/MM/yyyy");
            //ptw.Stage1_Date_Time = Convert.ToDateTime(ptw.Stage1_Date_Time).ToString("dd/MM/yyyy");
            if (ptw.Stage2_Assessor_Date_Time != null)
                ptw.Stage2_Assessor_Date_Time = Convert.ToDateTime(ptw.Stage2_Assessor_Date_Time).ToString("dd/MM/yyyy HH:mm");
            if (ptw.Stage3_WSH_Date_Time != null)
                ptw.Stage3_WSH_Date_Time = Convert.ToDateTime(ptw.Stage3_WSH_Date_Time).ToString("dd/MM/yyyy HH:mm");
            if (ptw.Stage4_Date_Time != null)
                ptw.Stage4_Date_Time = Convert.ToDateTime(ptw.Stage4_Date_Time).ToString("dd/MM/yyyy HH:mm");


            return ptw;
        }


    }
}