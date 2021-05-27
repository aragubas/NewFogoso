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
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fogoso
{
    public class Utils
    {
        /// <summary>
        /// Convert strings to rect.
        /// </summary>
        /// <returns>The to rect.</returns>
        /// <param name="StringToConver">String to conver.</param>
        public static Rectangle StringToRect(string StringToConver)
        {
            string[] RectCode = StringToConver.Split(',');

            int RectX = Convert.ToInt32(RectCode[0]);
            int RectY = Convert.ToInt32(RectCode[1]);
            int RectW = Convert.ToInt32(RectCode[2]);
            int RectH = Convert.ToInt32(RectCode[3]);

            return new Rectangle(RectX, RectY, RectW, RectH);
        }

        /// <summary>
        /// Convert string to color
        /// </summary>
        /// <returns>The to color.</returns>
        /// <param name="StringToConver">String to conver.</param>
        /// <param name="ColorAlphaOveride">Color alpha overide.</param>
        public static Color StringToColor(string StringToConver, int ColorAlphaOveride = -255)
        {
            Color ColorToReturn = Color.Magenta;

            int Color_R = 0;
            int Color_G = 0;
            int Color_B = 0;
            int Color_A = 0;

            string[] ColorCodes = StringToConver.Split(',');

            Color_R = Convert.ToInt32(ColorCodes[0]);
            Color_G = Convert.ToInt32(ColorCodes[1]);
            Color_B = Convert.ToInt32(ColorCodes[2]);
            if (ColorAlphaOveride != -255) { Color_A = ColorAlphaOveride; } else { Color_A = Convert.ToInt32(ColorCodes[3]); }

            ColorToReturn = Color.FromNonPremultiplied(Color_R, Color_G, Color_B, Color_A);

            return ColorToReturn;
        }

        /// <summary>
        /// Wraps the text.
        /// </summary>
        /// <returns>The text.</returns>
        /// <param name="spriteFont">Sprite font.</param>
        /// <param name="text">Text.</param>
        /// <param name="maxLineWidth">Max line width.</param>
        public static string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the substring.
        /// </summary>
        /// <returns>The substring.</returns>
        /// <param name="Input">Input.</param>
        /// <param name="Spliter">Spliter.</param>
        public static string GetSubstring(string Input, char Spliter)
        {
            return Input.Substring(Input.IndexOf(Spliter) + 1, Input.LastIndexOf(Spliter) - 1).Replace(Convert.ToString(Spliter), "");
        }

        /// <summary>
        /// Returns position based on camera offset
        /// </summary>
        /// <returns>The ased on camera offset.</returns>
        /// <param name="CameraPos">Camera current location</param>
        /// <param name="SourcePos">Source Position</param>
        public static int PositionOnCameraOffset(int CameraPos, int SourcePos)
        {
            return SourcePos + CameraPos;
        }

        public static int LimitNumberRange(int Mininum, int Input)
        {
            if (Input <= Mininum)
            {
                return Mininum;
            }

            return Input;
        }

        public static void InitialFSCheck()
        {
            ConsoleWriteWithTitle("FSCheck", "Checking ContentFolder...");

            // ############
            // # Pass 1   #
            // ############
            // Check if ContentFolder exists
            if (!Directory.Exists(Global.ContentFolder))
            {
                ConsoleWriteWithTitle("FSCheck", "Fatal Error!\nContentFolder does not exist.");

                //MessageBox.Show("Cannot find the ContentFolder.\nPath: " + Global.ContentFolder + "\n\nAborting Main Thread...", "InitialFSCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string Message = "Cannot find the ContentFolder.\nPath: " + Global.ContentFolder + "\n\nAborting Main Thread...";
                Environment.FailFast(Message);

            }

            // ############
            // # Pass 2   #
            // ############
            // Check if ContentFolder version matches with Hardcoded one
            ConsoleWriteWithTitle("FSCheck", "Checking ContentFolder Version...");

            // Check if Metadata File Exists
            string MetaFilePath = Environment.CurrentDirectory + "/" + Global.ContentFolder + "data_packpage.metadata";
            System.Console.WriteLine("FilePath to Check:\n" + MetaFilePath);

            if (!File.Exists(MetaFilePath))
            {
                ConsoleWriteWithTitle("FSCheck", "Fatal Error!\nContentFolder Metadata File does not exist.");

                //MessageBox.Show("Cannot find the ContentFolder MetadataFile.\nPath: " + Global.ContentFolder + "\n\nAborting Main Thread...", "InitialFSCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string Message = "Cannot find the ContentFolder MetadataFile.\nPath: " + Global.ContentFolder + "\n\nAborting Main Thread...";

                Environment.FailFast(Message);


            }

            // If exists, read just the first line
            string[] FileRead = File.ReadAllLines(MetaFilePath);

            // Check if MetadataFile is not empty
            if (FileRead.Length < 1)
            {
                ConsoleWriteWithTitle("FSCheck", "Fatal Error!\nContentFolder Metadata File is empty.");
                // Close the LogFile

                //MessageBox.Show("The ContentFolder MetadataFile is corrupted or empty.\n\nAborting Main Thread...", "InitialFSCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string Message = "The ContentFolder MetadataFile is corrupted or empty.\n\nAborting Main Thread...";
                Environment.FailFast(Message);

            }

            // Check if there is a Version Match
            if (FileRead[0] == Global.MatchVersion_CurrentVersionString)
            {
                ConsoleWriteWithTitle("FSCheck", "ContentFolder Version matches with Hardcoded [" + Global.MatchVersion_CurrentVersionString + "]");

            }
            else // If not, throw an error
            {
                ConsoleWriteWithTitle("FSCheck", "Fatal Error!\nContentFolder does not matches with Hardcoded [" + Global.MatchVersion_CurrentVersionString + "]");

                //MessageBox.Show("The current ContentFolder is from a different version of WIC Engine.\n\nAborting Main Thread...", "InitialFSCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string Message = "The current ContentFolder is from a different version of WIC Engine.\n\nAborting Main Thread...";
                Environment.FailFast(Message);

            }

            // ############
            // # Pass 3   #
            // ############
            // Check if Internal Folders Exists
            ConsoleWriteWithTitle("FSCheck", "Checking SubDirectories...");

            bool EverthingOk = true;

            List<string> DirList = new List<string>() { Global.BGM_SourceFolder, Global.MAP_SourceFolder, Global.FONT_SourceFolder, Global.IMAGE_SourceFolder,
                                                        Global.SOUND_SourceFolder, Global.REGISTRY_SourceFolder, Global.BIN_RootSourceFolder };

            foreach (var dir in DirList)
            {
                EverthingOk = Directory.Exists(dir);

            }

            if (!EverthingOk)
            {
                ConsoleWriteWithTitle("FSCheck", "Fatal Error!\nThis content folder is not valid");

                //MessageBox.Show("The current ContentFolder is missing some of Required Content Directories.\n\nAborting Main Thread...", "InitialFSCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string Message = "The current ContentFolder is missing some of Required Content Directories.\n\nAborting Main Thread...";

                Environment.FailFast(Message);

            }


            // ############
            // # Pass 4   #
            // ############
            // Check if ContentFolder has binnaries for current OS

            // Check if platforms.data exists
            string SupportedBinPlaftormsFile = Global.SourceDirectory + "bin/platforms.data";

            if (!File.Exists(SupportedBinPlaftormsFile))
            {
                ConsoleWriteWithTitle("FSCheck", "Fatal Error\nThis current ContentFolder is missing [platforms.data] in bin directory.");

                //MessageBox.Show("The current ContentFolder is missing [platforms.data] in bin directory.\n\nAborting Main Thread...", "InitialFSCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string Message = "The current ContentFolder is missing [platforms.data] in bin directory.\n\nAborting Main Thread...";

                Environment.FailFast(Message);

            }

            // Read the PlatformList File
            string[] PlatformList = File.ReadAllLines(SupportedBinPlaftormsFile);

            // Check if file has the current platform listed
            bool CurrentOSFound = false;
            for (int i = 0; i < PlatformList.Length; i++)
            {
                CurrentOSFound = PlatformList[i] == Global.OSName;

                if (CurrentOSFound) { break; }
            }


            // == Current platform is not supported
            if (!CurrentOSFound)
            {
                ConsoleWriteWithTitle("FSCheck", "Fatal Error!\nThe current ContentFolder does not has binnaries for the current os.");

                string Message = "The current ContentFolder does not has binnaries for the current OS\nThe program can crash anytime if it requires an missing binnary\n\nAborting Main Thread...";
                Environment.FailFast(Message);
                //MessageBox.Show("The current ContentFolder does not has binnaries for the current OS\nThe program can crash anytime if it requires an missing binnary\n\nAborting Main Thread...", "InitialFSCheck", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

            // Check if Binnary Folder Exists
            ConsoleWriteWithTitle("FSCheck", "Checking if Binnaries folder exist...");

            // Check if Binnary folder for current OS exists
            if (!Directory.Exists(Global.BIN_SourceFolder))
            {
                ConsoleWriteWithTitle("FSCheck", "Fatal Error\nThe current ContentFolder is missing binnary directory.");

                string Message = "The current ContentFolder is missing binnary directory.\n\nAborting Main Thread...";
                //MessageBox.Show("The current ContentFolder is missing binnary directory.\n\nAborting Main Thread...", "InitialFSCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.FailFast(Message);

            }


            ConsoleWriteWithTitle("FSCheck", "FSCheck has been completed.");

        }

        public static void ConsoleWriteWithTitle(string Title, string Text)
        {
            if (!Main.DebugModeEnabled){ return; }
            Console.WriteLine($"{Title} ; {Text}");
        }

        public static string SplitIntoLines(string Input, char pSplit)
        {
            string[] Splited = Input.Split(pSplit);
            string Result = "";

            foreach (var split in Splited)
            {
                Result += split + "\n";
            }

            return Result;

        }

        public static bool CheckKeyUp(KeyboardState oldState, KeyboardState newState, Microsoft.Xna.Framework.Input.Keys key)
        {
            return newState.IsKeyDown(key) && oldState.IsKeyUp(key);
        }

        public static bool CheckKeyDown(KeyboardState oldState, KeyboardState newState, Microsoft.Xna.Framework.Input.Keys key)
        {
            return newState.IsKeyDown(key) && oldState.IsKeyDown(key);
        }

        /// <summary>
        /// Play a variation of a sound
        /// </summary>
        /// <param name="VariatorName">Variator Name (key located at [/sound-variations/])</param>
        public static void SoundVariator(string VariatorName)
        {
            // Get sound list
            string[] SoundList = Registry.ReadKeyValue("/sound-variations/" + VariatorName).Split(';');
            
            // Generate random index for list
            Random rng = new Random();
            int RngValue = rng.Next(0, SoundList.Length);

            Console.WriteLine("Playing variation of sound {" + VariatorName + "} Variator: " + RngValue);
            //Play the random sound
            Sound.PlaySound(SoundList[RngValue].TrimEnd().TrimStart());
        }


        // newState.IsKeyDown(Keys.W) && oldState.IsKeyDown(Keys.W)

    }
}