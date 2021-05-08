using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogoso.GameLogic
{
    public static class AragubasTime
    {
        public static int Year;
        public static int Month;
        public static int Week;
        public static int Day;
        public static int Hour;
        public static int Minute;
        public static int Second;
        public static int Frames;

        public static void ResetTime()
        {
            Year = 0;
            Month = 0;
            Week = 0;
            Day = 0;
            Hour = 0;
            Minute = 0;
            Second = 0;
            Frames = 0;

        }

        public static void Update()
        {
            Frames += 1;

            if (Frames >= Convert.ToInt32(Registry.ReadKeyValue("/fps"))) { Second += 1; Frames = 0; }
            if (Second >= 30) { Minute += 1; Second = 0; }
            if (Minute >= 24) { Hour += 1; Minute = 0; }
            if (Hour >= 16) { Day += 1; Hour = 0; }
            if (Day >= 7) { Week += 1; Day = 0; }
            if (Week >= 32) { Month += 1; Week = 0; }
            if (Month >= 32) { Year += 1; Month = 0; }

        }


    }
}
