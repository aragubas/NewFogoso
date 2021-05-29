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
    public class CreateVar : TaiyouCommand
    {

        public CreateVar(string[] pArguments, string pScriptCaller, TaiyouLine pRootTaiyouLine)
        {
            OriginalArguments = pArguments;

            ScriptCaller = pScriptCaller;
            Title = "CreateVar";
            RootTaiyouLine = pRootTaiyouLine;

        }

        // Variables
        bool Initailized;
        string VarType;
        string VarTag;
        string VarDefaultValue;

        // === Creates a variable ===
        // 1 - Type
        // 2 - Tag
        // 3 - Default value
        //

        public override int Call()
        {
            string[] Arguments = ReplaceVarLiterals();

            if (!Initailized)
            {
                Initailized = true;

                VarType = GetArgument(Arguments, 0);
                VarTag = GetArgument(Arguments, 1);
                VarDefaultValue = GetArgument(Arguments, 2);

            }

            // Check if variable exists
            int VarID = Global.VarList_Keys.IndexOf(VarTag);

            if (VarID != -1) { return 0; }


            Global.VarList.Add(new Variable(Variable.StringToVarType(VarType), VarDefaultValue, VarTag));
            Global.VarList_Keys.Add(VarTag);
            return 0;
        }

    }
}
