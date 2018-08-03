using Autofac;
using Autofac.Integration.Mvc;
using NHibernate;
using SistemaVendas.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaVendas.App_Start
{
    public class AutoFacConfig
    {
        public static void Config()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            // Register ISessionFactory as Singleton 
            builder.Register(x => FluentySession.Setup()).SingleInstance();
            // Register ISession as instance per web request
            builder.Register(x => x.Resolve<ISessionFactory>().OpenSession())
                .InstancePerRequest();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}