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
    class MapSelector : UIControl
    {

        Rectangle CenterColision;
        Rectangle LeftColision;
        Rectangle RightColision;
        public int SelectedMap = -1;

        int CenterOffset;
        int LeftOffset;
        int RightOffset;


        Color CenterColor = Color.White;
        Color LeftColor = Color.White;
        Color RightColor = Color.White;

        
        //
        // This object has fixed size of: 568, 370
        //
        public MapSelector(Vector2 Position)
        {
            SetRectangle(new Rectangle((int)Position.X, (int)Position.Y, 0, 0));

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Sprites.GetSprite("/map_renderer/bg.png"), new Rectangle(Rectangle.X, Rectangle.Y, 568, 370), Color.White);

            spriteBatch.Draw(Sprites.GetSprite("/map_renderer/left.png"), new Rectangle(Rectangle.X + LeftOffset, Rectangle.X + LeftOffset, 568, 370), LeftColor);
            spriteBatch.Draw(Sprites.GetSprite("/map_renderer/center.png"), new Rectangle(Rectangle.X + CenterOffset, Rectangle.X + CenterOffset, 568, 370), CenterColor);
            spriteBatch.Draw(Sprites.GetSprite("/map_renderer/right.png"), new Rectangle(Rectangle.X + RightOffset, Rectangle.X + RightOffset, 568, 370), RightColor);

        }

        public override void Update()
        {
            base.Update();
            // =====================================
            // ABSOLUTELY THE BEST WAY TO DO THIS
            // YES THE BEST WAY EVER
            // TO DO THAT
            // =====================================
            CenterColision = new Rectangle(Rectangle.X + 215, Rectangle.Y + 130, 170, 170);
            LeftColision = new Rectangle(Rectangle.X, Rectangle.Y, 150, 370);
            RightColision = new Rectangle(Rectangle.X + 568 - 150, Rectangle.Y + 10, 150, 370);


            if (GameInput.CursorColision.Intersects(CenterColision) || SelectedMap == 0)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed) { SelectedMap = 0; }
                CenterColor = Color.FromNonPremultiplied(120, 120, 120, 255);

                CenterOffset = -2;

                // Change to Selection cursor
                if (GameInput.CursorColision.Intersects(CenterColision)) { GameInput.CursorImage = "selection.png"; }

            }
            else { CenterColor = Color.White; CenterOffset = 0; }

            if (GameInput.CursorColision.Intersects(LeftColision) || SelectedMap == 1)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed) { SelectedMap = 1; }
                LeftColor = Color.FromNonPremultiplied(120, 120, 120, 255);

                LeftOffset = -2;

                // Change to Selection cursor
                if (GameInput.CursorColision.Intersects(LeftColision)) { GameInput.CursorImage = "selection.png"; }

            }
            else { LeftColor = Color.White; LeftOffset = 0; }

            if (GameInput.CursorColision.Intersects(RightColision) || SelectedMap == 2)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed) { SelectedMap = 2; }
                RightColor = Color.FromNonPremultiplied(120, 120, 120, 255);

                RightOffset = -2;

                // Change to Selection cursor
                if (GameInput.CursorColision.Intersects(RightColision)) { GameInput.CursorImage = "selection.png"; }

            }
            else { RightColor = Color.White; RightOffset = 0; }

            switch (SelectedMap)
            {
                case 0:
                    CenterColor = Color.FromNonPremultiplied(200, 100, 230, 255);
                    break;

                case 1:
                    LeftColor = Color.FromNonPremultiplied(200, 100, 230, 255);
                    break;

                case 2:
                    RightColor = Color.FromNonPremultiplied(200, 100, 230, 255);
                    break;


            }


        }

        public override void SetRectangle(Rectangle pRectangle)
        {
            base.SetRectangle(pRectangle);
            Rectangle = new Rectangle(pRectangle.X, pRectangle.Y, 568, 370);
        } 




    }
}
