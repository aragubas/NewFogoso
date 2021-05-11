using Fogoso.GameLogic.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            ceira = new MapSelector(new Vector2(30, 30));

        }

        MapSelector ceira;

        public override void Draw(SpriteBatch spriteBatch)
        { 
            spriteBatch.Begin();

            ceira.Draw(spriteBatch);

            spriteBatch.End();

        } 

        public override void Update()
        {
            ceira.Update();



        }

        public override void Initialize()
        {
            
        }

    }
}
