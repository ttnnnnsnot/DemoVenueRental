using DemoVenueRental.Models;
using DemoVenueRental.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoVenueRental.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly UserService _userService;

        public UserController(System.Data.IDbConnection connection) : base(connection)
        {
            _userService = new UserService(connection);
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

                    return Ok(new ResultData<int>() { errorMsg = firstError ?? "資料有誤" });
                }

                var result = await _userService.Register(model);
                return Ok(result.ToJson());
            }
            catch (Exception ex) {
                HandleErrorRecord("注冊失敗!", ex);
                return Ok(new ResultData<int>() { errorMsg = "注冊失敗!" });
            }
        }

        // POST api/<DefController>/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] Login model)
        {
            return Ok(true);
        }
    }
}
