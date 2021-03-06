using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;

namespace Manzana.MonitoringService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((_, logging) => logging.AddEventLog())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<FileProcesser>();
                    services.AddOptions();
                    services.Configure<ServiceSettings>(hostContext.Configuration.GetSection("ServiceSettings"));

                })
                .UseWindowsService();


    }
}
