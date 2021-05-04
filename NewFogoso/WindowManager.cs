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

namespace Fogoso
{
    public static class WindowManager
    {

        /// <summary>
        /// Send command to Windows Manager
        /// </summary>
        /// <param name="Command">0 - Change Window Title, 1 - IsBorderless, 2 - AllowUserResizing, 3 - AllowAltF4</param>
        /// <param name="Argument">Command Argument</param>
        public static void ReceiveCommand(int Command, dynamic Argument)
        {
            switch (Command)
            {
                case 0:
                    Game1.Reference.Window.Title = Argument;
                    return;

                case 1:
                    Game1.Reference.Window.IsBorderless = Argument;
                    return;

                case 2:
                    Game1.Reference.Window.AllowUserResizing = Argument;
                    return;

                case 3:
                    Game1.Reference.Window.AllowAltF4 = Argument;
                    return;

            }
        }

    }
}
