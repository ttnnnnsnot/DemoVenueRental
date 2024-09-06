using Dapper;
using DemoVenueRental.Extensions;
using DemoVenueRental.Global;
using DemoVenueRental.Models;
using System.Data;

namespace DemoVenueRental.Sql
{
    public interface IDefData
    {
        Task<ResultData<List<SelectType>>> GetData(string typeName);
    }

    public class DefData : IDefData
    {
        private readonly IDbConnection _connection;

        public DefData(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<ResultData<List<SelectType>>> GetData(string typeName)
        {
            var result = new ResultData<List<SelectType>>();
            try
            {
                var sql = @"SELECT selectTypeId,name 
                            FROM SelectType 
                        WHERE typeName = @typeName Order by sort";

                var param = new
                {
                    typeName = typeName.ToVarchar(10),
                };

                result.data = (await _connection.QueryAsync<SelectType>(sql, param)).ToList();

                result.state = true;

                return result;
            }
            catch (Exception ex)
            {
                result.message = "取得資料失敗";
                LoggerService.LogError(result.message, ex);
                return result;
            }

        }
    }
}
