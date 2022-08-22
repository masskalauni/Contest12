using Contest.Data;
using Contest.Web.Constants;
using Contest.Web.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddDbContext<KolpiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
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

//services.AddAuthentication()
//    .AddGoogle(googleOptions =>
//    {
//        googleOptions.ClientId = "1098909678334-j9hktlgauqctf712m8fv08vddm560jot.apps.googleusercontent.com";
//        googleOptions.ClientSecret = "8vrbe318_KysX4yEA-MhD21f";

//    })
//    .AddMicrosoftAccount(microsoftOptions =>
//    {
//        microsoftOptions.ClientId = "780207cf-a4e6-42f5-bb3a-e402e73748c5";
//        microsoftOptions.ClientSecret = "nloNKVKF40]:$gocaUB589{";
//    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/identity/account/accessdenied";
});

//Add policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policy.RequireSuperAdminRole, policy => policy.RequireRole(Role.SuperAdmin));
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
});

//DI Registrations
builder.Services.AddSingleton<IEmailSender, EmailSender>();

var app = builder.Build();

if (app.Environment.EnvironmentName == "Development")
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
//UseStaticFiles before UseRouting
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();

//UseAuthentication and UseAuthorization: after, UseRouting and UseCors, but before UseEndpoints
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapDefaultControllerRoute();
});

app.Run();
