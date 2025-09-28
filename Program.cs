using dotnet_webapi_ef.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// ใช้ ENV ตรง ๆ จาก Railway
var connStr = Environment.GetEnvironmentVariable("MYSQL_URL");
if (string.IsNullOrWhiteSpace(connStr))
    throw new InvalidOperationException("MYSQL_URL is not set. Configure it in Railway → Variables.");

// ทดสอบเชื่อมต่อสั้น ๆ (แค่ log)


// Register DbContext (Pomelo + MySqlConnector)
builder.Services.AddDbContext<ApplicationDBContext>(opt =>
    opt.UseMySql(connStr, ServerVersion.AutoDetect(connStr)));

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
