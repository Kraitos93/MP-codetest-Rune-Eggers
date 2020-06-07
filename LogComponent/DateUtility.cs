using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogComponent
{
    public class DateUtility
    {
        public static DateTime GetTime()
        {
            return DateTime.Now;
        }

        public static DateTime GetTime(string timeZone)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
            currentTime.AddHours(timeZoneInfo.IsDaylightSavingTime(currentTime) ? 1 : 0);
            return currentTime;
        }

        public static string GetTimeStamp(DateTime date, string format)
        {
            return date.ToString(format);
        }

        public static bool IsSameDate(DateTime dateOne, DateTime dateTwo)
        {
            if ((dateOne.Date == dateTwo.Date))
            {
                return true;
            }
            return false;
        }
    }
}
