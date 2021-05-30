namespace Fogoso.GameLogic.Items
{
    public class byuu : GameItem
    {
        public byuu()
        {
            Metadata = new ItemMetadata("byuu", "@byuu_san", "byuu.png", "byuu");
            ActivationTime = new AragubasTimeObject(2, 0, 0, 0, 0, 0);
                         
        }

    }

}