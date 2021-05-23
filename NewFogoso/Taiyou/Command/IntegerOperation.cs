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
using System.Text.RegularExpressions;
using Fogoso.Taiyou;

namespace Fogoso.Taiyou.Command
{
    public class IntegerOperation : TaiyouCommand
    {
        public IntegerOperation(string[] pArguments, string pScriptCaller, TaiyouLine pRootTaiyouLine)
        {
            OriginalArguments = pArguments;

            ScriptCaller = pScriptCaller;
            Title = "IntegerOperation";
            RootTaiyouLine = pRootTaiyouLine;

        }

        // === Integer Operation ===
        // 1 - Operator Var Name
        // 2 - Math Operation
        // 3 - Actuator Var Name (you can use # for literals)

        public override int Call()
        {
            string[] Arguments = ReplaceVarLiterals();

            string OperatorVarName = GetArgument(Arguments, 0);
            string MathOperation = GetArgument(Arguments, 1);
            string ActuatorVarName = GetArgument(Arguments, 2);

            // Set the Operator
            int OperatorIndex = Global.VarList_Keys.IndexOf(OperatorVarName);
            if (OperatorIndex == -1) { throw new IndexOutOfRangeException("Cannot find the variable [" + OperatorVarName + "]."); }
            Variable OperatorVariable = Global.VarList[OperatorIndex];
            if (OperatorVariable.Type != "Int") { throw new Exception("Variable [" + OperatorVarName + "] is not an Integer."); }
            var OperatorValue = OperatorVariable.GetValue();

            if (!Regex.IsMatch(Convert.ToString(OperatorVariable.Value), @"\d"))
            {
                throw new IndexOutOfRangeException("Operator Value is not an number.");
            }

            // Set the Actuator
            int ActuatorIndex = Global.VarList_Keys.IndexOf(ActuatorVarName);
            string ActuatorValue = "";
            if (ActuatorIndex == -1)
            {
                // Check if Actuator is a literal
                if (ActuatorVarName.StartsWith("#", StringComparison.Ordinal))
                {
                    ActuatorValue = ActuatorVarName.Remove(0, 1);
                }

            }
            else { ActuatorValue = Global.VarList[ActuatorIndex].Value; }

            if (!Regex.IsMatch(ActuatorValue, @"\d"))
            {
                throw new IndexOutOfRangeException("Literals must start with '#' token. [" + OperatorVarName + "].");
            }
 
            int ActuatorInInteger = Convert.ToInt32(ActuatorValue);

            switch (MathOperation)
            {
                case "+":
                    Global.VarList[OperatorIndex].SetValue(OperatorValue + ActuatorInInteger);
                    break;

                case "-":
                    Global.VarList[OperatorIndex].SetValue(OperatorValue - ActuatorInInteger);
                    break;

                case "/":
                    Global.VarList[OperatorIndex].SetValue(OperatorValue / ActuatorInInteger);
                    break;

                case "*":
                    Global.VarList[OperatorIndex].SetValue(OperatorValue * ActuatorInInteger);
                    break;

                case "%":
                    Global.VarList[OperatorIndex].SetValue(OperatorValue % ActuatorInInteger);
                    break;

                default:
                    throw new IndexOutOfRangeException("Invalid MathOperation [" + MathOperation + "]");
            }

            return 0;
        }


    }
}
