using DemoVenueRental.Extensions;
using DemoVenueRental.Models;
using DemoVenueRental.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoVenueRental.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<DefController>/Logout
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            _userService.Logout();
            return Ok(true);
        }

        // GET: api/<DefController>/IsLogged
        [HttpGet("IsLoggedIn")]
        public IActionResult IsLoggedIn()
        {
            return Ok(_userService.IsLogged());
        }

        // GET: api/<DefController>/CheckEmail
        [HttpGet("CheckEmail")]
        public async Task<IActionResult> CheckEmail([FromQuery] string? Email)
        {
            try
            {
                var emailAddress = Request.Query["Register.Email"].FirstOrDefault()
                                ?? Email;

                if(string.IsNullOrEmpty(emailAddress))
                    return Ok(false);

                return Ok(!await _userService.IsEmailExist(emailAddress));
            }
            catch(Exception ex)
            {
                HandleErrorRecord("CheckEmail", ex);
                return Ok(false);
            }
        }

        // POST api/<DefController>/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var firstError = ModelState.Values.SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage)
                                      .FirstOrDefault();

                    return Ok(new ResultData<int>() { message = firstError ?? "資料有誤" });
                }

                var result = await _userService.Register(model);
                return Ok(result.ToSerialize());
            }
            catch (Exception ex) {
                return Ok(HandleError("註冊失敗!", ex));
            }
        }

        // POST api/<DefController>/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var firstError = ModelState.Values.SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage)
                                      .FirstOrDefault();

                    return Ok(new ResultData<int>() { message = firstError ?? "資料有誤" });
                }

                var result = await _userService.Login(model);
                return Ok(result.ToSerialize());
            }
            catch (Exception ex)
            {
                return Ok(HandleError("登入失敗!", ex));
            }
        }
    }
}
