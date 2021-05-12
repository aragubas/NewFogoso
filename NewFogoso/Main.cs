﻿/*
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
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fogoso
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Main Reference;
        public static bool DebugModeEnabled;

        // Fps Counter
        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        int _fps = 0;


        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / 60);

            Reference = this;
        }

        protected override void Initialize()
        {

            // Load all the taiyou scripts
            Taiyou.Global.LoadTaiyouScripts();
            Taiyou.Event.RegisterEvent("init", "initial");
            Taiyou.Event.TriggerEvent("init");

            // Load all Sprites, Sounds and Registry Keys
            Sprites.FindAllSprites();
            Sound.Initialize();
            Registry.Initialize();

            // Set Window Static Reference
            Global.GameWindowReference = Window;

            // Set Window Width and Height variables
            Global.WindowWidth = Window.ClientBounds.Width;
            Global.WindowHeight = Window.ClientBounds.Height;

            // Initialize Textbox Instance
            GameLogic.TextBox.KeyboardInput.Initialize(this, 120, 5);

            // Set Target FPS
            this.TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / Convert.ToInt32(Registry.ReadKeyValue("/fps")));

            // Set DebugMode variable
            DebugModeEnabled = Registry.ReadKeyValue("/debug_mode").ToLower().Equals("true");

            // Initialize GameInput and Debug
            GameInput.Initialize();
            Debug.Initialize();

            // Screen Initialization
            GameLogic.ScreenSelector.Initialize();

            // Create the Sprite Batch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Set Default Window Title
            Window.Title = Registry.ReadKeyValue("/default_window_title");

            base.Initialize();
        }

        // Restore Cursor Static Variables
        public MouseState restoreCursorMouseState; 

        private void RestoreCursor()
        {
            if (GameInput.ReservedModeID == -1) { GameInput.CursorImage = GameInput.defaultCursor; }

            // Update Cursor Position
            restoreCursorMouseState = Mouse.GetState();
            int X = restoreCursorMouseState.X;
            int Y = restoreCursorMouseState.Y;

            if (X >= Global.WindowWidth) { X = Global.WindowWidth; }
            if (Y >= Global.WindowHeight) { Y = Global.WindowHeight; }
            if (X <= 0) { X = 0; }
            if (Y <= 0) { Y = 0; }

            GameInput.CursorPosition.X = X;
            GameInput.CursorPosition.Y = Y;

        }

        protected override void Update(GameTime gameTime)
        {
            // Update Game Clock
            GameLogic.AragubasTime.Update();

            if (!this.IsActive)
            {
                return;
            }
            // Restore cursor
            RestoreCursor();


            // Update FPS Counter
            if (DebugModeEnabled)
            {
                _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                // 1 Second has passed
                if (_elapsed_time >= 1000.0f)
                {
                    _fps = _total_frames;
                    _total_frames = 0;
                    _elapsed_time = 0;
                }

                Window.Title = "Fogoso - Debug Mode FPS:" + _fps;

            }

            // Update Current Screen
            GameLogic.ScreenSelector.Update();

            // Update KeyboardObj
            GameLogic.TextBox.KeyboardInput.Update();
            
            // Update GameInput
            GameInput.Update();

            // Update SoundSystem
            Sound.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!this.IsActive)
            {
                return;
            }

            GraphicsDevice.Clear(Color.Black);

            GameLogic.ScreenSelector.Draw(spriteBatch);

            // Draw GameInput HUD
            GameInput.Draw(spriteBatch);

            // Draw Debug
            if (DebugModeEnabled) { Debug.RenderDebugInfo(spriteBatch); _total_frames++; }


            base.Draw(gameTime);
        }
    }
}
