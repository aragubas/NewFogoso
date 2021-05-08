using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogoso.GameLogic.UI
{
    class CeiraViewObj
    {
        public Vector2 CeiraPosition;
        public Vector2 CeiraTextSize;
        public Vector2 DestPosition;

        string CeiraText;
        SpriteFont font;
        public int ScrollWax;

        public CeiraViewObj(string pCeiraText, Vector2 InitialPos)
        {
            font = Game1.Reference.Content.Load<SpriteFont>("tyne");

            string Result = Utils.WrapText(font, pCeiraText, 140);

            CeiraText = Result;
            CeiraTextSize = font.MeasureString(CeiraText);

            CeiraPosition = InitialPos;
            DestPosition = InitialPos;

        }

        public void SetPosition(Vector2 pDestPos)
        {
            DestPosition = pDestPos;
        }

        public void Update()
        {
            if (CeiraPosition.X >= DestPosition.X) { CeiraPosition.X -= (CeiraPosition.X / DestPosition.X); }
            if (CeiraPosition.X <= DestPosition.X) { CeiraPosition.X += (CeiraPosition.X / DestPosition.X); }

            if (CeiraPosition.Y <= DestPosition.Y) { CeiraPosition.Y += (CeiraPosition.Y / DestPosition.Y); }
            if (CeiraPosition.Y >= DestPosition.Y) { CeiraPosition.Y -= (CeiraPosition.Y / DestPosition.Y); }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw Background
            spriteBatch.Draw(Sprites.GetSprite("/base.png"), new Rectangle((int)CeiraPosition.X, (int)CeiraPosition.Y, (int)CeiraTextSize.X, (int)CeiraTextSize.Y), Color.White);
            
            // Draw Text
            spriteBatch.DrawString(font, CeiraText, CeiraPosition, Color.Black);

        }
        

    }

    class ClickerButton : UIControl
    {

        Color currentColor;
        List<CeiraViewObj> CeiraViewer;
        int LastMinute;
        int RandomSinas;
        float CeiraPorSinas = 0.5f;

        public ClickerButton(Vector2 pPosition)
        {
            SetRectangle(new Rectangle((int)pPosition.X, (int)pPosition.Y, 1, 1));
            currentColor = Color.FromNonPremultiplied(250, 250, 250, 255);
            CeiraViewer = new List<CeiraViewObj>();

        }

        public void AddCeira(string Text)
        {
            CeiraViewer.Add(new CeiraViewObj(Text, new Vector2(Rectangle.X, Rectangle.Y))); 
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (var ceira in CeiraViewer)
            {
                ceira.Draw(spriteBatch);
            }
             
        }

        public override void Update()
        {


            CeiraViewObj[] ceiraCopy = new CeiraViewObj[CeiraViewer.Count];
            CeiraViewer.CopyTo(ceiraCopy);

            foreach (var ceira in ceiraCopy)
            {
                ceira.ScrollWax += 1;

                if (ceira.DestPosition.Y < -ceira.CeiraTextSize.Y) { CeiraViewer.Remove(ceira); continue; }



                ceira.Update();
                ceira.SetPosition(new Vector2(2, Rectangle.Y - ceira.CeiraTextSize.Y - ceira.ScrollWax));

            }

            if (GameInput.GetInputState("MINING_PRIMARY", true) || GameInput.GetInputState("MINING_PRIMARY", false) ||
                GameInput.GetInputState("MINING_SECONDARY", true) || GameInput.GetInputState("MINING_SECONDARY", false))
            {  
                Global.Ceira += CeiraPorSinas; 
                AddCeira(Global.Ceira.ToString());
                AragubasTime.Frames += 20;
                Random Oracle = new Random();

                if (Oracle.Next(0, 100) == RandomSinas)
                {
                    AragubasTime.Frames += 30;
                    Global.Ceira += CeiraPorSinas * 2;
                    Console.WriteLine("Capeta");

                }

                if (LastMinute != AragubasTime.Minute) 
                { 
                    LastMinute = AragubasTime.Minute; Global.Experience++;
                    Random ceira = new Random();
                    RandomSinas = ceira.Next(0, 100);



                    AragubasTime.Frames += 50;

                }

            } 

        }


    }
}
