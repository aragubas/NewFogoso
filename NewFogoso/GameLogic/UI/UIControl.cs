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
        public bool IsVisible;
        public Rectangle Rectangle;
        public bool Selected;
        public int SelectIndex;
        public int Index;
        public int MinWidth;
        public int MinHeight;
        public Vector2 PositionOffset = new Vector2();

        public UIControl()
        {

        }

        public virtual void Update() { if (!IsVisible) { return; } }

        public virtual void Draw(SpriteBatch spriteBatch) { if (!IsVisible) { return; } }

        public virtual void OnLoad() { }

    }
}
