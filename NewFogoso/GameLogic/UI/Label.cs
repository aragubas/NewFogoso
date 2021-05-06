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
        string Text;
        Color TextColor;
        Vector2 lastTextSize;

        #region DrawBackgroundEventHandler
        private delegate void DrawBackgroundEventHandler(Vector2 TextSize, SpriteBatch spriteBatch);
        private event DrawBackgroundEventHandler DrawBackground;

        // Method
        protected virtual void OnDrawBackground(SpriteBatch spriteBatch)
        {
            if (DrawBackground != null) { DrawBackground(GetTextSize(), spriteBatch); }
        }

        #endregion

        public Label(Vector2 pPosition, SpriteFont pFont, string pText)
        {
            Text = pText;
            Font = pFont;
            Position = pPosition;

            TextColor = Color.White;

        }

        public void SetTextColor(Color pNewColor)
        {
            TextColor = pNewColor;
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
            Text = pNewText;
            lastTextSize = Font.MeasureString(Text);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            OnDrawBackground(spriteBatch);
            spriteBatch.DrawString(Font, Text, Position, TextColor);

        } 


    }
}
