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
            // �t�m CookieExtension
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            BaseCookie.Configure(httpContextAccessor);

            // �إ�API�����~�B�z�^��
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
            // �M���B�z API �ШD�����~
            context.Response.ContentType = "application/json";
            var errorResponse = new ResultData();
            switch (context.Response.StatusCode)
            {
                case 400:
                    errorResponse.message = "�ШD���~";
                    break;
                case 401:
                    errorResponse.message = "�����v�A�Э��s�n�J";
                    break;
                case 403:
                    errorResponse.message = "�ڵ��X��";
                    break;
                case 404:
                    errorResponse.message = "�ШD�a�}�X��";
                    break;
                case 500:
                    errorResponse.message = "���A�����~";
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.message = "�s�����~";
                    break;
            }

            await context.Response.WriteAsync(errorResponse.ToSerialize());
        }
    }
}
