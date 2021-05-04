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
        private AnimationController Animator;

        public Panel(Rectangle pRectangle) 
        {
            ControlCollection = new List<UIControl>();
            _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
            Animator = new AnimationController(1, 0, 0.08f, true, false, true, 0);

            // Set Default Rectangle
            SetRectangle(pRectangle);

        }

        public void SetRectangle(Rectangle pRectangle)
        {
            PositionFix = Matrix.CreateTranslation(pRectangle.X, pRectangle.Y, 0);
            Rectangle = pRectangle;

        }

        public void SetMatrixScale(float pScaleX, float pScaleY)
        { 
            PositionFix = Matrix.CreateTranslation(Rectangle.X, Rectangle.Y, 0) * Matrix.CreateScale(pScaleX, pScaleY, 0);
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
            spriteBatch.Draw(Sprites.GetSprite("/base.png"), new Rectangle(0, 0, Rectangle.Width, Rectangle.Height), Color.FromNonPremultiplied(32, 32, 32, 64));

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

        public override void Update()
        {
            // Update UI Elements 
            foreach(UIControl control in ControlCollection)
            {
                // Set position offset
                control.PositionOffset.X = Rectangle.X;
                control.PositionOffset.Y = Rectangle.Y;

                control.Update();
            }

            Animator.Update();

            SetMatrixScale(1, Animator.GetValue());

            if (GameInput.GetInputState("DEBUG_TEST_1", false))
            { 
                Animator.Toggle();
            }

        }

        public void RemoveControl(UIControl control)
        {
            int ControlIndex = ControlCollection.IndexOf(control);

            if (ControlIndex == -1) { Console.WriteLine("UIControl : Cannot remove an unexistent control."); }

            ControlCollection.Remove(control);

        }

        public void AddControl(UIControl control)
        {
            ControlCollection.Add(control);
            control.Index = ControlCollection.Count - 1;


        }



    }
}
