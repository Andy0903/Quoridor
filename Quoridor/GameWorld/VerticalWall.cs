using Microsoft.Xna.Framework;

namespace Quoridor
{
    class VerticalWall : GraphicalObject
    {
        public VerticalWall(Vector2 position, Color color) : base(16, 144, "VerticalWall")
        {
            Position = position;
            Color = color;
        }
    }
}
