using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using DemoVenueRental.Global;
using System.Data.SqlClient;
using System.Data;
using DemoVenueRental.Sql;
using DemoVenueRental.Services;
using Microsoft.AspNetCore.DataProtection;

namespace DemoVenueRental.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 註冊服務
            services.AddControllers();

            // 註冊服務
            services.AddScoped<IDbConnection>(sp =>
            {
                var connection = new SqlConnection(AppSettings.MsSqlConnect);
                connection.Open();  // 在此處開啟連線
                return connection;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUserData, UserData>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IDefData, DefData>();
            services.AddScoped<IDefService, DefService>();

            // 全域設定
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).AddSessionStateTempDataProvider();

            // 設置 Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            // 設定 MultipartBodyLengthLimit
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 1024 * 1024 * 10;
                options.MemoryBufferThreshold = 1024 * 1024 * 1;
            });

            // 設定數據保護的密鑰環持久化
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(AppSettings.CookieKey))
                .SetApplicationName(AppSettings.ApplicationName);

            // 設定session
            services.AddSession();

            // 設置cookies登入驗証
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Login/AccessDenied";
                    options.LoginPath = "/Login/Index";
                });

            // 設定AddAntiforgery驗証
            services.AddAntiforgery(options =>
            {
                options.FormFieldName = "AntiforgeryToken";
                options.HeaderName = "X-CSRF-TOKEN";
            });
        }
    }
}
