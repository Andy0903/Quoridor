using Microsoft.Xna.Framework;

namespace QuoridorServer
{
    class Player
    {
        public Color Color { get; private set; }
        public string Name { get; private set; }
        public long RemoteUniqueIdentifier { get; private set; }
        public int NumberOfWalls { get; private set; }
        public Point WideTilePosition { get; set; }

        public Player(string name, long remoteUniqueID, int numberOfWalls, int slot)
        {
            Name = name;
            RemoteUniqueIdentifier = remoteUniqueID;
            NumberOfWalls = numberOfWalls;

            switch (slot)
            {
                case 0:
                    Color = Color.Red;
                    WideTilePosition = new Point(4, 8);
                    break;
                case 1:
                    Color = Color.Blue;
                    WideTilePosition = new Point(4, 0);
                    break;
                case 2:
                    Color = Color.Green;
                    WideTilePosition = new Point(0, 4);
                    break;
                case 3:
                    Color = Color.Yellow;
                    WideTilePosition = new Point(8, 4);
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
