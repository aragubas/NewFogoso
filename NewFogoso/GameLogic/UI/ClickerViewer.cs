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
        //public Vector2 DestPosition;


        string CeiraText;
        SpriteFont font;
        public AragubasTimeObject TimeTrigger;
        public bool IsEnabled = true;
        public bool WaitingDeletion;
         
        Color BackgroundColor;
        Color ForegroundColor;
        public float CeiraChangeValue;
        public int Opacity = 255;

        public CeiraViewObj(string pCeiraText, Vector2 InitialPos, Color pBackgroundColor, Color pForegroundColor, float CeiraChange)
        {
            font = Main.Reference.Content.Load<SpriteFont>("tyne");

            string Result = Utils.WrapText(font, pCeiraText, 140);

            CeiraText = Result;
            CeiraTextSize = font.MeasureString(CeiraText);

            CeiraPosition = InitialPos;
            //DestPosition = InitialPos;

            BackgroundColor = pBackgroundColor;
            ForegroundColor = pForegroundColor;

            CeiraChangeValue = CeiraChange;

            TimeTrigger = new AragubasTimeObject(2, 0, 0, 0, 0, 0);

        }

        public void SetPosition(Vector2 pDestPos)
        {
            //DestPosition = pDestPos;
            CeiraPosition = pDestPos;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (WaitingDeletion) { return; }
             
            BackgroundColor = new Color(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, Opacity);
            ForegroundColor = new Color(ForegroundColor.R, ForegroundColor.G, ForegroundColor.B, Opacity);
             
            if (!IsEnabled) { Opacity -= 10; }
            if (Opacity < 0) { WaitingDeletion = true; return; }

            // Draw Background
            spriteBatch.Draw(Sprites.GetSprite("/base.png"), new Rectangle((int)CeiraPosition.X, (int)CeiraPosition.Y, (int)CeiraTextSize.X, (int)CeiraTextSize.Y), BackgroundColor);
            
            // Draw Text
            spriteBatch.DrawString(font, CeiraText, CeiraPosition, ForegroundColor);

        }
        

    }

    class ClickerViewer : UIControl
    {

        Color currentColor;
        List<CeiraViewObj> CeiraViewer;
        int LastMinute;
        int RandomSinas;
        int CeiraMax = 100;
        int Scroll = 0;
        public float EstimatedIncome;
        AragubasTimeObject ClearEstimatedTimer;

        public ClickerViewer(Vector2 pPosition)
        {
            SetRectangle(new Rectangle((int)pPosition.X, (int)pPosition.Y, 1, 1));
            currentColor = Color.FromNonPremultiplied(250, 250, 250, 255);
            CeiraViewer = new List<CeiraViewObj>();
            ClearEstimatedTimer = new AragubasTimeObject(0, 1, 0, 0, 0, 0);
        }
  
        public void AddCeira(string Text, float CeiraChange=-1)
        {
            EstimatedIncome += CeiraChange;
            if (CeiraViewer.Count > CeiraMax) { return; }
            CeiraViewer.Add(new CeiraViewObj(Text, new Vector2(Rectangle.X, Rectangle.Y), Color.White, Color.Black, CeiraChange));

        }
     

        public void AddCeira(string Text, Color BackColor, Color ForeColor, float CeiraChange = -1)
        {
            EstimatedIncome += CeiraChange;
            if (CeiraViewer.Count > CeiraMax) { return; }
            CeiraViewer.Add(new CeiraViewObj(Text, new Vector2(Rectangle.X, Rectangle.Y), BackColor, ForeColor, CeiraChange));
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);


            for (int i = 0; i < CeiraViewer.Count; i++)
            {
                CeiraViewer[i].Draw(spriteBatch);

            }

        }
   
        public override void Update()
        {
            if (ClearEstimatedTimer.TimeHour()) 
            { 
                // Increase Ceira with Income
                CurrentSessionData.Ceira += EstimatedIncome; 
                // Reset Income
                EstimatedIncome = 0; 
                // Reset Timer
                ClearEstimatedTimer.ResetTime(); 
                // Play Click Sound
                if (EstimatedIncome > 0) { Sound.PlaySound("click", 0.9f); }; 
                 
                // Set all items to Disabled
                for(int i = 0; i < CeiraViewer.Count; i++) { CeiraViewer[i].IsEnabled = false; }
            }
        
            if (CeiraViewer.Count > 1)
            {
                int PositioningWax = -1;
                for(int i = 0; i < CeiraViewer.Count; i++)
                {
                    if (!CeiraViewer[i].IsEnabled) {if (CeiraViewer[i].WaitingDeletion) { CeiraViewer.Remove(CeiraViewer[i]); } continue; }
                    if (CeiraViewer[i].TimeTrigger.TimeMinute() || CeiraViewer[i].CeiraPosition.Y > Rectangle.Y) { PositioningWax--; CeiraViewer[i].IsEnabled = false; if (CeiraViewer[i].CeiraPosition.Y > Rectangle.Y) { CeiraViewer.Remove(CeiraViewer[i]); } continue; }
                    PositioningWax++;

                    CeiraViewer[i].SetPosition(new Vector2(2 + (PositioningWax % 3), Rectangle.Y - (PositioningWax * CeiraViewer[i].CeiraTextSize.Y) + Scroll)); 
                }

                if (CeiraViewer.Count > 1)
                {
                    if (CeiraViewer[CeiraViewer.Count - 1].CeiraPosition.Y > 0)
                    {
                        Scroll -= ((int)CeiraViewer[CeiraViewer.Count - 1].CeiraPosition.Y) / 15;
                    }
    
                    if (CeiraViewer[CeiraViewer.Count - 1].CeiraPosition.Y < 0)
                    {
                        Scroll++;
                    }
   
                }                
            }


            if (GameInput.GetInputState("MINING_PRIMARY", false) || GameInput.GetInputState("MINING_SECONDARY", false))
            {
                AddCeira(Convert.ToString(CurrentSessionData.CeiraPerWorkunit), CurrentSessionData.CeiraPerWorkunit);
                AragubasTime.Frames += 20;
                Random Oracle = new Random();
 
                if (Oracle.Next(0, 100) == RandomSinas)
                {
                    AragubasTime.Frames += AragubasTime.Frames * 2;
                    AddCeira(Convert.ToString(CurrentSessionData.CeiraPerWorkunit * CurrentSessionData.CeiraWorkunitBonusMultiplier) + " Bonus!", Color.Black, Color.White, CurrentSessionData.CeiraPerWorkunit * CurrentSessionData.CeiraWorkunitBonusMultiplier);
 
                }

                if (LastMinute != AragubasTime.Minute) 
                {
                    LastMinute = AragubasTime.Minute; CurrentSessionData.Experience++;
                    Random ceira = new Random();
                    RandomSinas = ceira.Next(0, 100);

                }

            } 

        }


    }
}
