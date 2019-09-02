using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kolpi.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.UI.Services;
using Kolpi.Web.Services;
using System;

namespace Kolpi.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
           
            services.AddDbContext<KolpiDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MsSqlConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                // Custom Password settings.
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 1;
            })
            .AddEntityFrameworkStores<KolpiDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = "1098909678334-j9hktlgauqctf712m8fv08vddm560jot.apps.googleusercontent.com";
                    googleOptions.ClientSecret = "8vrbe318_KysX4yEA-MhD21f";

                })
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = "780207cf-a4e6-42f5-bb3a-e402e73748c5";
                    microsoftOptions.ClientSecret = "nloNKVKF40]:$gocaUB589{";
                });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                    options.Conventions.AuthorizeAreaFolder("Identity", "/AccountAdmin");
                });

            services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = $"/Identity/Account/Login";
                    options.LogoutPath = $"/Identity/Account/Logout";
                });

            //DI Registrations
            services.AddSingleton<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{identifier?}");
            });
        }

        //private async Task CreateRoles(IServiceProvider serviceProvider)
        //{
        //    //initializing custom roles and admin user
        //    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        //    string[] roleNames = { Role.Admin, Role.Committee, Role.Participant };

        //    if (!(await roleManager.Roles.AnyAsync()))
        //    {
        //        foreach (var roleName in roleNames)
        //        {
        //            var roleExist = await roleManager.RoleExistsAsync(roleName);
        //            if (!roleExist)
        //            {
        //                await roleManager.CreateAsync(new IdentityRole(roleName));
        //            }
        //        }
        //    }

        //    List<string> admins = Configuration.GetSection("AppSettings:Admins").Get<List<string>>();
        //    foreach (var admin in admins)
        //    {
        //        var user = await userManager.FindByNameAsync(admin);

        //        //user has not registered yet, go for other one
        //        if (user == null)
        //            continue;

        //        //Already in admin role, go ahead
        //        if (await userManager.IsInRoleAsync(user, Role.Admin))
        //            continue;

        //        await userManager.AddToRoleAsync(user, Role.Admin);
        //    }
        //}
    }
}
