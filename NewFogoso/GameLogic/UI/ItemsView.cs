using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogoso.GameLogic.UI
{
    class ItemsViewItem
    {
        public Vector2 RenderOffset;
        public Rectangle Rectangle;

        public ItemsViewItem(string Name)
        {
            RenderOffset = new Vector2(0, 0);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprites.GetSprite("/base.png"), new Rectangle((int)RenderOffset.X, (int)RenderOffset.Y, Rectangle.Width, Rectangle.Height), Color.FromNonPremultiplied(100, 100, 100, 200));

        }

    }
    class ItemsView : UIControl
    {
        private Color PanelColor;
        List<ItemsViewItem> ItemsCollection;

        public ItemsView(Rectangle pRectangle)
        {
            // Set to loading cursor
            GameInput.CursorImage = "loading.png";

            // Initialize Stuff
            ItemsCollection = new List<ItemsViewItem>();

            // Set Rectangle
            SetRectangle(pRectangle);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Draw background
            spriteBatch.Draw(Sprites.GetSprite("/base.png"), new Rectangle(0, 0, Rectangle.Width, Rectangle.Height), Color.FromNonPremultiplied(PanelColor.R + 100, PanelColor.G + 100, PanelColor.B + 100, PanelColor.A));
            spriteBatch.Draw(Sprites.GetSprite("/base.png"), new Rectangle(1, 1, Rectangle.Width - 2, Rectangle.Height - 2), PanelColor);

            for (int i = 0; i < ItemsCollection.Count; i++)
            {
                ItemsCollection[i].Draw(spriteBatch);
            }


        }

        public override void Update()
        {
            base.Update(); 
        }

        public override void SetRectangle(Microsoft.Xna.Framework.Rectangle pRectangle)
        {
            base.SetRectangle(pRectangle);
        }

    }
}
