using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogoso.GameLogic.UI
{
    class Label : UIControl
    {
        Vector2 Position;
        SpriteFont Font;
        public string Text;
        Color TextColor;
        public short Opacity = 255;
        Vector2 lastTextSize;
 
        #region DrawBackgroundEventHandler
        public delegate void DrawBackgroundEventHandler(Rectangle Rect, SpriteBatch spriteBatch, Label sender);
        public event DrawBackgroundEventHandler DrawBackground;

        // Method
        protected virtual void OnDrawBackground(SpriteBatch spriteBatch)
        {
            if (DrawBackground != null) { DrawBackground(new Rectangle((int)Position.X, (int)Position.Y, (int)lastTextSize.X, (int)lastTextSize.Y), spriteBatch, this); }
        }

        #endregion

        public Label(Vector2 pPosition, FontDescriptor pFont, string pText)
        {
            Text = pText;
            Font = Fonts.GetSpriteFont(pFont);
            Position = pPosition;
 
            TextColor = Color.White;
            lastTextSize = Font.MeasureString(Text);
        }

        public void SetTextColor(Color pNewColor, short pOpacity)
        {
            TextColor = pNewColor; 
            Opacity = pOpacity;
        }

        public override void SetRectangle(Rectangle newRect)
        {
            base.SetRectangle(newRect);
            Position.X = newRect.X;
            Position.Y = newRect.Y;
            
        }
        
        /// <summary>
        /// Get TextSize from cache
        /// </summary>
        /// <returns>Vector2 with size</returns>
        public Vector2 GetTextSize()
        {
            if (lastTextSize == null) { lastTextSize = Font.MeasureString(Text); }

            return lastTextSize;
        } 

        /// <summary> 
        /// Set Size and Re-Measure the string
        /// </summary>
        /// <param name="pNewText">New text string</param>
        public void SetText(string pNewText)
        {
            if (Text == pNewText) { return; }
            Text = pNewText;
            lastTextSize = Font.MeasureString(Text);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            OnDrawBackground(spriteBatch);
            spriteBatch.DrawString(Font, Text, Position, Color.FromNonPremultiplied(TextColor.R, TextColor.G, TextColor.B, Opacity));

        } 


    }
}
