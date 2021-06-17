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
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;

namespace Fogoso
{ 

    public class FontDescriptor
    {
        public string Name;
        public float Size;
        public FontDescriptor(string FontName, float FontSize) { Name = FontName; Size = FontSize; }
    }
     
    public static class Fonts
    {
 
        // Sprites Variables
        public static Dictionary<FontDescriptor, SpriteFont> LoadedFonts = new Dictionary<FontDescriptor, SpriteFont>();
        public static float SmallFontSize = 8;
        public static float TyneFontSize = 7.8f;
        public static float DefaultFontSize = 10;
  
        public static SpriteFont BakeFont(string FontName, float FontSize)
        {  
            Utils.ConsoleWriteWithTitle("Fonts.BakeFont", $"Baker Font '{FontName}' with Size '{FontSize.ToString()}'", true);
            TtfFontBakerResult fontBakeResult = TtfFontBaker.Bake(File.ReadAllBytes($"{Global.FONT_SourceFolder}{Global.OSSlash}{FontName}.ttf"),
                FontSize,
                1024,
                1024,
                new[]
                {
                    CharacterRange.BasicLatin,
                    CharacterRange.Latin1Supplement,
                    CharacterRange.LatinExtendedA,
                    CharacterRange.Cyrillic
                }
            );
            Utils.ConsoleWriteWithTitle("Fonts.BakeFont", "Complete.", true);
  
            return fontBakeResult.CreateSpriteFont(Main.Reference.GraphicsDevice);
        } 

        public static void PrecacheFont(string FontName, float FontSize)
        {
            foreach(var Ceira in LoadedFonts.Keys)
            {
                if (Ceira.Name == FontName && Ceira.Size == FontSize) { return; }
            } 
  
            FontDescriptor ceira = new FontDescriptor(FontName, FontSize);
            LoadedFonts.Add(ceira, BakeFont(FontName, FontSize));
        }
        
        public static FontDescriptor GetFontDescriptor(string FontName, float FontSize)
        {
            foreach(var Ceira in LoadedFonts.Keys)
            {
                if (Ceira.Name == FontName && Ceira.Size == FontSize) { return Ceira; }
            }

            FontDescriptor ceira = new FontDescriptor(FontName, FontSize);
            LoadedFonts.Add(ceira, BakeFont(FontName, FontSize));
            return ceira;
        }
  
        public static SpriteFont GetSpriteFont(FontDescriptor descriptor)
        { 
            if (LoadedFonts.ContainsKey(descriptor)) { return LoadedFonts[descriptor]; }
            throw new FileNotFoundException($"Font descriptor was not loaded before\nDescriptorInfo:\nFontName: {descriptor.Name},FontSize:{descriptor.Size}");
        }


    }
}
