using Autofac;
using Autofac.Integration.Wcf;
using log4net;
using Manzana.DAL.Interfaces;
using Manzana.DAL.Repositories;
using ManzanaWCF.Domain;
using ManzanaWCF.Services;
using System;
using System.Configuration;
using System.Reflection;
using System.Web;

namespace ManzanaWCF
{
    public class Global : HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Start(object sender, EventArgs e)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ChequeService>();

            if(Enum.TryParse(ConfigurationManager.AppSettings["Repository"], out Repositories repository))
            {
                if(repository == Repositories.Fake)
                {
                    builder.RegisterType<ChequeRepositoryFakeDb>().As<IChequeRepository>();
                }
                if(repository == Repositories.Database)
                {
                    builder.RegisterType<ChequeRepository>().As<IChequeRepository>();
                }
            }
            else
            {
                Log.Error($"Repository type in configuration not found or incorrected");
            }
            var container = builder.Build();

            AutofacHostFactory.Container = container;

            Log.Info($"The application is running {DateTime.Now:MM/dd/yyyy HH:mm:ss}");
        }

        protected void Application_Stop(object sender, EventArgs e)
        {
            Log.Info($"The application is stopping {DateTime.Now:MM/dd/yyyy HH:mm:ss}");
        }
    }
}