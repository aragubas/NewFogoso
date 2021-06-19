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
 
namespace Fogoso.Taiyou
{
    public enum VariableType : byte
    {
        String,
        Integer,
        Float,
        Boolean
    }

    public enum VariableGenericType : byte
    {
        String,
        Number,
        Boolean
    }


    public class Variable
    {
        // Object Properties
        public VariableType Type;
        public string Tag;
        public object Value;
        public string SearchPattern;
        public VariableGenericType GenericVarType;

        public static VariableType StringToVarType(string VarTypeString)
        {
            switch(VarTypeString.ToLower())
            {
                case "string":
                    return VariableType.String;

                case "int":
                    return VariableType.Integer;

                case "float":
                    return VariableType.Float;

                case "bool":
                    return VariableType.Boolean;

                default:
                    throw new InvalidOperationException($"Invalid variable type {VarTypeString}.");
            }
        }

        public static VariableGenericType StringToVarGenericType(string VarTypeString)
        {
            switch(VarTypeString.ToLower())
            {
                case "string":
                    return VariableGenericType.String;

                case "int":
                    return VariableGenericType.Number;

                case "float":
                    return VariableGenericType.Number;

                case "bool":
                    return VariableGenericType.Boolean;

                default:
                    throw new InvalidOperationException($"Invalid variable type {VarTypeString}.");
            }

        }

        public static VariableGenericType VariableToVarGenericType(VariableType VarType)
        {
            switch(VarType)
            {
                case VariableType.String:
                    return VariableGenericType.String;

                case VariableType.Integer:
                    return VariableGenericType.Number;

                case VariableType.Float:
                    return VariableGenericType.Number;

                case VariableType.Boolean:
                    return VariableGenericType.Boolean;

            }
            throw new InvalidOperationException($"Invalid variable type {VarType}.");

        }


        public Variable(VariableType varType, object varValue, string VarTag)
        {
            Tag = VarTag;
            Type = varType;
            SearchPattern = "$" + VarTag + "$";
 
            // Set the variable Generic Type
            GenericVarType = VariableToVarGenericType(varType);
    
            // Check for VarCopy
            // TODO: Var copy feature temporary disabled.
            /*
            try{
                if (Convert.ToString(varValue).StartsWith("$", StringComparison.Ordinal))
                { 
                    int VarIndex = Global.VarList_Keys.IndexOf(varValue.ToString().Remove(0, 1));
                    if (VarIndex == -1) { throw new Exception($"Cannot find variable for copy operation. {varValue}"); }
                    Variable varWax = Global.VarList[VarIndex];
  
                    if (varWax.GenericVarType != GenericVarType) { throw new Exception("Cannot copy destination variable to current variable since they are different types of variable."); }

                    Value = Global.VarList[VarIndex].Value;
                    return;
                }
 
            }catch(ArgumentException) {throw new Exception("INTERNAL ERROR!\nError in literal conversion (Cannot convert value to string).");}
            */

            SetValue(varValue);
        }
 
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// Set the Variable value
        /// </summary>
        /// <param name="NewValue">New value.</param>
        public void SetValue(object NewValue)
        {
            switch (Type)
            {
                case VariableType.String:
                    Value = NewValue.ToString();
                    return;

                case VariableType.Integer:
                    Value = Int32.Parse(NewValue.ToString());
                    return;

                case VariableType.Float:
                    Value = float.Parse(NewValue.ToString());
                    return;

                case VariableType.Boolean:
                    Value = NewValue.ToString().ToLower().Equals("true");
                    return;

                default:
                    throw new InvalidOperationException("Variable type is invalid.");

            }

        }

    }
}
