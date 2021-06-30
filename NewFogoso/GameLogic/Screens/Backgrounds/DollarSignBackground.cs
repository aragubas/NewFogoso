using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogoso.GameLogic.Screens.Backgrounds
{
    class DollarSignBackground : GameScreen
    {
        Random RandomMizer;
        Matrix ScreenTransform;
        SpriteFont Font;
        UtilsObjects.ValueSmoother ColorRSmooth;
        UtilsObjects.ValueSmoother ColorGSmooth;
        UtilsObjects.ValueSmoother ColorBSmooth;
        UtilsObjects.ValueSmoother BGXSmooth;
        UtilsObjects.ValueSmoother BGYSmooth;
        UtilsObjects.ValueSmoother BGZSmooth;
        string CurrentBackgroundPattern = "01";
        Texture2D BackgroundFill;

        int SinasTimer;
        int SinasTimerMax = 5;

        public DollarSignBackground()
        {
            RandomMizer = new Random();
            ScreenTransform = Matrix.CreateTranslation(0, 0, 0);

            ColorRSmooth = new UtilsObjects.ValueSmoother(20, 20, true);
            ColorGSmooth = new UtilsObjects.ValueSmoother(20, 20, true);
            ColorBSmooth = new UtilsObjects.ValueSmoother(20, 20, true);
            BGXSmooth = new UtilsObjects.ValueSmoother(500, 10, true);
            BGYSmooth = new UtilsObjects.ValueSmoother(500, 10, true);
            BGZSmooth = new UtilsObjects.ValueSmoother(500, 10, true);
            SinasTimer = 5;

        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {  
            // Draw Static Background
            spriteBatch.Begin();
            spriteBatch.Draw(BackgroundFill, new Rectangle(0, 0, Global.WindowWidth, Global.WindowHeight), Color.FromNonPremultiplied((int)ColorRSmooth.GetValue(), (int)ColorGSmooth.GetValue(), (int)ColorBSmooth.GetValue(), 255));
            spriteBatch.End();

            // Draw Moving Part
            spriteBatch.Begin(transformMatrix:ScreenTransform);
            for (int x = 0; x < Global.WindowWidth / 13; x++)
            {
                for (int y = 0; y < Global.WindowHeight / 13; y++)
                {
                    spriteBatch.DrawString(Font, "$", new Vector2(x * 15, y * 15), Color.FromNonPremultiplied((int)ColorRSmooth.GetValue() + y, (int)ColorGSmooth.GetValue() + x, (int)ColorBSmooth.GetValue() + y, 255));

                }
            }
 
            spriteBatch.End();

        }

        public override void Update()
        {
            ScreenTransform = Matrix.CreateTranslation(BGXSmooth.GetValue(), BGYSmooth.GetValue(), 0) * Matrix.CreateRotationZ(BGZSmooth.GetValue());

            SinasTimer++;

            ColorRSmooth.Update();
            ColorGSmooth.Update();
            ColorBSmooth.Update();
            BGYSmooth.Update();
            BGXSmooth.Update();
            BGZSmooth.Update();

            if (SinasTimer >= SinasTimerMax)
            {
                SinasTimer = 0;
                SinasTimerMax = RandomMizer.Next(RandomMizer.Next(0, 25), RandomMizer.Next(26, 500));

                // Set Smooth Target Value
                ColorRSmooth.SetTargetValue(RandomMizer.Next(20, 150));
                ColorGSmooth.SetTargetValue(RandomMizer.Next(20, 150));
                ColorBSmooth.SetTargetValue(RandomMizer.Next(28, 150));

                // Set Smooth Target Value
                BGXSmooth.SetTargetValue((RandomMizer.Next(0, Global.WindowWidth) / 2 - Global.WindowWidth / 2) / 40 - RandomMizer.Next(5, 40));
                BGYSmooth.SetTargetValue((RandomMizer.Next(0, Global.WindowHeight) / 2 - Global.WindowHeight / 2) / 40 - RandomMizer.Next(5, 40));
                BGZSmooth.SetTargetValue(RandomMizer.Next(0, Global.WindowWidth) / Global.WindowWidth / RandomMizer.Next(50, 80));
 
            }
        }

        public override void Initialize()
        {
            base.Initialize(); 
            Font = Fonts.GetSpriteFont(Fonts.GetFontDescriptor("PressStart2P", 18));
            BackgroundFill = Sprites.GetSprite($"/background_patterns/{CurrentBackgroundPattern}.png");
             
        }
    }
}
