using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fogoso.UtilsObjects;

namespace Fogoso.GameLogic.UI
{
    class Panel : UIControl
    {
        public List<UIControl> ControlCollection;
        private Matrix PositionFix;
        private RasterizerState _rasterizerState;
        public AnimationController Animator;
        public bool AutoSizeWhenAdd;
        public int AutoSizePadding;
        Rectangle cursorRect;
        public Color PanelColor;

        public Panel(Rectangle pRectangle, bool pAutoSizeWhenAdd=false, int pAutoSizePadding=2) 
        {
            // Change to Loading Cursor
            GameInput.CursorImage = "loading.png";

            ControlCollection = new List<UIControl>();
            _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
            Animator = new AnimationController(1, 0, 0.09f, true, false, true, 0, true);
            AutoSizeWhenAdd = pAutoSizeWhenAdd; 
            AutoSizePadding = pAutoSizePadding;
            cursorRect = new Rectangle(0, 0, 1, 1);

            PanelColor = Color.FromNonPremultiplied(32, 32, 32, 200);

            // Set Default Rectangle
            SetRectangle(pRectangle);

        }

        public void SetMatrix(int pTranslationX, int pTranslationY, float pScaleX, float pScaleY)
        {  
            PositionFix = Matrix.CreateScale(pScaleX, pScaleY, 0) * Matrix.CreateTranslation(pTranslationX, pTranslationY, 0);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            // Copy last Scissor Rectangle
            Rectangle lastScissorRect = spriteBatch.GraphicsDevice.ScissorRectangle;
            
            // Begin Drawing
            spriteBatch.Begin(transformMatrix: PositionFix, rasterizerState:_rasterizerState,sortMode:SpriteSortMode.Immediate, blendState:BlendState.AlphaBlend);
            
            // Set Scissor Rectangle
            spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle;
             
            // Draw background
            spriteBatch.Draw(Sprites.GetSprite("/base.png"), new Rectangle(0, 0, Rectangle.Width, Rectangle.Height), Color.FromNonPremultiplied(PanelColor.R + 100, PanelColor.G + 100, PanelColor.B + 100, PanelColor.A));
            spriteBatch.Draw(Sprites.GetSprite("/base.png"), new Rectangle(1, 1, Rectangle.Width - 2, Rectangle.Height - 2), PanelColor);

            // Draw all UI Elements
            foreach (UIControl control in ControlCollection)
            {
                control.Draw(spriteBatch);
            }

            // End Sprite Batch
            spriteBatch.End();

            // Restore last ScissorRectangle
            spriteBatch.GraphicsDevice.ScissorRectangle = lastScissorRect;

        }

        private void UpdateElements()
        {
            // Set cursor rect
            cursorRect.Y = (int)GameInput.CursorPosition.Y;
            cursorRect.X = (int)GameInput.CursorPosition.X;

            // Update UI Elements 
            foreach (UIControl control in ControlCollection)
            {
                control.ColisionRect = new Rectangle(Rectangle.X + control.Rectangle.X, Rectangle.Y + control.Rectangle.Y, control.Rectangle.Width, control.Rectangle.Height);
                bool MouseIsColiding = cursorRect.Intersects(control.ColisionRect);

                if (control.OnlyUpdateWhenMouseHover && !MouseIsColiding) { control.InactiveUpdate(); continue; }
                // Set position offset
                control.PositionOffset.X = Rectangle.X;
                control.PositionOffset.Y = Rectangle.Y;

                control.Update();
            }

        }

        public override void Update()
        {
            base.Update();

            if (!Animator.GetEnabled()) { UpdateElements(); }
            
            Animator.Update();

            SetMatrix(Rectangle.X, Rectangle.Y, 1, Animator.GetValue());
        }

        public UIControl GetControlByTag(string pTag)
        {
            if (pTag == "unset") { throw new InvalidOperationException("GetControl by tag 'unset' is a invalid operation."); }

            foreach (UIControl control in ControlCollection)
            {
                if (control.Tag == pTag) { return control; }
            } 
            return null; 
        }

        public Vector2 GetLastPos(bool AddWidth, bool AddHeight, int DefaultX=0, int DefaultY=0)
        {
            int LastY = DefaultY;
            int LastX = DefaultX;

            foreach (UIControl control in ControlCollection)
            {
                if (control.Rectangle.X > LastX) { LastX = control.Rectangle.X; if (AddWidth) { LastX += control.Rectangle.Width; } }
                if (control.Rectangle.Y > LastY) { LastY = control.Rectangle.Y; if (AddHeight) { LastY += control.Rectangle.Height; } }

            }

            return new Vector2(LastX, LastY);
        }

        public void RemoveControl(UIControl control)
        {
            int ControlIndex = ControlCollection.IndexOf(control);

            if (ControlIndex == -1) { Console.WriteLine("UIControl : Cannot remove an unexistent control."); }

            ControlCollection.Remove(control);

        }

        public void AutoSize()
        {
            int autoWidth = 0;
            int autoHeight = 0;

            foreach (UIControl control in ControlCollection)
            {
                if (control.Rectangle.X + control.Rectangle.Width > autoWidth) { autoWidth = control.Rectangle.X + control.Rectangle.Width; }
                if (control.Rectangle.Y + control.Rectangle.Height > autoHeight) { autoHeight = control.Rectangle.Y + control.Rectangle.Height; }

            }

            Rectangle.Width = autoWidth + AutoSizePadding;
            Rectangle.Height = autoHeight + AutoSizePadding;

        }

        public void CenterHorizontally(UIControl pControl)
        {
            Rectangle rectCopy = pControl.Rectangle;
            rectCopy.X = Rectangle.Width / 2 - rectCopy.Width / 2;

            pControl.SetRectangle(rectCopy);


        }

        public void AddControl(UIControl control, string pTag="unset", bool AutoCenterHorizontally=false)
        {
            ControlCollection.Add(control);
            control.Tag = pTag;
            control.Index = ControlCollection.Count - 1;

            if (AutoSizeWhenAdd)
            {
                AutoSize();
            }

            if (AutoCenterHorizontally)
            { 
                CenterHorizontally(control);
            }
        }



    }
}
