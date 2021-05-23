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
using System.Linq;
using Fogoso.WriteToLog;

namespace Fogoso.Taiyou
{
    public static class Global
    {
        public static List<string> LoadedTaiyouScripts = new List<string>();
        public static List<List<TaiyouLine>> LoadedTaiyouScripts_Data = new List<List<TaiyouLine>>();

        // Global Variables
        public static List<Variable> VarList = new List<Variable>();
        public static List<string> VarList_Keys = new List<string>();

        // Functions List
        public static List<List<TaiyouLine>> Functions_Data = new List<List<TaiyouLine>>();
        public static List<string> Functions_Keys = new List<string>();

        // Function Headers List
        public static List<List<TaiyouLine>> ScriptHeaders_Data = new List<List<TaiyouLine>>();
        public static List<string> ScriptHeaders_Keys = new List<string>();
  
        // TSCN and TSUP lists
        static List<string> TSCN = new List<string>();
        static List<string> TSUP = new List<string>();

        // Instructions Dictionary Lists
        static List<string> Instructions_TSUP = new List<string>();
        static List<string> Instructions_NumberOfArguments = new List<string>();

        public static LogObject LastTaiyouLog;

        public static void CallScriptHeader(string ScriptName)
        {
            int HeaderIndex = ScriptHeaders_Keys.IndexOf(ScriptName);

            if (HeaderIndex == -1){ return; }

            // Create a temporary script
            TaiyouScript FunctionToRun = new TaiyouScript($"{TaiyouGlobal.TaiyouReservedNames_SCRIPT_HEADER}_{ScriptName}", true, ScriptHeaders_Data[HeaderIndex]);
            FunctionToRun.Interpret();

            // Remove Header from Memory
            ScriptHeaders_Data.RemoveAt(HeaderIndex);
            ScriptHeaders_Keys.RemoveAt(HeaderIndex);
              
        }
 
        public static int GetVarIndex(string VarName)
        {
            return VarList_Keys.IndexOf(VarName);
        }

        public static Variable GetVarObject(int Index)
        {
            return VarList[Index];
        }

        public static string LiteralReplacer(string Input)
        {
            string EntireInput = Input;
            if (!EntireInput.Contains("#"))
            {
                return EntireInput;
            }
            if (!EntireInput.Contains("$"))
            {
                return EntireInput;
            }

            while (EntireInput.Contains("#"))
            {
                int TokenStartIndex = EntireInput.IndexOf("#", StringComparison.Ordinal);
                int TokenEndIndex = EntireInput.IndexOf("$", StringComparison.Ordinal) + 1;
                int VarNameTokenSize = Math.Abs(TokenStartIndex - TokenEndIndex);
                string Ceira = EntireInput.Substring(TokenStartIndex, VarNameTokenSize);

                Variable VarValue;
                string VarName = Ceira.Replace("$", "").Replace("#", "");
                int VarIndex = GetVarIndex(VarName);
                VarValue = GetVarObject(VarIndex);

                EntireInput = EntireInput.Replace(Ceira, VarValue.ToString());
            }

            return EntireInput;

        }

        public static void LoadTaiyouScripts()
        {
            LogObject TaiyouLog = new LogObject("Taiyou_Initialization");
            LastTaiyouLog = TaiyouLog;

            // Clear the lists
            LoadedTaiyouScripts.Clear();
            LoadedTaiyouScripts_Data.Clear();
            Functions_Data.Clear();
            Functions_Keys.Clear();

            // Load the Dictionary File
            TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouInit", "Loading Dictionary File..."));

            string[] TaiyouDictonary = File.ReadAllLines(Fogoso. Global.TaiyouDirectory + "taiyou_dict.data");
            // Clear the Lists
            TSCN.Clear();
            TSUP.Clear();

            foreach (var item in TaiyouDictonary)
            {
                string[] Splited = item.Split(';');

                TSCN.Add(Splited[0]);
                TSUP.Add(Splited[1]);
            }

            TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouInit", "Loading Instructions Argument Size Dictionary"));

            string[] IntructionsDictionary = File.ReadAllLines(Fogoso.Global.TaiyouDirectory + "instructions_agr_size_dict.data");
            // Clear the Lists 
            Instructions_TSUP.Clear();
            Instructions_NumberOfArguments.Clear();

            foreach (var item in IntructionsDictionary)
            {
                string[] Splited = item.Split(';');

                Instructions_TSUP.Add(Splited[0]);
                Instructions_NumberOfArguments.Add(Splited[1]);
            }

            // Find all scripts
            TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouInit", "Listing Scripts on Script Path..."));

            string[] AllScripts = Directory.GetFiles(Fogoso.Global.TaiyouDirectory, "*.tiy", SearchOption.AllDirectories);

            if (AllScripts.Length == 0)
            {
                TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouInit", "No scripts to load!"));

                TaiyouLog.FinishLog(false);
                return;
            }

            // Read every script
            for (int i = 0; i < AllScripts.Length; i++)
            {
                // Set the Script Name
                string ScriptName = AllScripts[i].Replace(Fogoso.Global.TaiyouScriptsDirectory, "");
                ScriptName = ScriptName.Replace(".tiy", "");
                LoadedTaiyouScripts.Add(ScriptName);

                // ##########################################
                // ######### -- Parser Step 1 - #############
                // ##########################################
                TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step1", "Reading Script [" + ScriptName + "]"));
 
                // Define some variables
                string ReadText = File.ReadAllText(Fogoso.Global.TaiyouScriptsDirectory + ScriptName + ".tiy", new System.Text.UTF8Encoding());
                string[] ReadTextLines = ReadText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                List<string> CorrectTextLines = new List<string>();
                List<TaiyouLine> ParsedCode = new List<TaiyouLine>();

                // Remove every line that is less than 3 Characters
                int index = -1;

                bool IsReadingFunctionLine = false;
                bool LastFuncIsFunctionHeader = false;
                bool FunctionHeaderDeclared = false;
                string LastFuncLineName = "";
                var FunctionCode = new List<TaiyouLine>();

                for (int i2 = 0; i2 < ReadTextLines.Length; i2++)
                {
                    string line = ReadTextLines[i2];
                    line = line.TrimEnd(' ');

                    if (line.Length < 3) { TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step1", "Removed empty line")); continue; }
                    if (line.StartsWith(TaiyouGlobal.TaiyouToken_LINE_COMMENT, StringComparison.Ordinal)) { TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step1", "Removed comment line")); continue; }

                    // Initialize the Function Reading
                    if (line.StartsWith(TaiyouGlobal.TaiyouToken_FUNCTION_DECLARING, StringComparison.Ordinal))
                    {
                        // Check if last function has ended 
                        if (IsReadingFunctionLine)
                        {
                            throw new Exception(TaiyouParserError("Type Error!\nExpected EndFunction token before starting new function block\nAt Script [" + ScriptName + "]\nNear function [" + LastFuncLineName + "]\n\nTip: You probally missed to add the end function token at the end of a function.", TaiyouLog));
                        }

                        TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-FunctionBlock", "Found Function Block"));
                        string FunctionName = line.Split('"')[1];

                        // Check if function is Script Header
                        if (FunctionName.Equals("@HEADER"))
                        {
                            // Dont allow declaring script header twice
                            if (FunctionHeaderDeclared) { throw new Exception(TaiyouParserError("Type Error!\nScript Header declared twice\nAt Script [" + ScriptName + "]\nNear function [" + LastFuncLineName + "]\n\nTip: You probally tryied to declare script header twice", TaiyouLog));  }
                            TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-FunctionBlock", "Function is script header"));
                            FunctionName = $"{ScriptName}_HEADER";
                            LastFuncIsFunctionHeader = true;
      
                            IsReadingFunctionLine = true;
                            FunctionHeaderDeclared = true;

                            LastFuncLineName = ScriptName;
                            TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-ScriptHeader", "Header parse complete"));

                            continue;

                        }
                        // Check if function is global
                        else if (!FunctionName.StartsWith("global_", StringComparison.Ordinal))
                        {
                            TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-FunctionBlock", "Function is not global, ScriptName tag included."));
                            FunctionName = ScriptName + "_" + FunctionName;

                            IsReadingFunctionLine = true;
   
                            LastFuncLineName = FunctionName;
                            TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-FunctionBlock", "Non-Global Function Name[" + LastFuncLineName + "]"));

                            continue;

                        }

                        else if (FunctionName.StartsWith("global_", StringComparison.Ordinal))
                        {
                            TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-FunctionBlock", "Function is not global, ScriptName tag included."));
                            FunctionName = FunctionName.Replace("global_", "");
 
                            IsReadingFunctionLine = true;
   
                            LastFuncLineName = FunctionName;
                            TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-FunctionBlock", "Global Function Name[" + LastFuncLineName + "]"));
     
                            continue;

                        }
                    
                        continue;
                    }
 
                    // Read the function code
                    if (IsReadingFunctionLine)
                    {
                        // Check if function reading reached to end
                        if (line.StartsWith(TaiyouGlobal.TaiyouToken_FUNCTION_END, StringComparison.Ordinal))
                        { 
                            // Add the key and the data  
                            List<TaiyouLine> Copyied = new List<TaiyouLine>();
                            foreach (var item in FunctionCode)
                            {
                                Copyied.Add(item);
                            }

                            // Last function
                            if (!LastFuncIsFunctionHeader)
                            {
                                Functions_Data.Add(Copyied);
                                Functions_Keys.Add(LastFuncLineName);
                                TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-FunctionBlock", "Sucefully added function [" + LastFuncLineName + "]"));
                             
                            }else{
                                ScriptHeaders_Data.Add(Copyied);
                                ScriptHeaders_Keys.Add(ScriptName);
                                TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-FunctionHeader", "Sucefully added function header."));
                            }


                            IsReadingFunctionLine = false;
                            LastFuncIsFunctionHeader = false;
                            FunctionCode.Clear();
                            LastFuncLineName = "";
                            continue;

                        }

                        // Check if line a Command Line
                        // Only add lines with 4 spaces at the beggining
                        if (line.Length > 3)
                        {
                            if (line.StartsWith(TaiyouGlobal.TaiyouToken_FUNCTION_SPACING, StringComparison.Ordinal))
                            {
                                string EditedLine = line;
                                EditedLine = EditedLine.Remove(0, TaiyouGlobal.TaiyouToken_FUNCTION_SPACING.Length);
                                // If line is less than 3 Characters, ignore it
                                if (EditedLine.Length < 3)
                                {
                                    continue;
                                }
                                // If line is comment line, ignore it
                                if (EditedLine.StartsWith(TaiyouGlobal.TaiyouToken_LINE_COMMENT, StringComparison.Ordinal))
                                {
                                    continue;
                                }

                                // Run Code Revision for this line
                                EditedLine = LineRevision(EditedLine, ScriptName, line, index, TaiyouLog);

                                // Add to final FunctionCode list
                                FunctionCode.Add(new TaiyouLine(EditedLine, ScriptName, EditedLine));

                                TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-FunctionBlock", "Added Instruction [" + EditedLine + "] to function block"));
                            }
                            else
                            {
                                throw new Exception(TaiyouParserError("Identation Error!\nAt function [" + LastFuncLineName + "];\nLine [" + line + "]\n\nTip: You probally missed the first 4 spaces when adding commands to a function block." ,TaiyouLog));
                            }

                        }

                    }

                    // Add the Line if is not a Function Line
                    if (!IsReadingFunctionLine)
                    {
                        if (line.StartsWith(TaiyouGlobal.TaiyouToken_FUNCTION_END, StringComparison.Ordinal))
                        {
                            continue;
                        }
                        TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step1", "Added line [" + line + "] to next code revision"));
                        CorrectTextLines.Add(line);
                    }


                }

                // ##########################################
                // ######### -- Parser Step 2 - #############
                // ##########################################
                TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step2", "Checking if function start was not left behind"));

                // Check if a functions start was not left behind
                if (IsReadingFunctionLine)
                {
                    TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step2", "Fatal Error!\nA function has been initialized and not properly finished.\nin Script(" + ScriptName + ")\nin Function(" + LastFuncLineName + ")\nat Index(" + index + ")"));
                    TaiyouLog.FinishLog(false);

                    throw new FileLoadException(TaiyouParserError("ParserStep2: A function has been initialized and not finished properly.\nin Script(" + ScriptName + ")\nin FunctionName(" + LastFuncLineName + ")\nat Index(" + index + ").", TaiyouLog));
                }

                // ##########################################
                // ######### -- Parser Step 3 - #############
                // ##########################################
                TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step3", "Code Revision"));


                // Convert the result to Array
                string[] ScriptData = CorrectTextLines.ToArray();

                // Check if line is a valid command
                index = -1;
                foreach (var line in ScriptData)
                {
                    index += 1;

                    // Check if line is a Commented Line
                    if (line.StartsWith(TaiyouGlobal.TaiyouToken_LINE_COMMENT, StringComparison.Ordinal))
                    {
                        TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step3 Warning", "CommentLine detected, comment line should be removed in Step 2"));
                        TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step3 Warning", "Line: " + line));
                         
                        continue;
                    }

                    // Final Line
                    string FinalLine = line;

                    // ##########################################
                    // ######### -- Parser Step 4 - #############
                    // ##########################################
                    TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step4", "Convert code to TaiyouLine objects"));


                    // Replace TSCN with TSUP
                    FinalLine = LineRevision(FinalLine, ScriptName, line, index, TaiyouLog);

                    TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step4", "Added line [" + FinalLine + "] to TaiyouLine"));
                    ParsedCode.Add(new TaiyouLine(FinalLine, ScriptName, line));
                }

                // ##########################################
                // ######### -- Parser Step 5 - #############
                // ##########################################

                TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouParser_Step5", "Add Taiyou Line to LoadedScriptData list"));

                LoadedTaiyouScripts_Data.Add(ParsedCode);

            }
            TaiyouLog.Write(Utils.ConsoleWriteWithTitle("TaiyouInit", "Sucefully loaded all scripts."));
            TaiyouLog.FinishLog(false);


        }

        private static string TaiyouParserError(string Message, LogObject CurrentLog)
        {
            string Result = "Error while parsing the Taiyou Script\n" + Message;

            CurrentLog.Write(Utils.ConsoleWriteWithTitle("Taiyou_Parser_Error", "\n" + Result));

            CurrentLog.FinishLog(false);

            return Result;
        }

        public static string ReplaceWithTSUP(string Input, LogObject CurrentLog)
        {
            string LineInstruction = Input.Split(' ')[0];

            // Replace TSCN with TSUP
            int IntructionNameIndex = TSCN.IndexOf(LineInstruction);
            if (IntructionNameIndex == -1)
            {
                CurrentLog.Write(Utils.ConsoleWriteWithTitle("ReplaceWithTSUP Warning", "Unknow TSUP (" + LineInstruction + ")"));

                return Input;
            }

            string InstructionUpcode = TSUP[IntructionNameIndex];

            CurrentLog.Write(Utils.ConsoleWriteWithTitle("ReplaceWithTSUP", "Replaced (" + LineInstruction + ") with (" + InstructionUpcode + ");"));
           
            return Input.Replace(LineInstruction, InstructionUpcode);



        }
 
        public static string LineRevision(string Input, string KeyNameFiltred, string line, int index, LogObject CurrentLog)
        {
            CurrentLog.Write(Utils.ConsoleWriteWithTitle("TaiyouLineRevision", "Replacing with TSUP..."));

            string Pass1 = ReplaceWithTSUP(Input, CurrentLog);
            string FinalResult = "";

            if (Pass1.Length < 3)
            {
                throw new Exception(TaiyouParserError("LineRevision: Code is less than 3 characters.\nPlease check the code.\nin Script(" + KeyNameFiltred + ")\nin Line(" + line + ")\nat Index(" + index + ").", LastTaiyouLog));
            }

            // If the line does not ends with ';' token, throw an error
            CurrentLog.Write(Utils.ConsoleWriteWithTitle("TaiyouLineRevision", "Checking for EndLine Character Type Error"));

            if (!Pass1.EndsWith(";", StringComparison.Ordinal)) { throw new Exception(TaiyouParserError("Type Error!\nLine Ending expected.\n\nat Script [" + KeyNameFiltred + "]\nIn Line [" + line + "]\nTip: You probally missed an end like token ';' at the end of a function or there is blank whitespaces greater than 3 characters.", LastTaiyouLog)); }

            // Remove the ';' token
            string Pass2 = Pass1.Remove(Pass1.Length - 1, 1);
            string Instruction = Pass2.Substring(0, 3);

            CurrentLog.Write(Utils.ConsoleWriteWithTitle("TaiyouLineRevision", "Checking for ArgumentsSizeType  Errors"));
            int InstuctionsArgumentsSizeIndex = Instructions_TSUP.IndexOf(Instruction);
 
            // Check if instruction is not an acidental commentary line
            if (InstuctionsArgumentsSizeIndex == -1) { throw new Exception(TaiyouParserError("Type Error!\nInvalid Command or Instruction [" + Instruction + "]\nAt Script [" + KeyNameFiltred + "]\nIn Line [" + line + "]\nTip: You just typed an command that doesn't exist.", LastTaiyouLog)); }
            int CorrectNumberOfInstructions = Convert.ToInt32(Instructions_NumberOfArguments[InstuctionsArgumentsSizeIndex]);
            // Count how many times " appered and divide by 2 
            int NumberOfInstructions = Pass2.Remove(0, 3).Count(x => x == '"') / 2;

            if (NumberOfInstructions < CorrectNumberOfInstructions)
            {
                throw new Exception(TaiyouParserError("TaiyouLineRevision: The instruction on the line [" + Input + "] does not take less than " + CorrectNumberOfInstructions + " arguments.", LastTaiyouLog));
            }

            FinalResult = Pass2;
            CurrentLog.Write(Utils.ConsoleWriteWithTitle("TaiyouLineRevision", "LineRevision Completed"));

            return FinalResult;

        }

        public static void OptionLine(string[] Input, LogObject CurrentLog)
        {
            foreach (var option in Input)
            {
                Console.WriteLine("Received Option (" + option + ")");
                CurrentLog.Write(Utils.ConsoleWriteWithTitle("TaiyouLineRevision", "LineRevision Completed"));

            }

        }
 
        public static void SetGlobalVariables()
        {
            // Resolution Variables
            SetVariable("int", "Internal.GameWindow.Width", Fogoso.Global.WindowWidth.ToString());
            SetVariable("int", "Internal.GameWindow.Height", Fogoso.Global.WindowHeight.ToString());
            SetVariable("bool", "Internal.GameWindow.Fullscreen", Fogoso.Global.WindowIsFullcreen.ToString());
              
        }
 
        public static void SetVariable(string VarType, string VarTag, string VarValue)
        {
            // Var index
            int VarIndex = VarList_Keys.IndexOf(VarTag);

            // Variable doesn't exist
            if (VarIndex == -1)
            {
                Variable newVar = new Variable(VarType, VarValue, VarTag);

                VarList_Keys.Add(VarTag);
                VarList.Add(newVar);
                return;
            }
 
            VarList[VarIndex].SetValue(VarValue);

        }

        /// <summary>
        /// Reloads everthing
        /// </summary>
        public static void Reload()
        {
            Console.WriteLine(" -- Taiyou --\nReloading everthing...");
            //Game1.UpdateThread.Abort();

            LoadedTaiyouScripts.Clear();
            LoadedTaiyouScripts_Data.Clear();
            VarList.Clear();
            VarList_Keys.Clear();
            Event.EventList.Clear();
            Event.EventListNames.Clear();
            Functions_Keys.Clear();
            Functions_Data.Clear();

            // Re-Load all scripts
            LoadTaiyouScripts();
            Event.RegisterEvent("init", "initial");
            Event.TriggerEvent("init");
   
        }

    }
}
