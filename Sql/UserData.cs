using Dapper;
using DemoVenueRental.Global;

namespace DemoVenueRental.Sql
{
    public class UserData
    {
        public bool IsEmailExist(string Email, int LoginTypeId)
        {
            using (var conn = Connection.MsSql())
            {
                var sql = @"
                SELECT COUNT(1) 
                    FROM UserEmails
                        WHERE Email = @Email And 
                            LoginTypeId = @LoginTypeId                
                 ";

                var param = new { 
                    Email = new DbString 
                    { Value = Email, IsFixedLength = false, Length = 100, IsAnsi = true },
                    LoginTypeId
                };

                return conn.ExecuteScalar<int>(sql, param) > 0;
            }
        }
    }
}
