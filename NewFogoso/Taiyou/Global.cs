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
using Microsoft.Xna.Framework.Input;

namespace Fogoso.Taiyou
{
    public static class Global
    {
        public static List<string> LoadedTaiyouScripts = new List<string>();
        public static List<List<TaiyouLine>> LoadedTaiyouScripts_Data = new List<List<TaiyouLine>>();

        // Function Headers List
        public static List<string> ScriptHeaders_Keys = new List<string>();
        public static List<List<TaiyouLine>> ScriptHeaders_Data = new List<List<TaiyouLine>>();
   
        // TSCN and TSUP lists
        static List<string> TSCN = new List<string>();
        static List<string> TSUP = new List<string>();

        // Instructions Dictionary Lists
        static List<string> Instructions_TSUP = new List<string>();
        static List<string> Instructions_NumberOfArguments = new List<string>();

        // Namespaces Dictionary
        public static Dictionary<string, TaiyouNamespace> NamespacesDictionary = new Dictionary<string, TaiyouNamespace>();

        public static TaiyouNamespace AddNamespace(string NamespaceName)
        {
            
            return NamespacesDictionary[NamespaceName];
        }
        

        public static List<TaiyouLine> GetScript(string ScriptName)
        {
            int ScriptIndex = LoadedTaiyouScripts.IndexOf(ScriptName);
            if (ScriptIndex == -1) { return null; }

            return LoadedTaiyouScripts_Data[ScriptIndex];
        }

        public static void CallScriptHeader(string ScriptName)
        {
            int HeaderIndex = ScriptHeaders_Keys.IndexOf(ScriptName);

            if (HeaderIndex == -1){ return; }

            // Create a temporary script
            InterpreterInstance FunctionToRun = new InterpreterInstance($"{TaiyouGlobal.TaiyouReservedNames_SCRIPT_HEADER}_{ScriptName}", true, ScriptHeaders_Data[HeaderIndex]);
            FunctionToRun.Interpret();

            // Remove Header from Memory
            ScriptHeaders_Data.RemoveAt(HeaderIndex);
            ScriptHeaders_Keys.RemoveAt(HeaderIndex);
              
        }
 
        public static string LiteralReplacer(string Input, TaiyouLine RootTaiyouLine)
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

                string VarName = Ceira.Replace("$", "").Replace("#", "");
                Variable VarValue = RootTaiyouLine.GetVariableInAvailableNamespaces(VarName);
                if (VarValue == null) { throw new Exception($"Type Error in Execution!\nThe varible ({VarName}) was not found in literal."); }
 
                EntireInput = EntireInput.Replace(Ceira, VarValue.ToString());
            }

            return EntireInput;

        }

        public static void LoadTaiyouScripts()
        {
            // Clear the lists
            LoadedTaiyouScripts.Clear();
            LoadedTaiyouScripts_Data.Clear();
 
            // Load the Dictionary File
            Utils.ConsoleWriteWithTitle("TaiyouInit", "Loading Dictionary File...");

            string[] TaiyouDictonary = File.ReadAllLines(Fogoso. Global.TaiyouDirectory + "instructions_names.data");
            // Clear the Lists
            TSCN.Clear();
            TSUP.Clear();

            foreach (var item in TaiyouDictonary)
            {
                string[] Splited = item.Split(';');

                TSCN.Add(Splited[0]);
                TSUP.Add(Splited[1]);
            }

            Utils.ConsoleWriteWithTitle("TaiyouInit", "Loading Instructions Argument Size Dictionary");

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
            Utils.ConsoleWriteWithTitle("TaiyouInit", "Listing Scripts on Script Path...");

            string[] AllScripts = Directory.GetFiles(Fogoso.Global.TaiyouDirectory, "*.tiy", SearchOption.AllDirectories);

            if (AllScripts.Length == 0)
            {
                Utils.ConsoleWriteWithTitle("TaiyouInit", "No scripts to load!");
                return;
            }
            
            ///////////////////////////////////////
            // Initiality Declare System Namespace
            ///////////////////////////////////////
            NamespacesDictionary.Add("System", new TaiyouNamespace("system"));
 
            ////////////////////////////////////////
            // Initialiaty declare all namespaces
            ///////////////////////////////////////
            for (int i = 0; i < AllScripts.Length; i++)
            {
                // Set the Script Name
                string ScriptName = AllScripts[i].Replace(Fogoso.Global.TaiyouScriptsDirectory, "");
                ScriptName = ScriptName.Replace(".tiy", "").Replace(Fogoso.Global.OSSlash, ".");
                LoadedTaiyouScripts.Add(ScriptName);

                // Debug
                Utils.ConsoleWriteWithTitle("TaiyouParser_NamespaceDeclaring", "Reading Script [" + ScriptName + "]");
 
                // Define some variables
                string ReadText = File.ReadAllText(Fogoso.Global.TaiyouScriptsDirectory + ScriptName.Replace(".", Fogoso.Global.OSSlash) + ".tiy", new System.Text.UTF8Encoding());
                string[] ReadTextLines = ReadText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                // Read line by line
                for (int i2 = 0; i2 < ReadTextLines.Length; i2++)
                {
                    string line = ReadTextLines[i2];
                    line = line.Trim();
     
                    if (line.Length < 3) { continue; }
                    if (line.StartsWith(TaiyouGlobal.TaiyouToken_LINE_COMMENT, StringComparison.Ordinal)) { continue; }
                    
                    ///////////////////////////
                    // Read Namespace declarng
                    ///////////////////////////
                    if (line.StartsWith(TaiyouGlobal.TaiyouToken_COMMAND_BLOCK, StringComparison.Ordinal))
                    {
                        string LineRoutineTypeFiltred = line.Split(' ')[0].Remove(0, 1);
  
                        ////////////////////////////
                        // Read Namespace declaring 
                        ////////////////////////////
                        if (LineRoutineTypeFiltred.Equals(TaiyouGlobal.TaiyouToken_NAMESPACE_COMMAND_BLOCK))
                        {
                            string NamespaceName = line.Split('"')[1];

                            if (NamespacesDictionary.ContainsKey(NamespaceName)) { break; }
                             
                            // Create Namespace
                            // Item already added exception
                            NamespacesDictionary.Add(NamespaceName, new TaiyouNamespace(NamespaceName));
                            Utils.ConsoleWriteWithTitle("TaiyouParser_NamespaceDeclaring", $"Namespace ({NamespaceName}) has been created.");
                            break;
                        }

                    }

                }

            }


            // Read every script 
            for (int i = 0; i < AllScripts.Length; i++)
            {
                // Set the Script Name
                string ScriptName = AllScripts[i].Replace(Fogoso.Global.TaiyouScriptsDirectory, "");
                ScriptName = ScriptName.Replace(".tiy", "").Replace(Fogoso.Global.OSSlash, ".");
                LoadedTaiyouScripts.Add(ScriptName);

                // ##########################################
                // ######### -- Parser Step 1 - #############
                // ##########################################
                Utils.ConsoleWriteWithTitle("TaiyouParser_Step1", "Reading Script [" + ScriptName + "]");
 
                // Define some variables
                string ReadText = File.ReadAllText(Fogoso.Global.TaiyouScriptsDirectory + ScriptName.Replace(".", Fogoso.Global.OSSlash) + ".tiy", new System.Text.UTF8Encoding());
                string[] ReadTextLines = ReadText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                List<string> CorrectTextLines = new List<string>();
                List<TaiyouLine> ParsedCode = new List<TaiyouLine>();
 
                ///////////////////////////////////////////
                // Per-Script session available variables
                ///////////////////////////////////////////
                bool IsReadingCommandBlock = false;
                bool LastRoutineIsScriptHeader = false;
                bool ScriptHeaderAlreadyDeclared = false;
                string LastFuncLineName = "";
                List<TaiyouLine> RoutineCode = new List<TaiyouLine>();
                List<string> ImportedNamespaces = new List<string>();
                string InstanceNamespace = null;
                RoutineAccessLevel LastRoutineAccessLevel = 0;

                int LineNumber = 0; 


                for (int i2 = 0; i2 < ReadTextLines.Length; i2++)
                {
                    string line = ReadTextLines[i2];
                    line = line.Trim();
                    LineNumber += 1;
     
                    if (line.Length < 3) { continue; }
                    if (line.StartsWith(TaiyouGlobal.TaiyouToken_LINE_COMMENT, StringComparison.Ordinal)) { continue; }

                    ///////////////////////////////
                    // Continue Reading Function //
                    ///////////////////////////////
                    if (IsReadingCommandBlock)
                    {
                        // Check for End Function token
                        if (line.StartsWith(TaiyouGlobal.TaiyouToken_ROUTINE_END, StringComparison.Ordinal))
                        { 
                            // Add the key and the data  
                            List<TaiyouLine> Copyied = new List<TaiyouLine>();
                            foreach (var item in RoutineCode)
                            {
                                Copyied.Add(item);
                            }
 
                            // Add last function to Routine List into current namespace
                            if (!LastRoutineIsScriptHeader)
                            {
                                TaiyouRoutine routineObj = new TaiyouRoutine(Copyied, LastFuncLineName, LastRoutineAccessLevel);
                                NamespacesDictionary[InstanceNamespace].RoutineList.Add(routineObj);
                                Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-FunctionBlock", "Sucefully added Routine [" + LastFuncLineName + "]");
                                  
                            }else{ // Add last script header to ScriptHeader
                                ScriptHeaders_Data.Add(Copyied);
                                ScriptHeaders_Keys.Add(ScriptName);
                                Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-FunctionHeader", "Sucefully added Script Header.");
                                 
                            }


                            IsReadingCommandBlock = false;
                            LastRoutineIsScriptHeader = false;
                            RoutineCode.Clear();
                            LastFuncLineName = "";
                            continue;

                        }

                        // Check if line is not empty
                        if (line.Length > 3)
                        {
                            string EditedLine = line;
                            EditedLine = EditedLine.Trim().TrimEnd();
                            // If line is less than 3 Characters or is a commentary line, ignore it
                            if (EditedLine.Length < 3 || EditedLine.StartsWith(TaiyouGlobal.TaiyouToken_LINE_COMMENT, StringComparison.Ordinal))
                            {
                                continue;
                            }

                            // Run Line Revision
                            EditedLine = LineRevision(EditedLine, ScriptName, line, LineNumber);
   
                            // Add to final FunctionCode list
                            RoutineCode.Add(new TaiyouLine(EditedLine, ScriptName, LineNumber, ImportedNamespaces, InstanceNamespace));
 
                            Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-FunctionBlock", "Added Instruction [" + EditedLine + "] to function block");

                        }
                    }
 
 
                    // Check Routine Reading
                    if (line.StartsWith(TaiyouGlobal.TaiyouToken_COMMAND_BLOCK, StringComparison.Ordinal))
                    {

                        // Check if last routine has ended 
                        if (IsReadingCommandBlock)
                        {
                            throw new Exception(TaiyouParserError("Type Error!\nExpected EndRoutine token before starting new routine block\nAt Script [" + ScriptName + "]\nNear routine [" + LastFuncLineName + "]\n\nTip: You probally missed to add the end routine token at the end of a routine."));
                        } 
                        Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-RoutineBlock", "Routine Block Token Detected");
                         
                        string LineRoutineTypeFiltred = line.Split(' ')[0].Remove(0, 1);

                        ////////////////////////////
                        // Read Namespace declaring 
                        ////////////////////////////
                        if (LineRoutineTypeFiltred.Equals(TaiyouGlobal.TaiyouToken_NAMESPACE_COMMAND_BLOCK))
                        { 
                            string NamespaceName = line.Split('"')[1];

                            if (InstanceNamespace == null) { InstanceNamespace = NamespaceName; } else { throw new Exception(TaiyouParserError($"Namespace function called twice.\nAt Script {ScriptName}\nLine {line}")); }
                        }


                        ////////////////////
                        // Read Import Flag
                        ////////////////////
                        if (LineRoutineTypeFiltred.Equals(TaiyouGlobal.TaiyouToken_IMPORT_COMMAND_BLOCK))
                        {
                            string NamespaceName = line.Split('"')[1];

                            if (ImportedNamespaces.IndexOf(NamespaceName) != -1) { throw new Exception(TaiyouParserError($"Type Error!\nCannot import namespace ({NamespaceName}) because it does not exist.\nAt script ({ScriptName})\nLine: {line}")); }
                            
                            ImportedNamespaces.Add(NamespaceName);
                            continue;
                        }

                        ///////////////////////////////
                        // Read a Script Header Block
                        ///////////////////////////////
                        //$HEADER
                        //
                        if (LineRoutineTypeFiltred.Equals(TaiyouGlobal.TaiyouToken_SCRIPT_HEADER_DECLARING))
                        {
                            if (ScriptHeaderAlreadyDeclared) { throw new Exception(TaiyouParserError($"Type Error!\nScript Header declared twice!\nAt script ({ScriptName})\nLine: {line}")); }
                            // Set variables for Script Header Reading
                            LastRoutineIsScriptHeader = true;
                            IsReadingCommandBlock = true;
                            ScriptHeaderAlreadyDeclared = true;
                            
                            continue;
                        }

                        ////////////////////////
                        // Read a Routine Block
                        // ---------------------
                        // Example: Private Routine called main:
                        // $ROUTINE private, main
                        ////////////////////////
                        if (LineRoutineTypeFiltred.Equals(TaiyouGlobal.TaiyouToken_ROUTINE_DECLARING))
                        {
                            // Routine Name
                            string[] LineSplit = line.Split(',');
                            string ArgumentRoutineAccessLevel = LineSplit[0].Replace("$ROUTINE", "").Trim();
                            string RoutineName = LineSplit[1].Trim();

                            // Output to console 
                            Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-RoutinedBlock", $"Routine {RoutineName} has been declared");

                            // Set access modifier
                            switch(ArgumentRoutineAccessLevel.ToLower())
                            {
                                case "private":
                                {
                                    LastRoutineAccessLevel = RoutineAccessLevel.Private;
                                    break;
                                }

                                case "public":
                                {
                                    LastRoutineAccessLevel = RoutineAccessLevel.Public;
                                    break;
                                }

                                default:
                                {
                                    throw new Exception(TaiyouParserError($"Type Error!\nInvalid routine access modifier\nAt Script [{ScriptName}]\nNear function [{LastFuncLineName}]\n\nTip: Valid access modifier are 'private', 'public'"));
                                }

                            }

                            // Set Variable
                            IsReadingCommandBlock = true;
                            LastFuncLineName = RoutineName;
                            Utils.ConsoleWriteWithTitle("TaiyouParser_Step1-RoutineBlock", "Non-Global Routine Name[" + LastFuncLineName + "]");
                             
                            continue;
                        }
                        
                        // Otherwise, continue
                        continue;
                    }
     
                    // Add the Line if is not a Function Line
                    if (!IsReadingCommandBlock)
                    {
                        if (line.StartsWith(TaiyouGlobal.TaiyouToken_ROUTINE_END, StringComparison.Ordinal))
                        { 
                            Utils.ConsoleWriteWithTitle("TaiyouParser_Step1", "WARNING: Routine End Token Found but no Routine is being Currently Read.Expect loss of code if you see that message.");
                            continue;
                        }
                        Utils.ConsoleWriteWithTitle("TaiyouParser_Step1", $"Added line ({line}) to next code revision");
                        CorrectTextLines.Add(line);
                    }


                }

                // ##########################################
                // ######### -- Parser Step 2 - #############
                // ##########################################
                Utils.ConsoleWriteWithTitle("TaiyouParser_Step2", "Checking if function start was not left behind");

                // Check if a functions start was not left behind
                if (IsReadingCommandBlock)
                {
                    Utils.ConsoleWriteWithTitle("TaiyouParser_Step2", $"Type Error!\nA function has been initialized and not finished properly.\nin Script { ScriptName }\nNear Function { LastFuncLineName }\nLine Number: { LineNumber }.");
                    throw new Exception(TaiyouParserError($"Type Error!\nA function has been initialized and not finished properly.\nin Script { ScriptName }\nNear Function { LastFuncLineName }\nLine Number: { LineNumber }."));
                }
                 
                // ##########################################
                // ######### -- Parser Step 3 - #############
                // ##########################################
                Utils.ConsoleWriteWithTitle("TaiyouParser_Step3", "Read Non-Routine Code");
 

                // Convert the result to Array
                string[] ScriptData = CorrectTextLines.ToArray();

                // Check if line is a valid command
                foreach (var line in ScriptData)
                {
                    // Final Line
                    string FinalLine = line;
 
                    // #########################################
                    // ######### -- Parser Step 4 - #############
                    // ##########################################
                    Utils.ConsoleWriteWithTitle("TaiyouParser_Step4", "Convert code to TaiyouLine objects");


                    // Replace TSCN with TSUP
                    FinalLine = LineRevision(FinalLine, ScriptName, line, LineNumber);

                    Utils.ConsoleWriteWithTitle("TaiyouParser_Step4", "Added line [" + FinalLine + "] to TaiyouLine");
                    ParsedCode.Add(new TaiyouLine(FinalLine, ScriptName, LineNumber, ImportedNamespaces, InstanceNamespace));
                }
  
                // ##########################################
                // ######### -- Parser Step 5 - #############
                // ##########################################

                Utils.ConsoleWriteWithTitle("TaiyouParser_Step5", "Add Taiyou Line to LoadedScriptData list");

                LoadedTaiyouScripts_Data.Add(ParsedCode);

            }
            Utils.ConsoleWriteWithTitle("TaiyouInit", "Sucefully loaded all scripts.");

        }

        private static string TaiyouParserError(string Message)
        {
            string Result = "Error while parsing the Taiyou Script\n" + Message;

            Utils.ConsoleWriteWithTitle("Taiyou_Parser_Error", "\n" + Result);

            return Result;
        }

        public static string ReplaceWithTSUP(string Input, string KeyNameFiltred, string line, int lineNumber)
        {
            string LineInstruction = Input.Split(' ')[0].Replace(";", "");
 
            // Replace TSCN with TSUP
            int IntructionNameIndex = TSCN.IndexOf(LineInstruction);
            if (IntructionNameIndex == -1)
            {
                Utils.ConsoleWriteWithTitle("ReplaceWithTSUP Warning", "Unknow TSUP (" + LineInstruction + ")");
                throw new Exception(TaiyouParserError($"Type Error!\nInvalid Command or Instruction ({LineInstruction})\nAt Script {KeyNameFiltred}\nIn Line {line}\nLine Number {lineNumber}\nTip: You just typed an command that doesn't exist.")); 
            }

            string InstructionUpcode = TSUP[IntructionNameIndex];

            Utils.ConsoleWriteWithTitle("ReplaceWithTSUP", "Replaced (" + LineInstruction + ") with (" + InstructionUpcode + ");");
           
            return Input.Replace(LineInstruction, InstructionUpcode);



        }
 
        public static string LineRevision(string Input, string KeyNameFiltred, string line, int lineNumber)
        {
            Utils.ConsoleWriteWithTitle("TaiyouLineRevision", "Replacing with TSUP...");

            string Pass1 = ReplaceWithTSUP(Input, KeyNameFiltred, line, lineNumber);
            string FinalResult = "";

            if (Pass1.Length < 3)
            {
                throw new Exception(TaiyouParserError("LineRevision: Instruction is less than 3 characters.\nInstructions TSUP file contains invalid data."));
            }

            // If the line does not ends with ';' token, throw an error
            Utils.ConsoleWriteWithTitle("TaiyouLineRevision", "Checking for EndLine Character Type Error");
            if (!Pass1.EndsWith(";", StringComparison.Ordinal)) { throw new Exception(TaiyouParserError($"Type Error!\nLine Ending expected.\n\nat Script [{KeyNameFiltred}]\nIn Line [{ line }]\nLine Number {lineNumber}\nTip: You missed an end like token ';' at the end of a command.")); }
 
            // Remove the ';' token
            string Pass2 = Pass1.Remove(Pass1.Length - 1, 1);
            // Instruction should be first 3 characters
            string Instruction = Pass2.Substring(0, 3);
 
            Utils.ConsoleWriteWithTitle("TaiyouLineRevision", "Checking argument size.");
            int InstuctionsArgumentsSizeIndex = Instructions_TSUP.IndexOf(Instruction);
  
            // Check if instruction is valid
            if (InstuctionsArgumentsSizeIndex == -1) { throw new Exception(TaiyouParserError($"Parser Error!\nArgument Size Dictionary is missing a entry for {Instruction}.")); }
            int CorrectNumberOfInstructions = Int32.Parse(Instructions_NumberOfArguments[InstuctionsArgumentsSizeIndex]);
            // Count how many times " appered and divide by 2 
            int NumberOfInstructions = Pass2.Remove(0, 3).Count(x => x == '"') / 2;

            if (NumberOfInstructions < CorrectNumberOfInstructions)
            {
                throw new Exception(TaiyouParserError($"Type Error!\nThe command [{ Input }] does not take less than { CorrectNumberOfInstructions } arguments.\nAt line [{line}]\nLine Number {lineNumber}"));
            }

            FinalResult = Pass2;
            Utils.ConsoleWriteWithTitle("TaiyouLineRevision", "LineRevision Completed");

            return FinalResult;

        }

        public static void OptionLine(string[] Input)
        {
            foreach (var option in Input)
            {
                Console.WriteLine("Received Option (" + option + ")");
                Utils.ConsoleWriteWithTitle("TaiyouLineRevision", "LineRevision Completed");

            }

        }
 
        public static void SetGlobalVariables()
        {
            // Resolution Variables
            SetVariable(VariableType.Integer, "Internal.GameWindow.Width", Fogoso.Global.WindowWidth.ToString());
            SetVariable(VariableType.Integer, "Internal.GameWindow.Height", Fogoso.Global.WindowHeight.ToString());
            SetVariable(VariableType.Boolean, "Internal.GameWindow.Fullscreen", Fogoso.Global.WindowIsFullcreen.ToString());
             
            // Enviroment Variables
            SetVariable(VariableType.Boolean, "Internal.Environment.DebugMode", Main.DebugModeEnabled.ToString());
            SetVariable(VariableType.Integer, "Internal.Environment.FPS", Main.Reference._fps.ToString());
            
            // GameInput Variables 
            SetVariable(VariableType.String, "Internal.GameInput.Cursor.Default", GameInput.defaultCursor);
            SetVariable(VariableType.String, "Internal.GameInput.Cursor", GameInput.CursorImage);
            SetVariable(VariableType.Integer, "Internal.GameInput.Cursor.X", GameInput.CursorPosition.X.ToString());
            SetVariable(VariableType.Integer, "Internal.GameInput.Cursor.Y", GameInput.CursorPosition.Y.ToString());
            SetVariable(VariableType.Integer, "Internal.GameInput.Cursor.ReservedID", GameInput.ReservedModeID.ToString());
  
            // Raw cursor position
            SetVariable(VariableType.Integer, "Internal.Raw.CursorX", Mouse.GetState().X);
            SetVariable(VariableType.Integer, "Internal.Raw.CursorY", Mouse.GetState().Y);
   

        }
 
        public static void SetVariable(VariableType VarType, string VarTag, object VarValue)
        {
            // Set value if variable exsits
            foreach(Variable taiyouVar in NamespacesDictionary["System"].VarList)
            {
                if (taiyouVar.Tag == VarTag) { taiyouVar.SetValue(VarValue); return; }
            }   
 
            // Create Variable if it doesn't exist
            Variable newVar = new Variable(VarType, VarValue, VarTag);
            NamespacesDictionary["System"].VarList.Add(newVar);
            
            return;

        }

    }
}
