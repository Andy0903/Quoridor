using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quoridor
{
    public abstract class GraphicalObject
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Color Color { get; protected set; }
        public Vector2 Position { get; set; }

        protected Texture2D texture;

        public GraphicalObject(int width, int height, string textureFileName)
        {
            Width = width;
            Height = height;
            texture = Program.Game.Content.Load<Texture2D>(textureFileName);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color);
        }
    }
}
