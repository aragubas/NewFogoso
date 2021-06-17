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
using Microsoft.Xna.Framework.Graphics;
 
namespace Fogoso
{
    public static class Sprites
    {

        // Sprites Variables
        public static List<string> AllSpritedLoaded_Names = new List<string>();
        public static List<Texture2D> AllSpritedLoaded_Content = new List<Texture2D>();


        #region Load Functions
        // Load Sprite From File
        private static Texture2D LoadTexture2D_FromFile(string FileLocation)
        {
            FileStream fileStream = new FileStream(FileLocation, FileMode.Open);
            Texture2D ValToReturn = Texture2D.FromStream(Main.Reference.GraphicsDevice, fileStream);
            fileStream.Dispose();

            return ValToReturn;
        }

        // Load Custom Sprite
        public static void LoadCustomSprite(string FileLocation)
        {
            string SpriteFiltedName = Path.GetFileName(FileLocation);
            Console.WriteLine(SpriteFiltedName);

            if (!File.Exists(FileLocation)) { return; }

            AllSpritedLoaded_Content.Add(LoadTexture2D_FromFile(FileLocation));
            AllSpritedLoaded_Names.Add(SpriteFiltedName);


        }
        #endregion


        public static void FindAllSprites()
        {
            // First, we need to list all files on SPRITES directory
            string[] AllSprites = Directory.GetFiles(Global.IMAGE_SourceFolder, "*.png", SearchOption.AllDirectories);
            Utils.ConsoleWriteWithTitle("SpriteLoader", "Listing all sprites...");

            foreach (var file in AllSprites)
            {
                FileInfo info = new FileInfo(file);
                string FileFullName = info.FullName;
                string SpriteFiltedName = FileFullName.Replace(Environment.CurrentDirectory, "");
                SpriteFiltedName = SpriteFiltedName.Replace(Global.IMAGE_SourceFolder + Global.OSSlash, "");

                int SpriteID = AllSpritedLoaded_Names.IndexOf(SpriteFiltedName);

                if (SpriteID == -1 && info.Extension == ".png")
                {
                    AllSpritedLoaded_Content.Add(LoadTexture2D_FromFile(FileFullName));
                    AllSpritedLoaded_Names.Add(SpriteFiltedName.Replace(Global.OSSlash, "/"));

                    Utils.ConsoleWriteWithTitle("SpriteLoader", "Found [" + SpriteFiltedName + "].");

                }

            }

            Utils.ConsoleWriteWithTitle("SpriteLoader", "Operation Completed");

        }


        public static Texture2D GetSprite(string SpriteName)
        {
            if (SpriteName.EndsWith(".png")) { SpriteName.Insert(SpriteName.Length - 1, ".png"); }
            if (AllSpritedLoaded_Names.IndexOf(SpriteName) == -1)
            {
                // Check if error sprite exists
                if (AllSpritedLoaded_Names.IndexOf("/error.png") == -1)
                {
                    throw new Exception("Cannot find i: \n" + SpriteName);
                }

                return GetSprite(Registry.ReadKeyValue("/missing_texture"));
            }
            return AllSpritedLoaded_Content[AllSpritedLoaded_Names.IndexOf(SpriteName)]; // Return the correct sprite
 
        }


    }
}
