[assembly: WebActivator.PreApplicationStartMethod(typeof(Eng360Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Eng360Web.App_Start.NinjectWebCommon), "Stop")]

namespace Eng360Web.App_Start
{
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Models.Repository.Imp;
    using Models.Repository.Interface;
    using Models.Service.Imp;
    using Models.Service.Interface;
    using System;
    using System.Web.Http;

    public class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
            RegisterServices(kernel);
            return kernel;
        }

        // <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IEmployeeRepository>().To<EmployeeRepository>();
            kernel.Bind<IEmployeeServices>().To<EmployeeServices>();

            kernel.Bind<IProductRepository>().To<ProductRepository>();
            kernel.Bind<IProductServices>().To<ProductServices>();

            kernel.Bind<ISupplierRepository>().To<SupplierRepository>();
            kernel.Bind<ISupplierServices>().To<SupplierServices>();

            kernel.Bind<IClientServices>().To<ClientServices>();
            kernel.Bind<IClientRepository>().To<ClientRepository>();

            kernel.Bind<IQuoteServices>().To<QuoteServices>();
            kernel.Bind<IQuoteRepository>().To<QuoteRepository>();

            kernel.Bind<IProjectRepository>().To<ProjectRepository>();
            kernel.Bind<IProjectServices>().To<ProjectServices>();

            kernel.Bind<IProjectReportService>().To<ProjectReportService>();
            kernel.Bind<IProjectReportRepository>().To<ProjectReportRepository>();

            kernel.Bind<IPaymentServices>().To<PaymentServices>();
            kernel.Bind<IPaymentRepository>().To<PaymentRepository>();

            kernel.Bind<IPoServices>().To<PoServices>();
            kernel.Bind<IPoRepository>().To<PoRepository>();

            kernel.Bind<ICompanyServices>().To<CompanyServices>();
            kernel.Bind<ICompanyRepository>().To<CompanyRepository>();

            kernel.Bind<IClaimServices>().To<ClaimServices>();
            kernel.Bind<IClaimRepository>().To<ClaimRepository>();

            kernel.Bind<ITimeEntryServices>().To<TimeEntryServices>();
            kernel.Bind<ITimeEntryRepository>().To<TimeEntryRepository>();

            kernel.Bind<IUserServices>().To<UserServices>();
            kernel.Bind<IUserRepository>().To<UserRepository>();

            kernel.Bind<IQualityDefectServices>().To<QualityDefectServices>();
            kernel.Bind<IQualityDefectRepository>().To<QualityDefectRepository>();

            kernel.Bind<ISafetyServices>().To<SafetyServices>();
            kernel.Bind<ISafetyRepository>().To<SafetyRepository>();

            kernel.Bind<IMaterialManagementServices>().To<MaterialManagementServices>();
            kernel.Bind<IMaterialManagementRepository>().To<MaterialManagementRepository>();

            kernel.Bind<IPTWServices>().To<PTWServices>();
            kernel.Bind<IPTWRepository>().To<PTWRepository>();

            kernel.Bind<IRAServices>().To<RAServices>();
            kernel.Bind<IRARepository>().To<RARepository>();

            kernel.Bind<IInvoiceServices>().To<InvoiceServices>();
            kernel.Bind<IInvoiceRepository>().To<InvoiceRepository>();

            kernel.Bind<ITransportRepository>().To<TransportRepository>();
            kernel.Bind<ITransportServices>().To<TransportServices>();

        }
    }
}