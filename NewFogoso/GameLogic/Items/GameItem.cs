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

        public ItemMetadata(string KeyInfoPath)
        {
            ItemName = Registry.ReadKeyValue($"item_info/{KeyInfoPath}/name");
            ItemDescription = Registry.ReadKeyValue($"item_info/{KeyInfoPath}/description");
            ItemIcon = Registry.ReadKeyValue($"item_info/{KeyInfoPath}/icon"); 
            ItemKeyInfo = KeyInfoPath;
        } 
        public string ItemName;
        public string ItemDescription;
        public string ItemIcon;
        public string ItemKeyInfo;
    }
    
    public class ItemSummaryInfosObject
    {
        // Private fields for Item Summary
        private Rectangle IconRect;
        private Rectangle QuantityAreaRect;
        private Rectangle ItemDescriptionRect;
        private bool FirstUpdated;
        private GameItem rootGameItem;

        // Public fields
        public bool DisableDraw;


        // Caching Fields
        private bool Cached;
        private Texture2D ItemIconTexture;
        SpriteFont QuantityInfoFont;
        private string ItemDescriptionText;
        private string QuantityInfoText;

        public ItemSummaryInfosObject(GameItem pRootGameItem)
        {
            rootGameItem = pRootGameItem;
        }

        void CacheDrawing()
        {
            // Set Loading Cursor
            GameInput.CursorImage = "loading.png";
             
            Cached = true;
            ItemIconTexture = Sprites.GetSprite($"item_icon/" + rootGameItem.Metadata.ItemIcon);
            QuantityInfoFont = Fonts.GetSpriteFont(Fonts.GetFontDescriptor("PressStart2P", Fonts.SmallFontSize));
            ItemDescriptionText = Utils.WrapText(QuantityInfoFont, rootGameItem.Metadata.ItemDescription, ItemDescriptionRect.Width);
            QuantityInfoText = Utils.WrapText(QuantityInfoFont, $"You have {rootGameItem.Quantity}", QuantityAreaRect.Width - 4);

        }

        public void Draw(SpriteBatch spriteBatch, ItemsViewItem sender)
        {
            if (!FirstUpdated || DisableDraw) { return; }
             
            // Cache Icons and Stuff 
            if (!Cached) { CacheDrawing(); }
              
            // Set whiteNeutral Color for Opacity Effects
            Color whiteNeutral = Color.FromNonPremultiplied(255,255, 255, sender.Opacity);

            // Draw Item icon
            spriteBatch.Draw(ItemIconTexture, IconRect, whiteNeutral);

            // Draw Quantity
            spriteBatch.DrawString(QuantityInfoFont, QuantityInfoText, new Vector2(sender.Rectangle.X + 2, QuantityAreaRect.Y + 2), whiteNeutral);

            // Draw Item Description
            Utils.DrawStringBoundaries(spriteBatch, QuantityInfoFont, ItemDescriptionText, ItemDescriptionRect, whiteNeutral);

        }

        public void Update(ItemsViewItem sender)
        {
            if (!FirstUpdated) { FirstUpdated = true; }
            IconRect = new Rectangle(sender.Rectangle.X + 5, sender.Rectangle.Y + 12, 64, 64);
            QuantityAreaRect = new Rectangle(sender.Rectangle.X, sender.Rectangle.Y + 78, sender.Rectangle.Width, 16);
            ItemDescriptionRect = new Rectangle(sender.Rectangle.X + 1, sender.Rectangle.Y + 98, sender.Rectangle.Width - 2, sender.Rectangle.Height - 92);

        }
    }

    public abstract class GameItem
    {
        public AragubasTimeObject ActivationTime;
        public ItemMetadata Metadata;
        public float MaintenanceCost = 1f;
        public int Quantity = 1;
        public ItemSummaryInfosObject itemSummary;
          
        public virtual void UpdateLogic() { }
         
    }
}