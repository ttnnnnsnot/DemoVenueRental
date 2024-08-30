using Dapper;
using DemoVenueRental.Extensions;
using DemoVenueRental.Global;
using DemoVenueRental.Models;
using System.Data;

namespace DemoVenueRental.Sql
{
    public interface IUserData
    {
        Task<ResultData<int>> Register(Register model);
        Task<ResultData<int>> Login(Login model);
        Task<bool> IsEmailExist(string Email);
    }

    public class UserLoginResult
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }

    public class UserData : IUserData
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
            string hashedPassword = model.PasswordHash.ToHash();
            
            if (await IsEmailExist(model.Email))
            {
                result.message = "Email已存在";
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
                        Email = model.Email.ToNVarchar(100),
                        PasswordHash = hashedPassword.ToVarchar(80),
                        LastName = model.LastName.ToNVarchar(30),
                        Name = model.Name.ToNVarchar(30),
                        Phone = model.Phone.ToChar(10),
                    };

                    var userId = await _connection.ExecuteScalarAsync<int>(sql, param, transaction);
                
                    if (userId == 0)
                    {
                        result.message = "註冊失敗";
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
                    result.message = "註冊失敗";
                    LoggerService.LogError(result.message, ex);
                    return result;
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }

        public async Task<ResultData<int>> Login(Login model)
        {
            ResultData<int> result = new ResultData<int>();

            var sql = @"
                SELECT UserId, Email, PasswordHash
                FROM Users
                WHERE Email = @Email
            ";

            var param = new
            {
                Email = model.Email.ToNVarchar(100)
            };

            var user = await _connection.QueryFirstOrDefaultAsync<UserLoginResult>(sql, param);

            if (user == null)
            {
                result.message = "Email不存在";
                return result;
            }

            if (!model.PasswordHash.ToVerify(user.PasswordHash))
            {
                result.message = "密碼錯誤";
                return result;
            }

            result.state = true;
            result.data = user.UserId;
            return result;
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
                Email = Email.ToNVarchar(100)
            };
            return await _connection.ExecuteScalarAsync<int>(sql, param) > 0;
        }
    }
}
