using System.Data.SqlClient;

namespace DemoVenueRental.Global
{
    public static class Connection
    {
        public static SqlConnection MsSql()
        {
            return new SqlConnection(AppSettings.MsSqlConnect);
        }
    }
}
