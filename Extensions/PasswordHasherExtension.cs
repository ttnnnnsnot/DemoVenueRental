using BCrypt.Net;

namespace DemoVenueRental.Extensions
{
    public static class PasswordHasherExtension
    {
        /// <summary>
        /// Hash Default Version 1
        /// </summary>
        /// <param name="me"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static string ToHash(this string me, int version = 1)
        {
            return version switch
            {
                1 => BCrypt.Net.BCrypt.HashPassword(me, workFactor: 7),
                2 => BCrypt.Net.BCrypt.HashPassword(me, workFactor: 11),
                _ => throw new NotSupportedException("不支援的雜湊版本"),
            };
        }

        /// <summary>
        /// Verify Default Version 1
        /// </summary>
        /// <param name="me"></param>
        /// <param name="hashedPassword"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static bool ToVerify(this string me, string hashedPassword, int version = 1)
        {
            try
            {
                return version switch
                {
                    1 => BCrypt.Net.BCrypt.Verify(me, hashedPassword),
                    2 => BCrypt.Net.BCrypt.Verify(me, hashedPassword),
                    _ => throw new NotSupportedException("不支援的雜湊版本"),
                };
            }
            catch (SaltParseException ex)
            {
                throw new InvalidOperationException("雜湊密碼的格式不正確或版本號不一致", ex);
            }
        }
    }
}
