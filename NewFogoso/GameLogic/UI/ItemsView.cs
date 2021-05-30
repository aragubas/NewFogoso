using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using MonoGame.Extended;
 
namespace Fogoso.GameLogic.UI
{
    class ItemsViewItem
    {
        public Vector2 RenderOffset;
        public Rectangle Rectangle;
        public Rectangle ContentArea;
        SpriteFont Font;
        Vector2 TextSize;
        string TitleText;
        public bool Enabled = true;
        public int Opacity = 0;
        public List<string> Properties;
  
        // Event Handler
        internal delegate void delRenderContent(SpriteBatch spriteBatch, ItemsViewItem sender);
        public event delRenderContent RenderContentEvent;
        internal delegate void delUpdateContent(ItemsViewItem sender);
        public event delUpdateContent UpdateContentEvent;
    
        public void RenderContent(SpriteBatch spriteBatch)
        {
            if (RenderContentEvent != null) { RenderContentEvent.Invoke(spriteBatch, this); }
        }
  
        public void UpdateContent(ItemsViewItem sender)
        {
            if (UpdateContentEvent != null && Enabled) { UpdateContentEvent.Invoke(this); }
        }
 
        public ItemsViewItem(string Name, List<string> pProperties=null)
        {
            RenderOffset = new Vector2(0, 0);
            Rectangle = new Rectangle(0, 0, 250, 300);
            TitleText = Name;
            Properties = pProperties;
   
            Font = Fonts.GetSpriteFont(Fonts.GetFontDescriptor("PressStart2P", Fonts.SmallFontSize));
            TextSize = Font.MeasureString(TitleText);
              
            // Adjust Text Size
            if (TextSize.X > Rectangle.Width)
            {
                while (TextSize.X > Rectangle.Width)
                {
                    TitleText = TitleText.Remove(TitleText.Length - 1, 1);
                    TextSize = Font.MeasureString(TitleText);
                }
                TitleText = TitleText.Remove(TitleText.Length - 4, 3);
                TitleText += "...";
            }

        }
 
        public void SetRectangle(Rectangle newRectangle)
        {
            Rectangle = newRectangle;
            RenderOffset = new Vector2(Rectangle.X, Rectangle.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        { 
            if (Enabled && Opacity < 255) { Opacity += 20; } 

            // Render Background
            spriteBatch.FillRectangle(new Rectangle((int)RenderOffset.X, (int)RenderOffset.Y, Rectangle.Width, Rectangle.Height), Color.FromNonPremultiplied(50, 50, 56, Opacity - 55), 0);
            
            // Render Text Background
            spriteBatch.FillRectangle(new Rectangle((int)RenderOffset.X, (int)RenderOffset.Y, Rectangle.Width, (int)TextSize.Y), Color.FromNonPremultiplied(65, 65, 78, Opacity - 10), 0);
            spriteBatch.DrawString(Font, TitleText, new Vector2(RenderOffset.X, RenderOffset.Y), Color.FromNonPremultiplied(255, 255, 255, Opacity));
            
            // Render Content
            RenderContent(spriteBatch);
             
        }

    } 
 
    class ItemsView : UIControl
    {
        private Color PanelColor;
        List<ItemsViewItem> ItemsCollection;
        private int InputReservedID;
         public int ItemsSpacing = 8;
        private int Scroll;
        private int LastMouseX;
        private bool ScrollHoldStarted;
        private bool ScrollingEnabled = false;
        private int TotalWidth;
        private bool Sinas;

        public ItemsView(Rectangle pRectangle)
        {
            // Set to loading cursor
            GameInput.CursorImage = "loading.png";

            ItemsCollection = new List<ItemsViewItem>();
  
            InputReservedID = GameInput.GenerateInputReservedID();

            PanelColor = Color.DimGray;

            // Set Rectangle
            SetRectangle(pRectangle);
        }
 
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Draw background
            spriteBatch.DrawRectangle(new Rectangle(0, 0, Rectangle.Width, Rectangle.Height), Color.FromNonPremultiplied(PanelColor.R + 100, PanelColor.G + 100, PanelColor.B + 100, PanelColor.A), 2);
            spriteBatch.FillRectangle(new Rectangle(1, 1, Rectangle.Width - 2, Rectangle.Height - 2), PanelColor);

            for (int i = 0; i < ItemsCollection.Count; i++)
            {
                if (!ItemsCollection[i].Enabled){ continue; }
                ItemsCollection[i].Draw(spriteBatch);
            }


        }

        public void AddItem(ItemsViewItem item)
        {
            ItemsCollection.Add(item);
            AutoArrange();
        }

        public override void Update()
        {
            base.Update(); 

            for (int i = 0; i < ItemsCollection.Count; i++)
            {
                if (ItemsCollection[i].RenderOffset.X + ItemsCollection[i].Rectangle.Width < 0 || ItemsCollection[i].RenderOffset.X - ItemsCollection[i].Rectangle.Width > Rectangle.Width)
                {
                    ItemsCollection[i].Enabled = false;
                    ItemsCollection[i].Opacity = 0;
                }else { ItemsCollection[i].Enabled = true; ItemsCollection[i].UpdateContent(ItemsCollection[i]); }


            }

            if (!Sinas)
            {
                Sinas = true;
                // Initialize Stuff
                ItemsCollection = new List<ItemsViewItem>();
    
                for (int i = 0; i< 20; i++)
                {
                    AddItem(new ItemsViewItem($"Enceiradoaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa {i}"));
                }

            }
 
            // Move when Scrolling
            if (ScrollHoldStarted && ScrollingEnabled)
            {
                GameInput.ReservedModeID = InputReservedID;

                if (Mouse.GetState().MiddleButton == ButtonState.Released)
                {
                    ScrollHoldStarted = false;
                    LastMouseX = 0;
                    GameInput.ReservedModeID = -1;

                }
                else
                {
                    SetScrollX((int)GameInput.CursorPosition.X - LastMouseX);

                }
            }
  
            // Start Scrolling if enabled
            if (GameInput.CursorColision.Intersects(new Rectangle((int)PositionOffset.X, (int)PositionOffset.Y, Rectangle.Width, Rectangle.Height)) && ScrollingEnabled)
            {
                if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
                {
                    //Scroll = 4;
                    if (!ScrollHoldStarted)
                    {
                        ScrollHoldStarted = true;
                        LastMouseX = (int)GameInput.CursorPosition.X + Math.Abs(Scroll);
                    }
                }

            }


        }

        public void SetScrollX(int ScrollValue)
        {
            if (!ScrollingEnabled) { return; }
            Scroll = ScrollValue;
            if (Scroll > 4)
            { 
                Scroll = 4;
            }

            // Limit Scroll
            if (Math.Abs(Scroll - Rectangle.Width + 4) > TotalWidth)
            {
                Scroll = -(TotalWidth - Rectangle.Width + 4);
            }

            // Set Position for All Controls
            for (int i = 0; i < ItemsCollection.Count; i++)
            {
                if (i == 0) { ItemsCollection[i].SetRectangle(new Rectangle(Scroll, ItemsCollection[i].Rectangle.Y, ItemsCollection[i].Rectangle.Width, ItemsCollection[i].Rectangle.Height)); continue; }
                ItemsCollection[i].SetRectangle(new Rectangle(ItemsCollection[i - 1].Rectangle.X + ItemsCollection[i - 1].Rectangle.Width + ItemsSpacing, ItemsCollection[i].Rectangle.Y, ItemsCollection[i].Rectangle.Width, ItemsCollection[i].Rectangle.Height));
 
            }

        }
  
        public void AutoArrange()
        {
            Scroll = 0;

            // Set to loading cursor
            GameInput.CursorImage = "loading.png";
            TotalWidth = 0;

            // Calculate Total Width
            for (int i = 0; i < ItemsCollection.Count; i++)
            {
                ItemsCollection[i].SetRectangle(new Rectangle(ItemsCollection[i].Rectangle.X, 4, ItemsCollection[i].Rectangle.Width, ItemsCollection[i].Rectangle.Height));
                TotalWidth += ItemsCollection[i].Rectangle.Width + ItemsSpacing;

            }


            // Set all Item's X to the previuos item X plus its width and item spacing
            for (int i = 0; i < ItemsCollection.Count; i++)
            {
                if (i == 0) { ItemsCollection[i].SetRectangle(new Rectangle(ItemsSpacing, ItemsSpacing, ItemsCollection[i].Rectangle.Width, ItemsCollection[i].Rectangle.Height)); continue; }
                ItemsCollection[i].SetRectangle(new Rectangle(ItemsCollection[i - 1].Rectangle.X + ItemsCollection[i - 1].Rectangle.Width + ItemsSpacing, ItemsSpacing, ItemsCollection[i].Rectangle.Width, ItemsCollection[i].Rectangle.Height));

            }
 
            // Center Horizontally all controls
            for (int i = 0; i < ItemsCollection.Count; i++)
            {
                Rectangle rectCopy = ItemsCollection[i].Rectangle;
                rectCopy.Y = Rectangle.Height / 2 - rectCopy.Height / 2;
 
                ItemsCollection[i].SetRectangle(rectCopy);

            }
 
            // Set Scrolling
            ScrollingEnabled = TotalWidth >= Rectangle.Width;
 

        }
 
        public override void SetRectangle(Microsoft.Xna.Framework.Rectangle pRectangle)
        {
            base.SetRectangle(pRectangle);
        }

    }
}
