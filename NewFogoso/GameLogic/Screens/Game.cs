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
    class Game : GameScreen
    {
        Panel TestPanel;
        Button TestButton;
        Panel OtherPanel;
        Button OtherButton;


        public override void Initialize()
        { 
            TestPanel = new Panel(new Rectangle(0, 0, 2, 2), true, 7);
            TestButton = new Button(new Vector2(2, 2), "Play");

            TestPanel.AddControl(TestButton, "play-button", true);

            TestButton.ButtonPress += TestButton_ButtonPress;

        }

        void TestButton_ButtonPress(Button sender)
        {
            OtherPanel = new Panel(new Rectangle(TestPanel.Rectangle.Right + 4, TestPanel.Rectangle.Y + 5, 200, 200), true, 5);
            OtherButton = new Button(new Vector2(2, 2), "Omaldite");
            OtherPanel.AddControl(OtherButton, "ceiraButton", true);

        }

        

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            TestPanel.Draw(spriteBatch);
            if (OtherPanel != null) { OtherPanel.Draw(spriteBatch); }

        }

        public override void Update()
        {
            TestPanel.Update();
            if (OtherPanel != null) { OtherPanel.Update(); }

        }

        public override string ToString()
        {
            return "Fogoso Main Game Screen";
        }

    }
}
