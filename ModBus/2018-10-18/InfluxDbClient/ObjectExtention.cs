using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.InfluxDbClient
{
    public static class ObjectExtention
    {
        private static DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>Converts to unix time in milliseconds.</summary>
        /// <param name="date">The date.</param>
        /// <returns>The number of elapsed milliseconds</returns>
        public static long ToUnixTime(this DateTime date)
        {
            return Convert.ToInt64((date - _epoch).TotalMilliseconds);
        }

        /// <summary>Converts from unix time in milliseconds.</summary>
        /// <param name="unixTimeInMillis">The unix time in millis.</param>
        /// <returns></returns>
        public static DateTime FromUnixTime(this long unixTimeInMillis)
        {
            return _epoch.AddMilliseconds(unixTimeInMillis);
        }

    }

}
