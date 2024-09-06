using System.Security.Cryptography;
using System.Text;

namespace DemoVenueRental.Global
{
    public enum CookieKey
    {
        PlaceId,
    }

    public static class BaseCookie
    {
        private static IHttpContextAccessor? _httpContextAccessor;
        private const string SecretKey = "%$^YHR309rfjkvm545"; // 用於簽名的密鑰

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static bool Delete(CookieKey key)
        {            
            _httpContextAccessor?.HttpContext?.Response.Cookies.Delete(key.ToString());
            return true;
        }

        public static string Get(CookieKey key)
        {
            try
            {
                var signedValue = _httpContextAccessor?.HttpContext?.Request?.Cookies?[key.ToString()];
                if (string.IsNullOrEmpty(signedValue) || !VerifyCookieValue(signedValue))
                {
                    return "";
                }

                return ExtractCookieValue(signedValue);
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
                var signedValue = SignCookieValue(value);
                _httpContextAccessor?.HttpContext?.Response.Cookies.Append(key.ToString(), signedValue, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(Days),
                    HttpOnly = true, // 防止XSS攻擊
                    Secure = true, // 防止CSRF攻擊
                    SameSite = SameSiteMode.Strict // 防止跨站請求偽造
                });
            }
            catch (Exception ex)
            {
                LoggerService.LogError("CookieExtension.Set", ex);
            }
        }

        private static string SignCookieValue(string value)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
                var signedValue = $"{value}.{Convert.ToBase64String(hash)}";
                return signedValue;
            }
        }

        private static bool VerifyCookieValue(string signedValue)
        {
            var parts = signedValue.Split('.');
            if (parts.Length != 2)
            {
                return false;
            }

            var value = parts[0];
            var signature = parts[1];

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
                var expectedSignature = Convert.ToBase64String(hash);
                return signature == expectedSignature;
            }
        }

        private static string ExtractCookieValue(string signedValue)
        {
            var parts = signedValue.Split('.');
            return parts[0];
        }
    }
}
