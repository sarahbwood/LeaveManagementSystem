using LeaveManagementSystem.Application;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
DataServicesRegistration.AddDataServices(builder.Services, builder.Configuration); // register services from the Data layer
ApplicationServicesRegistration.AddApplicationServices(builder.Services); // register services from the Application layer

// logging
builder.Host.UseSerilog((ctx, config) => 
    config
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration) // ctx refers to the build context
);

// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminSupervisorOnly", policy =>
    {
        policy.RequireRole(Roles.Administrator, Roles.Supervisor); // Admin OR Supervisor
    });
});  

builder.Services.AddHttpContextAccessor();

// TODO : Add options ensure that users use their company email address
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<LeaveManagementSystemWebContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware 

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();
