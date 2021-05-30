using Fogoso.GameLogic.Items;
using System.Collections.Generic;

namespace Fogoso.GameLogic
{
    public class CurrentSessionData
    {
        // Wax Variables
        public static float Ceira = 2;
        public static float CeiraPerWorkunit = 0.05f;
        public static float CeiraWorkunitBonusMultiplier = 1.5f;
        public static int Experience;
        public static List<GameItem> UserItems;
 
 
        public static void Reset()
        {
            Ceira = 2;
            CeiraPerWorkunit = 0.1f;
            CeiraWorkunitBonusMultiplier = 2.9f;
            Experience = 0;
            UserItems = new List<GameItem>();

        }
    }
}
