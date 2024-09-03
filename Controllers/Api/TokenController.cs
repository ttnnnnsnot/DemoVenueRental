using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace DemoVenueRental.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public TokenController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        [HttpGet]
        public IActionResult SetAntiforgeryToken()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            var tokenValue = tokens.RequestToken ?? "";
            HttpContext.Response.Cookies.Append(tokens.HeaderName, tokenValue, new CookieOptions
            {
                HttpOnly = false,
                SameSite = SameSiteMode.Strict
            });

            return Ok(true);
        }
    }
}
