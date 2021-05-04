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

using System.IO;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Fogoso
{
    public class Sound
    {

        public static List<SoundEffect> soundEffects = new List<SoundEffect>();
        public static List<string> soundEffects_name = new List<string>();
        public static List<SoundEffectInstance> SoundInstances = new List<SoundEffectInstance>();
        public static List<int> SoundInstancesID = new List<int>();

        public static SoundEffect LoadSoundFromFile(string FileName)
        {
            FileStream fileStream = null;
            fileStream = new FileStream(FileName, FileMode.Open);

            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);

            Stream stream = new MemoryStream(data);

            SoundEffect SoundToReturn = SoundEffect.FromStream(stream);
            fileStream.Dispose();

            return SoundToReturn;
        }


        public static void Initialize()
        {
            string[] AllSounds = Directory.GetFiles(Global.SOUND_SourceFolder, "*.wav", SearchOption.AllDirectories);

            foreach (var sound in AllSounds)
            {
                System.Console.WriteLine("Loading sound: {" + sound + "}");
                soundEffects.Add(LoadSoundFromFile(sound));

                string FileName = sound.Replace(Global.SOUND_SourceFolder, "").Remove(0, 1).Replace(".wav", "");
                System.Console.WriteLine(FileName);
                soundEffects_name.Add(FileName.Replace("\\", "/"));
            }

            System.Console.WriteLine("All sounds loaded sucefully.");

        }

        static string LastResError = "";
        static int LastInstanceID = 0;

        public static int CreateSoundEffectInstance(string SoundKeyName)
        {
            SoundEffect InstanceToCreate = GetSoundEffect(SoundKeyName);

            if (InstanceToCreate == null)
            {
                // Don't flood the console with the same message over and over again
                if (LastResError == SoundKeyName)
                {
                    return -1;
                }
                System.Console.WriteLine("Error while creating sound instance {" + SoundKeyName + "}.");
                LastResError = SoundKeyName;
                return -1;

            }
            LastInstanceID += 1;


            // Create SoundEffect instance in memory
            SoundInstances.Add(InstanceToCreate.CreateInstance());
            SoundInstancesID.Add(LastInstanceID);

            // Return InstanceID
            return LastInstanceID;
        }

        public static SoundEffectInstance GetSoundEffectInstance(int InstanceID)
        {
            int InstanceIDInMemory = SoundInstancesID.IndexOf(InstanceID);

            if (InstanceIDInMemory == -1)
            {
                return null;
            }

            return SoundInstances[InstanceIDInMemory];
        }

        public static void PlaySoundEffectInstance(int InstanceID)
        {
            SoundEffectInstance Instance = GetSoundEffectInstance(InstanceID);

            if (Instance == null)
            {
                System.Console.WriteLine("Cannot find SoundEffect Instance {" + InstanceID + "}.");
                return;
            }

            Instance.Play();
        }

        public static void StopSoundEffectInstance(int InstanceID)
        {
            SoundEffectInstance Instance = GetSoundEffectInstance(InstanceID);

            if (Instance == null)
            {
                System.Console.WriteLine("Cannot find SoundEffect Instance {" + InstanceID + "}.");
                return;
            }

            Instance.Stop();
        }

        public static void PanSoundEffectInstance(int InstanceID, float SoundPanning)
        {
            SoundEffectInstance Instance = GetSoundEffectInstance(InstanceID);

            if (Instance == null)
            {
                System.Console.WriteLine("Cannot find SoundEffect Instance {" + InstanceID + "}.");
                return;
            }

            Instance.Pan = SoundPanning;
        }

        public static void ResumeSoundEffectInstance(int InstanceID)
        {
            SoundEffectInstance Instance = GetSoundEffectInstance(InstanceID);

            if (Instance == null)
            {
                System.Console.WriteLine("Cannot find SoundEffect Instance {" + InstanceID + "}.");
                return;
            }

            Instance.Resume();
        }


        public static SoundEffect GetSoundEffect(string SoundKeyName)
        {
            int AudioIndex = soundEffects_name.IndexOf(SoundKeyName);

            // When sound resouce was not found.
            if (AudioIndex == -1)
            {
                return null;
            }

            return soundEffects[AudioIndex];

        }

        public static void PlaySound(string SoundKeyName)
        {
            SoundEffect Audio = GetSoundEffect(SoundKeyName);

            // Return if cannot load sound effect
            if (Audio == null)
            {
                // Don't flood the console with the same message over and over again
                if (LastResError == SoundKeyName)
                {
                    return;
                }
                System.Console.WriteLine("Error while playing sound {" + SoundKeyName + "}.");
                LastResError = SoundKeyName;
                return;
            }

            try
            {
                // Play loaded resouce
                Audio.Play();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error while playing sound effect.");
                System.Console.WriteLine(ex.Message);
            }

        }

    }

}