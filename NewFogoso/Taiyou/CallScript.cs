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
using System.Threading;

namespace Fogoso.Taiyou
{
    public static class CallScript
    {
        private static string LastCallError = "";

  
        public static object Call(string pScriptName)
        {
            try
            {
                InterpreterInstance instance = new InterpreterInstance(pScriptName);
                return instance.Interpret();

            }catch(Exception ex)
            {
                if (LastCallError != pScriptName)
                {
                    Utils.ConsoleWriteWithTitle("CallTaiyouScript", $"Error while calling taiyou script {ex.Message} ", true);
                    LastCallError = pScriptName;
                }
                return null;
            }
             
        } 
          
    }
}
