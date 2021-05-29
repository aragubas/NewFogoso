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
namespace Fogoso.Taiyou.Command
{
    public static class OffsetInteger
    {
        
        public static void call(string[] Args, string CallerScript)
        {
            string OperatorVarName = Utils.GetSubstring(Args[0], '"');
            string OffsetAmount = Utils.GetSubstring(Args[1], '"');
            int iOffsetAmount = 0;

            // Operator Var
            int OperatorVarIndex = Global.VarList_Keys.IndexOf(OperatorVarName);
            Variable OperatorVariable = Global.VarList[OperatorVarIndex];
            int OperatorValue = 0;

            // Check if OperatorVariable is an integer
            if (OperatorVariable.Type != VariableType.Integer) { throw new Exception("Operator variable is not an integer."); }
            OperatorValue = OperatorVariable.GetValue();


            // Get the OffsetAmmount from Variable
            if (OffsetAmount.StartsWith("$", StringComparison.Ordinal))
            {
                int OffsetAmountVarIndex = Global.VarList_Keys.IndexOf(OffsetAmount.Remove(0, 1));
                if (Global.VarList[OffsetAmountVarIndex].Type != VariableType.Integer) { throw new Exception("OffserAmmount variable is not an integer"); }

                iOffsetAmount = Global.VarList[OffsetAmountVarIndex].GetValue();
            }
            else
            {
                iOffsetAmount = Int32.Parse(OffsetAmount);
            }


            OperatorVariable.SetValue(OperatorValue + iOffsetAmount);

        }


    }
}