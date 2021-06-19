/*
   ####### BEGIN APACHE 2.0 LICENSE #######
   Copyright 2021 Aragubas

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
using System.Collections.Generic;
using Fogoso.Taiyou;

namespace Fogoso.Taiyou.Command
{
    public class Jump : TaiyouCommand
    {
        public Jump(string[] pArguments, string pScriptCaller, TaiyouLine pRootTaiyouLine)
        {
            OriginalArguments = pArguments;

            ScriptCaller = pScriptCaller;
            Title = "Jump";
            RootTaiyouLine = pRootTaiyouLine;

        }

        bool RoutineInited = false;
        InterpreterInstance RoutineToRun;

        public override object Call()
        {
            string[] Arguments = ReplaceVarLiterals();

            if (!RoutineInited)
            {
                RoutineInited = true;

                string RoutineName = GetArgument(Arguments, 0);

                List<TaiyouLine> AllCode = RootTaiyouLine.GetRoutineInAvailableNamespaces(RoutineName);

                if (AllCode == null) { throw new TaiyouExecutionError(this, "Type Error at Runtime", $"Cannot find Routine ({RoutineName})"); }

 
                // Create a Temporary Script
                RoutineToRun = new InterpreterInstance("", true, AllCode);

            }

            // Run the Routine
            bool RoutineDone = false;
            object Return = null;
            while(!RoutineDone)
            {
                Return = RoutineToRun.Interpret();
                if (ReturnCodes.RoutineJumpStart.Equals(Return)) { RoutineDone = false; } else{ RoutineDone = true; }
            }
            return Return;

        }


    }
}
