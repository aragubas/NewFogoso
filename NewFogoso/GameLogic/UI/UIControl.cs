using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fogoso.GameLogic.UI
{
    public abstract class UIControl
    {
        public bool IsVisible = true; 
        public bool IsEnabled = true;
        public bool ForceReadjust;
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
 
        /// <summary>
        /// Control Update logic
        /// </summary>
        public virtual void Update() { if (!IsVisible) { return; } }

        /// <summary>
        /// Update logic when inactive
        /// </summary>
        public virtual void InactiveUpdate() { if (!IsVisible) { return; } }

        /// <summary>
        /// Draw control
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        public virtual void Draw(SpriteBatch spriteBatch) { if (!IsVisible || !IsEnabled) { return; } }

        /// <summary>
        /// On load
        /// </summary>
        public virtual void OnLoad() { }

        /// <summary> 
        /// Set control rectangle (override for resizing logic)
        /// </summary>
        /// <param name="pRectangle">New Rectangle</param>
        public virtual void SetRectangle(Rectangle pRectangle) { Rectangle = pRectangle; }
  
        /// <summary>
        /// Breif description of this control
        /// </summary>
        public virtual new string ToString() { return "UI Control"; }
 
    }
}
