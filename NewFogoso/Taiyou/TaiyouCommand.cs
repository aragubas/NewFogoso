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
using Fogoso;
using Fogoso.Taiyou;

namespace Fogoso.Taiyou
{
    public abstract class TaiyouCommand
    {
        public string ScriptCaller { get; set; }
        public string Title { get; set; }
        public string[] OriginalArguments { get; set; }
        public TaiyouLine RootTaiyouLine;

        public virtual int Call()
        {
            return 0;
        }

        public string[] ReplaceVarLiterals()
        {
            string[] pArguments = new string[OriginalArguments.Length];
            OriginalArguments.CopyTo(pArguments, 0);

            for (int i = 0; i < OriginalArguments.Length; i++)
            {
                pArguments[i] = Fogoso.Taiyou.Global.LiteralReplacer(OriginalArguments[i]);
            }

            return pArguments;
        }

        public string GetArgument(string[] Arguments, int ArgumentIndex)
        {
            return Utils.GetSubstring(Arguments[ArgumentIndex], '"');
        }

    }
}
