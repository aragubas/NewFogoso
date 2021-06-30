using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using Fogoso.UtilsObjects;
using Fogoso.GameLogic.UI;
 
namespace Fogoso.GameLogic.OverlayScreens
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
        public bool DisableText;

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
            if (!DisableText) { spriteBatch.DrawString(font, CeiraText, CeiraPosition, Color.FromNonPremultiplied(ForegroundColor.R, ForegroundColor.G, ForegroundColor.B, Opacity)); }
 
        }
         

    }

    public class ClickerViewPanel : OverlayScreen
    {
        Color currentColor;
        List<CeiraViewObj> CeiraViewer;
        int LastMinute;
        int RandomSinas;
        int CeiraMax = 100;
        public float EstimatedIncome;
        AragubasTimeObject ClearEstimatedTimer;
        public Rectangle Rectangle;
        private RasterizerState _rasterizerState;
        private Matrix PositionFix;
        private AnimationController Animator;
        Label MoneyInfosLabel;
        Label MinimizedStateLabel;
        UtilsObjects.ValueSmoother CurrentMoneyValueSmoother;
        bool Minimized;
        bool Visible;
        Label DateLabel;
        Label TimeLabel;
 
        public ClickerViewPanel()
        {
            Rectangle = new Rectangle(0, 0, 200, Global.WindowHeight);
               
            currentColor = Color.FromNonPremultiplied(60, 60, 68, 230);
            CeiraViewer = new List<CeiraViewObj>();
            ClearEstimatedTimer = new AragubasTimeObject(0, 1, 0, 0, 0, 0);
            _rasterizerState = new RasterizerState() { ScissorTestEnable = true };  
            Animator = new AnimationController(1, 0.15f, 0.1f, true, false, true, 0);
            MoneyInfosLabel = new Label(new Vector2(0, 0), Fonts.GetFontDescriptor("PressStart2P", Fonts.DefaultFontSize), "Loading...");
            MinimizedStateLabel = new Label(new Vector2(0, Global.WindowHeight / 2 - 16), Fonts.GetFontDescriptor("PressStart2P", 32), ">");
 
            MoneyInfosLabel.DrawBackground += DrawLabelBackground;
            MinimizedStateLabel.DrawBackground += DrawMinimizedStateLabelBackground;
  
            CurrentMoneyValueSmoother = new UtilsObjects.ValueSmoother(10, 20, false);

            DateLabel = new Label(new Vector2(205, ScreenSelector.WorkingArea.Y + 5), Fonts.GetFontDescriptor("PressStart2P", Fonts.SmallFontSize), "amet");
            TimeLabel = new Label(new Vector2(205, ScreenSelector.WorkingArea.Y + 20), Fonts.GetFontDescriptor("PressStart2P", Fonts.SmallFontSize), "sit");

            DateLabel.DrawBackground += DrawLabelBackground;
            TimeLabel.DrawBackground += DrawLabelBackground;


            CurrentSessionData.clickerViewPanel = this;
        } 

        void DrawMinimizedStateLabelBackground(Rectangle Rect, SpriteBatch spriteBatch, Label sender)
        {
            Color bgColor = Color.FromNonPremultiplied(32, 32, 43, 230);
             
            spriteBatch.DrawRectangle(Rect, Color.FromNonPremultiplied(15, 15, 22, 50), 2);
            spriteBatch.FillRectangle(Rect, bgColor);
 
        }

        void DrawLabelBackground(Rectangle Rect, SpriteBatch spriteBatch, Label sender)
        {
            Color bgColor = Color.FromNonPremultiplied(93, 93, 103, sender.Opacity - 30);
            
            if (CurrentSessionData.Ceira < 0)
            {
                bgColor = Color.FromNonPremultiplied(215, 215, 225, sender.Opacity);
            }      
             
            spriteBatch.DrawRectangle(Rect, Color.FromNonPremultiplied(15, 15, 22, sender.Opacity - 215), 2);
            spriteBatch.FillRectangle(Rect, bgColor);
 
        }

        void DrawDateMoneyInfosLabel(SpriteBatch spriteBatch)
        {
            DateLabel.Draw(spriteBatch);
            TimeLabel.Draw(spriteBatch);
            
        }

        void UpdateDateTimeLabel()
        {
            DateLabel.SetText($"{AragubasTime.GetDecadeNameWithYear()} - {AragubasTime.GetMonthName()}/{AragubasTime.Week}, {AragubasTime.GetDayName()}");
 
            TimeLabel.SetText($"{AragubasTime.Hour}:{AragubasTime.Minute}:{AragubasTime.Second}");
 
            SetClockOpacity();
        }
        
        private void SetMatrix(int pTranslationX, int pTranslationY, float pScaleX, float pScaleY)
        {  
            PositionFix = Matrix.CreateScale(pScaleX, pScaleY, 0) * Matrix.CreateTranslation(pTranslationX, pTranslationY, 0);
        }


        public void AddCeira(string Text, float CeiraChange=-1)
        {
            EstimatedIncome += CeiraChange;
            if (CeiraViewer.Count > CeiraMax) { return; } 
            CeiraViewer.Add(new CeiraViewObj(Text, new Vector2(Rectangle.X, Rectangle.Bottom), Color.White, Color.Black, CeiraChange));
 
        }
       

        public void AddCeira(string Text, Color BackColor, Color ForeColor, float CeiraChange = -1)
        {
            EstimatedIncome += CeiraChange;
            if (CeiraViewer.Count > CeiraMax) { return; }
            CeiraViewer.Add(new CeiraViewObj(Text, new Vector2(Rectangle.X, Rectangle.Bottom), BackColor, ForeColor, CeiraChange));
        }


        public void ToggleMinimized()
        {
            if (Animator.GetEnabled()) { return; }
            Animator.Toggle();

            Minimized = Animator.GetState() == 1;
        }
 
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Copy last Scissor Rectangle
            Rectangle lastScissorRect = spriteBatch.GraphicsDevice.ScissorRectangle;
            
            // Begin Drawing
            spriteBatch.Begin(transformMatrix: PositionFix, rasterizerState:_rasterizerState,sortMode:SpriteSortMode.Immediate, blendState:BlendState.AlphaBlend);
            
            // Set Scissor Rectangle
            spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle;

            // Draw background
            spriteBatch.DrawRectangle(new Rectangle(0, 0, Rectangle.Width, Rectangle.Height), Color.FromNonPremultiplied(currentColor.R + 100, currentColor.G + 100, currentColor.B + 100, currentColor.A), 2);
            spriteBatch.FillRectangle(new Rectangle(1, 1, Rectangle.Width - 2, Rectangle.Height - 2), currentColor);
  
            MoneyInfosLabel.Draw(spriteBatch);

            for (int i = 0; i < CeiraViewer.Count; i++)
            {
                CeiraViewer[i].Draw(spriteBatch);

            }

            // End Sprite Batch
            spriteBatch.End();

            // Restore last ScissorRectangle
            spriteBatch.GraphicsDevice.ScissorRectangle = lastScissorRect;

            if (!Visible)
            {
                spriteBatch.Begin();
    
                MinimizedStateLabel.Draw(spriteBatch);

                spriteBatch.End();
            }

            spriteBatch.Begin();
            DrawDateMoneyInfosLabel(spriteBatch);
            spriteBatch.End();

        }

        void SetClockOpacity()
        {
            short newOpacity = Convert.ToInt16(255 * Animator.GetValue());
            if (newOpacity < 50) { newOpacity = 0; }
 
            DateLabel.Opacity = newOpacity;
            TimeLabel.Opacity = newOpacity;
        }
   
        void MiningAction()
        {
            //if (Minimized) { ToggleMinimized(); }

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
 
        public override void Update()
        {
            // Update Animator
            Animator.Update();

            // Set Visible Flag
            Visible = !Minimized && Animator.GetState() == 1 && !Animator.Ended;
            
            // Update DateTime Label
            UpdateDateTimeLabel();
            
            // Set Transformation Matrix
            SetMatrix(Rectangle.X, Rectangle.Y, Animator.GetValue(), 1);

            // Set Working Area
            if (Visible) { ScreenSelector.WorkingArea = new Rectangle(Rectangle.Width, 0, Global.WindowWidth - Rectangle.Width, Global.WindowHeight); } else { ScreenSelector.WorkingArea = new Rectangle(32, 0, Global.WindowWidth - 32, Global.WindowHeight); }

            // Toggle Clicker Viewer Key
            if (GameInput.GetInputState("TOGGLE_CLICKVIEWER", false)) { ToggleMinimized(); }
            
            if (Visible)
            {
                CurrentMoneyValueSmoother.Update();
                CurrentMoneyValueSmoother.SetTargetValue(CurrentSessionData.Ceira); 

                MoneyInfosLabel.SetText($"$ {CurrentMoneyValueSmoother.GetValue().ToString("0.00")}\nExp {CurrentSessionData.Experience}\nIncome: {CurrentSessionData.clickerViewPanel.EstimatedIncome.ToString("0.00")}");
            }
             
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
                    CeiraViewer[i].DisableText = !Visible;  
                    if (CeiraViewer[i].CeiraPosition.Y < CeiraViewer[i].CeiraTextSize.Y) { CeiraViewer.Remove(CeiraViewer[i]); continue; }
                    if (!CeiraViewer[i].IsEnabled) { if (CeiraViewer[i].WaitingDeletion) { CeiraViewer.Remove(CeiraViewer[i]); } continue; }
                    if (CeiraViewer[i].TimeTrigger.TimeTriggered()) { CeiraViewer[i].IsEnabled = false; if (CeiraViewer[i].CeiraPosition.Y > Rectangle.Y) { CeiraViewer.Remove(CeiraViewer[i]); } continue; }

   
                    CeiraViewer[i].SetPosition(new Vector2(Rectangle.X, Rectangle.Bottom - (i * CeiraViewer[i].CeiraTextSize.Y))); 
                }  
            }
   
            // Detect when pressing mining button
            if (GameInput.GetInputState("MINING_PRIMARY", false)) { MiningAction(); }
            if (GameInput.GetInputState("MINING_SECONDARY", false)) { MiningAction(); }

        }

    }
}