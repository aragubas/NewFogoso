using Fogoso.GameLogic.Items;

namespace Fogoso.GameLogic
{
    public static class Maintenance
    {
        public static float CalculateItemsMaintenance()
        {
            float TotalMaintenance = CurrentSessionData.BaseMaintenance;
            for (int i = 0; i < CurrentSessionData.UserItems.Count; i++)
            {
                TotalMaintenance += CurrentSessionData.UserItems[i].MaintenanceCost;
            }

            return TotalMaintenance;
        }
    }
}