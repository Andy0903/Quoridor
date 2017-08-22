using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quoridor
{
    class Wall : GraphicalObject
    {
        public enum Alignment
        {
            Vertical,
            Horizontal,
        }

        public Alignment Align { get; set; }

        public Wall() : base(16, 144, "Wall")
        {

        }

        public override void Draw(SpriteBatch aSB)
        {
            if (Align == Alignment.Vertical)
            {
                base.Draw(aSB);
            }
            else
            {
                float rotation = 90f;
                aSB.Draw(myTexture, Position, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Color, rotation, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
        }
    }
}
