// using lotto_api.Data;
// using lotto_api.Services;
using dotnet_webapi_ef.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;


var builder = WebApplication.CreateBuilder(args);

var connStr = Environment.GetEnvironmentVariable("MYSQL_URL");




try
{
    using var c = new MySqlConnection(connStr);
    await c.OpenAsync();
    Console.WriteLine("MySQL connected OK");
    await c.CloseAsync();
}
catch (Exception ex)
{
    Console.WriteLine("MySQL connect failed: " + ex.Message);
}

builder.Services.AddDbContext<ApplicationDBContext>(opt =>
    opt.UseMySql(connStr, ServerVersion.AutoDetect(connStr)));
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
