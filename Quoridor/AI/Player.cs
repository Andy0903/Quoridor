namespace Quoridor.AI
{
    public class Player
    {
        public int NumberOfWalls { get; private set; }
        public Point Position { get; private set; }
        public Color Color { get; private set; }

        public Player(int numberOfWalls, Point position, Microsoft.Xna.Framework.Color color)
        {
            NumberOfWalls = numberOfWalls;
            Position = new Point(position.X, position.Y);

            if (color == Microsoft.Xna.Framework.Color.Red)
            {
                Color = Color.Red;
            }
            else if (color == Microsoft.Xna.Framework.Color.Blue)
            {
                Color = Color.Blue;
            }
            else if (color == Microsoft.Xna.Framework.Color.Green)
            {
                Color = Color.Green;
            }
            else if (color == Microsoft.Xna.Framework.Color.Yellow)
            {
                Color = Color.Yellow;
            }

        }
    }
}
