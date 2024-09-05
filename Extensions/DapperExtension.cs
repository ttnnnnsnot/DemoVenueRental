using Dapper;

namespace DemoVenueRental.Extensions
{
    public static class DapperExtension
    {
        /// <summary>
        ///     Length of the string is default 4000
        /// </summary>
        public static DbString ToVarchar(this string me)
        {
            if (string.IsNullOrEmpty(me))
            {
                return new DbString { Value = "", IsAnsi = true, IsFixedLength = false };
            }
            return new DbString { Value = me, IsAnsi = true, IsFixedLength = false };
        }

        /// <summary>
        ///     Length of the string -1 for max
        /// </summary>
        public static DbString ToVarchar(this string me, int length)
        {
            if (string.IsNullOrEmpty(me))
            {
                return new DbString { Value = "", Length = length, IsAnsi = true, IsFixedLength = false };
            }
            return new DbString { Value = me, Length = length, IsAnsi = true, IsFixedLength = false };
        }

        /// <summary>
        ///     Length of the string is default 4000
        /// </summary>
        public static DbString ToChar(this string me)
        {
            if (string.IsNullOrEmpty(me))
            {
                return new DbString { Value = "", IsAnsi = true, IsFixedLength = true };
            }
            return new DbString { Value = me, IsAnsi = true, IsFixedLength = true };
        }

        /// <summary>
        ///     Length of the string -1 for max
        /// </summary>
        public static DbString ToChar(this string me, int length)
        {
            if (string.IsNullOrEmpty(me))
            {
                return new DbString { Value = "", Length = length, IsAnsi = true, IsFixedLength = true };
            }
            return new DbString { Value = me, Length = length, IsAnsi = true, IsFixedLength = true };
        }

        /// <summary>
        ///     Length of the string is default 4000
        /// </summary>
        public static DbString ToNVarchar(this string me)
        {
            if (string.IsNullOrEmpty(me))
            {
                return new DbString { Value = "", IsAnsi = false, IsFixedLength = false };
            }
            return new DbString { Value = me, IsAnsi = false, IsFixedLength = false };
        }

        /// <summary>
        ///     Length of the string -1 for max
        /// </summary>
        public static DbString ToNVarchar(this string me, int length)
        {
            if (string.IsNullOrEmpty(me))
            {
                return new DbString { Value = "", Length = length, IsAnsi = false, IsFixedLength = false };
            }
            return new DbString { Value = me, Length = length, IsAnsi = false, IsFixedLength = false };
        }

        /// <summary>
        ///     Length of the string is default 4000
        /// </summary>
        public static DbString ToNChar(this string me)
        {
            if (string.IsNullOrEmpty(me))
            {
                return new DbString { Value = "", IsAnsi = false, IsFixedLength = true };
            }
            return new DbString { Value = me, IsAnsi = false, IsFixedLength = true };
        }

        /// <summary>
        ///     Length of the string -1 for max
        /// </summary>
        public static DbString ToNChar(this string me, int length)
        {
            if (string.IsNullOrEmpty(me))
            {
                return new DbString { Value = "", Length = length, IsAnsi = false, IsFixedLength = true };
            }
            return new DbString { Value = me, Length = length, IsAnsi = false, IsFixedLength = true };
        }
    }
}