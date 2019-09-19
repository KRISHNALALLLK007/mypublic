using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sample.Demo.DataService;
using Sample.Shared.Utilities.Constants;
using Sample.Shared.Utilities.Logging;
using Serilog;

namespace Sample.Demo.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider; 
                var loggerFactory = service.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(typeof(Program));
                InitializeCustomLogHandler(service);
                try
                {
                    logger.LogInformation("Application is starting");
                    var context = service.GetRequiredService<DemoDbContext>();
                    context.Database.Migrate();
                    logger.LogInformation("DB migrations are success");
                    host.Run();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex," : Exception");
                }
            } 
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().UseUrls("http://localhost:4000");
        }

        private static void InitializeCustomLogHandler(IServiceProvider service)
        {
            var loghandler = service.GetRequiredService<ILogHandler>();
            loghandler.Initialize();
        }
    }
}
