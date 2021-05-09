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
        Random ceira;
        float EffectMultiplier = 35f;
        Matrix ScreenTransform;
        Color GeneralColor;
        int ColorR;
        int ColorG;
        int ColorB;
        UtilsObjects.ValueSmoother ceirinhaR;
        UtilsObjects.ValueSmoother ceirinhaG;
        UtilsObjects.ValueSmoother ceirinhaB;
        int SinasTimer;

        public DollarSignBackground()
        {
            ceira = new Random();
            ScreenTransform = Matrix.CreateTranslation(0, 0, 0);

            ceirinhaR = new UtilsObjects.ValueSmoother(5, 20, true);
            ceirinhaG = new UtilsObjects.ValueSmoother(5, 20, true);
            ceirinhaB = new UtilsObjects.ValueSmoother(5, 20, true);
            SinasTimer = 500;

        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        { 
            spriteBatch.Begin(transformMatrix: ScreenTransform);

            for (int x = 0; x < Global.WindowWidth / 13; x++)
            {
                for (int y = 0; y < Global.WindowHeight / 13; y++)
                {
                    spriteBatch.DrawString(Game1.Reference.Content.Load<SpriteFont>("12pt"), "$", new Vector2(x * 15, y * 15), Color.FromNonPremultiplied(ColorR + y, ColorG + x, ColorB + y, 255));

                }
            }

            spriteBatch.End();

        }

        public override void Update()
        {
            ScreenTransform = Matrix.CreateTranslation((GameInput.CursorPosition.X / 2 - Global.WindowWidth / 2) / EffectMultiplier - 10, (GameInput.CursorPosition.Y / 2 - Global.WindowHeight / 2) / EffectMultiplier - 10, 0) * Matrix.CreateRotationZ(GameInput.CursorPosition.X / Global.WindowWidth / 50);

            SinasTimer++;

            ceirinhaR.Update();
            ceirinhaG.Update();
            ceirinhaB.Update();


            if (SinasTimer >= 500)
            {
                SinasTimer = 0;

                ceirinhaR.SetTargetValue(ceira.Next(0, 255));
                ceirinhaG.SetTargetValue(ceira.Next(0, 255));
                ceirinhaB.SetTargetValue(ceira.Next(0, 255));
            }

            ColorR = (int)ceirinhaR.GetValue();
            ColorG = (int)ceirinhaG.GetValue();
            ColorB = (int)ceirinhaB.GetValue();


        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
