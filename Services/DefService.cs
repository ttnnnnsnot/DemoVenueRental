using DemoVenueRental.Global;
using DemoVenueRental.Models;
using DemoVenueRental.Sql;
using System.Data;

namespace DemoVenueRental.Services
{
    public class DefService : BaseService
    {
        private DefData _defData;

        public DefService(IDbConnection connection) : base(connection)
        {
            _defData = new DefData(connection);
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
                return resultData.ToJson();
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
            return resultData.ToJson();
        }
    }
}
