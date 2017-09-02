namespace Quoridor.AI
{
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Microsoft.Xna.Framework.Point(Point p)
        {
            return new Microsoft.Xna.Framework.Point(p.X, p.Y);
        }

        public static implicit operator Point(Microsoft.Xna.Framework.Point p)
        {
            return new Point(p.X, p.Y);
        }
    }
}
