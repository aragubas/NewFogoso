using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogoso.GameLogic.UI
{
    class Button : UIControl
    {
        // Event Handler
        #region ButtonPress Event Handler
        // Declare Event Handler
        public delegate void ButtonPressEventHandler(Button sender);
        public event ButtonPressEventHandler ButtonPress;

        // Method
        protected virtual void OnButtonPress()
        {
            if (ButtonPress != null) { ButtonPress(this); }
        }

        #endregion

        public string Text;
        Vector2 Size;
        Vector2 Location;
        Rectangle ColisionWithOffset;
        Rectangle cursorRect;
        int BorderSize = 2;
        Color _backColor;
        Color _foreColor;

        public Button(Vector2 pLocation, string pText)
        {
            Text = pText;

            // Set minimum size
            MinWidth = (int)Game1.Reference.Content.Load<SpriteFont>("default").MeasureString(Text).X;
            MinHeight = (int)Game1.Reference.Content.Load<SpriteFont>("default").MeasureString(Text).Y;

            Location = pLocation;
            Location.X += BorderSize;
            Location.Y += BorderSize;

            Size = new Vector2(MinWidth, MinHeight);

            Rectangle = new Rectangle((int)Location.X - BorderSize, (int)Location.Y - BorderSize, (int)Size.X + BorderSize * 2, (int)Size.Y + BorderSize * 2);

            // Initialize some stuff
            cursorRect = new Rectangle(0, 0, 1, 1);

            // Set Default Colors
            SetColor(0);

        }

        private void SetColor(short State)
        {
            switch (State)
            {
                case 0:
                    _backColor = Color.FromNonPremultiplied(42, 42, 42, 100);
                    _foreColor = Color.FromNonPremultiplied(128, 128, 128, 230);
                    break;

                case 1:
                    _backColor = Color.FromNonPremultiplied(64, 64, 64, 100);
                    _foreColor = Color.FromNonPremultiplied(230, 230, 230, 255);
                    break;

            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprites.GetSprite("/base.png"), Rectangle, _backColor);

            spriteBatch.DrawString(Game1.Reference.Content.Load<SpriteFont>("default"), Text, new Vector2(Location.X, Location.Y), _foreColor);
        }

        public override void Update()
        {
            base.Update();

            SetColor(0);
            
            // Update Rectangles
            cursorRect.Y = (int)GameInput.CursorPosition.Y;
            cursorRect.X = (int)GameInput.CursorPosition.X;
            /////////////////////////////////////////////////
            ColisionWithOffset.X = (int)PositionOffset.X;
            ColisionWithOffset.Y = (int)PositionOffset.Y;
            ColisionWithOffset.Width = (int)Rectangle.Width;
            ColisionWithOffset.Height = (int)Rectangle.Height;


            if (cursorRect.Intersects(ColisionWithOffset)) 
            {
                GameInput.CursorImage = "selection.png";
                SetColor(1);
            }


        }


    }
}
