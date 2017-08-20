using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quoridor
{
    class WideTile : Tile
    {
        private Texture2D myTexture;
        public Color Color { get; set; }

        public WideTile(Vector2 aPosition) : base(32, 32, aPosition)
        {
            myTexture = Program.Game.Content.Load<Texture2D>("WideTile");
            Color = Color.White;
        }

        public void Draw(SpriteBatch aSpritebatch)
        {
            aSpritebatch.Draw(myTexture, Position, Color);
        }
    }
}
