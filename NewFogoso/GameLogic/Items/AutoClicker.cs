namespace Fogoso.GameLogic.Items
{
    public class AutoClicker : GameItem
    { 
        public AutoClicker()
        { 
            Metadata = new ItemMetadata("autoClicker");
            ActivationTime = new AragubasTimeObject(1, 0, 0, 0, 0, 0);
            
        }
 
        public override void UpdateLogic()
        { 
            if (!ActivationTime.TimeTriggered()){ return; }
  
            
        }
 
    }

}