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

        public override void Initialize()
        {
            TestPanel = new Panel(new Rectangle(5, 5, 200, 200));
            TestButton = new Button(new Vector2(2, 2), "Lorem Ipsum");

            TestPanel.AddControl(TestButton);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            TestPanel.Draw(spriteBatch);

        }

        public override void Update()
        {
            TestPanel.Update();
        }

        public override string ToString()
        {
            return "Fogoso Main Game Screen";
        }

    }
}
