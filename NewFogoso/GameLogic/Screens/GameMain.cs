using Fogoso.GameLogic.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
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
        ClickerViewer refClickerViewer;

        public GameMain()
        {
            // Change to Loading Cursor
            GameInput.CursorImage = "loading.png";

            LeftPanel = new Panel(new Rectangle(0, 0, 200, Global.WindowHeight));
            CenterPanel = new Panel(new Rectangle(205, 35, Global.WindowWidth - 210, Global.WindowHeight - 85));
            DateLabel = new Label(new Vector2(205, 5), Fonts.GetFontDescriptor("PressStart2P", Fonts.SmallFontSize), "amet");
            TimeLabel = new Label(new Vector2(205, 20), Fonts.GetFontDescriptor("PressStart2P", Fonts.SmallFontSize), "sit");
            TabsButton = new ToolStripPanel(new Rectangle(205, 35 + (Global.WindowHeight - 85) + 5, Global.WindowWidth - 210, 40), true);
 
            refClickerViewer = new ClickerViewer(new Vector2(3, LeftPanel.Rectangle.Bottom));
            Label sinas = new Label(new Vector2(5, 5), Fonts.GetFontDescriptor("PressStart2P", Fonts.DefaultFontSize), "Loading...");
            Button InfosButton = new Button(new Vector2(5, 5), "Infos");
            Button ItemsViewButton = new Button(new Vector2(5, 5), "Items");
  
            TabsButton.AddControl(InfosButton, "infos-button");
            TabsButton.AddControl(ItemsViewButton, "items-button");

            LeftPanel.AddControl(refClickerViewer, "clicker-viewer");
            LeftPanel.AddControl(sinas, "ceira-label");
            MoneyInfosLabel = sinas;

            // Add Event Listeners
            sinas.DrawBackground += DrawLabelBackground;
            DateLabel.DrawBackground += DrawLabelBackground;
            TimeLabel.DrawBackground += DrawLabelBackground;
            InfosButton.ButtonPress += InfosButton_ButtonPress;
            ItemsViewButton.ButtonPress += ItemsViewButton_ButtonPress;

            ceira = new UtilsObjects.ValueSmoother(10, 20, false);
 
            // Open Item View
            ItemsViewButton_ButtonPress(null);
        }

        void ItemsViewButton_ButtonPress(Button sender)
        {
            CenterPanel.ControlCollection.Clear();

            List<UIControl> newControlCollection = new List<UIControl>();
            ItemsView ceiraView = new ItemsView(new Rectangle(5, 5, 200, 200));
            ceiraView.Tag = "items-view";
             
            newControlCollection.Add(ceiraView);

            CenterPanel.SwitchControlCollection(newControlCollection);
            CenterPanel.ControlFill("items-view");

        }
 
        void InfosButton_ButtonPress(Button sender)
        {
            CenterPanel.ControlCollection.Clear();
  
            List<UIControl> newControlCollection = new List<UIControl>();


            CenterPanel.SwitchControlCollection(newControlCollection);
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
   
            spriteBatch.DrawRectangle(ceira, Color.FromNonPremultiplied(15, 15, 22, 50), 2);
            spriteBatch.FillRectangle(Rect, bgColor);
 
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

            string DateText = AragubasTime.GetDecadeNameWithYear() + " - " + AragubasTime.GetMonthName() + "/" + AragubasTime.Week + "," + AragubasTime.GetDayName();
            DateLabel.SetText(DateText);

            string TimeText = AragubasTime.Hour + " : " + AragubasTime.Minute + " : " + AragubasTime.Second;
            TimeLabel.SetText(TimeText);
 
            // Refresh SmootObj Value
            ceira.Update();
            ceira.SetTargetValue(CurrentSessionData.Ceira);

            // Refresh MoneyInfos Label
            if (refClickerViewer != null)
            {
                MoneyInfosLabel.SetText($"$ {ceira.GetValue().ToString("0.00")}\nExp {CurrentSessionData.Experience}\nIncome: {refClickerViewer.EstimatedIncome.ToString("0.00")}");
            }
 

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
