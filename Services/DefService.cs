using DemoVenueRental.Extensions;
using DemoVenueRental.Models;
using DemoVenueRental.Sql;

namespace DemoVenueRental.Services
{
    public interface IDefService
    {
        Task<string> GetDef(string typeName);
    }

    public class DefService : BaseService , IDefService
    {
        private readonly IDefData _defData;

        public DefService(IDefData defData)
        {
            _defData = defData;
        }

        public async Task<string> GetDef(string typeName)
        {
            var resultData = new ResultData<SelectType>();
            var def = new SelectType();
            def.listItem = await _defData.GetSelectData(typeName);

            if (def.listItem.Count == 0)
            {
                resultData.state = false;
                resultData.errorMsg = "查無資料";
                return resultData.ToSerialize();
            }

            if (typeName == "sport")
            {
                def.defaultText = "選擇項目";
                def.iconClass = "fa-solid fa-medal";
            }
            else
            {
                def.defaultText = "選擇地區";
                def.iconClass = "fa-solid fa-map-pin";
            }

            resultData.data = def;
            resultData.state = true;
            return resultData.ToSerialize();
        }
    }
}
