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
        // Namespace Name
        public string NamespaceName;
          
        // Variables
        //public List<Variable> VarList = new List<Variable>();
        public Dictionary<string, Variable> VariableDict = new Dictionary<string, Variable>();

        // Routines
        public List<TaiyouRoutine> RoutineList = new List<TaiyouRoutine>();

        public TaiyouNamespace(string pNamespaceName)
        {
            pNamespaceName = NamespaceName;
        }
 
    }


}