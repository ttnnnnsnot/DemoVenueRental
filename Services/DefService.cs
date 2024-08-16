using DemoVenueRental.Global;
using DemoVenueRental.Models;

namespace DemoVenueRental.Services
{
    public class DefService
    {
        public string GetDef(int id)
        {
            var resultData = new ResultData<SelectType>();
            var def = new SelectType();

            if (id != 1)
            {
                def.defaultText = "選擇地區";
                def.iconClass = "fa-solid fa-map-pin";
                def.listItem = ["新北市", "台北市", "桃園市"];
            }

            resultData.data = def;
            resultData.state = true;
            return resultData.ToJson();
        }

        public string HandleError(string errorMessage, Exception ex)
        {
            LoggerService.LogError(errorMessage, ex);
            var errorResult = new ResultData<object> { state = false, errorMsg = errorMessage };
            return errorResult.ToJson();
        }
    }
}
