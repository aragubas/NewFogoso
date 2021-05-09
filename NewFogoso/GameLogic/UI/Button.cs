using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            Sound.PlaySound("hud/click", 0.2f);
            if (ButtonPress != null) { ButtonPress(this); }
        }

        #endregion

        public string Text;
        Vector2 Size;
        Vector2 Location;
        Rectangle ColisionWithOffset;
        int BorderSize = 2;
        Color _backColor;
        Color _foreColor;
        MouseState oldState;
        Rectangle cursorRect;
        bool InactiveUpdateRan;

        public Button(Vector2 pLocation, string pText)
        {
            // Change to Loading Cursor
            GameInput.CursorImage = "loading.png";

            Text = pText;

            // Set minimum size
            MinWidth = (int)Game1.Reference.Content.Load<SpriteFont>("default").MeasureString(Text).X;
            MinHeight = (int)Game1.Reference.Content.Load<SpriteFont>("default").MeasureString(Text).Y;

            Location = pLocation;
            Location.X += BorderSize;
            Location.Y += BorderSize;

            Size = new Vector2(MinWidth, MinHeight);

            Rectangle = new Rectangle((int)Location.X - BorderSize, (int)Location.Y - BorderSize, (int)Size.X + BorderSize * 2, (int)Size.Y + BorderSize * 2);

            // Set Default Colors
            SetColor(0);

            OnlyUpdateWhenMouseHover = true;

        }

        public override void SetRectangle(Microsoft.Xna.Framework.Rectangle pRectangle)
        {
            base.SetRectangle(pRectangle);

            Location = new Vector2(Rectangle.X, Rectangle.Y);
            Location.X += BorderSize;
            Location.Y += BorderSize;

        }

        private void SetColor(short State)
        {
            switch (State)
            {
                case -1:
                    _backColor = Color.FromNonPremultiplied(12, 12, 12, 200);
                    _foreColor = Color.FromNonPremultiplied(92, 92, 92, 230);
                    break;

                case 0:
                    _backColor = Color.FromNonPremultiplied(42, 42, 42, 200);
                    _foreColor = Color.FromNonPremultiplied(128, 128, 128, 230);
                    break;

                case 1:
                    _backColor = Color.FromNonPremultiplied(64, 64, 64, 200);
                    _foreColor = Color.FromNonPremultiplied(230, 230, 230, 255);
                    break;

                case 2:
                    _backColor = Color.FromNonPremultiplied(94, 94, 94, 200);
                    _foreColor = Color.FromNonPremultiplied(255, 255, 255, 255);
                    break;

                case 3:
                    _backColor = Color.FromNonPremultiplied(255, 255, 255, 200);
                    _foreColor = Color.FromNonPremultiplied(94, 94, 94, 255);
                    break;

            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprites.GetSprite("/base.png"), Rectangle, _backColor);

            spriteBatch.DrawString(Game1.Reference.Content.Load<SpriteFont>("default"), Text, new Vector2(Location.X, Location.Y), _foreColor);
        }

        public override void InactiveUpdate()
        {
            if (!InactiveUpdateRan) { InactiveUpdateRan = true; } else { return; }
            SetColor(-1);

        }

        public override void Update()
        {
            if (!IsEnabled) { SetColor(-1); }
            base.Update();
            InactiveUpdateRan = false;

            //Set Default Color
            SetColor(0);

            // Get New State
            MouseState newState = Mouse.GetState();

            GameInput.CursorImage = "selection.png";
            SetColor(1);

            // Detect MouseDown Event
            if (newState.LeftButton == ButtonState.Released) { SetColor(2); }

            // Detect MouseUp Event
            if (newState.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed) { SetColor(3); OnButtonPress(); }

            // Set OldState
            oldState = newState;
        }


    }
}
