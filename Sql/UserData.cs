using Dapper;
using DemoVenueRental.Global;
using DemoVenueRental.Models;
using System.Data;

namespace DemoVenueRental.Sql
{
    public class UserLoginResult
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }

    public class UserData
    {
        private readonly IDbConnection _connection;
        public UserData(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<ResultData<int>> Register(Register model)
        {
            ResultData<int> result = new ResultData<int>();

            // 使用 BCrypt 加密密碼
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);
          
            if(await IsEmailExist(model.Email))
            {
                result.errorMsg = "Email已存在";
                return result;
            }

            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    var sql = @"
                        INSERT INTO Users (Email, PasswordHash, LastName, Name, Phone)
                            VALUES (@Email, @PasswordHash, @LastName, @Name, @Phone);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);
                        ";

                    var param = new
                    {
                        Email = DapperExtension.ToNVarchar(model.Email, 100),
                        PasswordHash = DapperExtension.ToChar(hashedPassword, 60),
                        LastName = DapperExtension.ToNVarchar(model.LastName, 30),
                        Name = DapperExtension.ToNVarchar(model.Name, 30),
                        Phone = DapperExtension.ToChar(model.Phone, 10),
                    };

                    var userId = await _connection.ExecuteScalarAsync<int>(sql, param, transaction);
                
                    if (userId == 0)
                    {
                        result.errorMsg = "註冊失敗";
                        transaction.Rollback();
                        return result;
                    }

                    result.state = true;
                    result.data = userId;

                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.errorMsg = "註冊失敗";
                    LoggerService.LogError("註冊失敗", ex);
                    return result;
                }
            }
        }

        public async Task<bool> IsEmailExist(string Email)
        {
            var sql = @"
                SELECT CASE WHEN EXISTS (
                    SELECT 1 FROM Users WHERE Email = @Email
                ) THEN 1 ELSE 0 END           
                 ";

            var param = new
            {
                Email = DapperExtension.ToNVarchar(Email,100)
            };
            return await _connection.ExecuteScalarAsync<int>(sql, param) > 0;
        }
    }
}
