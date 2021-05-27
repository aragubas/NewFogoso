using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogoso
{
    class DebugItem
    {
        public string Name, Value;

        public DebugItem(string pName, string pValue)
        {
            Name = pName;
            Value = pValue;
        }


    }

    public static class Debug
    {
        static List<DebugItem> DebugStuff;


        public static void Initialize()
        {
            DebugStuff = new List<DebugItem>();

        }

        public static void DebugThing(string Name, string Value)
        {
            // Return if DebugMode is disabled
            if (!Main.DebugModeEnabled) { return; }

            // Create a Debug item
            DebugItem sinas = new DebugItem(Name, Value);

            DebugStuff.Add(sinas);
        }

        public static void RenderDebugInfo(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            for(int i = 0; i < DebugStuff.Count; i++)
            {
                string Text = "'" + DebugStuff[i].Name + "' = [" + DebugStuff[i].Value + "]";
                spriteBatch.DrawString(Main.Reference.Content.Load<SpriteFont>("default"), Text, new Vector2(6, 6 + (i * 15)), Color.Black);
                spriteBatch.DrawString(Main.Reference.Content.Load<SpriteFont>("default"), Text, new Vector2(5, 5 + (i * 15)), Color.Red);

            }
 
            spriteBatch.End();
            DebugStuff.Clear();

        }
    }
}
