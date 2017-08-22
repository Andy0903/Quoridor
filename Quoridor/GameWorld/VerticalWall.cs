using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quoridor
{
    class VerticalWall : GraphicalObject
    {
        public VerticalWall(Vector2 aPosition, Color aColor) : base(16, 144, "VerticalWall")
        {
            Position = aPosition;
            Color = aColor;
        }
    }
}
