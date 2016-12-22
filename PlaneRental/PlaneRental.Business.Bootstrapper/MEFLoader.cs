using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Web.Http.Dependencies;
using PlaneRental.Business.Managers;
using PlaneRental.Data;
using PlaneRental.Business;
using System.Web.Mvc;

namespace PlaneRental.Business.Bootstrapper
{
    public static class MefLoader
    {
        public static CompositionContainer Init()
        {
            AggregateCatalog catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new AssemblyCatalog(typeof(PlaneRentalEngine).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(AccountRepository).Assembly));
            
            CompositionContainer container = new CompositionContainer(catalog);

            return container;
        }

    }
    public class MefDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        private readonly CompositionContainer _container;

        public MefDependencyResolver(CompositionContainer container)
        {
            _container = container;
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        /// <summary>
        /// Called to request a service implementation.
        /// 
        /// Here we call upon MEF to instantiate implementations of dependencies.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>Service implementation or null.</returns>
        public object GetService(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            var name = AttributedModelServices.GetContractName(serviceType);
            var export = _container.GetExportedValueOrDefault<object>(name);
            return export;
        }

        /// <summary>
        /// Called to request service implementations.
        /// 
        /// Here we call upon MEF to instantiate implementations of dependencies.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>Service implementations.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            var exports = _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
            return exports;
        }

        public void Dispose()
        {
        }
    }

    public static class MefConfig
    {
        public static void RegisterMef()
        {
            var asmCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var container = new CompositionContainer(asmCatalog);
            var resolver = new MefDependencyResolver(container);
            // Install MEF dependency resolver for MVC
            DependencyResolver.SetResolver(resolver);
            // Install MEF dependency resolver for Web API
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}
