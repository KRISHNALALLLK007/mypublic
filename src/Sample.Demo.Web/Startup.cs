using System;
using System.IO;
using Elastic.Apm.NetCoreAll;
using GuidanceAudit.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Demo.DataService;
using Swashbuckle.AspNetCore.Swagger;

namespace Sample.Demo.Web
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        public Startup()
        {
            Configuration = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json")
                     .AddJsonFile("serilogconfig.json")
                     .Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddMvc();
            services.RegisterAllServices();
            var connectionString = Configuration.GetValue<string>("DemoDB");
            services.AddDbContext<DemoDbContext>(options =>
                options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Sample Demo MicroService API",
                    Description = "Demo MicroService API",
                    TermsOfService = "None"
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "Sample.Demo.Web.xml");
                c.IncludeXmlComments(xmlPath);
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseCors(builder => builder
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowCredentials());
            app.UseAllElasticApm(Configuration);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // UseAuthentication adds the authentication middleware to the pipeline so authentication will be performed automatically on every call into the host.
            app.UseAuthentication();
            app.UseMvc();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample Demo MicroService API v1");
                c.RoutePrefix = string.Empty;
            });
            app.UseHttpStatusCodeExceptionMiddleware();
        }
    }
}
