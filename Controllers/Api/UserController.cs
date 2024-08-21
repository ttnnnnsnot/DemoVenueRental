using DemoVenueRental.Global;
using DemoVenueRental.Models;
using DemoVenueRental.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace DemoVenueRental.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAntiforgery _antiforgery;
        private readonly UserService _userService = new UserService();

        public UserController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        // GET: api/<DefController>/CheckEmail
        [HttpGet("CheckEmail")]
        public IActionResult CheckEmail([FromQuery] string? Email)
        {
            try
            {
                var emailAddress = Request.Query["Register.Email"].FirstOrDefault()
                                ?? Email;

                if(string.IsNullOrEmpty(emailAddress))
                    return Ok(false);

                return Ok(!_userService.IsEmailExist(emailAddress, 1));
            }
            catch(Exception ex)
            {
                _userService.HandleErrorRecord("CheckEmail", ex);
                return Ok(false);
            }
        }

        // POST api/<DefController>/Register
        [HttpPost("Register")]
        public IActionResult Register([FromBody] Register model)
        {
            return Ok(true);
        }

        // POST api/<DefController>/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] Login model)
        {
            return Ok(true);
        }
    }
}
