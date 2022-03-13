using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NLog;
using Eng360Web.Models;
using Eng360Web.Models.Domain;
using Eng360Web.Models.ViewModel;
using Eng360Web.Models.Authentication;
using Ninject;
using Eng360Web.App_Start;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.Repository.Interface;
using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.Service.Interface;

namespace Eng360Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            StartAutoMapper();
            GlobalConfiguration.Configuration.MessageHandlers.Add(new BasicAuthenticationMessageHandler());


        }
       
   
        private void StartAutoMapper()
        {
            Mapper.CreateMap<short, bool>().ConvertUsing<BooleanTypeConverter>();
            Mapper.CreateMap<bool, short>().ConvertUsing<ShortTypeConverter>();
            Mapper.CreateMap<int, bool>().ConvertUsing<BooleanIntTypeConverter>();
            Mapper.CreateMap<bool, int>().ConvertUsing<IntTypeConverter>();

            Mapper.CreateMap<UserViewModel, eng_users>();
            Mapper.CreateMap<eng_users, UserViewModel>();

            Mapper.CreateMap<EmployeeViewModel, eng_employee_profile>();
            Mapper.CreateMap<eng_employee_profile, EmployeeViewModel>();

            Mapper.CreateMap<eng_module, ModuleViewModel>();
            Mapper.CreateMap<ModuleViewModel, eng_module>();

            Mapper.CreateMap<ScreenViewModel, eng_screens>();
            Mapper.CreateMap<eng_screens, ScreenViewModel>();

            Mapper.CreateMap<eng_usergroup, GroupViewModel>();
            Mapper.CreateMap<GroupViewModel, eng_usergroup>();

            Mapper.CreateMap<eng_address_master, AddressViewModel>();
            Mapper.CreateMap<AddressViewModel, eng_address_master>();

            Mapper.CreateMap<eng_product_master, ProductViewModel>();
            Mapper.CreateMap<ProductViewModel, eng_product_master>();

            Mapper.CreateMap<eng_supplier_master, SupplierViewModel>();
            Mapper.CreateMap<SupplierViewModel, eng_supplier_master>();

            Mapper.CreateMap<FunctionViewModel, eng_sys_function>();
            Mapper.CreateMap<eng_sys_function, FunctionViewModel>();

            Mapper.CreateMap<IndustryViewModel, eng_sys_industry>();
            Mapper.CreateMap<eng_sys_industry, IndustryViewModel>();

            Mapper.CreateMap<QuoteStatusViewModel, eng_sys_quotestatus>();
            Mapper.CreateMap<eng_sys_quotestatus, QuoteStatusViewModel>();

            Mapper.CreateMap<ClientContactViewModel, eng_client_contact>();
            Mapper.CreateMap<eng_client_contact, ClientContactViewModel>();

            Mapper.CreateMap<ClientViewModel, eng_client_master>();
            Mapper.CreateMap<eng_client_master, ClientViewModel>();

            Mapper.CreateMap<QuoteDescriptionViewModel, eng_quote_description>();
            Mapper.CreateMap<eng_quote_description, QuoteDescriptionViewModel>();

            Mapper.CreateMap<QuoteViewModel, eng_quote_master>();
            Mapper.CreateMap<eng_quote_master, QuoteViewModel>();

            //Proejct Report
            Mapper.CreateMap<ProjectReportViewModel, eng_project_report>();
            Mapper.CreateMap<eng_project_report, ProjectReportViewModel>();

            Mapper.CreateMap<ProjectReportTaskStatusViewModel, eng_sys_task_status>();
            Mapper.CreateMap<eng_sys_task_status, ProjectReportTaskStatusViewModel>();

            Mapper.CreateMap<ProjectReportFilesViewModel, eng_project_report_files>();
            Mapper.CreateMap<eng_project_report_files, ProjectReportFilesViewModel>();


            Mapper.CreateMap<LocationViewModel, eng_sys_location>();
            Mapper.CreateMap<eng_sys_location, LocationViewModel>();

            Mapper.CreateMap<ProjectStatusViewModel, eng_sys_project_status>();
            Mapper.CreateMap<eng_sys_project_status, ProjectStatusViewModel>();


            Mapper.CreateMap<ProjectViewModel, eng_project_master>();
            Mapper.CreateMap<eng_project_master, ProjectViewModel>();

            Mapper.CreateMap<PaymentStatusViewModel, eng_sys_pymt_status>();
            Mapper.CreateMap<eng_sys_pymt_status, PaymentStatusViewModel>();

            Mapper.CreateMap<PaymentReceivableViewModel, eng_pymt_receivable>();
            Mapper.CreateMap<eng_pymt_receivable, PaymentReceivableViewModel>();

            Mapper.CreateMap<PaymentPayableViewModel, eng_pymt_payable>();
            Mapper.CreateMap<eng_pymt_payable, PaymentPayableViewModel>();

            Mapper.CreateMap<PoViewModel, eng_po_master>();
            Mapper.CreateMap<eng_po_master, PoViewModel>();

            Mapper.CreateMap<PoDescriptionViewModel, eng_po_description>();
            Mapper.CreateMap<eng_po_description, PoDescriptionViewModel>();

            Mapper.CreateMap<ClaimViewModel, eng_claim>();
            Mapper.CreateMap<eng_claim, ClaimViewModel>();

            Mapper.CreateMap<ClaimDescriptionViewModel, eng_claim_description>();
            Mapper.CreateMap<eng_claim_description, ClaimDescriptionViewModel>();

            Mapper.CreateMap<ClaimTypeViewModel, eng_sys_claimtype>();
            Mapper.CreateMap<eng_sys_claimtype, ClaimTypeViewModel>();

            Mapper.CreateMap<ClaimFilesViewModel, eng_claim_files>();
            Mapper.CreateMap<eng_claim_files, ClaimFilesViewModel>();


            Mapper.CreateMap<TimeEntryViewModel, eng_time_entry>();
            Mapper.CreateMap<eng_time_entry, TimeEntryViewModel>();
            Mapper.CreateMap<MobileTimeEntryViewModel, eng_time_entry>();
            Mapper.CreateMap<eng_time_entry, MobileTimeEntryViewModel>();
            Mapper.CreateMap<TimeEntryViewModel, MobileTimeEntryViewModel>();
            Mapper.CreateMap<MobileTimeEntryViewModel, TimeEntryViewModel>();
            


            Mapper.CreateMap<QualityDefectViewModel, eng_qa_defect>();
            Mapper.CreateMap<eng_qa_defect, QualityDefectViewModel>();

            Mapper.CreateMap<QualityDefectDetailViewModel, eng_qa_defect_detail>();
            Mapper.CreateMap<eng_qa_defect_detail, QualityDefectDetailViewModel>();

            Mapper.CreateMap<QualityDefectFilesViewModel, eng_qa_defect_files>();
            Mapper.CreateMap<eng_qa_defect_files, QualityDefectFilesViewModel>();

            Mapper.CreateMap<QualityDefectCPAViewModel, eng_qa_defect_cpa>();
            Mapper.CreateMap<eng_qa_defect_cpa, QualityDefectCPAViewModel>();

            Mapper.CreateMap<QualityDefectCPAFilesViewModel, eng_qa_defect_cpa_files>();
            Mapper.CreateMap<eng_qa_defect_cpa_files, QualityDefectCPAFilesViewModel>();

            Mapper.CreateMap<eng_project_report, MobileProjectReportViewModel>();
            Mapper.CreateMap< MobileProjectReportViewModel, eng_project_report>();


            Mapper.CreateMap<MobileCliamViewModel, ClaimViewModel>();
            Mapper.CreateMap<ClaimViewModel, MobileCliamViewModel>();

            Mapper.CreateMap<MobileClaimDescViewModel, ClaimDescriptionViewModel>();
            Mapper.CreateMap<ClaimDescriptionViewModel, MobileClaimDescViewModel>();

            Mapper.CreateMap<SafetyMasterViewModel, eng_safety_master>();
            Mapper.CreateMap<eng_safety_master, SafetyMasterViewModel>();

            Mapper.CreateMap<SafetyChildHazardListViewModel, eng_safety_hazard_list>();
            Mapper.CreateMap<eng_safety_hazard_list, SafetyChildHazardListViewModel>();

            Mapper.CreateMap<SafetyChildPPEListViewModel, eng_safety_ppe_list>();
            Mapper.CreateMap<eng_safety_ppe_list, SafetyChildPPEListViewModel>();

            Mapper.CreateMap<SafetyChildWorkerListViewModel, eng_safety_worker_list>();
            Mapper.CreateMap<eng_safety_worker_list, SafetyChildWorkerListViewModel>();

            Mapper.CreateMap<SafetyHazardListViewModel, eng_sys_safety_hazard>();
            Mapper.CreateMap<eng_sys_safety_hazard, SafetyHazardListViewModel>();

            Mapper.CreateMap<SafetyPPEListViewModel, eng_sys_safety_ppelist>();
            Mapper.CreateMap<eng_sys_safety_ppelist, SafetyPPEListViewModel>();

            Mapper.CreateMap<StoreMasterViewModel, eng_store_master>();
            Mapper.CreateMap<eng_store_master, StoreMasterViewModel>();

            Mapper.CreateMap<MMInwardViewModel, eng_inward>();
            Mapper.CreateMap<eng_inward, MMInwardViewModel>();

            Mapper.CreateMap<MMInwardDescViewModel, eng_mm_inwdesc>();
            Mapper.CreateMap<eng_mm_inwdesc, MMInwardDescViewModel>();

            Mapper.CreateMap<MMOutwardViewModel, eng_outward>();
            Mapper.CreateMap<eng_outward, MMOutwardViewModel>();

            Mapper.CreateMap<MMOutwardDescViewModel, eng_mm_outdesc>();
            Mapper.CreateMap<eng_mm_outdesc, MMOutwardDescViewModel>();

            Mapper.CreateMap<MMTransMasterViewModel, eng_mm_trmaster>();
            Mapper.CreateMap<eng_mm_trmaster, MMTransMasterViewModel>();

            Mapper.CreateMap<MMStockViewModel, GetStock_Result>();
            Mapper.CreateMap<GetStock_Result, MMStockViewModel>();

            Mapper.CreateMap<MMStockAdjustViewModel, eng_mm_stockadj_master>();
            Mapper.CreateMap<eng_mm_stockadj_master, MMStockAdjustViewModel>();

            Mapper.CreateMap<MobileSafetyMasterViewModel, SafetyMasterViewModel>();
            Mapper.CreateMap<SafetyMasterViewModel, MobileSafetyMasterViewModel>();

            Mapper.CreateMap<DashBoardHREmpViewModel, DB_Emp_Result>();
            Mapper.CreateMap<DB_Emp_Result, DashBoardHREmpViewModel>();

            Mapper.CreateMap<DashBoardCompanyViewModel, DB_Company_Result>();
            Mapper.CreateMap<DB_Company_Result, DashBoardCompanyViewModel>();

            Mapper.CreateMap<DashBoardVehicleViewModel, DB_Vehicle_Result>();
            Mapper.CreateMap<DB_Vehicle_Result, DashBoardVehicleViewModel>();

            Mapper.CreateMap<DashBoardPayableViewModel, DB_Payable_Result>();
            Mapper.CreateMap<DB_Payable_Result, DashBoardPayableViewModel>();

            Mapper.CreateMap<DashBoardReceivableViewModel, DB_Receivable_Result>();
            Mapper.CreateMap<DB_Receivable_Result, DashBoardReceivableViewModel>();

            Mapper.CreateMap<DashBoardOperationViewModel, DB_Operation_Result>();
            Mapper.CreateMap<DB_Operation_Result, DashBoardOperationViewModel>();

            Mapper.CreateMap<MobileQualityDefectViewModel, QualityDefectViewModel>();
            Mapper.CreateMap<QualityDefectViewModel, MobileQualityDefectViewModel>();

            Mapper.CreateMap<MobileSupplierViewModel, SupplierViewModel>();
            Mapper.CreateMap<SupplierViewModel, MobileSupplierViewModel>();

            Mapper.CreateMap<MobileQualityDefectCPAViewModel, QualityDefectCPAViewModel>();
            Mapper.CreateMap<QualityDefectCPAViewModel, MobileQualityDefectCPAViewModel>();


            Mapper.CreateMap<MobileQualityDefectDetailViewModel, QualityDefectDetailViewModel>();
            Mapper.CreateMap<QualityDefectDetailViewModel, MobileQualityDefectDetailViewModel>();
            Mapper.CreateMap<MobileQualityDefectCPAViewModel, eng_qa_defect_cpa>();
            Mapper.CreateMap<eng_qa_defect_cpa, MobileQualityDefectCPAViewModel>();

            Mapper.CreateMap<DashBoardDirectorProjectViewModel, DB_Director_Project_Result>();
            Mapper.CreateMap<DB_Director_Project_Result, DashBoardDirectorProjectViewModel>();

            Mapper.CreateMap<DashBoardSVManProjectViewModel, DB_SVMan_Project_Result>();
            Mapper.CreateMap<DB_SVMan_Project_Result, DashBoardSVManProjectViewModel>();

            Mapper.CreateMap<DashBoardSVManDTTRViewModel, DB_SVMan_DTTR_Result>();
            Mapper.CreateMap<DB_SVMan_DTTR_Result, DashBoardSVManDTTRViewModel>();

            Mapper.CreateMap<DashBoardSVManQualityViewModel, DB_SVMan_Quality_Result>();
            Mapper.CreateMap<DB_SVMan_Quality_Result, DashBoardSVManQualityViewModel>();

            Mapper.CreateMap<MobilePTWRConfigSatgeOne, eng_sys_ptw_stage1_config>();
            Mapper.CreateMap<eng_sys_ptw_stage1_config, MobilePTWRConfigSatgeOne>();

            Mapper.CreateMap<MobilePTWstages, eng_sys_ptw_stages>();
            Mapper.CreateMap<eng_sys_ptw_stages, MobilePTWstages>();

            Mapper.CreateMap<MobilePTWMaster, eng_ptw_master>();
            Mapper.CreateMap<eng_ptw_master, MobilePTWMaster>();

            Mapper.CreateMap<PTWMasterViewModel, eng_ptw_master>();
            Mapper.CreateMap<eng_ptw_master, PTWMasterViewModel>();

            Mapper.CreateMap<PTWStage1ViewModel, eng_PTW_Detail_Satge1>();
            Mapper.CreateMap<eng_PTW_Detail_Satge1, PTWStage1ViewModel>();

            Mapper.CreateMap<PTWStage4ViewModel, eng_PTW_Detail_Satge4>();
            Mapper.CreateMap<eng_PTW_Detail_Satge4, PTWStage4ViewModel>();

            Mapper.CreateMap<PTWEmployeeViewModel, eng_PTW_Employee_Details>();
            Mapper.CreateMap<eng_PTW_Employee_Details, PTWEmployeeViewModel>();

            Mapper.CreateMap<PTWConspcMasterViewModel, eng_ptw_conspc_master>();
            Mapper.CreateMap<eng_ptw_conspc_master, PTWConspcMasterViewModel>();

            Mapper.CreateMap<PTWConspcStage1ViewModel, eng_PTW_Conspc_Detail_Stage1>();
            Mapper.CreateMap<eng_PTW_Conspc_Detail_Stage1, PTWConspcStage1ViewModel>();

            Mapper.CreateMap<PTWConspcStage5ViewModel, eng_PTW_Conspc_Detail_Stage5>();
            Mapper.CreateMap<eng_PTW_Conspc_Detail_Stage5, PTWConspcStage5ViewModel>();

            Mapper.CreateMap<PTWConspcEmployeeViewModel, eng_PTW_Conspc_Employee_Details>();
            Mapper.CreateMap<eng_PTW_Conspc_Employee_Details, PTWConspcEmployeeViewModel>();

            Mapper.CreateMap<RAMastersViewModel, eng_ra_work_activity>();
            Mapper.CreateMap<eng_ra_work_activity, RAMastersViewModel>();

            Mapper.CreateMap<RAMastersViewModel, eng_ra_process>();
            Mapper.CreateMap<eng_ra_process, RAMastersViewModel>();

            Mapper.CreateMap<RAMastersViewModel, eng_ra_control_measures>();
            Mapper.CreateMap<eng_ra_control_measures, RAMastersViewModel>();

            Mapper.CreateMap<RAMastersViewModel, eng_ra_hazardlist>();
            Mapper.CreateMap<eng_ra_hazardlist, RAMastersViewModel>();

            Mapper.CreateMap<RAMastersViewModel, eng_ra_location>();
            Mapper.CreateMap<eng_ra_location, RAMastersViewModel>();

            Mapper.CreateMap<RAMastersViewModel, eng_ra_possible_accident_health>();
            Mapper.CreateMap<eng_ra_possible_accident_health, RAMastersViewModel>();

            Mapper.CreateMap<RATransMasterViewModel, eng_ra_trans_master>();
            Mapper.CreateMap<eng_ra_trans_master, RATransMasterViewModel>();

            Mapper.CreateMap<RATransDetail1ViewModel, eng_ra_trans_detail1>();
            Mapper.CreateMap<eng_ra_trans_detail1, RATransDetail1ViewModel>();

            Mapper.CreateMap<RATransRACMViewModel, eng_ra_trans_racm>();
            Mapper.CreateMap<eng_ra_trans_racm, RATransRACMViewModel>();

            Mapper.CreateMap<RALikelihoodViewModel, eng_sys_rm_likelihood>();
            Mapper.CreateMap<eng_sys_rm_likelihood, RALikelihoodViewModel>();

            Mapper.CreateMap<RASeverityViewModel, eng_sys_rm_severity>();
            Mapper.CreateMap<eng_sys_rm_severity, RASeverityViewModel>();

            Mapper.CreateMap<SafetyInspectionItemsViewModel, eng_sys_safety_insp_items>();
            Mapper.CreateMap<eng_sys_safety_insp_items, SafetyInspectionItemsViewModel>();

            Mapper.CreateMap<SafetyInspectionMasterViewModel, eng_safety_insp_master>();
            Mapper.CreateMap<eng_safety_insp_master, SafetyInspectionMasterViewModel>();

            Mapper.CreateMap<SafetyInspectionDetailViewModel, eng_safety_insp_detail>();
            Mapper.CreateMap<eng_safety_insp_detail, SafetyInspectionDetailViewModel>();

            Mapper.CreateMap<NewSafetyInspectionViewModel, eng_safety_esh>();
            Mapper.CreateMap<eng_safety_esh, NewSafetyInspectionViewModel>();

            Mapper.CreateMap<RATransMasterViewModel, MobileRATransMasterViewModel>();
            Mapper.CreateMap<MobileRATransMasterViewModel, RATransMasterViewModel>();

            Mapper.CreateMap<MobileRATransRACMViewModel, RATransRACMViewModel>();
            Mapper.CreateMap<RATransRACMViewModel, MobileRATransRACMViewModel>();

            Mapper.CreateMap<MobileRATransRACMViewModel, eng_ra_trans_racm>();
            Mapper.CreateMap<eng_ra_trans_racm, MobileRATransRACMViewModel>();

            Mapper.CreateMap<MobileNewSafetyInspectionViewModel, NewSafetyInspectionViewModel>();
            Mapper.CreateMap<NewSafetyInspectionViewModel, MobileNewSafetyInspectionViewModel>();

            Mapper.CreateMap<eng_safety_esh, MobileNewSafetyInspectionViewModel>();
            Mapper.CreateMap<MobileNewSafetyInspectionViewModel, eng_safety_esh>();

            Mapper.CreateMap<InvoiceMasterViewModel, eng_invoice_master>();
            Mapper.CreateMap<eng_invoice_master, InvoiceMasterViewModel>();

            Mapper.CreateMap<InvoiceDescriptionViewModel, eng_invoice_details>();
            Mapper.CreateMap<eng_invoice_details, InvoiceDescriptionViewModel>();

            Mapper.CreateMap<PermissionViewModel, eng_permission>();
            Mapper.CreateMap<eng_permission, PermissionViewModel>();

            Mapper.CreateMap<TransportViewModel, eng_transport_master>();
            Mapper.CreateMap<eng_transport_master, TransportViewModel>();

        }

        /// <summary>
        /// Converts shor to int
        /// </summary>
        private class BooleanTypeConverter : TypeConverter<short, bool>
        {
            protected override bool ConvertCore(short source)
            {
                if (source == 1)
                    return true;
                return false;
            }
        }

        private class BooleanIntTypeConverter : TypeConverter<int, bool>
        {
            protected override bool ConvertCore(int source)
            {
                if (source == 1)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// Converts bool to short
        /// </summary>
        private class ShortTypeConverter : TypeConverter<bool, short>
        {
            protected override short ConvertCore(bool source)
            {
                if (source)
                    return 1;
                return 0;
            }
        }

        private class IntTypeConverter : TypeConverter<bool, int>
        {
            protected override int ConvertCore(bool source)
            {
                if (source)
                    return 1;
                return 0;
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                logger.Error("Claim Report Error:");
                logger.Error(exception.Message);
                logger.Error(exception.StackTrace);

                int i = 0;
            }
        }

        protected void Session_Start()
        {

        }
        protected void Session_End()
        {
            Session.Abandon();
        }
    }
}
