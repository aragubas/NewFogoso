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
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace Fogoso.Taiyou
{
    public enum ReturnCodes : byte
    {
        Error,
        ExecutionHalt,
        Halt,
        RoutineJumpStart
    }
     
    public class InterpreterInstance
    {

        private string scriptName;
        private List<TaiyouLine> Code;
        public bool HaltExecution = false;
        bool ScriptHeaderCalled;

        public InterpreterInstance(string ScriptName, bool DirectCode = false, List<TaiyouLine> taiyouLines = null)
        {
            scriptName = ScriptName;
 
            // Find the script on Script List
            if (DirectCode)
            {
                SetCode(taiyouLines);

                return;
            }
            if (!Global.LoadedTaiyouScriptsDict.ContainsKey(ScriptName)) { throw new EntryPointNotFoundException("the Taiyou Script (" + ScriptName + ") does not exist."); }

            SetCode(Global.LoadedTaiyouScriptsDict[ScriptName]);
  
        }
   
        private void SetCode(List<TaiyouLine> Lines)
        {
            Code = Lines;

            // Set parent script for all lines
            for(int i = 0; i < Code.Count; i++)
            {
                Code[i].ParentScript = this;
            }
 
        }

        public object Interpret()
        {             
            if (HaltExecution) { HaltExecution = false; }
            // Call Script Header Function
            if (!ScriptHeaderCalled) { ScriptHeaderCalled = true; Global.CallScriptHeader(scriptName); }

            for(int i = 0; i < Code.Count; i++)
            {
                if (HaltExecution)
                {
                    return ReturnCodes.ExecutionHalt;
                } 

                object ReturnObject = Code[i].call();
   
                if (ReturnObject != null) { return ReturnObject; }
            }

            return null;
        }



    }
}