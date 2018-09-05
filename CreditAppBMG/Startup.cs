﻿using AutoMapper;
using CreditAppBMG.CustomAttributes;
using CreditAppBMG.Entities;
using CreditAppBMG.ViewModels;
//using CreditAppBMG.DAL.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreditAppBMG
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //var connection = @"Server=.;Database=CreditApp;Trusted_Connection=True;";
            //services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddDbContext<CreditAppContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<States, StatesEntity>();

                cfg.CreateMap<CreditDataEntity, CreditData>();
                //.ForSourceMember(x => x.CreditDataFiles, opt => opt.Ignore())
                //.IgnoreAllPropertiesWithAnInaccessibleSetter()
                //.IgnoreAllSourcePropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<CreditData, CreditDataEntity>();
                //.ForMember(x => x.CreditDataFiles, opt => opt.Ignore())
                
                cfg.AddGlobalIgnore("CreditDataFiles");

                cfg.CreateMap<USZipCodes, ZipCodesUSEntity>().ReverseMap();

                cfg.CreateMap<Distributor, DistributorEntity>()
                //.ForMember(x=>x.Id, opt=>opt.Ignore())
                .ReverseMap();
                
            });

            var mapper = config.CreateMapper();

            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc().AddViewOptions(options => options.HtmlHelperOptions.ClientValidationEnabled = true);
            services.AddMvc().AddMvcOptions(opts => {

                opts.ModelMetadataDetailsProviders.Add(new CustomMetadataProvider());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
