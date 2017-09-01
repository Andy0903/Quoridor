using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuoridorNetwork;

namespace Quoridor
{
    public class NarrowVerticalTile : Tile
    {
        private Texture2D texture;

        public NarrowVerticalTile(Vector2 position) : base(TileType.NarrowVertical, position)
        {
            texture = Program.Game.Content.Load<Texture2D>("NarrowVertical");
        }
    }
}
