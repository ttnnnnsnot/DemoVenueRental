using DemoVenueRental.Global;
using DemoVenueRental.Models;
using DemoVenueRental.Sql;

namespace DemoVenueRental.Services
{
    public class UserService : BaseService
    {
        private UserData _userData = new UserData();

        public bool IsEmailExist(string Email, int LoginTypeId)
        {
            return _userData.IsEmailExist(Email, LoginTypeId);
        }
    }
}
