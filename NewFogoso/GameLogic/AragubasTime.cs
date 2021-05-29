using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogoso.GameLogic
{
    public static class AragubasTime
    {
        public static int Year = 1;
        public static int Month = 1;
        public static int Week = 1;
        public static int Day = 1;
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

        public static string GetDecadeNameWithYear()
        {
            if (Year < 25)
            {    
                return "Octunder " + Math.Abs(Year - 25).ToString();
            } 
            else if (Year < 35)
            { 
                return "Notrunda " + Math.Abs(Year - 35).ToString();
            }
            else if (Year <= 42)
            { 
                return "Corrupted " + Math.Abs(Year - 42).ToString();
            }

            return "Undefined " + Year.ToString();

        }

        public static string GetDayName()
        {
            switch (Day)
            {
                case 1:
                    return "Narilda";

                case 2:
                    return "Doiize";

                case 3:
                    return "Lalanida";

                case 4:
                    return "Kronilha";

                case 5:
                    return "Cacanildo";

                case 6:
                    return "Scrolla";

                case 7:
                    return "Abutrec";

                default:
                    return "Invalid";
            }

        }

        public static string GetMonthName()
        {
            switch (Month)
            {
                case 1:
                    return "Tropin";

                case 2:
                    return "Jatubeiro";

                case 3:
                    return "Crotela";

                case 4:
                    return "Laubarin";

                default: 
                    return "Invalid";
            }
        }

        public static void Update()
        {
            Frames += 1;

            if (Frames >= Int32.Parse(Registry.ReadKeyValue("/fps"))) { Second += 1; Frames = 0; }
            if (Second >= 13) { Minute += 1; Second = 1; }
            if (Minute >= 13) { Hour += 1; Minute = 1; }
            if (Hour >= 7) { Day += 1; Hour = 1; }
            if (Day >= 5) { Week += 1; Day = 1; }
            if (Week >= 6) { Month += 1; Week = 1; }
            if (Month >= 5) { Year += 1; Month = 1; }
            if (Year >= 42) { Year = 41; }
 
        }


    }
}
