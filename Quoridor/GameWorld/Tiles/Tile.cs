using Microsoft.Xna.Framework;

namespace Quoridor
{
    public abstract class Tile
    {
        public bool IsOccupied { get; set; }
        public int Height { get; protected set; }
        public int Width { get; protected set; }
        public Vector2 Position { get; protected set; }

        public Tile(TileType aType, Vector2 aPosition)
        {
            IsOccupied = false;
            Position = aPosition;

            switch (aType)
            {
                case TileType.NarrowVertical:
                    Width = 16;
                    Height = 64;
                    break;
                case TileType.NarrowHorizontal:
                    Width = 64;
                    Height = 16;
                    break;
                case TileType.Wide:
                    Width = 64;
                    Height = 64;
                    break;
                default:
                    break;
            }
        }
    }
}
