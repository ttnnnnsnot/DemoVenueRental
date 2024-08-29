using DemoVenueRental.Global;
using DemoVenueRental.Models;
using DemoVenueRental.Params;
using DemoVenueRental.Sql;

namespace DemoVenueRental.Services
{
    public interface IPlaceService
    {
        Task<ResultData<PlaceResult>> InsertPlace(PlaceInfo placeInfo);
    }
    public class PlaceService : IPlaceService
    {
        private readonly IPlaceData _placeData;
        private readonly IUserService _userService;

        public PlaceService(IPlaceData placeData, IUserService userService)
        {
            _placeData = placeData;
            _userService = userService;
        }

        public async Task<ResultData<PlaceResult>> InsertPlace(PlaceInfo placeInfo)
        {
            placeInfo.UserId = _userService.GetUserId();

            if(placeInfo.UserId == 0)
                return new ResultData<PlaceResult> { message = "使用者未登入" };

            var result = await _placeData.InsertPlace(placeInfo);

            if(!result.state)
                return result;

            BaseCookie.Set(CookieKey.PlaceId, result.data.PlaceId.ToString());
            BaseCookie.Set(CookieKey.UserId, result.data.UserId.ToString());

            return result;
        }
    }
}
