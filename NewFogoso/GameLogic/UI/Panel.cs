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
        private Color PanelColor;

        public Panel(Rectangle pRectangle, bool pAutoSizeWhenAdd=false, int pAutoSizePadding=2) 
        {
            // Change to Loading Cursor
            GameInput.CursorImage = "loading.png";

            ControlCollection = new List<UIControl>();
            _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
            Animator = new AnimationController(1, 0, 0.09f, true, false, true, 0);
            AutoSizeWhenAdd = pAutoSizeWhenAdd; 
            AutoSizePadding = pAutoSizePadding;

            PanelColor = Color.FromNonPremultiplied(32, 32, 32, 200);

            // Set Default Rectangle
            SetRectangle(pRectangle);

        }

        private void SetMatrix(int pTranslationX, int pTranslationY, float pScaleX, float pScaleY)
        {  
            PositionFix = Matrix.CreateScale(pScaleX, pScaleY, 0) * Matrix.CreateTranslation(pTranslationX, pTranslationY, 0);
        }

        public void SwitchControlCollection(List<UIControl> pNewControlCollection)
        {
            Animator.ForceState(0);
            ControlCollection.Clear();
            for (int i = 0; i < pNewControlCollection.Count; i++)
            {
                ControlCollection.Add(pNewControlCollection[i]);
            }
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

        public void ControlFill(string ReferenceTag)
        {
            if (ControlCollection.Count > 2) { Utils.ConsoleWriteWithTitle("Panel.ControlFill", "Cannot fill control, there is more than one control on this panel.", true); return; }
            UIControl control = GetControlByTag(ReferenceTag);
            if (control == null) { Utils.ConsoleWriteWithTitle("Panel.ControlFill", "Cannot find control for fill operation."); return; }
            
            control.SetRectangle(new Rectangle(0, 0, Rectangle.Width, Rectangle.Height));
            control.PositionOffset = new Vector2(Rectangle.X, Rectangle.Y);
              
  
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

            for (int i = 0; i < ControlCollection.Count; i++)
            { 
                if (ControlCollection[i].Tag == pTag) { return ControlCollection[i]; }
                
            }
            return null; 
        }

        public Vector2 GetLastPos(bool AddWidth, bool AddHeight, int DefaultX=0, int DefaultY=0)
        {
            int LastY = DefaultY;
            int LastX = DefaultX;

            for (int i = 0; i < ControlCollection.Count; i++)
            {
                if (ControlCollection[i].Rectangle.X > LastX) { LastX = ControlCollection[i].Rectangle.X; if (AddWidth) { LastX += ControlCollection[i].Rectangle.Width; } }
                if (ControlCollection[i].Rectangle.Y > LastY) { LastY = ControlCollection[i].Rectangle.Y; if (AddHeight) { LastY += ControlCollection[i].Rectangle.Height; } }
                
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

            for (int i = 0; i < ControlCollection.Count; i++)
            {
                if (ControlCollection[i].Rectangle.X + ControlCollection[i].Rectangle.Width > autoWidth) { autoWidth = ControlCollection[i].Rectangle.X + ControlCollection[i].Rectangle.Width; }
                if (ControlCollection[i].Rectangle.Y + ControlCollection[i].Rectangle.Height > autoHeight) { autoHeight = ControlCollection[i].Rectangle.Y + ControlCollection[i].Rectangle.Height; }
                
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
            // Set to loading cursor
            GameInput.CursorImage = "loading.png";

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
