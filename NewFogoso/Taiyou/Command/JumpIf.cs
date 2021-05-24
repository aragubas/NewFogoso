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
using Fogoso;
using Fogoso.Taiyou;

namespace Fogoso.Taiyou.Command
{
    public class JumpIf : TaiyouCommand
    {
        public JumpIf(string[] pArguments, string pScriptCaller, TaiyouLine pRootTaiyouLine)
        {
            OriginalArguments = pArguments;
            ScriptCaller = pScriptCaller;
            Title = "JumpIf";
            RootTaiyouLine = pRootTaiyouLine;

        }

        // JumpIf - Essential Logic Block
        // =======================================
        // 1 - Comparator 1
        // 2 - Comparation Type ('==', '!=', '>=', '<=', '<', '>')
        // 3 - Comparator 2
        // 4 - Function to call if true
        // 5 - (optional) Function to call if false
        // 
        // ----
        // Avaliable Routines:
        // ----
        // UPDATE_INTERNAL_VARIABLE  |  Update interval variables (such as video mode, cursor position etc.)
 
        bool ArgumentsSet = false;
        string pComparator1;
        string pComparatorType;
        string pComparator2;
        string pFunctionToCall;
        string pFunctionToCallIfFalse;

        public override int Call()
        {
            string[] Arguments = ReplaceVarLiterals();

            if (!ArgumentsSet)
            {
                ArgumentsSet = true;

                pComparatorType = GetArgument(Arguments, 1).ToLower();
                pFunctionToCall = GetArgument(Arguments, 3);
                try{ pFunctionToCallIfFalse = GetArgument(Arguments, 4); } catch(IndexOutOfRangeException) { pFunctionToCallIfFalse = ""; }
            }
            pComparator1 = GetArgument(Arguments, 0);
            pComparator2 = GetArgument(Arguments, 2);

            switch (pComparatorType)
            {
                case "==":
                    if (pComparator1 == pComparator2)
                    {
                        // Do Action
                        return CallFunction(pFunctionToCall);
                        
                    }else{
                        if (pFunctionToCallIfFalse != "")
                        {
                            return CallFunction(pFunctionToCallIfFalse);
                        }
                    }
                    break;

                case "!=":
                    if (pComparator1 != pComparator2)
                    {
                        // Do Action
                        return CallFunction(pFunctionToCall);
                    }else{
                        if (pFunctionToCallIfFalse != "")
                        {
                            return CallFunction(pFunctionToCallIfFalse);
                        }
                    }
                    break;


                case ">=":
                    if (float.Parse(pComparator1) >= float.Parse(pComparator2))
                    {
                        // Do Action
                        return CallFunction(pFunctionToCall);
                    }else{
                        if (pFunctionToCallIfFalse != "")
                        {
                            return CallFunction(pFunctionToCallIfFalse);
                        }
                    }
                    break;

                case ">":
                    if (float.Parse(pComparator1) > float.Parse(pComparator2))
                    {
                        // Do Action
                        return CallFunction(pFunctionToCall);
                    }else{
                        if (pFunctionToCallIfFalse != "")
                        {
                            return CallFunction(pFunctionToCallIfFalse);
                        }
                    }
                    break;


                case "<=":
                    if (float.Parse(pComparator1) <= float.Parse(pComparator2))
                    {
                        // Do Action
                        return CallFunction(pFunctionToCall);
                    }else{
                        if (pFunctionToCallIfFalse != "")
                        {
                            return CallFunction(pFunctionToCallIfFalse);
                        }
                    }
                    break;

                case "<":
                    if (float.Parse(pComparator1) < float.Parse(pComparator2))
                    {
                        // Do Action
                        return CallFunction(pFunctionToCall);
                    }else{
                        if (pFunctionToCallIfFalse != "")
                        {
                            return CallFunction(pFunctionToCallIfFalse);
                        }
                    }
                    break;

                default:
                    Utils.ConsoleWriteWithTitle("TaiyouCommand.JumpIf", $"Invalid comparation type {pComparatorType}");
                    break;
 
            }

            return 0;
        }

        bool FunctionCallInitiated = false;
        TaiyouScript FunctionToRun;

        private int CallFunction(string FunctionToCall)
        {
            // Create a temporary script
            if (!FunctionCallInitiated)
            {
                FunctionCallInitiated = true;
                if (!FunctionToCall.StartsWith("global_", StringComparison.Ordinal))
                {
                    FunctionToCall = ScriptCaller + "_" + FunctionToCall;
                }


                int FunctionIndex = Global.Functions_Keys.IndexOf(FunctionToCall);

                if (FunctionIndex == -1) { throw new TaiyouExecutionError(this, "Type Error!", "Cannot find function [" + FunctionToCall + "]"); }

                List<TaiyouLine> AllCode = Global.Functions_Data[FunctionIndex];

                // Create an Script Instance
                FunctionToRun = new TaiyouScript("", true, Global.Functions_Data[FunctionIndex]);
 
            }

            return FunctionToRun.Interpret();

        }


    }
}
