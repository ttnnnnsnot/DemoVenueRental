namespace DemoVenueRental.Global
{
    public static class AppSettings
    {
        public static IConfiguration Configuration { get; set; } =
            new ConfigurationBuilder().Build();

        public static string Serilog => Configuration["Serilog"] ?? "";
        public static string MsSqlConnect =>
            Configuration["MsSqlConnect"] ?? "";

        public static string CookieKey => Configuration["CookieKey"] ?? @"D:\Web\WebKey\DemoVenueRental";

        //設定Cookie驗証需要的設定
        public const string ApplicationName = "DemoVenueRental";
        //上傳檔案的路徑
        public const string FileUserUrl = "wwwroot/userfiles";
        //單個上傳檔案限制為20個
        public const int FileCount = 20;
        //單個上傳檔案限制為1MB (1024 * 1024 * 1
        public const long FileSizeLimit = 1024 * 1024 * 1;
        //限制檔案上傳格式
        public const string FileExtensions = "jpg,jpeg,png,gif";
    }
}
