using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mvc.Data;
using NToastNotify;
using Microsoft.AspNetCore.Mvc.Razor;

using Microsoft.AspNetCore.Localization;
using System.Globalization;
using mvc;
using SQLitePCL;
using mvc.Data.SeedDataBase;
using Microsoft.AspNetCore.Identity.UI.Services;
using MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //options.UseSqlite(connectionString)
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(
    options =>{ 
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 4;
        options.Password.RequiredUniqueChars = 4;
    
    }
    
    
    )
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(
    options => {
        options.AddPolicy(MVCConstants.POLICIES.APP_POLICY.NAME, policy => policy.RequireRole(MVCConstants.POLICIES.APP_POLICY.POLICY_ROLES));
        options.AddPolicy(MVCConstants.POLICIES.APP_POLICY_EDITABLE_CRUD.NAME, policy => policy.RequireRole(MVCConstants.POLICIES.APP_POLICY_EDITABLE_CRUD.POLICY_ROLES));
        options.AddPolicy(MVCConstants.POLICIES.APP_POLICY_ADMIN.NAME, policy => policy.RequireRole(MVCConstants.POLICIES.APP_POLICY_ADMIN.POLICY_ROLES));
        
    }



);


builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews();
        //.AddRazorRuntimeCompilation();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Localization configuration
const string defaultCulture = "pt";

CultureInfo enCI = new CultureInfo(defaultCulture);

var supportedCultures = new[]
{
    enCI,
    new CultureInfo("pt"),
    new CultureInfo("es")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(defaultCulture);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});


builder.Services
    .AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    // è necessario que exista a data notation no modelo senão não vai traduzir
    .AddDataAnnotationsLocalization(options =>
    {
            options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(Resource)) ;
    })
    .AddNToastNotifyToastr(new ToastrOptions()
    {
        ProgressBar = true,
        PositionClass = ToastPositions.TopRight
    });

builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

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

app.UseAuthorization();

app.UseRequestLocalization(
    app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value
);

app.UseNToastNotify();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


SeedDB(); // Executed every time the application restarts

app.Run();

void SeedDB()
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    SeedDatabase.Seed(dbContext, userManager, roleManager);
}