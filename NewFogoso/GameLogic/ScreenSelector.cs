/*
   ####### BEGIN APACHE 2.0 LICENSE #######
   Copyright 2020 Aragubas

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.

   ####### END APACHE 2.0 LICENSE #######




   ####### BEGIN MONOGAME LICENSE #######
   THIS GAME-ENGINE WAS CREATED USING THE MONOGAME FRAMEWORK
   Github: https://github.com/MonoGame/MonoGame#license 

   MONOGAME WAS CREATED BY MONOGAME TEAM

   THE MONOGAME LICENSE IS IN THE MONOGAME_License.txt file on the root folder. 

   ####### END MONOGAME LICENSE ####### 


*/

using System;
using Fogoso.GameLogic.Screens;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework; 
using Fogoso.GameLogic.OverlayScreens;

namespace Fogoso.GameLogic
{
    public static class ScreenSelector
    {
        public static GameScreen CurrentSelectedScreen;
        public static GameScreen CurrentBackgroundScreen;
        public static OverlayScreen CurrentOverlayScreen;
        private static Rectangle LastWorkingArea;
        public static bool WorkingAreaChanged;
        public static Rectangle WorkingArea;
        public static bool SuspendScreenUpdating;


        public static void Update()
        {
            LastWorkingArea = WorkingArea;

            if (!SuspendScreenUpdating) { CurrentSelectedScreen.Update(); }
            if (CurrentBackgroundScreen != null) { CurrentBackgroundScreen.Update(); }
 
            if (CurrentOverlayScreen != null) { CurrentOverlayScreen.Update(); }

            WorkingAreaChanged = LastWorkingArea != WorkingArea;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentBackgroundScreen != null) { CurrentBackgroundScreen.Draw(spriteBatch); }

            CurrentSelectedScreen.Draw(spriteBatch);
  
            if (CurrentOverlayScreen != null) { CurrentOverlayScreen.Draw(spriteBatch); }
        }

        public static void SetCurrentScreen(int ScreenID)
        {
            // Change to Loading Cursor
            GameInput.CursorImage = "loading.png";

            switch (ScreenID)
            {
                case 0:
                    CurrentSelectedScreen = new Screens.MainMenu();
                    break;

                case 1:
                    CurrentSelectedScreen = new Screens.GameMain();
                    break;

                case 2:
                    CurrentSelectedScreen = new Screens.EconomicalMap();
                    break;

                default:
                    throw new InvalidOperationException("The screen id (" + ScreenID + ") is invalid.");

            }

            // Initialize Selected Screen
            CurrentSelectedScreen.Initialize();

            Utils.ConsoleWriteWithTitle("ScreenSelector", "Current screen has been set to {" + CurrentSelectedScreen.ToString() + "} id {" + ScreenID.ToString() + "}");

        }

        public static void SetBackgroundScreen(int ScreenID)
        {
            // Change to Loading Cursor
            GameInput.CursorImage = "loading.png";

            // Screen Switch Logic
            switch (ScreenID)
            {
                case -1:
                    CurrentBackgroundScreen = null;
                    break;

                case 0:
                    CurrentBackgroundScreen = new Screens.Backgrounds.DollarSignBackground();
                    break;

                default: 
                    throw new InvalidOperationException("The background screen id (" + ScreenID + ") is invalid.");

            }

            if (CurrentBackgroundScreen != null)
            {
                CurrentBackgroundScreen.Initialize(); 
                Utils.ConsoleWriteWithTitle("ScreenSelector", "Current screen has been set to {" + CurrentSelectedScreen.ToString() + "} id {" + ScreenID.ToString() + "}");
            }


        }


        public static void Initialize()
        {
            // Set Initial Screen
            SetCurrentScreen(0);
            SetBackgroundScreen(0); 
            WorkingArea = new Rectangle(0, 0, Global.WindowWidth, Global.WindowHeight);

        }

    }
}
