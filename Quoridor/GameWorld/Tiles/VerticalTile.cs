using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuoridorNetwork;

namespace Quoridor
{
    public class VerticalTile : Tile
    {
        private Texture2D texture;

        public VerticalTile(Vector2 position) : base(TileType.Vertical, position)
        {
            texture = Program.Game.Content.Load<Texture2D>("NarrowVertical");
        }
    }
}
