using Microsoft.Xna.Framework;
using System;

namespace Quoridor.AI
{
    public class Tile
    {
        public Color Color { get; private set; }
        public Point Position { get; private set; }
        public bool IsOccupied { get; private set; }

        public Tile(bool isOccupied, Point position, Microsoft.Xna.Framework.Color color)
        {
            IsOccupied = isOccupied;
            Position = new Point(position.X, position.Y);

            if (color == new Microsoft.Xna.Framework.Color(1, 0.4f, 0.4f))
            {
                Color = Color.Red;
            }
            else if (color == new Microsoft.Xna.Framework.Color(0.4f, 0.4f, 1f))
            {
                Color = Color.Blue;
            }
            else if (color == new Microsoft.Xna.Framework.Color(0.4f, 1f, 0.4f))
            {
                Color = Color.Green;
            }
            else if (color == new Microsoft.Xna.Framework.Color(0.8f, 0.8f, 0))
            {
                Color = Color.Yellow;
            }
            else
            {
                Color = Color.None;
            }

        }
    }
}
