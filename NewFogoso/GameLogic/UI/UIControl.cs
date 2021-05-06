using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogoso.GameLogic.UI
{
    public abstract class UIControl
    {
        public bool IsVisible = true; 
        public bool IsEnabled = true;
        public Rectangle Rectangle;
        public bool Selected; 
        public int SelectIndex;
        public int Index;
        public int MinWidth;
        public int MinHeight;
        public Vector2 PositionOffset = new Vector2();
        public Rectangle ColisionRect;
        public string Tag;
        public bool OnlyUpdateWhenMouseHover;

        public UIControl()
        {

        }

        /// <summary>
        /// Update logic
        /// </summary>
        public virtual void Update() { if (!IsVisible) { return; } }

        /// <summary>
        /// Update logic when inactive
        /// </summary>
        public virtual void InactiveUpdate() { if (!IsVisible) { return; } }

        /// <summary>
        /// Draw the control
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch) { if (!IsVisible || !IsEnabled) { return; } }

        /// <summary>
        /// On load
        /// </summary>
        public virtual void OnLoad() { }

        /// <summary> 
        /// Set control rectangle (override for resizing logic)
        /// </summary>
        /// <param name="pRectangle"></param>
        public virtual void SetRectangle(Rectangle pRectangle) { Rectangle = pRectangle; }


    }
}
