using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogoso.GameLogic
{
    public class TimedEvent
    {
        public AragubasTimeObject ActivationTime;
        public bool AutoDelete;
        public bool WaitingDeletion;
        public delegate void delActivation(TimedEvent sender);
        public event delActivation ActivationEvent;
   
        public TimedEvent(AragubasTimeObject pActivationTime, bool pAutoDelete)
        {
            ActivationTime = pActivationTime;

            AutoDelete = pAutoDelete;
        }

        public void CheckTime()
        {  
            if (ActivationTime.TimeTriggered())
            { 
                if (ActivationEvent != null){ ActivationEvent.Invoke(this); if (AutoDelete) { WaitingDeletion = true; } }
            }
             
        }


    }

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
        public static Dictionary<string, TimedEvent> TimedEvents = new Dictionary<string, TimedEvent>();
        public delegate void CeiraGate(TimedEvent sender);

     
        public static void ResetTime()
        {
            Year = 1;
            Month = 1;
            Week = 1;
            Day = 1;
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
                    return "Abutrecdo";

                case 2:
                    return "Braunildo";

                case 3:
                    return "Canildo";

                case 4:
                    return "Dorimado";

                case 5:
                    return "Enalildo";

                case 6:
                    return "Foconildo";

                case 7:
                    return "Gaunetdo";

                default:
                    return "Invalid";
            }

        }

        public static string GetMonthName()
        {
            switch (Month)
            {
                case 1:
                    return "Abrilze";

                case 2:
                    return "Bonocze";

                case 3:
                    return "Cauteze";

                case 4:
                    return "Dimalze";
 
                default: 
                    return "Invalid";
            }
        }

        public static void Update()
        {
            Frames += 1;
 
            if (Frames >= Int32.Parse(Registry.ReadKeyValue("/fps"))) { Second += 1; Frames = 0; CheckTimedEvents(); }
            if (Second >= 13) { Minute += 1; Second = 1; }
            if (Minute >= 13) { Hour += 1; Minute = 1; }
            if (Hour >= 7) { Day += 1; Hour = 1; }
            if (Day >= 5) { Week += 1; Day = 1; }
            if (Week >= 6) { Month += 1; Week = 1; }
            if (Month >= 5) { Year += 1; Month = 1; }
            if (Year >= 42) { Year = 41; }
 
        }

        public static void CheckTimedEvents()
        {            
            foreach(KeyValuePair<string, TimedEvent> ceira in TimedEvents)
            {
                if (ceira.Value.WaitingDeletion) { TimedEvents.Remove(ceira.Key); continue; }
 
                ceira.Value.CheckTime();
            }
        }


    }
}
