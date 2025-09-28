// using lotto_api.Data;
// using lotto_api.Services;
using dotnet_webapi_ef.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;


var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("DefaultConnection") ??
"Server=ns1.server-82-26-104-71.da.direct;Port=3306;" +
    "Database=activi89_mb68_66011212090;" +
    "User=activi89_mb68_66011212090;" +
    "Password=KSsZwmmm8CCkVjpGaTUp;" +
    "SslMode=Preferred";

try
{
    using var c = new MySqlConnection(conn);
    await c.OpenAsync();
    Console.WriteLine("MySQL connected OK");
    await c.CloseAsync();
}
catch (Exception ex)
{
    Console.WriteLine("MySQL connect failed: " + ex.Message);
}

builder.Services.AddDbContext<ApplicationDBContext>(opt =>
    opt.UseMySql(conn, ServerVersion.AutoDetect(conn)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.ReferenceLoopHandling =
        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddMemoryCache();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
