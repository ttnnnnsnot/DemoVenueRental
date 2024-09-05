using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        /// 反序列化
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
        public static async Task<T> ToDeserializeAsync<T>(this Task<string> task) where T : class, new ()
        {
            try
            {
                var result = await task;
                return JsonSerializer.Deserialize<T>(result, Default) ?? new T();
            }
            catch
            {
                return new T();
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string ToSerialize<T>(this T me)
        {
            try
            {
                return JsonSerializer.Serialize<T>(me, Default) ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <returns></returns>
        public static async Task<string> ToSerializeAsync<T>(this Task<T> task)
        {
            try
            {
                var result = await task;
                return JsonSerializer.Serialize<T>(result, Default) ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
