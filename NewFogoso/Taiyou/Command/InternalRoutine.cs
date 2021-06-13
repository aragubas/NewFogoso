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

    public class InternalRoutine : TaiyouCommand
    {
        internal abstract class Routine
        {
            public virtual void Run() { }
        }
 
        internal class UPDATE_INTERNAL_VARIABLES : Routine
        {
            public override void Run() => Global.SetGlobalVariables();
        }
 
        internal class UPDATE_SCREEN_SELECTOR : Routine
        {
            public override void Run() => GameLogic.ScreenSelector.Update();
        }
 
        internal class UPDATE_GAME_INPUT : Routine
        {
            public override void Run() => GameInput.Update();
        }

        internal class UPDATE_KEYBOARD_OBJ : Routine
        {
            public override void Run() => GameLogic.TextBox.KeyboardInput.Update();
        }

        internal class UPDATE_SOUND_SYSTEM : Routine
        {
            public override void Run() => Sound.Update();
        }

        internal class UPDATE_ARAGUBAS_TIME : Routine
        {
            public override void Run() => GameLogic.AragubasTime.Update();
        }

        internal class INIT_STATIC_EVENTS : Routine
        {
            public override void Run() => Taiyou.StaticlyLinkedEvents.LoadEvents();
        }


        Routine selectedRoutine;
        public InternalRoutine(string[] pArguments, string pScriptCaller, TaiyouLine pRootTaiyouLine)
        {
            OriginalArguments = pArguments;
            ScriptCaller = pScriptCaller;
            Title = "InternalRoutine";
            RootTaiyouLine = pRootTaiyouLine;

        }

        // InternalFunc - Call a internal routine 
        // =======================================
        // 1 - Routine Name
        //
        // ----
        // Avaliable Routines:
        // ----
        // UPDATE_INTERNAL_VARIABLE | Update interval variables (such as video mode, cursor position etc.)
        // UPDATE_SCREEN_SELECTOR   | Update current active screen and background
        // UPDATE_GAME_INPUT        | Update input handling system
        // UPDATE_KEYBOARD_OBJ      | Update keyboard obj
        // UPDATE_SOUND_SYSTEM      | Update sound system
        // UPDATE_ARAGUBAS_TIME     | Update aragubas time
        // INIT_STATIC_EVENTS       | Initialize Staticly Linked Events

        public override object Call()
        {
            if (selectedRoutine == null)
            {
                string[] Arguments = ReplaceVarLiterals();

 
                switch(GetArgument(Arguments, 0).ToUpper())
                {
                    case "UPDATE_INTERNAL_VARIABLES":
                        selectedRoutine = new UPDATE_INTERNAL_VARIABLES();
                        break;

                    case "UPDATE_SCREEN_SELECTOR":
                        selectedRoutine = new UPDATE_SCREEN_SELECTOR();
                        break;

                    case "UPDATE_GAME_INPUT":
                        selectedRoutine = new UPDATE_GAME_INPUT();
                        break;

                    case "UPDATE_KEYBOARD_OBJ":
                        selectedRoutine = new UPDATE_KEYBOARD_OBJ();
                        break;

                    case "UPDATE_SOUND_SYSTEM":
                        selectedRoutine = new UPDATE_SOUND_SYSTEM();
                        break;

                    case "UPDATE_ARAGUBAS_TIME":
                        selectedRoutine = new UPDATE_ARAGUBAS_TIME();
                        break;

                    case "INIT_STATIC_EVENTS":
                        selectedRoutine = new INIT_STATIC_EVENTS();
                        break;
         
                    default:
                        Utils.ConsoleWriteWithTitle(Title, $"Invalid routine ({GetArgument(Arguments, 0).ToUpper()})");
                        break;
                        
                }

            }
 
            if (selectedRoutine != null) { selectedRoutine.Run(); }
            return null;
        }


    }
}
