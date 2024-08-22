using Dapper;
using DemoVenueRental.Global;
using DemoVenueRental.Models;
using System.Data;

namespace DemoVenueRental.Sql
{
    public class DefData
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
