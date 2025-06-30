using HaarpTech_Licenta.Data;
using HaarpTech_Licenta.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HaarpTech_Licenta.Enums;
using HaarpTech_Licenta.Repository;
using HaarpTech_Licenta.Utils;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
GlobalizationUtils.ConfigureGlobalizationUtils();
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
       .UseSqlServer(connectionString)
       .EnableDetailedErrors()
);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IDatabaseConnection, DatabaseConnection>();
builder.Services.AddScoped<ISedintaRepository, SedintaRepository>();
builder.Services.AddScoped<IRaportCerintaRepository, RaportCerintaRepository>();
builder.Services.AddScoped<ICerereOfertaRepository, CerereOfertaRepository>();
builder.Services.AddScoped<ICerintaProdusRepository, CerintaProdusRepository>();
builder.Services.AddScoped<IStatusTichetRepository, StatusTichetRepository>();
builder.Services.AddScoped<IAngajatRepository, AngajatRepository>();
builder.Services.AddScoped<INotaComandaRepository, NotaComandaRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IClientDataRepository, ClientDataRepository>();
builder.Services.AddScoped<IFacturaRepository, FacturaRepository>();

/// microsoft docs  https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-9.0&tabs=visual-studio
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+!";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
}); 
var app = builder.Build();

// Rotativa configuration
// --- 2. Setup Rotativa pointing to the project-root “Rotativa” folder ---
// aici spunem Rotativa unde să caute wkhtmltopdf.exe
var rotativaFolder = Path.Combine(app.Environment.ContentRootPath, "Rotativa");
RotativaConfiguration.Setup(rotativaFolder, string.Empty);

// --- 3. Creare automată a folderului NoteDeComandaFiles ---
// astfel încât, înainte să răspundem la orice cerere, folderul există
var noteFolder = Path.Combine(app.Environment.ContentRootPath, "NoteDeComandaFiles");
Directory.CreateDirectory(noteFolder);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();  
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

    using (var scope = app.Services.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = Enum.GetNames(typeof(Role)); 

        foreach (var role in roles)
        {
             if (!await userManager.RoleExistsAsync(role))
            {
                await userManager.CreateAsync(new IdentityRole(role));
            }
        }
    }


    using (var scope = app.Services.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string email = "admin@admin.com";
        string password = "Parola123!@";

        if(await userManager.FindByEmailAsync(email) == null)
        {
            var user = new ApplicationUser();
            user.UserName = email;
            user.Email = email;
            user.FirstName = "adminFname";
            user.LastName = "adminLname";
            user.Country = "Romania";
            user.CompanyName = "Haarp Tech";
            user.CompanyDescription = "Haarp Teche este o companie specializata in dezvoltare software";
            user.DateCreated = DateTime.Now;
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;

            await userManager.CreateAsync(user,password);
            await userManager.AddToRoleAsync(user, Role.Admin.ToString());
        }
    }
app.Run();
