using DemoVenueRental.Extensions;
using DemoVenueRental.Models;
using DemoVenueRental.Sql;

namespace DemoVenueRental.Services
{
    public interface IDefService
    {
        Task<ResultData<List<SelectType>>> GetDef(string typeName);
    }

    public class DefService : IDefService
    {
        private readonly IDefData _defData;

        public DefService(IDefData defData)
        {
            _defData = defData;
        }

        public async Task<ResultData<List<SelectType>>> GetDef(string typeName)
        {
            var result = new ResultData<List<SelectType>>();

            result = await _defData.GetData(typeName);

            return result;
        }
    }
}
