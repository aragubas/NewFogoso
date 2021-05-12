using Fogoso.GameLogic.UI;
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
        Label DateLabel;
        Label TimeLabel;
        Label MoneyInfosLabel;
        ToolStripPanel TabsButton;
        UtilsObjects.ValueSmoother ceira;

        public GameMain()
        {
            // Change to Loading Cursor
            GameInput.CursorImage = "loading.png";

            LeftPanel = new Panel(new Rectangle(0, 0, 200, Global.WindowHeight));
            CenterPanel = new Panel(new Rectangle(205, 35, Global.WindowWidth - 210, Global.WindowHeight - 85));
            DateLabel = new Label(new Vector2(205, 5), Main.Reference.Content.Load<SpriteFont>("default"), "amet");
            TimeLabel = new Label(new Vector2(205, 20), Main.Reference.Content.Load<SpriteFont>("small"), "sit");
            TabsButton = new ToolStripPanel(new Rectangle(205, 35 + (Global.WindowHeight - 85) + 5, Global.WindowWidth - 210, 40), true);

            ClickerViewer ceiraClickerButton = new ClickerViewer(new Vector2(3, LeftPanel.Rectangle.Bottom));
            Label sinas = new Label(new Vector2(5, 5), Main.Reference.Content.Load<SpriteFont>("default"), "Loading...");
            Button InfosButton = new Button(new Vector2(5, 5), "Infos");
            Button ItemsViewButton = new Button(new Vector2(5, 5), "Items");

            TabsButton.AddControl(InfosButton, "infos-button");
            TabsButton.AddControl(ItemsViewButton, "items-button");

            for (int i = 0; i < 6; i++)
            {
                Button ceiraBUtton = new Button(new Vector2(5, 5), "ceira-" + i);

                TabsButton.AddControl(ceiraBUtton, "enceirado");
            }

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
            
            if (CurrentSessionData.Ceira < 0)
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
            TabsButton.Draw(spriteBatch);

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
            TabsButton.Update();

            string DateText = AragubasTime.GetDecadeNameWithYear() + " - " + AragubasTime.GetMonthName() + " - " + AragubasTime.Week + "," + AragubasTime.GetDayName();
            DateLabel.SetText(DateText);

            string TimeText = AragubasTime.Hour + " : " + AragubasTime.Minute + " : " + AragubasTime.Second;
            TimeLabel.SetText(TimeText);

            // Refresh SmootObj Value
            ceira.Update();
            ceira.SetTargetValue(CurrentSessionData.Ceira);

            // Refresh MoneyInfos Label
            MoneyInfosLabel.SetText("$ " + ceira.GetValue().ToString("0.00") + "\nExp " + CurrentSessionData.Experience);


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
