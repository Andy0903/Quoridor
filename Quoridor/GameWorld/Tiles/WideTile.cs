using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quoridor
{
    class WideTile : Tile
    {
        private Texture2D myTexture;
        public Color Color { get; set; }

        public WideTile(Vector2 aPosition) : base(TileType.Wide, aPosition)
        {
            myTexture = Program.Game.Content.Load<Texture2D>("WideTile");
            Color = Color.DimGray;
        }

        public void Draw(SpriteBatch aSB)
        {
            aSB.Draw(myTexture, Position, Color);
        }
    }
}
