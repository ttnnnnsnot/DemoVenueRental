using Dapper;
using DemoVenueRental.Extensions;
using DemoVenueRental.Global;
using DemoVenueRental.Models;
using DemoVenueRental.Params;
using System.Data;

namespace DemoVenueRental.Sql
{
    public interface IPlaceData
    {
        Task<ResultData<PlaceResult>> InsertPlace(PlaceInfo placeInfo);
    }

    public class PlaceData : IPlaceData
    {
        private readonly IDbConnection _connection;
        public PlaceData(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<ResultData<PlaceResult>> InsertPlace(PlaceInfo placeInfo)
        {
            ResultData<PlaceResult> result = new ResultData<PlaceResult>();

            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    var sql = @"
                        INSERT INTO PlaceInfo ([UserId], [Name], [CityId], [Address], [Describe], [Rules], [Opening], [StateId])
                            VALUES (@UserId, @Name, @CityId, @Address, @Describe, @Rules, @Opening, @StateId);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);
                        ";

                    var param = new
                    {
                        placeInfo.UserId,
                        Name = (placeInfo.Name ?? string.Empty).ToNVarchar(30),
                        placeInfo.CityId,
                        Address = (placeInfo.Address ?? string.Empty).ToNVarchar(100),
                        Describe = placeInfo.Describe.ToNVarchar(),
                        Rules = placeInfo.Rules.ToNVarchar(),
                        placeInfo.Opening,
                        placeInfo.StateId,
                    };

                    var placeId = await _connection.QueryFirstOrDefaultAsync<int>(sql, param, transaction);

                    if(placeId == 0)
                    {
                        transaction.Rollback();
                        result.message = "新增場所失敗";
                        return result;
                    }

                    transaction.Commit();

                    result.state = true;
                    result.data = new PlaceResult
                    {
                        PlaceId = placeId,
                        UserId = placeInfo.UserId,
                    };
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.message = "新增場所失敗";
                    LoggerService.LogError(result.message, ex);
                    return result;
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }
    }
}
