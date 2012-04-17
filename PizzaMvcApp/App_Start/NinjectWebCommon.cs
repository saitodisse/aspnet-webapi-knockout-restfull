using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Services;
using NHibernate;
using PizzaModel.Repos;
using PizzaModel.Services;
using PizzaNHibernate.Helpers;
using PizzaNHibernate.Repos;

[assembly: WebActivator.PreApplicationStartMethod(typeof(PizzaMvcApp.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(PizzaMvcApp.App_Start.NinjectWebCommon), "Stop")]

namespace PizzaMvcApp.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
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

            GlobalConfiguration.Configuration.ServiceResolver.SetResolver(System.Web.Mvc.DependencyResolver.Current.ToServiceResolver());
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
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ISession>().ToMethod(ctx => new NhCastle().InitSessionFactory().OpenSession()).InRequestScope();

            kernel.Bind<IPizzaService>().To<PizzaService>();
            kernel.Bind<IIngredientService>().To<IngredientService>();
            kernel.Bind<IIngredientDAO>().To<IngredientDAO>();
            kernel.Bind<IPizzaDAO>().To<PizzaDAO>();
            kernel.Bind<IAdminRepo>().To<AdminRepo>();
        }        
    }

    public class ServiceResolverAdapter : IDependencyResolver
    {
        private readonly System.Web.Mvc.IDependencyResolver dependencyResolver;

        public ServiceResolverAdapter(System.Web.Mvc.IDependencyResolver dependencyResolver)
        {
            if (dependencyResolver == null) throw new ArgumentNullException("dependencyResolver");
            this.dependencyResolver = dependencyResolver;
        }

        public object GetService(Type serviceType)
        {
            return dependencyResolver.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return dependencyResolver.GetServices(serviceType);
        }
    }

    public static class ServiceResolverExtensions
    {
        public static IDependencyResolver ToServiceResolver(this System.Web.Mvc.IDependencyResolver dependencyResolver)
        {
            return new ServiceResolverAdapter(dependencyResolver);
        }
    }
}
