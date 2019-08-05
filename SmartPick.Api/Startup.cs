using System.Linq;
using CbRedisSentinel;
using EFCore.DbContextFactory.Extensions;
using KafkaConnector.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SmartPick.Api.DependencyInjection.Configuration;
using SmartPick.Api.DependencyInjection.Messaging;
using SmartPick.Api.DependencyInjection.Models;
using SmartPick.Core;
using SmartPick.Core.Interfaces;
using SmartPick.Data;
using SmartPick.Data.CbTote;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace SmartPick.Api
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public Startup(IConfiguration config, ILogger<Startup> logger)
        {
            _config = config;
            _logger = logger;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.BindApplicationConfiguration(_config, "ApplicationConfiguration");

            services.AddMvc().AddJsonOptions(JsonOptions).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            if (_config.GetSection("Redis").Exists() && _config.GetSection("Redis").GetSection("SentinelServers").Exists())
            {
                var licenseKey = _config.GetSection("servicestack:license")?.Value;
                services.AddDistributedRedisCacheSentinel(ConfigureRedis, licenseKey);
            }
            else
                services.AddDistributedMemoryCache();

            services.AddDbContextFactory<CbToteContext>(UseMySql);
            services.AddSingleton<IDataManager, DataManager>();

            services.AddKafkaListener<SelectionListener>().HostKafkaService();

            services.AddScoped<ISmartPickServiceFactory, SmartPickServiceFactory>();
        }

        private static void JsonOptions(MvcJsonOptions options)
        {
            var settings = options.SerializerSettings;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.ContractResolver = new DefaultContractResolver{NamingStrategy = new SnakeCaseNamingStrategy()};
        }

        private void UseMySql(DbContextOptionsBuilder opt)
        {
            var sqlConnection = new MySqlConnectionStringBuilder(_config.GetConnectionString("cbtote"))
            {
                PersistSecurityInfo = true,
                AllowZeroDateTime = true,
                UserID = _config.GetSection("CbTote")["Username"],
                Password = _config.GetSection("CbTote")["Password"]
            };
            _logger.LogInformation($"Connection string: {sqlConnection}");
            opt.UseMySQL(sqlConnection.ConnectionString);
        }

        private void ConfigureRedis(RedisSentinelOptions opt)
        {
            var redisConfig = new RedisConfiguration();
            _config.Bind("Redis", redisConfig);
            if (redisConfig.SentinelServers == null) return;

            opt.Hosts = redisConfig.SentinelServers.Select(s => $"{s.Host}:{s.Port}").ToArray();
            opt.AuthPass = redisConfig.Password;
            opt.MasterGroup = redisConfig.MasterName;
            opt.InstanceName = "smart-pick";
            opt.ScanForOtherSentinels = false;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // When the app runs in the Development environment:
                //   Use the Developer Exception Page to report app runtime errors.
                //   Use the Database Error Page to report database runtime errors.
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                // Use the HTTP Strict Transport Security Protocol (HSTS) Middleware.
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
