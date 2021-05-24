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
    public class SetVarValue : TaiyouCommand
    {
        public SetVarValue(string[] pArguments, string pScriptCaller, TaiyouLine pRootTaiyouLine)
        {
            OriginalArguments = pArguments;

            ScriptCaller = pScriptCaller;
            Title = "SetVarValue";
            RootTaiyouLine = pRootTaiyouLine;

        }

        // === Set value of other variable ===
        // 1 - OperatorVarName
        // 2 - New Value Type
        // 3 - New Value
        //


        public override int Call()
        {
            string[] Arguments = ReplaceVarLiterals();

            string OperatorVarName = GetArgument(Arguments, 0);
            string NewValueType = GetArgument(Arguments, 1);
            string NewValue = GetArgument(Arguments, 2);

            // Get the Operator Variable 
            Variable OperatorVariable = Global.VarList[Global.VarList_Keys.IndexOf(OperatorVarName)];

            // Get the NewValueType
            switch (NewValueType)
            {
                case "String":
                    OperatorVariable.SetValue(Convert.ToString(NewValue));
                    return 0;

                case "Bool":
                    OperatorVariable.SetValue(Convert.ToBoolean(NewValue));
                    return 0;

                case "Float":
                    OperatorVariable.SetValue(float.Parse(NewValue));
                    return 0;

                case "Int":
                    OperatorVariable.SetValue(Convert.ToInt32(NewValue));
                    return 0;

                default:
                    throw new TaiyouExecutionError(this, "Type Error!", "Invalid Operation");

            }
        }


    }
}
