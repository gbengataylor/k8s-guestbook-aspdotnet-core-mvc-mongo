using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GuestbookWebApi.Interfaces;
using GuestbookWebApi.Data;
using GuestbookWebApi.Model;

namespace GuestbookWebApi
{
    public class Startup
    {
        
        private readonly IHostingEnvironment _currentEnvironment;
        private readonly ILogger _logger;

        public Startup(IHostingEnvironment env, ILoggerFactory logger)
        {
            
            _currentEnvironment = env;
            _logger = logger.CreateLogger("Startup");
            
            Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");

            var builder = new ConfigurationBuilder()
                .SetBasePath(_currentEnvironment.ContentRootPath)
                .AddJsonFile("exe/appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"exe/appsettings.{_currentEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            
            Configuration = builder.Build();

            if(_currentEnvironment.IsEnvironment("Kubernetes")){

                var connectionStringPath = "exe/dbsecrets/connectionString";

                Console.WriteLine("Environment == Kubernetes");
                Console.WriteLine($"Check for the secret connection at {connectionStringPath}");

                if(System.IO.File.Exists(connectionStringPath)){

                    Console.WriteLine("Secret found! Will overwrite the connection string accordingly");
                    var secretConnectionString = System.IO.File.ReadAllText("exe/dbsecrets/connectionString");
                    Configuration["MongoConnection:ConnectionString"] = secretConnectionString;

                }else{

                    Console.WriteLine("No secret found. Will not overwrite the connection string in the configuration");

                }

            }
            
            Console.WriteLine($"MongoConnection:ConnectionString: {Configuration["MongoConnection:ConnectionString"]}");
            Console.WriteLine($"MongoConnection:Database: {Configuration["MongoConnection:Database"]}");
            Console.WriteLine();           

        }

        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddMvc();

            if(_currentEnvironment.IsEnvironment("Kubernetes")){

                services.Configure<Settings>(options =>
                {
                    options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                    options.Database = Configuration.GetSection("MongoConnection:Database").Value;
                });

            }else{

                services.Configure<Settings>(options =>
                {
                    options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                    options.Database = Configuration.GetSection("MongoConnection:Database").Value;
                });

            }
            _logger.LogInformation("Setting ConnectionString {ConnectionString}", Configuration.GetSection("MongoConnection:ConnectionString").Value);    
            _logger.LogInformation("Setting Database {Database}", Configuration.GetSection("MongoConnection:Database").Value);    

            services.AddTransient<IGuestRepository, GuestRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
