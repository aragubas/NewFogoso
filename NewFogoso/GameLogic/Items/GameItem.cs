using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Fogoso.GameLogic.UI;
using MonoGame.Extended;

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
        /// <param name="KeyInfoPath">Item KeyFile's data (inside GameData/source/reg/item_info)<param>

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
        public float MaintenanceCost = 1f;
        public int Quantity = 1;
 
        public void RenderItemViewSummary(SpriteBatch spriteBatch, ItemsViewItem sender)
        {
            Rectangle IconRect = new Rectangle(sender.Rectangle.X + 5, sender.Rectangle.Y + 12, 64, 64);
            Rectangle QuantityInfoArea = new Rectangle(sender.Rectangle.X, sender.Rectangle.Y + 78, sender.Rectangle.Width, 16);
            Color whiteNeutral = Color.FromNonPremultiplied(255,255, 255, sender.Opacity);

            spriteBatch.Draw(Sprites.GetSprite($"item_icon/" + Metadata.ItemIcon), IconRect, whiteNeutral);

 
            spriteBatch.DrawRectangle(QuantityInfoArea, Color.Red, 1);
            SpriteFont font1 = Fonts.GetSpriteFont(Fonts.GetFontDescriptor("PressStart2P", Fonts.SmallFontSize));
   
            spriteBatch.DrawString(font1, Utils.WrapText(font1, $"You have {Quantity}", sender.Rectangle.Width - 4), new Vector2(sender.Rectangle.X + 2, QuantityInfoArea.Y + 2), whiteNeutral);
            
        }


        public void UpdateItemViewSummary(ItemsViewItem sender)
        {
            
        } 
         


        public virtual void UpdateLogic() { }
    }
}