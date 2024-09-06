using DemoVenueRental.Extensions;
using DemoVenueRental.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoVenueRental.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefController : BaseController
    {
        private readonly IDefService _defService;
        public DefController(IDefService defService)
        {
            _defService = defService;
        }

        // GET api/<DefController>/typeName
        [HttpGet("{typeName}")]
        public async Task<IActionResult> Get(string typeName)
        {
            try
            {
                return Ok(await _defService.GetDef(typeName).ToSerializeAsync());
            }
            catch(Exception ex)
            {
                return Ok(HandleError("取得資訊錯誤!", ex));
            }
        }

    }
}
