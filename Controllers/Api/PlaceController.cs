using DemoVenueRental.Extensions;
using DemoVenueRental.Global;
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

        [HttpGet("PlaceId")]
        public IActionResult Get()
        {
            return Ok(BaseCookie.Get(CookieKey.PlaceId));
        }

        [HttpDelete("PlaceId")]
        public IActionResult Delete()
        {
            return Ok(BaseCookie.Delete(CookieKey.PlaceId));
        }

        [HttpGet("PlaceInfo")]
        public async Task<IActionResult> GetPlaceInfo()
        {
            return Ok(await _placeService.GetInfo().ToSerializeAsync());
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

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PlaceInfo model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(ModelStateFirstError(ModelState));
                }

                return Ok(await _placeService.UpdatePlace(model).ToSerializeAsync());
            }
            catch (Exception ex)
            {
                return Ok(HandleError("儲存失敗!", ex));
            }
        }

        [HttpPut("SportType")]
        public async Task<IActionResult> Put([FromBody] int[] SelectTypeId)
        {
            try
            {
                return Ok(await _placeService.UpdateSportType(SelectTypeId).ToSerializeAsync());
            }
            catch (Exception ex)
            {
                return Ok(HandleError("儲存失敗!", ex));
            }
        }
    }
}
