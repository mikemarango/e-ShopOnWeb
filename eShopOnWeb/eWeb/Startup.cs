using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using eWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using eWeb.Services;
using Infrastructure.FileSystem;
using ApplicationCore.Interfaces;
using Infrastructure.Logging;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace eWeb
{
    public class Startup
    {
        private IServiceCollection _services;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CatalogContext>(c =>
            {
                try
                {
                    c.UseSqlServer(Configuration.GetConnectionString("CatalogConnection"));
                    c.ConfigureWarnings(wb =>
                    {
                        wb.Log(RelationalEventId.QueryClientEvaluationWarning);
                    });
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            });

            // Add Identity DbContext
            services.AddDbContext<AppIdentityDbContext>(o =>
                o.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddMemoryCache();

            services.AddScoped<ICatalogService, CachedCatalogService>();

            services.AddScoped<CatalogService>();

            services.Configure<CatalogSettings>(Configuration);

            services.AddSingleton<IImageService, LocalFileImageService>();

            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            services.AddMvc();

            _services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.CreateLogger("Logging");

            loggerFactory.CreateLogger("Console");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();

                app.Map("/allservices", builder => builder.Run(async context =>
                {
                    var sb = new StringBuilder();
                    sb.Append("<h1>All Services</h1>");
                    sb.Append("<table><thead>");
                    sb.Append("<tr><th>Type</th><th>Lifetime</th><th>Instance</th></tr>");
                    sb.Append("</thead><tbody>");

                    foreach (var svc in _services)
                    {
                        sb.Append("<tr>");
                        sb.Append($"<td>{svc.ServiceType.FullName}</td>");
                        sb.Append($"<td>{svc.Lifetime}</td>");
                        sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("</tbody></table>");
                    await context.Response.WriteAsync(sb.ToString());
                }));
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // SeedData
            CatalogContextSeed.SeedAsync(app, loggerFactory).Wait();
        }
    }
}
