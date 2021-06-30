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
        public static Dictionary<string, string> LoadedKeys = new Dictionary<string, string>();

        public static string ReadKeyValue(string KeyName)
        {
            if (!KeyName.StartsWith("/", StringComparison.Ordinal)) { KeyName = KeyName.Insert(0, Global.OSSlash); };
            if (!LoadedKeys.ContainsKey(KeyName)) { throw new KeyNotFoundException($"Cannot find key ({KeyName})"); }
            return LoadedKeys[KeyName];
        }
        
        public static void WriteKey(string KeyName, string KeyValue)
        {
            File.WriteAllText(Global.REGISTRY_SourceFolder + Global.OSSlash + KeyName + ".data", KeyValue, new System.Text.UTF8Encoding());

        }

        public static void Initialize()
        {
            LoadedKeys.Clear();

            Console.WriteLine("Registry.Initialize : Start");

            string[] AllKeys = Directory.GetFiles(Global.REGISTRY_SourceFolder, "*.data", SearchOption.AllDirectories);

            for (int i = 0; i < AllKeys.Length; i++)
            {
                string KeyNameFiltred = AllKeys[i].Replace(Global.REGISTRY_SourceFolder, "");
                KeyNameFiltred = KeyNameFiltred.Replace(".data", "");

                string key = KeyNameFiltred.Replace(Global.OSSlash, "/");
                string value = File.ReadAllText(Global.REGISTRY_SourceFolder + Global.OSSlash + KeyNameFiltred + ".data").Trim();

                value = value.Replace("%n", Environment.NewLine);
                value = value.Replace("%usr", Environment.UserName);
                value = value.Replace("%current_dir", Environment.CurrentDirectory);
                value = value.Replace("%machine_name", Environment.MachineName);
                value = value.Replace("%processor_count", Environment.ProcessorCount.ToString());
                value = value.Replace("%source_dir", Global.SourceDirectory.ToString());
                value = value.Replace("%bgm_source_dir", Global.BGM_SourceFolder.ToString());
                value = value.Replace("%font_source_dir", Global.FONT_SourceFolder.ToString());
                value = value.Replace("%image_source_dir", Global.IMAGE_SourceFolder.ToString());
                value = value.Replace("%sound_source_dir", Global.SOUND_SourceFolder.ToString());

                LoadedKeys.Add(key, value);

                Utils.ConsoleWriteWithTitle("RegistryInitialize", $"Loaded key {key}");

            }
            Utils.ConsoleWriteWithTitle("RegistryInitialize", "Operation Completed.");


        }
    }
}
