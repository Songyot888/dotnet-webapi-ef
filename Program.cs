// using dotnet_webapi_ef.Data;
using dotnet_webapi_ef.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// ใช้ ENV ตรง ๆ จาก Railway
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");
// ทดสอบเชื่อมต่อสั้น ๆ (แค่ log)


// Register DbContext (Pomelo + MySqlConnector)
builder.Services.AddDbContext<RailwayContext>(opt =>
    opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers().AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.ReferenceLoopHandling =
        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// (ถ้ามี Auth/CORS ค่อยใส่ UseAuthentication/UseAuthorization/UseCors ที่นี่)
app.MapControllers();

app.Run();
