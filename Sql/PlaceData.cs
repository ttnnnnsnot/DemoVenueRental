using Dapper;
using DemoVenueRental.Extensions;
using DemoVenueRental.Global;
using DemoVenueRental.Models;
using DemoVenueRental.Params;
using System.Data;
using System.Text;

namespace DemoVenueRental.Sql
{
    public interface IPlaceData
    {
        Task<ResultData<PlaceResult>> InsertPlace(PlaceInfo placeInfo);
        Task<ResultData<PlaceResult>> UpdatePlace(PlaceInfo placeInfo);
        Task<ResultData<PlaceViewModel>> GetInfo(int placeId,int userId);
        Task<ResultData> UpdateSportType(int placeId, int[] sportType);
    }

    public class PlaceData : IPlaceData
    {
        private readonly IDbConnection _connection;
        public PlaceData(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<ResultData> UpdateSportType(int placeId,int[] sportType)
        {
            var result = new ResultData();

            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    var sql = @"
                    DELETE FROM PlaceType WHERE [PlaceId] = {=placeId};
                    ";

                    var param = new
                    {
                        placeId
                    };

                    await _connection.ExecuteAsync(sql, param, transaction);

                    var sportTypes = new List<PlaceTypeInstall>();
                    foreach (var type in sportType)
                    {
                        sportTypes.Add(new PlaceTypeInstall
                        {
                            PlaceId = placeId,
                            SelectTypeId = type
                        });
                    }

                    sql = @"INSERT INTO [dbo].[PlaceType]
                            ([PlaceId], [SelectTypeId])
                            VALUES(@PlaceId, @SelectTypeId);";

                    int affectedRows = await _connection.ExecuteAsync(sql, sportTypes, transaction);

                    if (affectedRows == 0)
                    {
                        transaction.Rollback();
                        result.message = "更新場所類型失敗!";
                        return result;
                    }

                    transaction.Commit();
                    result.state = true;
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.message = "更新場所類型失敗!";
                    LoggerService.LogError(result.message, ex);
                    return result;
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }

        public async Task<ResultData<PlaceViewModel>> GetInfo(int placeId, int userId)
        {
            var result = new ResultData<PlaceViewModel>();

            try
            {
                var sql = @"
                SELECT [PlaceId], [UserId], [Name], [CityId], [Address], [Describe], [Rules], [Opening], [StateId]
                FROM PlaceInfo
                WHERE [PlaceId] = {=placeId} AND [UserId] = {=userId};

                SELECT [PlaceImgId], [imgUrl], [OrderNum]
                FROM PlaceImg
                WHERE [PlaceId] = {=placeId}
                ORDER BY [OrderNum];

                SELECT [SelectTypeId]
                FROM PlaceType
                WHERE [PlaceId] = {=placeId};
                ";

                var param = new
                {
                    placeId,
                    userId
                };

                using (var multi = await _connection.QueryMultipleAsync(sql, param))
                {
                    var placeInfo = await multi.ReadFirstOrDefaultAsync<PlaceInfo>();
                    if (placeInfo == null)
                    {
                        result.message = "場所不存在";
                        return result;
                    }

                    var placeImgs = (await multi.ReadAsync<PlaceImg>()).ToList();
                    var placeTypes = (await multi.ReadAsync<PlaceType>()).ToList();

                    result.state = true;
                    result.data = new PlaceViewModel
                    {
                        PlaceInfo = placeInfo,
                        PlaceImgs = placeImgs,
                        PlaceType = placeTypes
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                result.message = "取得場所資訊失敗!";
                LoggerService.LogError(result.message, ex);
                return result;
            }
        }

        public async Task<ResultData<PlaceResult>> UpdatePlace(PlaceInfo placeInfo)
        {
            var result = new ResultData<PlaceResult>();

            try
            {
                var sql = @"
                UPDATE PlaceInfo 
                    SET [Name] = @Name, [CityId] = @CityId,
                        [Address] = @Address, [Describe] = @Describe,
                        [Rules] = @Rules
                    WHERE [PlaceId] = {=PlaceId} AND [UserId] = {=UserId};
                ";

                var param = new
                {
                    placeInfo.PlaceId,
                    placeInfo.UserId,
                    Name = (placeInfo.Name ?? string.Empty).ToNVarchar(30),
                    placeInfo.CityId,
                    Address = (placeInfo.Address ?? string.Empty).ToNVarchar(100),
                    Describe = (placeInfo.Describe ?? string.Empty).ToNVarchar(),
                    Rules = (placeInfo.Rules ?? string.Empty).ToNVarchar(),
                };

                var affectedRows = await _connection.ExecuteAsync(sql, param);

                if (affectedRows == 0)
                {
                    result.message = "更新場所失敗";
                    return result;
                }

                result.state = true;
                result.data = new PlaceResult
                {
                    PlaceId = placeInfo.PlaceId,
                };
                return result;
            }
            catch (Exception ex)
            {
                result.message = "更新場所失敗!";
                LoggerService.LogError(result.message, ex);
                return result;
            }
        }

        public async Task<ResultData<PlaceResult>> InsertPlace(PlaceInfo placeInfo)
        {
            ResultData<PlaceResult> result = new ResultData<PlaceResult>();
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
                    Describe = (placeInfo.Describe ?? string.Empty).ToNVarchar(),
                    Rules = (placeInfo.Rules ?? string.Empty).ToNVarchar(),
                    placeInfo.Opening,
                    placeInfo.StateId,
                };

                var placeId = await _connection.QueryFirstOrDefaultAsync<int>(sql, param);

                if (placeId == 0)
                {
                    result.message = "新增場所失敗";
                    return result;
                }

                result.state = true;
                result.data = new PlaceResult
                {
                    PlaceId = placeId,
                };
                return result;
            }
            catch (Exception ex)
            {
                result.message = "新增場所失敗";
                LoggerService.LogError(result.message, ex);
                return result;
            }
        }
    }


}
