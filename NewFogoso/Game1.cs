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
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Game1 Reference;

        // Fps Counter
        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        int _fps = 0;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / 60);

            Reference = this;
        }

        protected override void Initialize()
        {
            Console.WriteLine("CurrentPlatform is: " + Global.OSName);

            // Do all initial checks to the WIC Engine FS
            Utils.InitialFSCheck();

            // Load all the taiyou scripts
            Taiyou.Global.LoadTaiyouScripts();
            Taiyou.Event.RegisterEvent("init", "initial");
            Taiyou.Event.TriggerEvent("init");


            Sprites.FindAllSprites();
            Sound.Initialize();
            Registry.Initialize();

            GameLogic.ScreenSelector.Initialize();

            Global.WindowWidth = Window.ClientBounds.Width;
            Global.WindowHeight = Window.ClientBounds.Height;

            // Initialize Textbox Instance
            GameLogic.TextBox.KeyboardInput.Initialize(this, 120, 5);

            // Set Target FPS
            this.TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / Convert.ToInt32(Registry.ReadKeyValue("/fps")));

            GameInput.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        {

        }

        // Restore Cursor Static Variables
        public MouseState restoreCursorMouseState; 

        private void RestoreCursor()
        {
            GameInput.CursorImage = GameInput.defaultCursor;

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
            if (!this.IsActive)
            {
                return;
            }
            // Restore cursor
            RestoreCursor();

            // Update FPS Counter
            _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // 1 Second has passed
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }

            Window.Title = "Fogoso - FPS:" + _fps;


            GameLogic.ScreenSelector.Update();

            GameLogic.TextBox.KeyboardInput.Update();
            GameInput.Update();

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

            // FPS Counter
            _total_frames++;

            base.Draw(gameTime);
        }
    }
}
