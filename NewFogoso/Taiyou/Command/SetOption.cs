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
using Fogoso.Taiyou;
namespace Fogoso.Taiyou.Command
{
    public class SetOption : TaiyouCommand
    {
        public SetOption(string[] pArguments, string pScriptCaller, TaiyouLine pRootTaiyouLine)
        {
            OriginalArguments = pArguments;

            ScriptCaller = pScriptCaller;
            Title = "SetOption";
            RootTaiyouLine = pRootTaiyouLine;

        }

        // JumpIf - Set internal engine values
        // =======================================
        // 1 - Option Name
        // 2 - Value
        // 
        // ----
        // Avaliable Options:
        // ----
        // FIXED_TIME_STEP   | If FPS should be locked or not
        // MOUSE_VISIBLE     | If cursor should be visible or not
        // WINDOW_TITLE      | Window Title
        // WINDOW_BORDERLESS | If window should be borderless or not
        // RELOAD            | Reload the game engine
        // CURSOR_IMAGE      | Set Cursor Image
 

        public override object Call()
        {
            string[] Arguments = ReplaceVarLiterals();

            string OptionName = GetArgument(Arguments, 0);
            string OptionValue = GetArgument(Arguments, 1);

            switch (OptionName.ToUpper())
            {
                case "FIXED_TIME_STEP":
                    Main.Reference.IsFixedTimeStep = Convert.ToBoolean(OptionValue);
                    return null;

                case "MOUSE_VISIBLE":
                    Main.Reference.IsMouseVisible = Convert.ToBoolean(OptionValue);
                    return null;

                case "WINDOW_TITLE":
                    Main.Reference.Window.Title = OptionValue;
                    return null;

                case "WINDOW_BORDERLESS":
                    Main.Reference.Window.IsBorderless = Convert.ToBoolean(OptionValue);
                    return null;

                case "CURSOR_IMAGE":
                    GameInput.CursorImage = OptionValue;
                    return null;

                case "CURSOR_X":
                    GameInput.CursorPosition.X = Int32.Parse(OptionValue);
                    return null;
   
                case "CURSOR_Y":
                    GameInput.CursorPosition.Y = Int32.Parse(OptionValue);
                    return null;
 

                default:
                    throw new ArgumentOutOfRangeException("Invalid argument: [" + OptionName + "]");

            }

        }



    }
}