using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using MonoGame.Extended;
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
        public int Opacity = 0;

        public CeiraViewObj(string pCeiraText, Vector2 InitialPos, Color pBackgroundColor, Color pForegroundColor, float CeiraChange)
        {
            font = Fonts.GetSpriteFont(Fonts.GetFontDescriptor("PressStart2P", Fonts.TyneFontSize));
  
            string Result = Utils.WrapText(font, pCeiraText, 140);

            CeiraText = Result;
            CeiraTextSize = font.MeasureString(CeiraText);

            CeiraPosition = InitialPos;

            BackgroundColor = pBackgroundColor;
            ForegroundColor = pForegroundColor;

            CeiraChangeValue = CeiraChange;

            TimeTrigger = new AragubasTimeObject(1, 0, 0, 0, 0, 0);

        }
        public void SetPosition(Vector2 pDestPos)
        {
            CeiraPosition = pDestPos;
        }
  
        public void Draw(SpriteBatch spriteBatch)
        {
            if (WaitingDeletion) { return; }
            
            if (IsEnabled && Opacity < 255) { Opacity += 10; }
            if (!IsEnabled) { Opacity -= 25; }
            if (Opacity < 0) { WaitingDeletion = true; return; }
 
            // Draw Background
            spriteBatch.FillRectangle(new Rectangle((int)CeiraPosition.X, (int)CeiraPosition.Y, (int)CeiraTextSize.X, (int)CeiraTextSize.Y), Color.FromNonPremultiplied(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, Opacity));

            // Draw Text
            spriteBatch.DrawString(font, CeiraText, CeiraPosition, Color.FromNonPremultiplied(ForegroundColor.R, ForegroundColor.G, ForegroundColor.B, Opacity));
 
        }
         

    }

    class ClickerViewer : UIControl
    {

        Color currentColor;
        List<CeiraViewObj> CeiraViewer;
        int LastMinute;
        int RandomSinas;
        int CeiraMax = 100;
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
            if (ClearEstimatedTimer.TimeTriggered()) 
            { 
                // Increase Ceira with Income
                CurrentSessionData.Ceira += EstimatedIncome; 
 
                // Reset Income
                EstimatedIncome = 0; 

                // Reset Timer
                ClearEstimatedTimer.ResetTime(); 
                  
                // Set all items to Disabled
                for(int i = 0; i < CeiraViewer.Count; i++) { CeiraViewer[i].IsEnabled = false; }
            }
        
            if (CeiraViewer.Count > 1)
            {
                for(int i = 0; i < CeiraViewer.Count; i++)
                {
                    if (CeiraViewer[i].CeiraPosition.Y < CeiraViewer[i].CeiraTextSize.Y) { CeiraViewer.Remove(CeiraViewer[i]); continue; }
                    if (!CeiraViewer[i].IsEnabled) {if (CeiraViewer[i].WaitingDeletion) { CeiraViewer.Remove(CeiraViewer[i]); } continue; }
                    if (CeiraViewer[i].TimeTrigger.TimeTriggered()) { CeiraViewer[i].IsEnabled = false; if (CeiraViewer[i].CeiraPosition.Y > Rectangle.Y) { CeiraViewer.Remove(CeiraViewer[i]); } continue; }

 
                    CeiraViewer[i].SetPosition(new Vector2(i % 2, Rectangle.Y - (i * CeiraViewer[i].CeiraTextSize.Y))); 
                } 
            }
  
            // Detect when pressing mining button
            if (GameInput.GetInputState("MINING_PRIMARY", false) || GameInput.GetInputState("MINING_SECONDARY", false))
            {
                AddCeira(CurrentSessionData.CeiraPerWorkunit.ToString(), CurrentSessionData.CeiraPerWorkunit);
                AragubasTime.Frames = AragubasTime.Frames * 2;
                Random Oracle = new Random();
                Sound.PlaySound("click", 0.06f);

                if (Oracle.Next(0, 100) == RandomSinas)
                {
                    AragubasTime.Frames += AragubasTime.Frames * 2;
                    AddCeira((CurrentSessionData.CeiraPerWorkunit * CurrentSessionData.CeiraWorkunitBonusMultiplier).ToString() + " Bonus!", Color.Black, Color.White, CurrentSessionData.CeiraPerWorkunit * CurrentSessionData.CeiraWorkunitBonusMultiplier);
 
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
