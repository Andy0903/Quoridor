using Microsoft.Xna.Framework;

namespace QuoridorServer
{
    class Tile
    {
        public bool IsOccupied { get; set; }
        public Color Color { get; set; }
        public Point Position { get; private set; }

        public Tile(Point index)
        {
            IsOccupied = false;
            Color = Color.NONE;
            Position = index;
        }
    }
}
