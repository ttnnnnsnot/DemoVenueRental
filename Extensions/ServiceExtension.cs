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
using Microsoft.AspNetCore.Authentication;

namespace DemoVenueRental.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // ���U�A��
            services.AddControllers();

            // �K�[ IHttpContextAccessor �A��
            services.AddHttpContextAccessor();

            // ���U�A��
            services.AddScoped<IDbConnection>(sp =>
            {
                var connection = new SqlConnection(AppSettings.MsSqlConnect);
                connection.Open();  // �b���B�}�ҳs�u
                return connection;
            });

            services.AddScoped<IUserData, UserData>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IDefData, DefData>();
            services.AddScoped<IDefService, DefService>();

            services.AddScoped<IPlaceData, PlaceData>();
            services.AddScoped<IPlaceService, PlaceService>();

            // ����]�w
            services.AddControllersWithViews(options =>
            {
                // �K�[������������
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            // �]�m Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            // �]�w MultipartBodyLengthLimit
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 1024 * 1024 * 10;
                options.MemoryBufferThreshold = 1024 * 1024 * 1;
            });

            // �]�w�ƾګO�@���K�_�����[��
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(AppSettings.CookieKey))
                .SetApplicationName(AppSettings.ApplicationName);

            // �]�wsession
            services.AddSession();

            // �]�mcookies�n�J���
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Home/AccessDenied";
                    options.LoginPath = "/Home/NoLogined";
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context => RedirectToLogin(context),
                        OnRedirectToAccessDenied = context => RedirectToLogin(context)
                    };
                });


            // �]�wAddAntiforgery���
            services.AddAntiforgery();
            //// �]�wAddAntiforgery���
            //services.AddAntiforgery(options =>
            //{
            //    options.FormFieldName = "AntiforgeryToken";
            //    options.HeaderName = "X-CSRF-TOKEN";
            //});
        }

        private static Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            var uri = new Uri(context.RedirectUri);
            var path = uri.GetLeftPart(UriPartial.Path); // �u�O�d���|����

            context.Response.Redirect(path);
            return Task.CompletedTask;
        }
    }
}
