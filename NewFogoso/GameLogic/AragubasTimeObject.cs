using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogoso.GameLogic
{

    class AragubasTimeObject
    {
        public int Year = 0;
        public int Month = 0;
        public int Week = 0;
        public int Day = 0;
        public int Hour = 0;
        public int Minute = 0;

        public AragubasTimeObject(int pMinuteOffset, int pHourOffset, int pDayOffset, int pWeekOffset, int pMonthOffset, int pYearOffset)
        {
            // Change to loading cursor
            GameInput.CursorImage = "loading.png";

            Year = AragubasTime.Year + pYearOffset;
            Month = AragubasTime.Month + pMonthOffset;
            Week = AragubasTime.Week + pWeekOffset;
            Day = AragubasTime.Day + pDayOffset;
            Hour = AragubasTime.Hour + pHourOffset;
            Minute = AragubasTime.Minute + pMinuteOffset;

        }

        public bool TimeTriggered()
        { 
            bool PassOne = Year == AragubasTime.Year;
            bool PassTwo = Month == AragubasTime.Month;
            bool PassThree = Week == AragubasTime.Week;
            bool PassSix = Day == AragubasTime.Day;
            bool PassFour = Hour == AragubasTime.Hour;
            bool PassFive = Minute >= AragubasTime.Minute;

            return PassOne && PassTwo && PassThree && PassFour && PassFive && PassSix;

        }


    }
}
