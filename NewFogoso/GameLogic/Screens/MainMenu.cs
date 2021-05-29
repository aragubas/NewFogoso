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
    class MainMenu : GameScreen
    {
        Panel MainMenuPanel;
        Panel SubPanel;

        Button PlayButton;
        Button NewSessionButton;
        Button SettingsButton;

        bool SkipToInitialScreenToggle;

        public override void Initialize()
        {
            // Change to Loading Cursor
            GameInput.CursorImage = "loading.png";

            MainMenuPanel = new Panel(new Rectangle(50, 300, 2, 2), true, 7);
            PlayButton = new Button(new Vector2(2, 2), "Play");

            MainMenuPanel.AddControl(PlayButton, "play-button", true);

            SettingsButton = new Button(new Vector2(MainMenuPanel.GetLastPos(false, true, 2).X, MainMenuPanel.GetLastPos(false, true, 2).Y + 2), "Settings");

            MainMenuPanel.AddControl(SettingsButton, "settings-button", true);

            MainMenuPanel.AutoSize();

            PlayButton.ButtonPress += TestButton_ButtonPress;

        }

        private void CreateSubPanel()
        {
            SubPanel = new Panel(new Rectangle(MainMenuPanel.Rectangle.Right + 2, MainMenuPanel.Rectangle.Y, 200, 200), true, 5);
            
        }

        void TestButton_ButtonPress(Button sender)
        {
            CreateSubPanel();
            NewSessionButton = new Button(new Vector2(2, 2), "New Session");

            NewSessionButton.ButtonPress += NewSessionButton_Click;

            SubPanel.AddControl(NewSessionButton, "new-session-button", true);
        }

        void NewSessionButton_Click(Button sender)
        {
            ScreenSelector.SetCurrentScreen(1);

        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            MainMenuPanel.Draw(spriteBatch);
            if (SubPanel != null) { SubPanel.Draw(spriteBatch); }
        }

        public override void Update()
        {
            if (!SkipToInitialScreenToggle) { SkipToInitialScreenToggle = true; SkipToInitialScreen(); }
            MainMenuPanel.Update();
            if (SubPanel != null) { SubPanel.Update(); }

        }

        private void SkipToInitialScreen()
        {
            int Ceira = Int32.Parse(Registry.ReadKeyValue("/initial_screen"));
            if (Ceira != 0) { ScreenSelector.SetCurrentScreen(Ceira); }

        }

        public override string ToString()
        {
            return "Fogoso Main Menu";
        }

    }
}
