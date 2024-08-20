using Dapper;
using DemoVenueRental.Global;
using DemoVenueRental.Models;

namespace DemoVenueRental.Sql
{
    public class DefData
    {
        public List<SelectData> GetSelectData(string typeName)
        {
            using (var conn = Connection.MsSql()) 
            {
                var sql = @"SELECT selectTypeId,name FROM SelectType WHERE typeName = @typeName Order by sort";

                return conn.Query<SelectData>(sql, new { typeName = new DbString { Value = typeName, IsFixedLength = false, Length = 10, IsAnsi = true } }).ToList();
            }
        }
    }
}
