using DemoVenueRental.Global;
using DemoVenueRental.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace DemoVenueRental.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static void ConfigureMiddleware(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 配置 CookieExtension
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            BaseCookie.Configure(httpContextAccessor);

            // 建立API的錯誤處理回應
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>();
                    if (exception != null)
                    {
                        if (context.Request.Path.StartsWithSegments("/api"))
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            await ApiErrorResult(context);
                        }
                        else if (!context.Request.Path.StartsWithSegments("/Home/Error"))
                        {
                            context.Response.Redirect("/Home/Error");
                        }
                    }
                });
            });

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        public static async Task ApiErrorResult(HttpContext context)
        {
            // 專門處理 API 請求的錯誤
            context.Response.ContentType = "application/json";
            var errorResponse = new ResultData();
            switch (context.Response.StatusCode)
            {
                case 400:
                    errorResponse.message = "請求錯誤";
                    break;
                case 401:
                    errorResponse.message = "未授權，請重新登入";
                    break;
                case 403:
                    errorResponse.message = "拒絕訪問";
                    break;
                case 404:
                    errorResponse.message = "請求地址出錯";
                    break;
                case 500:
                    errorResponse.message = "伺服器錯誤";
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.message = "連接錯誤";
                    break;
            }

            await context.Response.WriteAsync(errorResponse.ToSerialize());
        }
    }
}
