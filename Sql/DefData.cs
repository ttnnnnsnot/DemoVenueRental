using Dapper;
using DemoVenueRental.Extensions;
using DemoVenueRental.Models;
using System.Data;

namespace DemoVenueRental.Sql
{
    public interface IDefData
    {
        Task<List<SelectData>> GetSelectData(string typeName);
    }

    public class DefData : IDefData
    {
        private readonly IDbConnection _connection;

        public DefData(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<SelectData>> GetSelectData(string typeName)
        {
            var sql = @"SELECT selectTypeId,name 
                            FROM SelectType 
                        WHERE typeName = @typeName Order by sort";

            var param = new
            {
                typeName = DapperExtension.ToNVarchar(typeName, 10)
            };

            return (await _connection.QueryAsync<SelectData>(sql, param)).ToList();
        }
    }
}
