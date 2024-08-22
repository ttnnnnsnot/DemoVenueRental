using DemoVenueRental.Global;
using DemoVenueRental.Models;
using DemoVenueRental.Sql;
using System.Data;

namespace DemoVenueRental.Services
{
    public class UserService : BaseService
    {
        private UserData _userData;

        public UserService(IDbConnection connection) : base(connection)
        {
            _userData = new UserData(connection);
        }

        public async Task<bool> IsEmailExist(string Email)
        {
            return await _userData.IsEmailExist(Email);
        }

        public async Task <ResultData<int>> Register(Register model)
        {
            return await _userData.Register(model);
        }
    }
}
