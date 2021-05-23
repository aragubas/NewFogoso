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

        bool FunctionInited = false;
        TaiyouScript FunctionToRun;

        public override int Call()
        {
            string[] Arguments = ReplaceVarLiterals();

            if (!FunctionInited)
            {
                FunctionInited = true;

                string FunctionName = GetArgument(Arguments, 0);

                // Set the right function name
                if (!FunctionName.StartsWith("global_", StringComparison.Ordinal))
                {
                    FunctionName = ScriptCaller + "_" + FunctionName;
                }
                if (FunctionName.StartsWith("global_", StringComparison.Ordinal))
                {
                    FunctionName = FunctionName.Replace("global_", "");
                }
   

                int FunctionIndex = Global.Functions_Keys.IndexOf(FunctionName);

                if (FunctionIndex == -1) { throw new TaiyouExecutionError(this, "Type Error!", "Cannot find function [" + FunctionName + "]"); }

                List<TaiyouLine> AllCode = Global.Functions_Data[FunctionIndex];

                // Create a Temporary Script
                FunctionToRun = new TaiyouScript("", true, Global.Functions_Data[FunctionIndex]);

            }

            // Run the Function
            FunctionToRun.Interpret();

            return 0;
        }


    }
}
