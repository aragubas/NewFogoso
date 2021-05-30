using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fogoso.UtilsObjects;
using Microsoft.Xna.Framework.Input;

namespace Fogoso.GameLogic.UI
{
    class ToolStripPanel : UIControl
    {
        public List<UIControl> ControlCollection;
        private Matrix PositionFix;
        private RasterizerState _rasterizerState;
        public AnimationController Animator;
        private Color PanelColor;
        private Color PanelOriginalColor;
        private Color PanelReservedColor;        
        public int ItemsSpacing = 2;
        private int Scroll;
        private int LastMouseX;
        private bool ScrollHoldStarted;
        private int TotalWidth;
        private bool ScrollingEnabled = false;
        private int InputReservedID;

        public ToolStripPanel(Rectangle pRectangle, bool defaultMinimalHeight=false)
        {
            ControlCollection = new List<UIControl>();
            _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
            Animator = new AnimationController(1, 0, 0.09f, true, false, true, 0);

            PanelOriginalColor = Color.FromNonPremultiplied(64, 64, 64, 210);
            PanelReservedColor = Color.FromNonPremultiplied(128, 128, 128, 230);

            PanelColor = PanelOriginalColor;

            InputReservedID = GameInput.GenerateInputReservedID();
             
            if (defaultMinimalHeight) { this.MinHeight = pRectangle.Height; }

            SetRectangle(pRectangle);
        }

        private void SetMatrix(int pTranslationX, int pTranslationY, float pScaleX, float pScaleY)
        {
            PositionFix = Matrix.CreateScale(pScaleX, pScaleY, 0) * Matrix.CreateTranslation(pTranslationX, pTranslationY, 0);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            // Copy last Scissor Rectangle
            Rectangle lastScissorRect = spriteBatch.GraphicsDevice.ScissorRectangle;


            spriteBatch.Begin(transformMatrix: PositionFix, rasterizerState: _rasterizerState, sortMode: SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend);


            // Set Scissor Rectangle
            spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle;

            // Draw background
            spriteBatch.DrawRectangle(new Rectangle(0, 0, Rectangle.Width, Rectangle.Height), Color.FromNonPremultiplied(PanelColor.R + 130, PanelColor.G + 130, PanelColor.B + 146, PanelColor.A), 2);
            spriteBatch.FillRectangle(new Rectangle(1, 1, Rectangle.Width - 2, Rectangle.Height - 2), PanelColor);

            // Draw all UI Elements
            for (int i = 0; i < ControlCollection.Count; i++)
            {
                ControlCollection[i].Draw(spriteBatch);

            }

            // End Sprite Batch
            spriteBatch.End();

            // Restore last ScissorRectangle
            spriteBatch.GraphicsDevice.ScissorRectangle = lastScissorRect;

        }

        private void UpdateElements()
        {
            // Update UI Elements 
            if (GameInput.CursorColision.Intersects(Rectangle))
            {
                for (int i = 0; i < ControlCollection.Count; i++)
                {
                    ControlCollection[i].ColisionRect = new Rectangle(Rectangle.X + ControlCollection[i].Rectangle.X, Rectangle.Y + ControlCollection[i].Rectangle.Y, ControlCollection[i].Rectangle.Width, ControlCollection[i].Rectangle.Height);
                    bool MouseIsColiding = GameInput.CursorColision.Intersects(ControlCollection[i].ColisionRect);

                    if (ControlCollection[i].OnlyUpdateWhenMouseHover && !MouseIsColiding) { ControlCollection[i].InactiveUpdate(); continue; }
                    // Set position offset
                    ControlCollection[i].PositionOffset.X = this.PositionOffset.X + Rectangle.X;
                    ControlCollection[i].PositionOffset.Y = this.PositionOffset.Y + Rectangle.Y;

                    ControlCollection[i].Update();

                }

            }
            else
            {
                for (int i = 0; i < ControlCollection.Count; i++)
                {
                    ControlCollection[i].ColisionRect = new Rectangle(Rectangle.X + ControlCollection[i].Rectangle.X, Rectangle.Y + ControlCollection[i].Rectangle.Y, ControlCollection[i].Rectangle.Width, ControlCollection[i].Rectangle.Height);

                    if (ControlCollection[i].OnlyUpdateWhenMouseHover) { ControlCollection[i].InactiveUpdate(); continue; }
                    // Set position offset
                    ControlCollection[i].PositionOffset.X = this.PositionOffset.X + Rectangle.X;
                    ControlCollection[i].PositionOffset.Y = this.PositionOffset.Y + Rectangle.Y;

                    ControlCollection[i].Update();

                }

            }

        } 

        public override void Update()
        {
            base.Update();

            // Only update UI elements when Animator is done
            if (!Animator.GetEnabled()) { UpdateElements(); }

            // Update Animator
            Animator.Update();

            // Update Rendering Matrix
            SetMatrix(Rectangle.X, Rectangle.Y, 1, Animator.GetValue());

            // Move when Scrolling
            if (ScrollHoldStarted && ScrollingEnabled)
            {
                PanelColor = PanelReservedColor;
                GameInput.ReservedModeID = InputReservedID;

                if (Mouse.GetState().MiddleButton == ButtonState.Released)
                {
                    ScrollHoldStarted = false;
                    LastMouseX = 0;
                    GameInput.ReservedModeID = -1;
                    PanelColor = PanelOriginalColor;

                }
                else
                {
                    SetScrollX((int)GameInput.CursorPosition.X - LastMouseX);

                }
            }
  
            // Start Scrolling if enabled
            if (GameInput.CursorColision.Intersects(Rectangle) && ScrollingEnabled)
            {

                if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
                {
                    //Scroll = 4;
                    if (!ScrollHoldStarted)
                    {
                        ScrollHoldStarted = true;
                        LastMouseX = (int)GameInput.CursorPosition.X + Math.Abs(Scroll);
                    }
                }

            }

        }

        public void SetScrollX(int ScrollValue)
        {
            if (!ScrollingEnabled) { return; }
            Scroll = ScrollValue;
            if (Scroll > 4)
            { 
                Scroll = 4;
            }

            // Limit Scroll
            if (Math.Abs(Scroll - Rectangle.Width + 4) > TotalWidth)
            {
                Scroll = -(TotalWidth - Rectangle.Width + 4);
            }

            // If no MinHeight was set, set Y to 4
            if (this.MinHeight == 0)
            {
                for (int i = 0; i < ControlCollection.Count; i++)
                {
                    if (i == 0) { ControlCollection[i].SetRectangle(new Rectangle(Scroll, 4, ControlCollection[i].Rectangle.Width, ControlCollection[i].Rectangle.Height)); continue; }
                    ControlCollection[i].SetRectangle(new Rectangle(ControlCollection[i - 1].Rectangle.X + ControlCollection[i - 1].Rectangle.Width + ItemsSpacing, 4, ControlCollection[i].Rectangle.Width, ControlCollection[i].Rectangle.Height));

                }

            }
            else
            { 
                // If MinHeight was set, don't touch Y value
                for (int i = 0; i < ControlCollection.Count; i++)
                {
                    if (i == 0) { ControlCollection[i].SetRectangle(new Rectangle(Scroll, ControlCollection[i].Rectangle.Y, ControlCollection[i].Rectangle.Width, ControlCollection[i].Rectangle.Height)); continue; }
                    ControlCollection[i].SetRectangle(new Rectangle(ControlCollection[i - 1].Rectangle.X + ControlCollection[i - 1].Rectangle.Width + ItemsSpacing, ControlCollection[i].Rectangle.Y, ControlCollection[i].Rectangle.Width, ControlCollection[i].Rectangle.Height));

                }

            }

        }

        /// <summary>
        /// Auto-Arrange all UI controls inside panel
        /// </summary>
        public void AutoArrange()
        {
            Scroll = 0;

            // Set to loading cursor
            GameInput.CursorImage = "loading.png";
            TotalWidth = 0;

            // Set all Item's Y to 4
            for (int i = 0; i < ControlCollection.Count; i++)
            {
                ControlCollection[i].SetRectangle(new Rectangle(ControlCollection[i].Rectangle.X, 4, ControlCollection[i].Rectangle.Width, ControlCollection[i].Rectangle.Height));
                TotalWidth += ControlCollection[i].Rectangle.Width + ItemsSpacing;

            }

            // Set Scrolling
            ScrollingEnabled = TotalWidth >= Rectangle.Width;

            // Set all Item's X to the previuos item X plus its width and item spacing
            for (int i = 0; i < ControlCollection.Count; i++)
            {
                if (i == 0) { ControlCollection[i].SetRectangle(new Rectangle(4, 4, ControlCollection[i].Rectangle.Width, ControlCollection[i].Rectangle.Height)); continue; }
                ControlCollection[i].SetRectangle(new Rectangle(ControlCollection[i - 1].Rectangle.X + ControlCollection[i - 1].Rectangle.Width + ItemsSpacing, 4, ControlCollection[i].Rectangle.Width, ControlCollection[i].Rectangle.Height));

            }
            if (this.MinHeight == 0)
            {
                int LastAutoHeight = 0;

                // Set Height Automatcly
                for (int i = 0; i < ControlCollection.Count; i++)
                {
                    if (ControlCollection[i].Rectangle.Height > LastAutoHeight) { LastAutoHeight = ControlCollection[i].Rectangle.Height; }
                }

                SetRectangle(new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, LastAutoHeight + 8));

            }
            else
            {
                // Center Horizontally all controls
                for (int i = 0; i < ControlCollection.Count; i++)
                {
                    Rectangle rectCopy = ControlCollection[i].Rectangle;
                    rectCopy.Y = Rectangle.Height / 2 - rectCopy.Height / 2;

                    ControlCollection[i].SetRectangle(rectCopy);

                }
            }

        }

        public void AddControl(UIControl control, string pTag = "unset")
        {
            // Set to loading cursor
            GameInput.CursorImage = "loading.png";

            ControlCollection.Add(control);
            control.Tag = pTag;
            control.Index = ControlCollection.Count - 1;

            AutoArrange();

        }

        public UIControl GetControlByTag(string pTag)
        {
            if (pTag == "unset") { throw new InvalidOperationException("GetControl by tag 'unset' is a invalid operation."); }

            for (int i = 0; i < ControlCollection.Count; i++)
            {
                if (ControlCollection[i].Tag == pTag) { return ControlCollection[i]; }

            }
            return null;
        }



        public override void SetRectangle(Rectangle pRectangle)
        {
            base.SetRectangle(pRectangle);

            AutoArrange();
        }

    }
}
