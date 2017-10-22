using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Analyst.DBAccess.Contexts;
using Analyst.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.WebApi;
using Unity.Exceptions;

namespace Analyst.Web.App_Start
{
    public class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            
            UnityContainer container = new UnityContainer();

            config.DependencyResolver = new UnityDependencyResolver(container);//framework version
            //config.DependencyResolver = new CustomUnityResolver_v1(container);//custom version 1, no anda
            //config.DependencyResolver = new CustomUnityResolver_v2(container);//custom version 1, no anda

            CustomMap(container);

            //y puede ser mucho mas complejo
            //https://msdn.microsoft.com/en-us/library/dn178463(v=pandp.30).aspx
        }

        private static void CustomMap(UnityContainer container)
        {
            container.RegisterType<IEdgarService, EdgarService>();
            container.RegisterType<IAnalystRepository, AnalystRepository>();
            container.RegisterType<AnalystContext, AnalystContext>();
            container.RegisterInstance<HttpConfiguration>(GlobalConfiguration.Configuration);
        }
    }

    

    public class CustomUnityResolver_v1 //: IDependencyResolver
    {
        protected IUnityContainer container;

        public CustomUnityResolver_v1(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new CustomUnityResolver_v1(child);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            container.Dispose();
        }
    }

    public class CustomUnityResolver_v2 //: IDisposable, IDependencyResolver
    {
        protected IUnityContainer Container;

        public CustomUnityResolver_v2(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            Container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return Container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public T GetService<T>()
        {
            try
            {
                var serviceType = typeof(T);
                return (T)Container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return default(T);
            }
        }

        public T GetService<T>(string name)
        {
            try
            {
                var serviceType = typeof(T);
                return (T)Container.Resolve(serviceType, name);
            }
            catch (ResolutionFailedException)
            {
                return default(T);
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return Container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            var child = Container.CreateChildContainer();
            return new CustomUnityResolver_v2(child);
        }

        public void Dispose()
        {
            if (Container == null)
            {
                return;
            }

            Container.Dispose();
            Container = null;
        }

        /*
        protected override void DisposeManagedResources()
        {
            if (Container == null)
            {
                return;
            }

            Container.Dispose();
            Container = null;
        }
        */

    }
}