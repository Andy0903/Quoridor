﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuoridorNetwork;

namespace Quoridor
{
    public class WideTile : Tile
    {
        Texture2D texture;
        public Color Color { get; set; }

        public WideTile(Vector2 position) : base(TileType.Wide, position)
        {
            texture = Game1.CM.Load<Texture2D>("WideTile");
            Color = Color.DimGray;
        }

        public void Draw(SpriteBatch aSB)
        {
            aSB.Draw(texture, Position, Color);
        }
    }
}
