using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fogoso
{
    public class InputKeyArgument
    {
        public Keys KeyObj;
        public string GamepadEq;

        public string ActionName = "";

        public InputKeyArgument(string pActionName, string pKeyboardEq)
        {
            ActionName = pActionName;

            // Set KeyboardEq
            SetKeyboardEq(pKeyboardEq);

        }


        public void SetKeyboardEq(string pKeyboardEq)
        {
            pKeyboardEq = pKeyboardEq.ToUpper();

            switch (pKeyboardEq)
            {
                case "A":
                    KeyObj = Keys.A;
                    break;

                case "B":
                    KeyObj = Keys.B;
                    break;

                case "C":
                    KeyObj = Keys.C;
                    break;

                case "D":
                    KeyObj = Keys.D;
                    break;

                case "E":
                    KeyObj = Keys.E;
                    break;

                case "F":
                    KeyObj = Keys.F;
                    break;

                case "G":
                    KeyObj = Keys.G;
                    break;

                case "H":
                    KeyObj = Keys.H;
                    break;

                case "I":
                    KeyObj = Keys.I;
                    break;
                case "J":
                    KeyObj = Keys.J;
                    break;

                case "K":
                    KeyObj = Keys.K;
                    break;

                case "L":
                    KeyObj = Keys.L;
                    break;

                case "M":
                    KeyObj = Keys.M;
                    break;

                case "N":
                    KeyObj = Keys.N;
                    break;

                case "O":
                    KeyObj = Keys.O;
                    break;

                case "P":
                    KeyObj = Keys.P;
                    break;

                case "Q":
                    KeyObj = Keys.Q;
                    break;

                case "R":
                    KeyObj = Keys.R;
                    break;

                case "S":
                    KeyObj = Keys.S;
                    break;

                case "T":
                    KeyObj = Keys.T;
                    break;

                case "U":
                    KeyObj = Keys.U;
                    break;

                case "V":
                    KeyObj = Keys.V;
                    break;

                case "W":
                    KeyObj = Keys.W;
                    break;

                case "X":
                    KeyObj = Keys.X;
                    break;

                case "Y":
                    KeyObj = Keys.Y;
                    break;

                case "Z":
                    KeyObj = Keys.Z;
                    break;

                case "SPACE":
                    KeyObj = Keys.Space;
                    break;

                case "ENTER":
                    KeyObj = Keys.Enter;
                    break;

                case "L_SHIFT":
                    KeyObj = Keys.LeftShift;
                    break;

                case "R_SHIFT":
                    KeyObj = Keys.RightShift;
                    break;

                case "ESC":
                    KeyObj = Keys.Escape;
                    break;

                case "F1":
                    KeyObj = Keys.F1;
                    break;

                case "F2":
                    KeyObj = Keys.F2;
                    break;

                case "F3":
                    KeyObj = Keys.F3;
                    break;

                case "F4":
                    KeyObj = Keys.F4;
                    break;

                case "F5":
                    KeyObj = Keys.F5;
                    break;

                case "F6":
                    KeyObj = Keys.F6;
                    break;

                case "F7":
                    KeyObj = Keys.F7;
                    break;

                case "F8":
                    KeyObj = Keys.F8;
                    break;

                case "F9":
                    KeyObj = Keys.F9;
                    break;

                case "F10":
                    KeyObj = Keys.F10;
                    break;

                case "F11":
                    KeyObj = Keys.F11;
                    break;

                case "F12":
                    KeyObj = Keys.F12;
                    break;

                case "TAB":
                    KeyObj = Keys.Tab;
                    break;

                default:
                    throw new Exception("Invalid KeyCode {" + pKeyboardEq + "}");


            }

        }
    }


    public class GameInput
    {
        public static KeyboardState oldState;
        public static Dictionary<string, InputKeyArgument> LoadedInputKeys = new Dictionary<string, InputKeyArgument>();
        public static int ReservedModeID = -1;

        // Cursor
        public static string CursorImage = "arrow.png";
        public static readonly string defaultCursor = "arrow.png";
        public static Vector2 CursorPosition;
        public static Rectangle CursorColision;
 
        public static int GenerateInputReservedID()
        {
            Random cierra = new Random();
            return cierra.Next(Int32.MinValue, Int32.MaxValue);
        } 

        public static void Initialize()
        {
            Utils.ConsoleWriteWithTitle("GameInput", "Loading input context...");

            string[] InputContextFile = Registry.ReadKeyValue("/input_context").Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            Utils.ConsoleWriteWithTitle("GameInput", $"Input Context Data {Registry.ReadKeyValue("/input_context")}");
 
            for (int i = 0; i < InputContextFile.Length; i++)
            {
                Utils.ConsoleWriteWithTitle("GameInput", $"Line [{InputContextFile[i]}]");
                if (InputContextFile[i].Length < 3 || InputContextFile[i].StartsWith("#"))
                {
                    Utils.ConsoleWriteWithTitle("GameInput", "Blank or Comment line skipped");
                    continue; 
                }

                string[] LineSplit = InputContextFile[i].Split(',');
 
                string ActionName = LineSplit[0].Trim();
                Utils.ConsoleWriteWithTitle("GameInput", "Loaded {" + ActionName + "}");
                 
                InputKeyArgument NewWax = new InputKeyArgument(ActionName, LineSplit[1].Trim());
                LoadedInputKeys.Add(ActionName, NewWax);

            }

            Utils.ConsoleWriteWithTitle("GameInput", "Context Input read complete.");
            
        }

        private static string LastInputContextFindError = "";
        public static InputKeyArgument GetInputKeyArg(string InputContext)
        {
            if (!LoadedInputKeys.ContainsKey(InputContext))
            {
                if (LastInputContextFindError != InputContext)
                {
                    LastInputContextFindError = InputContext;
                    Utils.ConsoleWriteWithTitle("GameInput", "Cannot find InputContext {" + InputContext+ "}");

                }
                return null;
            }
            return LoadedInputKeys[InputContext];
        }

        public static bool GetInputState(string KeyactionArg, bool KeyDown = false, bool ShowInViewer = false)
        {
            InputKeyArgument Wax = GetInputKeyArg(KeyactionArg);

            if (Wax == null)
            {
                return false;
            }

            if (KeyDown)
            {
                return Utils.CheckKeyDown(oldState, Keyboard.GetState(), Wax.KeyObj);
            }
            return Utils.CheckKeyUp(oldState, Keyboard.GetState(), Wax.KeyObj);
        }

        private static void DrawCursor(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprites.GetSprite("/cursor/" + CursorImage), CursorPosition, color: Color.White);
 
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw Cursor
            DrawCursor(spriteBatch);

            spriteBatch.End();
        }

        public static void Update()
        {
            CursorColision = new Rectangle((int)CursorPosition.X, (int)CursorPosition.Y, 1, 1);
            oldState = Keyboard.GetState();
            
        }

    }
}