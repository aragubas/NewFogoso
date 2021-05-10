using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogoso.GameLogic.Screens
{
    class EconomicalMap : GameScreen
    {
        public EconomicalMap()
        {

        }

        Rectangle CenterColision;
        Rectangle LeftColision;
        Rectangle RightColision;

        Color CenterColor = Color.White;
        Color LeftColor = Color.White;
        Color RightColor = Color.White;

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Sprites.GetSprite("/map_renderer/bg.png"), new Rectangle(10, 10, 568, 370), Color.White);
            spriteBatch.Draw(Sprites.GetSprite("/map_renderer/left.png"), new Rectangle(10, 10, 568, 370), LeftColor);
            spriteBatch.Draw(Sprites.GetSprite("/map_renderer/center.png"), new Rectangle(10, 10, 568, 370), CenterColor);
            spriteBatch.Draw(Sprites.GetSprite("/map_renderer/right.png"), new Rectangle(10, 10, 568, 370), RightColor);

            spriteBatch.End();

        }

        public override void Update()
        {
            CenterColision = new Rectangle(215, 130, 170, 170);
            LeftColision = new Rectangle(10, 10, 200, 370);
            RightColision = new Rectangle(568 - 150, 10, 150, 370);

            if (GameInput.CursorColision.Intersects(CenterColision))
            {
                CenterColor = Color.FromNonPremultiplied(120, 120, 120, 255);
            }
            else { CenterColor = Color.White; }

            if (GameInput.CursorColision.Intersects(LeftColision))
            {
                LeftColor = Color.FromNonPremultiplied(120, 120, 120, 255);
            }
            else { LeftColor = Color.White; }

            if (GameInput.CursorColision.Intersects(RightColision))
            {
                RightColor = Color.FromNonPremultiplied(120, 120, 120, 255);
            }
            else { RightColor = Color.White; }


        }

        public override void Initialize()
        {
            
        }

    }
}
