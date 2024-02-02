using Data;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext();
builder.Services.AddRepositories();
builder.Services.AddAppServices();
builder.Services.AddBackgroundWorkers();
builder.Services.AddJwtAuthentication();
builder.Services.AddEmailService();
builder.Services.AddFilesService();
builder.Services.AddExportService();

builder.Services.AddHttpContextAccessor();

const string corsPolicyName = "corsPolicy";
builder.Services.AddCors(
    p => p.AddPolicy(
        name: corsPolicyName,
        build =>
        {
            build.AllowCredentials().AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5173", "http://localhost:5174");
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
