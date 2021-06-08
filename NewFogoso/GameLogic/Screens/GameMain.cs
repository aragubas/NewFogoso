using Fogoso.GameLogic.UI;
using Fogoso.GameLogic.OverlayScreens;
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
        Panel CenterPanel;
        Label DateLabel;
        Label TimeLabel;
        ToolStripPanel TabsButton;

        public GameMain()
        {
            // Change to Loading Cursor
            GameInput.CursorImage = "loading.png";

            CenterPanel = new Panel(new Rectangle(ScreenSelector.WorkingArea.X + 5, ScreenSelector.WorkingArea.Y + 35, ScreenSelector.WorkingArea.Width - 210, ScreenSelector.WorkingArea.Height - 85));
            DateLabel = new Label(new Vector2(ScreenSelector.WorkingArea.X + 5, ScreenSelector.WorkingArea.Y + 5), Fonts.GetFontDescriptor("PressStart2P", Fonts.SmallFontSize), "amet");
            TimeLabel = new Label(new Vector2(ScreenSelector.WorkingArea.X + 5, ScreenSelector.WorkingArea.Y + 20), Fonts.GetFontDescriptor("PressStart2P", Fonts.SmallFontSize), "sit");
            TabsButton = new ToolStripPanel(new Rectangle(ScreenSelector.WorkingArea.X + 5, 35 + (ScreenSelector.WorkingArea.Height - 85) + 5, ScreenSelector.WorkingArea.Width - 210, 40), true);
 
            Button InfosButton = new Button(new Vector2(5, 5), "Infos");
            Button ItemsViewButton = new Button(new Vector2(5, 5), "Items");
   
            TabsButton.AddControl(InfosButton, "infos-button");
            TabsButton.AddControl(ItemsViewButton, "items-button");


            // Add Event Listeners
            DateLabel.DrawBackground += DrawLabelBackground;
            TimeLabel.DrawBackground += DrawLabelBackground;
            InfosButton.ButtonPress += InfosButton_ButtonPress;
            ItemsViewButton.ButtonPress += ItemsViewButton_ButtonPress;
 
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

            CurrentSessionData.Reset();
            for(int i = 0; i < CurrentSessionData.UserItems.Count; i++)
            { 
                ceiraView.AddItem(new ItemsViewItem(CurrentSessionData.UserItems[i].Metadata.ItemName, isGameItem:CurrentSessionData.UserItems[i]));
            }
         
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

            CenterPanel.Draw(spriteBatch);
            TabsButton.Draw(spriteBatch);
 
            spriteBatch.Begin();
            DateLabel.Draw(spriteBatch);
            TimeLabel.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update()
        {
            // Working area has been changed
            if (ScreenSelector.WorkingAreaChanged)
            {
                CenterPanel.SetRectangle(new Rectangle(ScreenSelector.WorkingArea.X + 5, ScreenSelector.WorkingArea.Y + 35, ScreenSelector.WorkingArea.Width - 10, ScreenSelector.WorkingArea.Height - 85));
                CenterPanel.ForceReadjust = true;

                TabsButton.SetRectangle(new Rectangle(ScreenSelector.WorkingArea.X + 5, 35 + (ScreenSelector.WorkingArea.Height - 85) + 5, ScreenSelector.WorkingArea.Width - 10, 40));
 
            }

             
            if (GameInput.GetInputState("PAUSE_KEY", false))
            {
                ScreenSelector.SetCurrentScreen(0);
                return;
            }

            CenterPanel.Update();
            TabsButton.Update();

            string DateText = AragubasTime.GetDecadeNameWithYear() + " - " + AragubasTime.GetMonthName() + "/" + AragubasTime.Week + "," + AragubasTime.GetDayName();
            DateLabel.SetText(DateText);

            string TimeText = AragubasTime.Hour + " : " + AragubasTime.Minute + " : " + AragubasTime.Second;
            TimeLabel.SetText(TimeText);

        }

        public override void Initialize()
        {
            base.Initialize();

            ScreenSelector.CurrentOverlayScreen = new ClickerViewPanel(); 
        }

        public override string ToString()
        {
            return "Fogoso Game Screen";
        }



    }
}
