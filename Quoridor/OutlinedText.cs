using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quoridor
{
    public static class OutlinedText
    {
        public static void DrawCenteredText(SpriteBatch spritebatch, SpriteFont font, float scale, string aText, Vector2 pos, Color? color = null)
        {
            Vector2 textSize = font.MeasureString(aText);
            Vector2 position = new Vector2(pos.X - (textSize.X / 2 * scale), pos.Y - (textSize.Y / 2 * scale));
            DrawOutlinedText(spritebatch, font, aText, position, scale, color ?? Color.White);
        }

        public static void DrawOutlinedText(SpriteBatch spritebatch, SpriteFont font, string text, Vector2 position, float scale, Color color)
        {
            TextShadow(spritebatch, font, text, position, scale);

            spritebatch.DrawString(font, text, position,
                   color, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
        }

        private static void TextShadow(SpriteBatch spritebatch, SpriteFont font, string text, Vector2 position, float scale)
        {
            spritebatch.DrawString(font, text, new Vector2(position.X - 2, position.Y),
                 Color.Black, 0, Vector2.Zero, scale, SpriteEffects.None, 1);

            spritebatch.DrawString(font, text, new Vector2(position.X + 2, position.Y),
                 Color.Black, 0, Vector2.Zero, scale, SpriteEffects.None, 1);

            spritebatch.DrawString(font, text, new Vector2(position.X, position.Y - 2),
                 Color.Black, 0, Vector2.Zero, scale, SpriteEffects.None, 1);

            spritebatch.DrawString(font, text, new Vector2(position.X - 2, position.Y + 2),
                 Color.Black, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
        }
    }
}