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
                return "Octunder " + Convert.ToString(Math.Abs(Year - 25));
            }
            else if (Year < 35)
            {
                return "Notrunda " + Convert.ToString(Math.Abs(Year - 35));
            }
            else if (Year < 42)
            { 
                return "Corrupted " + Convert.ToString(Math.Abs(Year - 42));
            }

            return "Undefined " + Convert.ToString(Year);

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
                    return "Invalid Day Name";
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
                    return "Invalid Month";
            }
        }

        public static void Update()
        {
            Frames += 1;


            if (Frames >= Convert.ToInt32(Registry.ReadKeyValue("/fps"))) { Second += 1; Frames = 0; }
            if (Second >= 31) { Minute += 1; Second = 1; }
            if (Minute >= 25) { Hour += 1; Minute = 1; }
            if (Hour >= 9) { Day += 1; Hour = 1; }
            if (Day >= 8) { Week += 1; Day = 1; }
            if (Week >= 9) { Month += 1; Week = 1; }
            if (Month >= 5) { Year += 1; Month = 1; }


        }


    }
}
