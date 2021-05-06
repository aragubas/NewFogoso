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
        Panel LowerPanel;
        Label DateLabel;
        Label TimeLabel;

        public GameMain()
        {
            // Change to Loading Cursor
            GameInput.CursorImage = "loading.png";

            LeftPanel = new Panel(new Rectangle(0, 0, 200, Global.WindowHeight));
            CenterPanel = new Panel(new Rectangle(205, 50, Global.WindowWidth - 210, Global.WindowHeight - 100));
            LowerPanel = new Panel(new Rectangle(205, 50 + (Global.WindowHeight - 100) + 5, Global.WindowWidth - 210, 40));
            DateLabel = new Label(new Vector2(205, 5), Game1.Reference.Content.Load<SpriteFont>("default"), "amet");
            TimeLabel = new Label(new Vector2(205, 20), Game1.Reference.Content.Load<SpriteFont>("small"), "sit");

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
            LeftPanel.Update();
            CenterPanel.Update();
            LowerPanel.Update();

            string DateText = "Year " + AragubasTime.Year + " Month " + AragubasTime.Month + " Week " + AragubasTime.Week + " Day " + AragubasTime.Day;
            DateLabel.SetText(DateText);

            string TimeText = "Minute " + AragubasTime.Minute + " Second " + AragubasTime.Second;
            TimeLabel.SetText(TimeText);

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
