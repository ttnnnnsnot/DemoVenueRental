using DemoVenueRental.Global;
using DemoVenueRental.Models;
using DemoVenueRental.Params;
using DemoVenueRental.Sql;

namespace DemoVenueRental.Services
{
    public interface IPlaceService
    {
        Task<ResultData<PlaceResult>> InsertPlace(PlaceInfo placeInfo);
        Task<ResultData<PlaceResult>> UpdatePlace(PlaceInfo placeInfo);
        Task<ResultData<PlaceViewModel>> GetInfo();
        Task<ResultData> UpdateSportType(int[] sportType);
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

        public async Task<ResultData<PlaceViewModel>> GetInfo()
        {
            var userId = _userService.GetUserId();
            int.TryParse(BaseCookie.Get(CookieKey.PlaceId), out int placeId);

            if (placeId <= 0)
                return new ResultData<PlaceViewModel> { message = "場所不存在" };

            var result = await _placeData.GetInfo(placeId, userId);

            if (!result.state)
                return result;

            return result;
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

            return result;
        }

        public async Task<ResultData<PlaceResult>> UpdatePlace(PlaceInfo placeInfo)
        {
            int.TryParse(BaseCookie.Get(CookieKey.PlaceId), out int placeId);

            if (placeId <= 0)
                return new ResultData<PlaceResult> { message = "場所不存在" };

            placeInfo.PlaceId = placeId;
            placeInfo.UserId = _userService.GetUserId();

            if (placeInfo.UserId == 0)
                return new ResultData<PlaceResult> { message = "使用者未登入" };

            var result = await _placeData.UpdatePlace(placeInfo);

            if (!result.state)
                return result;

            BaseCookie.Set(CookieKey.PlaceId, result.data.PlaceId.ToString());

            return result;
        }

        public async Task<ResultData> UpdateSportType(int[] sportType)
        {
            int.TryParse(BaseCookie.Get(CookieKey.PlaceId), out int placeId);

            if (placeId <= 0)
                return new ResultData { message = "場所不存在" };

            if (sportType.Length == 0)
                return new ResultData { message = "無資料需要更新" };

            var result = await _placeData.UpdateSportType(placeId, sportType);

            if (!result.state)
                return result;

            return result;
        }
    }
}
