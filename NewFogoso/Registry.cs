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
using Microsoft.Xna.Framework;

namespace Fogoso
{
    public static class Registry
    {
        public static List<string> LoadedKeysNames = new List<string>();
        public static List<string> LoadedKeysValues = new List<string>();


        public static string ReadKeyValue(string KeyName)
        {
            if (!KeyName.StartsWith("/", StringComparison.Ordinal)) { KeyName = KeyName.Insert(0, Global.OSSlash); };
            int KeyIndex = LoadedKeysNames.IndexOf(KeyName);

            try
            {
                string ValToReturn = LoadedKeysValues[KeyIndex];

                return ValToReturn;
            }
            catch (Exception) { throw new FileNotFoundException("Key:" + KeyName + " does not exist."); }
        }

        public static bool KeyExists(string KeyName)
        {
            int KeyIndex = LoadedKeysNames.IndexOf(KeyName);

            if (KeyIndex == -1) { return false; }
            return true;
        }

        public static void WriteKey(string KeyName, string KeyValue)
        {
            File.WriteAllText(Global.REGISTRY_SourceFolder + Global.OSSlash + KeyName + ".data", KeyValue, new System.Text.UTF8Encoding());

        }

        public static void Initialize()
        {
            LoadedKeysNames.Clear();
            LoadedKeysValues.Clear();

            Console.WriteLine("Registry.Initialize : Start");

            string[] AllKeys = Directory.GetFiles(Global.REGISTRY_SourceFolder, "*.data", SearchOption.AllDirectories);

            for (int i = 0; i < AllKeys.Length; i++)
            {
                string KeyNameFiltred = AllKeys[i].Replace(Global.REGISTRY_SourceFolder, "");
                KeyNameFiltred = KeyNameFiltred.Replace(".data", "");

                LoadedKeysNames.Add(KeyNameFiltred.Replace(Global.OSSlash, "/"));
                LoadedKeysValues.Add(File.ReadAllText(Global.REGISTRY_SourceFolder + Global.OSSlash + KeyNameFiltred + ".data").Trim());
                LoadedKeysValues[i] = LoadedKeysValues[i].Replace("%n", Environment.NewLine);
                LoadedKeysValues[i] = LoadedKeysValues[i].Replace("%usr", Environment.UserName);
                LoadedKeysValues[i] = LoadedKeysValues[i].Replace("%current_dir", Environment.CurrentDirectory);
                LoadedKeysValues[i] = LoadedKeysValues[i].Replace("%machine_name", Environment.MachineName);
                LoadedKeysValues[i] = LoadedKeysValues[i].Replace("%processor_count", Environment.ProcessorCount.ToString());
                LoadedKeysValues[i] = LoadedKeysValues[i].Replace("%source_dir", Global.SourceDirectory.ToString());
                LoadedKeysValues[i] = LoadedKeysValues[i].Replace("%bgm_source_dir", Global.BGM_SourceFolder.ToString());
                LoadedKeysValues[i] = LoadedKeysValues[i].Replace("%font_source_dir", Global.FONT_SourceFolder.ToString());
                LoadedKeysValues[i] = LoadedKeysValues[i].Replace("%image_source_dir", Global.IMAGE_SourceFolder.ToString());
                LoadedKeysValues[i] = LoadedKeysValues[i].Replace("%sound_source_dir", Global.SOUND_SourceFolder.ToString());

                Console.WriteLine("Found Key: [" + LoadedKeysNames[i] + "]");

            }

            Console.WriteLine("Registry.Initialize : Operation Completed");


        }
    }
}
