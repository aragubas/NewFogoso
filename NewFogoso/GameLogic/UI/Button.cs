using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
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
        public void PeformClick()
        {
            Sound.PlaySound("hud/click", 0.2f);
            if (ButtonPress != null) { ButtonPress(this); }
        }
 
        #endregion

        public string Text;
        Vector2 Size;
        Vector2 Location;
        int BorderSize = 2;
        Color _backColor;
        Color _foreColor;
        MouseState oldState;
        SpriteFont Font;
        bool InactiveUpdateRan;
        private bool MouseOverSoundPlayed;

        public Button(Vector2 pLocation, string pText)
        {
            // Change to Loading Cursor
            GameInput.CursorImage = "loading.png";

            Text = pText;

            // Set minimum size
            Font = Fonts.GetSpriteFont(Fonts.GetFontDescriptor("PressStart2P", Fonts.DefaultFontSize));
            MinWidth = (int)Font.MeasureString(Text).X;
            MinHeight = (int)Font.MeasureString(Text).Y;

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
            spriteBatch.FillRectangle(Rectangle, _backColor);

            spriteBatch.DrawString(Font, Text, new Vector2(Location.X, Location.Y), _foreColor);
        }

        public override void InactiveUpdate()
        {
            if (!InactiveUpdateRan) { InactiveUpdateRan = true; } else { return; }
            MouseOverSoundPlayed = false;
            SetColor(-1);

        }

        public override void Update()
        {
            if (GameInput.ReservedModeID != -1) { SetColor(-1); return; }
            base.Update();
            InactiveUpdateRan = false; 

            if (!MouseOverSoundPlayed)
            {
                MouseOverSoundPlayed = true;
                Sound.PlaySound("hud/select", 0.02f);

            }

            //Set Default Color 
            SetColor(0);

            // Get New State
            MouseState newState = Mouse.GetState();

            GameInput.CursorImage = "selection.png";
            SetColor(1);

            // Detect MouseDown Event
            if (newState.LeftButton == ButtonState.Released) { SetColor(2); }
 
            // Detect MouseUp Event
            if (newState.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed) { SetColor(3); PeformClick(); }

            // Set OldState
            oldState = newState;
        }


    }
}
