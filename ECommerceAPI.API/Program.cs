using Serilog;
using ECommerceAPI.API.Extensions;
using ECommerceAPI.API.Middleware;
using ECommerceAPI.API.HealthChecks;
using ECommerceAPI.Application.Extensions;
using ECommerceAPI.Infrastructure.Extensions;
using ECommerceAPI.Infrastructure.Identity;
using ECommerceAPI.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq(builder.Configuration["Serilog:SeqServerUrl"] ?? "http://localhost:5341")
    .CreateLogger();

builder.Host.UseSerilog();

// Application Services
builder.Services.AddApplicationServices();

// Infrastructure Services
builder.Services.AddInfrastructureServices(builder.Configuration);

// Identity Services
builder.Services.AddIdentityServices(builder.Configuration);

// API Services
builder.Services.AddCorsPolicy();
builder.Services.AddRateLimitingPolicies();
builder.Services.AddApiVersioningAndSwagger();
builder.Services.AddResponseCaching();
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!)
    .AddCheck<CloudinaryHealthCheck>("cloudinary");

var app = builder.Build();

// Seed Admin User
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var configuration = services.GetRequiredService<IConfiguration>();
    await IdentitySeedService.SeedAsync(userManager, roleManager, configuration);
}

// Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();

// Swagger (Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Pipeline
app.UseSerilogRequestLogging();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();
app.UseRateLimiter();

app.MapControllers();

app.Run();
