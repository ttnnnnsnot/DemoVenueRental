namespace DemoVenueRental.Global
{
    public enum CookieKey
    {
        PlaceId,
        UserId,
    }
    public static class BaseCookie
    {
        private static IHttpContextAccessor? _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static string Get(CookieKey key)
        {
            try
            {
                return _httpContextAccessor?.HttpContext?.Request?.Cookies?[key.ToString()]?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                LoggerService.LogError("CookieExtension.Get", ex);
                return "";
            }
        }

        public static void Set(CookieKey key, string value, int Days = 1) 
        {
            try
            {
                _httpContextAccessor?.HttpContext?.Response.Cookies.Append(key.ToString(), value, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(Days),
                    HttpOnly = true, // 防止XSS攻擊
                    Secure = true, // 防止CSRF攻擊
                });
            }
            catch(Exception ex)
            {
                LoggerService.LogError("CookieExtension.Set", ex);
            }
        }
    }
}
