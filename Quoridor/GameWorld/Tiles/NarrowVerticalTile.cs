using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quoridor
{
    class NarrowVerticalTile : Tile
    {
        private Texture2D myTexture;
        public Color Color { get; set; }

        public NarrowVerticalTile(Vector2 aPosition) : base(TileType.NarrowVertical, aPosition)
        {
            myTexture = Program.Game.Content.Load<Texture2D>("NarrowVertical");
            Color = Color.Red;
        }

        public void Draw(SpriteBatch aSB)
        {
            aSB.Draw(myTexture, Position, Color);
        }
    }
}
