using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quoridor
{
    public abstract class GraphicalObject
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Color Color { get; protected set; }
        public Vector2 Position { get; protected set; }

        protected Texture2D myTexture;

        public GraphicalObject(int aWidth, int aHeight, string aGraphicsString)
        {
            Width = aWidth;
            Height = aHeight;
            myTexture = Program.Game.Content.Load<Texture2D>(aGraphicsString);
        }

        public virtual void Draw(SpriteBatch aSB)
        {
            aSB.Draw(myTexture, Position, Color);
        }
    }
}
