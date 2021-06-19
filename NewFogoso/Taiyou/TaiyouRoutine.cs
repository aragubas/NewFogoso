using System.Collections.Generic;

namespace Fogoso.Taiyou
{
    public enum RoutineAccessLevel : byte
    {
        Private,
        Public
    }

    public class TaiyouRoutine
    {
        public List<TaiyouLine> RoutineLines = new List<TaiyouLine>();
        public string RoutineName = "";
        public RoutineAccessLevel AccessLevel;
        
        public TaiyouRoutine(List<TaiyouLine> pRoutineLines, string pRoutineName, RoutineAccessLevel pAccessLevel)
        {
            RoutineLines = pRoutineLines;
            RoutineName = pRoutineName;
            AccessLevel = pAccessLevel;

        }


    }
}