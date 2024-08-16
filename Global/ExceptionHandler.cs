using DemoVenueRental.Models;
using System.Net;

namespace DemoVenueRental.Global
{
    public static class ExceptionHandler
    {
        public static async Task res(HttpContext context, string msg = "錯誤測試")
        {
            // 專門處理 API 請求的錯誤
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = context.Response.StatusCode == 404 ? (int)HttpStatusCode.OK : context.Response.StatusCode;

            var errorResponse = new ResultData<object>() { errorMsg = msg };

            await context.Response.WriteAsync(JsonSerializerService.Serialize(errorResponse));
        }
    }
}
