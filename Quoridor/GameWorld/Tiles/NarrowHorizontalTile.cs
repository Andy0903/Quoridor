
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quoridor
{
    class NarrowHorizontalTile : Tile
    {
        private Texture2D myTexture;
        public Color Color { get; set; }

        public NarrowHorizontalTile(Vector2 aPosition) : base(TileType.NarrowHorizontal, aPosition)
        {
            myTexture = Program.Game.Content.Load<Texture2D>("NarrowHorizontal");
            Color = Color.Blue;
        }

        public void Draw(SpriteBatch aSB)
        {
            aSB.Draw(myTexture, Position, Color);
        }
    }
}
