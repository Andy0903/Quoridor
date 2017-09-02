using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace QuoridorServer
{
    class Player
    {
        public Color Color { get; private set; }
        public string Name { get; private set; }
        public NetConnection Connection { get; private set; }
        public int NumberOfWalls { get; private set; }
        public Point Position { get; set; }

        public Player(string name, NetConnection connection, int numberOfWalls, int slot)
        {
            Name = name;
            Connection = connection;
            NumberOfWalls = numberOfWalls;

            switch (slot)
            {
                case 0:
                    Color = Color.Red;
                    Position = new Point(4, 8);
                    break;
                case 1:
                    Color = Color.Blue;
                    Position = new Point(4, 0);
                    break;
                case 2:
                    Color = Color.Green;
                    Position = new Point(0, 4);
                    break;
                case 3:
                    Color = Color.Yellow;
                    Position = new Point(8, 4);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }

        public void DecrementWalls()
        {
            NumberOfWalls--;
        }
    }
}
