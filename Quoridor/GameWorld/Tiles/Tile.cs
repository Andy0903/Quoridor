using Microsoft.Xna.Framework;
using QuoridorNetwork;

namespace Quoridor
{
    public abstract class Tile
    {
        public bool IsOccupied { get; set; }
        public int Height { get; protected set; }
        public int Width { get; protected set; }
        public Vector2 Position { get; protected set; }

        public Tile(TileType type, Vector2 position)
        {
            IsOccupied = false;
            Position = position;

            switch (type)
            {
                case TileType.Vertical:
                    Width = 16;
                    Height = 64;
                    break;
                case TileType.Horizontal:
                    Width = 64;
                    Height = 16;
                    break;
                case TileType.Wide:
                    Width = 64;
                    Height = 64;
                    break;
            }
        }
    }
}
