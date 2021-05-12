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
        float EffectMultiplier = 35f;
        Matrix ScreenTransform;
        Color GeneralColor;
        int ColorR;
        int ColorG;
        int ColorB;
        UtilsObjects.ValueSmoother ColorRSmooth;
        UtilsObjects.ValueSmoother ColorGSmooth;
        UtilsObjects.ValueSmoother ColorBSmooth;
        UtilsObjects.ValueSmoother BGXSmooth;
        UtilsObjects.ValueSmoother BGYSmooth;
        UtilsObjects.ValueSmoother BGZSmooth;

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
            spriteBatch.Begin(transformMatrix: ScreenTransform);

            for (int x = 0; x < Global.WindowWidth / 13; x++)
            {
                for (int y = 0; y < Global.WindowHeight / 13; y++)
                {
                    spriteBatch.DrawString(Main.Reference.Content.Load<SpriteFont>("12pt"), "$", new Vector2(x * 15, y * 15), Color.FromNonPremultiplied(ColorR + y, ColorG + x, ColorB + y, 255));

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

                ColorRSmooth.SetTargetValue(RandomMizer.Next(0, 150));
                ColorGSmooth.SetTargetValue(RandomMizer.Next(0, 150));
                ColorBSmooth.SetTargetValue(RandomMizer.Next(0, 150));

                BGXSmooth.SetTargetValue((GameInput.CursorPosition.X / 2 - Global.WindowWidth / 2) / EffectMultiplier - RandomMizer.Next(5, 40));
                BGYSmooth.SetTargetValue((GameInput.CursorPosition.Y / 2 - Global.WindowHeight / 2) / EffectMultiplier - RandomMizer.Next(5, 40));
                BGZSmooth.SetTargetValue(GameInput.CursorPosition.X / Global.WindowWidth / RandomMizer.Next(50, 80));

            }


            ColorR = (int)ColorRSmooth.GetValue();
            ColorG = (int)ColorGSmooth.GetValue();
            ColorB = (int)ColorBSmooth.GetValue();


        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
