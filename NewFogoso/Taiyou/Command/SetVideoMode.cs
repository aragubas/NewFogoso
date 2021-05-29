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
using System.Linq;

namespace Fogoso.Taiyou.Command
{
    public class SetVideoMode : TaiyouCommand
    {
        public SetVideoMode(string[] pArguments, string pScriptCaller, TaiyouLine pRootTaiyouLine)
        {
            OriginalArguments = pArguments;
            ScriptCaller = pScriptCaller;
            Title = "SetVideoMode";
            RootTaiyouLine = pRootTaiyouLine;

        }

        // SerVideoMode - Video mode changer
        // ===================================
        // 1 - Width
        // 2 - Height
        // 3 - Fullscreen
        public override int Call()
        {
            string[] Arguments = ReplaceVarLiterals();

            int VideoModeWidth = Int32.Parse(GetArgument(Arguments, 0));
            int VideoModeHeight = Int32.Parse(GetArgument(Arguments, 1));
            bool VideoModeIsFullscreen = GetArgument(Arguments, 1).ToLower().Equals("true");
 
            Main.Reference.graphics.PreferredBackBufferWidth = VideoModeWidth;
            Main.Reference.graphics.PreferredBackBufferHeight = VideoModeHeight;
            Main.Reference.graphics.IsFullScreen = VideoModeIsFullscreen;
            Main.Reference.graphics.ApplyChanges();
 
            Fogoso.Global.WindowWidth = VideoModeWidth;
            Fogoso.Global.WindowHeight = VideoModeHeight;
            Fogoso.Global.WindowIsFullcreen = VideoModeIsFullscreen;

            return 0;
        }


    }
}
