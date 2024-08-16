using System.Text.Json;
using System.Text.Json.Serialization;

namespace DemoVenueRental.Global
{
    /// <summary>
    /// 提供 JSON 序列化和反序列化的靜態服務類。
    /// </summary>
    public static class JsonSerializerService
    {
        private static JsonSerializerOptions Default { get; } = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static T Deserialize<T>(string json) where T : class, new()
        {
            return JsonSerializer.Deserialize<T>(json, Default) ?? new T();
        }
        public static string Serialize<T>(T json) where T : class
        {
            return JsonSerializer.Serialize(json, Default);
        }
    }
}
