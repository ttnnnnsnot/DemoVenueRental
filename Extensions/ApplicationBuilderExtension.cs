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

        public static async Task ApiErrorResult(HttpContext context, string msg = "錯誤測試")
        {
            // 專門處理 API 請求的錯誤
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = context.Response.StatusCode == 404 ? (int)HttpStatusCode.OK : context.Response.StatusCode;

            var errorResponse = new ResultData() { message = msg };

            await context.Response.WriteAsync(errorResponse.ToSerialize());
        }
    }
}
