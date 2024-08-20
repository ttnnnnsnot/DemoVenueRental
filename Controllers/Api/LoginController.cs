using DemoVenueRental.Global;
using DemoVenueRental.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace DemoVenueRental.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public LoginController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        // POST api/<DefController>
        [HttpPost]
        public string Post([FromBody] Login model)
        {
            return JsonSerializerService.Serialize(ModelState.IsValid.ToString());
        }
    }
}
