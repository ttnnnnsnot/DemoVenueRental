using DemoVenueRental.Extensions;
using DemoVenueRental.Models;
using DemoVenueRental.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoVenueRental.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlaceController : BaseController
    {
        private readonly IPlaceService _placeService;

        public PlaceController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceInfo model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(ModelStateFirstError(ModelState));
                }

                return Ok(await _placeService.InsertPlace(model).ToSerializeAsync());
            }
            catch (Exception ex)
            {
                return Ok(HandleError("儲存失敗!", ex));
            }
        }
    }
}
