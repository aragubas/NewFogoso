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
namespace Fogoso.Taiyou
{
    public class TaiyouLine
    {
        public TaiyouCommand FunctionCall;
        public string[] Arguments;
        public string ScriptOfOrigin = "";
        public string OriginalLineCode = "";
        public int OriginalLineNumber = -1;
        public InterpreterInstance ParentScript;

        public TaiyouLine(string Line, string pScriptOfOrigin, int pOriginalLineNumber)
        {
            string CommandCode = Line.Substring(0, 3);
            // Set the Arguments string
            Arguments = Line.Remove(0, 3).Split(',');
            ScriptOfOrigin = pScriptOfOrigin;
            OriginalLineCode = Line;
            OriginalLineNumber = pOriginalLineNumber;
             
            // Switch Case the TGEUC Interpretation
            switch (CommandCode)
            {
                case "001":
                    FunctionCall = new Command.WriteLine(Arguments, ScriptOfOrigin, this);
                    break;

                case "002":
                    FunctionCall = new Command.CreateVar(Arguments, ScriptOfOrigin, this);
                    break;

                case "003":
                    FunctionCall = new Command.IntegerOperation(Arguments, ScriptOfOrigin, this);
                    break;

                case "004":
                    FunctionCall = new Command.EventHandler(Arguments, ScriptOfOrigin, this);
                    break;

                case "005":
                    FunctionCall = new Command.Jump(Arguments, ScriptOfOrigin, this);
                    break;

                case "006":
                    FunctionCall = new Command.SetOption(Arguments, ScriptOfOrigin, this);
                    break;

                case "007":
                    FunctionCall = new Command.JumpIf(Arguments, ScriptOfOrigin, this);
                    break;

                case "008":
                    FunctionCall = new Command.SetVarValue(Arguments, ScriptOfOrigin, this);
                    break;

                case "009":
                    FunctionCall = new Command.Halt(Arguments, ScriptOfOrigin, this);
                    break;

                case "00A":
                    FunctionCall = new Command.HaltExecution(Arguments, ScriptOfOrigin, this);
                    break;

                case "00B":
                    FunctionCall = new Command.HaltIfEqual(Arguments, ScriptOfOrigin, this);
                    break;

                case "00C":
                    FunctionCall = new Command.SetVideoMode(Arguments, ScriptOfOrigin, this);
                    break;

                case "00D":
                    FunctionCall = new Command.SetCursor(Arguments, ScriptOfOrigin, this);
                    break;
 
                case "00E":
                    FunctionCall = new Command.InternalRoutine(Arguments, ScriptOfOrigin, this);
                    break;
 
                case "00F":
                    FunctionCall = new Command.CallScript(Arguments, ScriptOfOrigin, this);
                    break;
 
                case "00G":
                    FunctionCall = new Command.DebugVariable(Arguments, ScriptOfOrigin, this);
                    break;
 
                case "00H":
                    FunctionCall = new Command.PrecacheFont(Arguments, ScriptOfOrigin, this);
                    break;
 
                case "00I":
                    FunctionCall = new Command.JumpToStart(Arguments, ScriptOfOrigin, this);
                    break;
 

                default:
                    Console.WriteLine("Taiyou.Interpreter : Unknow TSUP (" + CommandCode + ")");
                    throw new ArgumentOutOfRangeException("Taiyou.Interpreter : Unknow TSUP (" + CommandCode + ")");

            }


        }

        public object call()
        {
            return FunctionCall.Call();
        }

        public string[] GetArguments()
        {
            return Arguments;
        }

    }
}
