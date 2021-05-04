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
using System.IO;
using Fogoso;

namespace Fogoso.WriteToLog
{
    public class LogObject
    {
        string AllText;
        string LogTitle;


        public LogObject(string pLogTitle)
        {
            LogTitle = pLogTitle;
            AllText = "========== Log Started in: " + DateTime.Now.ToShortTimeString() + " ==========";
        }

        public void Write(string text)
        {
            AllText += "\n" + text;
        }

        public void FinishLog(bool WriteDetails)
        {
            // Add Bottom Information
            AllText += "\n\nWIC Engine Version [" + Global.MatchVersion_CurrentVersionString + "]\n" +
                       "Log Written at: " + DateTime.Now.ToShortTimeString() + " in " + DateTime.Now.ToShortDateString();

            // Create Directory
            Directory.CreateDirectory(Global.SystemLog_Folder);

            // WriteFile
            string FileName = Global.SystemLog_Folder + "/" + LogTitle + ".log";
            File.WriteAllText(FileName, AllText);


        }

    }
}
