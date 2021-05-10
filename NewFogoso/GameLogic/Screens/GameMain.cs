﻿using Fogoso.GameLogic.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogoso.GameLogic.Screens
{
    class GameMain : GameScreen 
    {
        Panel LeftPanel;
        Panel CenterPanel;
        Panel LowerPanel;
        Label DateLabel;
        Label TimeLabel;
        Label MoneyInfosLabel;
        UtilsObjects.ValueSmoother ceira;

        public GameMain()
        {
            // Change to Loading Cursor
            GameInput.CursorImage = "loading.png";

            LeftPanel = new Panel(new Rectangle(0, 0, 200, Global.WindowHeight));
            CenterPanel = new Panel(new Rectangle(205, 35, Global.WindowWidth - 210, Global.WindowHeight - 85));
            LowerPanel = new Panel(new Rectangle(205, 35 + (Global.WindowHeight - 85) + 5, Global.WindowWidth - 210, 40));
            DateLabel = new Label(new Vector2(205, 5), Main.Reference.Content.Load<SpriteFont>("default"), "amet");
            TimeLabel = new Label(new Vector2(205, 20), Main.Reference.Content.Load<SpriteFont>("small"), "sit");


            ClickerButton ceiraClickerButton = new ClickerButton(new Vector2(3, LeftPanel.Rectangle.Bottom));
            Label sinas = new Label(new Vector2(5, 5), Main.Reference.Content.Load<SpriteFont>("default"), "Loading...");

            LeftPanel.AddControl(ceiraClickerButton, "ceira-clicker");
            LeftPanel.AddControl(sinas, "ceira-label");
            MoneyInfosLabel = sinas;

            sinas.DrawBackground += DrawLabelBackground;
            DateLabel.DrawBackground += DrawLabelBackground;
            TimeLabel.DrawBackground += DrawLabelBackground;

            ceira = new UtilsObjects.ValueSmoother(3, 5, false);

        }

        void DrawLabelBackground(Rectangle Rect, SpriteBatch spriteBatch)
        {
            Color bgColor = Color.FromNonPremultiplied(120, 120, 120, 230);
            
            if (Global.Ceira < 0)
            {
                bgColor = Color.FromNonPremultiplied(215, 120, 80, 255);
            }
            Rectangle ceira = new Rectangle(Rect.X, Rect.Y, Rect.Width, Rect.Height);
            ceira.Inflate(2, 2);

            spriteBatch.Draw(Sprites.GetSprite("/base.png"), ceira, Color.FromNonPremultiplied(15, 15, 15, 100));
            spriteBatch.Draw(Sprites.GetSprite("/base.png"), Rect, bgColor);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            LeftPanel.Draw(spriteBatch);
            CenterPanel.Draw(spriteBatch);
            LowerPanel.Draw(spriteBatch);

            spriteBatch.Begin();
            DateLabel.Draw(spriteBatch);
            TimeLabel.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update()
        {
            if (GameInput.GetInputState("PAUSE_KEY", false))
            {
                ScreenSelector.SetCurrentScreen(0);
                return;
            }

            LeftPanel.Update();
            CenterPanel.Update();
            LowerPanel.Update();

            string DateText = "Year " + AragubasTime.Year + " Month " + AragubasTime.Month + " Week " + AragubasTime.Week + " Day " + AragubasTime.Day;
            DateLabel.SetText(DateText);

            string TimeText = "Minute " + AragubasTime.Minute + " Second " + AragubasTime.Second;
            TimeLabel.SetText(TimeText);

            // Refresh SmootObj Value
            ceira.Update();
            ceira.SetTargetValue(Global.Ceira);

            // Refresh MoneyInfos Label
            MoneyInfosLabel.SetText("$ " + ceira.GetValue().ToString("0.00") + "\nExp " + Global.Experience);


        }

        public override void Initialize()
        {
            base.Initialize();


        }

        public override string ToString()
        {
            return "Fogoso Game Screen";
        }



    }
}
