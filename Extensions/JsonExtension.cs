using System.Text.Json;
using System.Text.Json.Serialization;

namespace DemoVenueRental.Extensions
{
    public static class JsonExtension
    {
        private static JsonSerializerOptions Default { get; } = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <returns></returns>
        public static T ToDeserialize<T>(this string me) where T : class, new()
        {
            try
            {
                return JsonSerializer.Deserialize<T>(me, Default) ?? new T();
            }
            catch
            {
                return new T();
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string ToSerialize<T>(this T me) where T : class
        {
            try
            {
                return JsonSerializer.Serialize(me, Default) ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
