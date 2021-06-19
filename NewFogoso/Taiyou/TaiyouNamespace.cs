using System.Collections.Generic;

namespace Fogoso.Taiyou
{

    public enum AccessLevel : byte
    {
        Private,
        Public
    }

    public class TaiyouNamespace
    {
        public string NamespaceName = "default";
        
        // Variables
        public List<Variable> VarList = new List<Variable>();

        // Routines
        //public List<string> Routines_Keys = new List<string>();
        //public List<List<TaiyouLine>> Routines_Data = new List<List<TaiyouLine>>();

        public List<TaiyouRoutine> RoutineList = new List<TaiyouRoutine>();

        public TaiyouNamespace(string pNamespaceName)
        {
            pNamespaceName = NamespaceName;
        }

    }


}