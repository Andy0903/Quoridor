
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuoridorNetwork;

namespace Quoridor
{
    public class HorizontalTile : Tile
    {
        private Texture2D texture;
        
        public HorizontalTile(Vector2 position) : base(TileType.Horizontal, position)
        {
            texture = Game1.CM.Load<Texture2D>("NarrowHorizontal");
        }
    }
}
