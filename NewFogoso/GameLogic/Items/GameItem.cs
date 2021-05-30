namespace Fogoso.GameLogic.Items
{
    public class ItemMetadata
    {
        /// <summary>
        /// Item Metadata
        /// </summary>
        /// <param name="Name">Item Name (will be displayed at Items View)<param>
        /// <param name="Description">A short description on what it is<param>
        /// <param name="Icon">Icon Path (inside GameData/source/image/item_icon)<param>
        /// <param name="KeyInfoPath">Item KeyFile's data (inside GameData/sorce/reg/item_info)<param>

        public ItemMetadata(string Name, string Description, string Icon, string KeyInfoPath)
        {
            ItemName = Name;
            ItemDescription = Description;
            ItemIcon = Icon;
            ItemKeyInfo = KeyInfoPath;
        } 
        public string ItemName;
        public string ItemDescription;
        public string ItemIcon;
        public string ItemKeyInfo;
    }
    
    public abstract class GameItem
    {
        public AragubasTimeObject ActivationTime;
        public ItemMetadata Metadata;
        public float MaintenanceCost;
        public int Quantity;

        public void Update()
        {
            
        }

        public virtual void UpdateLogic() { }
    }
}