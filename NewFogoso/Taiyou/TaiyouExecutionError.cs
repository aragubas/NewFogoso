/*
   ####### BEGIN APACHE 2.0 LICENSE #######
   Copyright 2021 Aragubas

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
    public class TaiyouExecutionError : Exception
    {
        string ScriptOfOrigin;
        string CommandTitle;
        string ErrorTitle;
        string ErrorDescription;

        public TaiyouExecutionError(TaiyouCommand commandOrigin, string pErrorTitle, string pErrorDescription)
        {
            ScriptOfOrigin = commandOrigin.ScriptCaller;
            CommandTitle = commandOrigin.Title;
            ErrorTitle = pErrorTitle;
            ErrorDescription = pErrorDescription;

        }

        // Override the Message property
        public override string Message
        {
            get
            {
                string message = ErrorTitle + "\nat Script [" + ScriptOfOrigin + "]\nin Function [" + CommandTitle + "]\n\n" + ErrorDescription;
                Utils.ConsoleWriteWithTitle("TaiyouExecutionError!", message);
                return message;
            }
        }
    }
}
