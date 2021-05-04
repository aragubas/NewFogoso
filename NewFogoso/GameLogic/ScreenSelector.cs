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

namespace Fogoso.GameLogic
{
    public static class ScreenSelector
    {
        public static GameScreen CurrentSelectedScreen;

        public static void Update()
        {
            CurrentSelectedScreen.Update();

        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            CurrentSelectedScreen.Draw(spriteBatch);

        }

        public static void SetCurrentScreen(int ScreenID)
        {
            switch (ScreenID)
            {
                case 0:
                    CurrentSelectedScreen = new Screens.Game();
                    break;

                default:
                    throw new InvalidOperationException("The screen id (" + ScreenID + ") is invalid.");

            }

            CurrentSelectedScreen.Initialize();

            Console.WriteLine("ScreenSelector ; Current screen has been set to {" + CurrentSelectedScreen.ToString() + "} id {" + ScreenID.ToString() + "}");
        }

        public static void Initialize()
        {
            int intCurrentSelectedScreen = Convert.ToInt32(Registry.ReadKeyValue("/initial_screen"));

            SetCurrentScreen(intCurrentSelectedScreen);



        }

    }
}
