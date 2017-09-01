
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuoridorNetwork;

namespace Quoridor
{
    public class NarrowHorizontalTile : Tile
    {
        private Texture2D texture;
        
        public NarrowHorizontalTile(Vector2 position) : base(TileType.NarrowHorizontal, position)
        {
            texture = Program.Game.Content.Load<Texture2D>("NarrowHorizontal");
        }
    }
}
