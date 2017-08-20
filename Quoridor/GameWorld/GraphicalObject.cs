using Microsoft.Xna.Framework.Graphics;

namespace Quoridor
{
    abstract class GraphicalObject
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        protected Texture2D myTexture;

        public GraphicalObject(int aWidth, int aHeight, string aGraphicsString)
        {
            Width = aWidth;
            Height = aHeight;
            myTexture = Program.Game.Content.Load<Texture2D>(aGraphicsString);
        }
    }
}
