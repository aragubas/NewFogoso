using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogoso.GameLogic
{

    public class AragubasTimeObject
    {
        int Year = 0;
        int Month = 0;
        int Week = 0;
        int Day = 0;
        int Hour = 0;
        int Minute = 0;
        int offsetYear = 0;
        int offsetMonth = 0;
        int offsetWeek = 0;
        int offsetDay = 0;
        int offsetHour = 0;
        int offsetMinute = 0;
 
        public AragubasTimeObject(int pMinuteOffset, int pHourOffset, int pDayOffset, int pWeekOffset, int pMonthOffset, int pYearOffset)
        {
            offsetYear = pYearOffset;
            offsetMonth = pMonthOffset;
            offsetWeek = pWeekOffset; 
            offsetDay = pDayOffset;
            offsetHour = pHourOffset;
            offsetMinute = pMinuteOffset;
            ResetTime();
  
        }

        public void ResetTime()
        {
            Year = AragubasTime.Year + offsetYear;
            Month = AragubasTime.Month + offsetMonth;
            Week = AragubasTime.Week + offsetWeek;
            Day = AragubasTime.Day + offsetDay;
            Hour = AragubasTime.Hour + offsetHour;
            Minute = AragubasTime.Minute + offsetMinute;
 
        }

        public bool TimeYear() { return AragubasTime.Year >= Year; }
        public bool TimeMonth() { return AragubasTime.Month >= Month; }
        public bool TimeWeek() { return AragubasTime.Week >= Week; }
        public bool TimeDay() { return AragubasTime.Day >= Day; }
        public bool TimeHour() { return AragubasTime.Hour >= Hour; }
        public bool TimeMinute() { return AragubasTime.Minute >= Minute; }
        public bool TimeTriggered() {return TimeYear() && TimeMonth() && TimeWeek() && TimeDay() && TimeHour() && TimeMinute(); }

    }
}
