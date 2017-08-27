using Microsoft.Xna.Framework;

namespace QuoridorServer
{
    class Tile
    {
        public enum TileType
        {
            NarrowVertical,
            NarrowHorizontal,
            Wide
        }

        public enum Colors
        {
            Red,
            Blue,
            Green,
            Yellow,
            NONE
        }

        public bool IsOccupied { get; set; }
        public Colors Color { get; set; }
        public Point Position { get; private set; }

        public Tile(Point aIndex)
        {
            IsOccupied = false;
            Color = Colors.NONE;
            Position = aIndex;
        }
    }
}
