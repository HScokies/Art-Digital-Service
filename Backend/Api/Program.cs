using Data;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext();
builder.Services.AddRepositories();
builder.Services.AddAppServices();
builder.Services.AddBackgroundWorkers();
builder.Services.AddJwtAuthentication();
builder.Services.AddEmailService();
builder.Services.AddFilesService();
builder.Services.AddExportService();

builder.Services.AddHttpContextAccessor();

string APP_URL = Environment.GetEnvironmentVariable("APP_URL") ?? "http://localhost:5173";
const string corsPolicyName = "corsPolicy";
builder.Services.AddCors(
    p => p.AddPolicy(
        name: corsPolicyName,
        build =>
        {
            build.AllowCredentials().AllowAnyHeader().AllowAnyMethod().WithOrigins(APP_URL);
        })
    );

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.UseCors(corsPolicyName);

app.Run();
