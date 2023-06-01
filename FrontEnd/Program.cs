using FrontEnd.Data;
using FrontEnd.Services;
using FrontEnd.Middleware;
using FrontEnd.HealthChecks;
using FrontEnd.Areas.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration
    .GetConnectionString("IdentityDbContextConnection")
    ?? throw new InvalidOperationException("Connection string 'IdentityDbContextConnection' not found.");

var services = builder.Services;
services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlite(connectionString));

// Helath Checks
builder.Services.AddHealthChecks()
    .AddCheck<BackendHealthCheck>("backend")
    .AddDbContextCheck<IdentityDbContext>();

// Identity
services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddClaimsPrincipalFactory<ClaimsPrincipalFactory>();

// Policy and claim
services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
    {
        policy.RequireAuthenticatedUser()
              .RequireIsAdminClaim();
    });
});

// Add services to the container.
services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin", "Admin");
});

// HttpClient
services.AddHttpClient<IApiClient, ApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["serviceUrl"]);
});

// Admin service
services.AddSingleton<IAdminService, AdminService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.UseMiddleware<RequireLoginMiddleware>();

app.MapRazorPages();

app.MapHealthChecks("/health");

app.Run();
