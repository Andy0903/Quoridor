using System;

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


        public static bool operator ==(Point lhs, Point rhs)
        {
            const int acceptableRadius = 20;
            return (lhs.X > rhs.X - acceptableRadius && lhs.X < rhs.X + acceptableRadius
                && lhs.Y > rhs.Y - acceptableRadius && lhs.Y < rhs.Y + acceptableRadius);
        }

        public static bool operator !=(Point lhs, Point rhs)
        {
            return (lhs.X != rhs.X || lhs.Y != rhs.Y);
        }

        public static implicit operator Microsoft.Xna.Framework.Point(Point p)
        {
            return new Microsoft.Xna.Framework.Point(p.X, p.Y);
        }

        public static implicit operator Point(Microsoft.Xna.Framework.Vector2 p)
        {
            return new Point((int)p.X, (int)p.Y);
        }

        public static implicit operator Point(Microsoft.Xna.Framework.Point p)
        {
            return new Point(p.X, p.Y);
        }
    }
}
