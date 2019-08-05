using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace SmartPick.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("Starting Main");
                CreateWebHostBuilder(args).Build().Run(); 
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder<Startup>(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var root = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
                    config.AddJsonFile(root + "opt/dotnet/smart_pick/shared/appsettings.infrastructure.json", false, true); //this will be managed by infrastructure team
                    
                })
                .ConfigureLogging(logging =>
                {//the default builder will load configurations from all default providers and use startup type
                    logging.ClearProviders();
                })
                .UseNLog()
                .UseSentry();  //https://github.com/getsentry/sentry-dotnet
    }
}
