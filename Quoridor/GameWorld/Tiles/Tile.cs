using Microsoft.Xna.Framework;

namespace Quoridor
{
    abstract class Tile
    {
        public int Height { get; protected set; }
        public int Width { get; protected set; }
        public Vector2 Position { get; protected set; }

        public Tile(int aWidth, int aHeight, Vector2 aPosition)
        {
            Height = aHeight;
            Width = aWidth;
            Position = aPosition;
        }
    }
}
