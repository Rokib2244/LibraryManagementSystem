using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using LibraryManagement.Training;
//using Fluent.Infrastructure.FluentModel;
//using Fluent.Infrastructure.FluentStartup;
using LibraryManagement.Training.Contexts;
using LibraryManagement.Web;
using LibraryManagement.Web.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;
using ApplicationDbContext = LibraryManagement.Web.Data.ApplicationDbContext;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration));

// builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
//     .AddJsonFile("appsettings.json", false, true)
//     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
//     .AddEnvironmentVariables()
//     .Build();

try
{
    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var assemblyName = Assembly.GetExecutingAssembly().FullName;

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new WebModule(connectionString,
            assemblyName, builder.Configuration, builder.Environment));
        containerBuilder.RegisterModule(new TrainingModule(connectionString,
            assemblyName));
    });

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString, m => m.MigrationsAssembly(assemblyName)));
    builder.Services.AddDbContext<TrainingContext>(options =>
                options.UseSqlServer(connectionString,m => m.MigrationsAssembly(assemblyName)));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>();

    // builder.Services
    //.AddEntityFrameworkStores<LibraryManagement.Web.Data.ApplicationDbContext>()
    // .AddEntityFrameworkStores<LibraryManagement.Web.Data.ApplicationDbContext>()
    // .AddUserManager<ApplicationUserManager>()   
    // .AddSignInManager<ApplicationSignInManager>()
    // .AddDefaultTokenProviders();
    //.AddRoleManager<ApplicationRoleManager>()
    //.AddIdentity<ApplicationUser, ApplicationRole>()

    builder.Services.AddAuthentication()
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.LoginPath = new PathString("/Account/Login");
            options.AccessDeniedPath = new PathString("/Account/Login");
            options.LogoutPath = new PathString("/Account/Logout");
            options.Cookie.Name = "FirstDemoPortal.Identity";
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromHours(1);
        });
        //.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
        //{
        //    x.RequireHttpsMetadata = false;
        //    x.SaveToken = true;
        //    x.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        //        ValidAudience = builder.Configuration["Jwt:Audience"],
        //    };
        //});

    builder.Services.Configure<IdentityOptions>(options =>
    {
        // Password settings.
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 0;

        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // User settings.
        options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("CourseManagementPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole("Admin");
            policy.RequireRole("Teacher");
        });

        options.AddPolicy("CourseViewPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("ViewCourse", "true");
        });
        options.AddPolicy("CourseCreatePolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("CreateCourse", "true");
        });

        //options.AddPolicy("CourseViewRequirementPolicy", policy =>
        //{
        //    policy.RequireAuthenticatedUser();
        //    policy.Requirements.Add(new CourseViewRequirement());
        //});

        //options.AddPolicy("ApiRequirementPolicy", policy =>
        //{
        //    policy.AuthenticationSchemes.Clear();
        //    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        //    policy.RequireAuthenticatedUser();
        //    policy.Requirements.Add(new ApiRequirement());
        //});

        //options.DefaultPolicy = options.GetPolicy("CourseViewRequirementPolicy");
    });

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    //builder.Services.AddSingleton<IAuthorizationHandler, CourseViewRequirementHandler>();
    //builder.Services.AddSingleton<IAuthorizationHandler, ApiRequirementHandler>();

    var siteSettings = builder.Configuration.GetSection("SiteSettings");

    //builder.Services.Configure<Smtp>(builder.Configuration.GetSection("Smtp"));
    //builder.Services.Configure<SiteSettings>(siteSettings);

    builder.Services.AddControllersWithViews();

    var useUrl = siteSettings.GetValue<bool>("UseUrlSettings");

    if (useUrl)
        builder.WebHost.UseUrls("http://*:80");

    var app = builder.Build();

    Log.Information("Application Starting.");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseSession();

    app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.MigrateAsync().Wait();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddControllersWithViews();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseMigrationsEndPoint();
//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

//app.Run();
