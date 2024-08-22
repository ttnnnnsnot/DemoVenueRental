using System.Data;

namespace DemoVenueRental.Services
{
    public class BaseService
    {
        protected readonly IDbConnection _connection;

        public BaseService(IDbConnection connection)
        {
            _connection = connection;
        }
    }
}
