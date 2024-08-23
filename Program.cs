using DemoVenueRental.Extensions;
using DemoVenueRental.Global;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 設置 Serilog
builder.Host.UseSerilog();

// 設定 Configuration
AppSettings.Configuration = builder.Configuration;

// 註冊服務
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

// 配置中間件
app.ConfigureMiddleware(app.Environment);

app.Run();