
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuoridorNetwork;

namespace Quoridor
{
    public class HorizontalTile : Tile
    {
        private Texture2D texture;
        
        public HorizontalTile(Vector2 position) : base(TileType.NarrowHorizontal, position)
        {
            texture = Program.Game.Content.Load<Texture2D>("NarrowHorizontal");
        }
    }
}
