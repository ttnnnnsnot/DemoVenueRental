using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using DemoVenueRental.Global;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// 註冊服務
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddControllersWithViews();

// 設定 Configuration
AppSettings.Configuration = builder.Configuration;

// 設置 Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(AppSettings.Configuration)
    .CreateLogger();

// 設置 Serilog
builder.Host.UseSerilog();

// 設定 MultipartBodyLengthLimit
builder.Services.Configure<FormOptions>(options =>
{
    // 單一次表單資料使用的預設是128MB，在此設定1MB
    options.MultipartBodyLengthLimit = 1024 * 1024 * 10;
    // 表單資料暫存到記憶體前的閾值，超過此大小就會使用暫存檔案
    options.MemoryBufferThreshold = 1024 * 1024 * 1;
});

// 設定數據保護的密鑰環持久化
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(AppSettings.CookieKey)) // 請根據實際情況設置目錄
    .SetApplicationName(AppSettings.ApplicationName);


// 設定session
builder.Services.AddRazorPages().AddSessionStateTempDataProvider();
builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();
builder.Services.AddSession();

// 設置cookies登入驗証
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Login/AccessDenied";
        options.LoginPath = "/Login/Index";
    });

// 設定AddAntiforgery驗証 防止跨網站偽造要求 (XSRF/CSRF) 攻擊
builder.Services.AddAntiforgery(options =>
{
    // Set Cookie properties using CookieBuilder properties†.
    options.FormFieldName = "AntiforgeryFieldname";
    options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
    options.SuppressXFrameOptionsHeader = false;
});

// 全域設定
//使用 AutoValidateAntiforgeryToken 屬性，而非廣泛套用 ValidateAntiForgeryToken 屬性，然後用 IgnoreAntiforgeryToken 屬性將其覆寫。 此屬性的運作方式與 ValidateAntiForgeryToken 屬性相同，不同之處在於它在處理使用下列 HTTP 方法提出的要求時不需要權杖：GET、HEAD、OPTIONS、TRACE
// IgnoreAntiforgeryToken 篩選條件可用來消除指定動作的防偽權杖需求
// ValidateAntiForgeryToken 除非要求包含有效的防偽權杖，否則對套用此篩選動作的要求會遭到封鎖
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

var app = builder.Build();

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
                // 專門處理 API 請求的錯誤
                await ExceptionHandler.res(context);
            }
            else
            {
                // 處理非 API 請求的錯誤，例如重定向到錯誤頁面
                context.Response.Redirect("/Home/Error");
            }
        }
    });
});

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 設置session
app.UseSession();

// 設置cookies登入驗証
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
