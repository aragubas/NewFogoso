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
namespace Fogoso.Taiyou
{
    public class Variable
    {
        // Object Properties
        public string Type;
        public string Tag;
        public dynamic Value;
        public string SearchPattern;
        public string GenericVarType;

        public Variable(string varType, dynamic varValue, string VarTag)
        {
            Tag = VarTag;
            Type = varType;
            SearchPattern = "$" + VarTag + "$";
   
            // Set the variable Generic Type
            SetVarGenericType(varType);
   
            // Check for literals
            try{
                if (Convert.ToString(varValue).StartsWith("$", StringComparison.Ordinal))
                { 
                    int VarIndex = Global.VarList_Keys.IndexOf(varValue.Remove(0, 1));
                    if (VarIndex == -1) { throw new Exception("Cannot find variable [" + varValue + "]."); }
                    Variable varWax = Global.VarList[VarIndex];

                    if (varWax.GenericVarType != GenericVarType) { throw new Exception("Cannot copy destination variable to current variable since they are different types of variable."); }

                    Value = Global.VarList[VarIndex].Value;
                    return;
                }
 
            }catch(ArgumentException) {throw new Exception("INTERNAL ERROR!\nError in literal conversion (Cannot convert value to string).");}
   
            SetValue(varValue);
        }

        /// <summary>
        /// Set the variable generic type
        /// </summary>
        /// <param name="input">Input.</param>
        private void SetVarGenericType(string input)
        {
            switch (input.ToLower())
            {
                case "bool":
                    GenericVarType = "Boolean";
                    break;

                case "int":
                    GenericVarType = "Number";
                    break;

                case "float":
                    GenericVarType = "Number";
                    break;

                case "string":
                    GenericVarType = "String";
                    break;

                default:
                    throw new ArgumentException("Cannot set GenericType\nVariable type [" + input + "] is invalid.");

            }

        }
 
        /// <summary>
        /// Returns the value
        /// </summary>
        /// <returns>The value.</returns>
        public dynamic GetValue()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// Set the Variable value
        /// </summary>
        /// <param name="NewValue">New value.</param>
        public void SetValue(dynamic NewValue)
        {
            switch (Type.ToLower())
            {
                case "string":
                    Value = Convert.ToString(NewValue);
                    return;

                case "int":
                    Value = Convert.ToInt32(NewValue);
                    return;

                case "float":
                    Value = float.Parse(NewValue);
                    return;

                case "bool":
                    Value = Convert.ToBoolean(NewValue);
                    return;

                default:
                    throw new InvalidOperationException("Variable type is invalid.");

            }

        }

    }
}
