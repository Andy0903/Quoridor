using Microsoft.Xna.Framework;

namespace QuoridorServer
{
    class Player
    {
        public Tile.Colors Color { get; private set; }

        public string Name { get; private set; }
        public int myTimeOut;
        public long RemoteUniqueIdentifier { get; private set; }
        public int NumberOfWalls { get; private set; }

        public Point WideTilePosition { get; set; }

        public Player(string aName, long aRemoteUniqueIdentifier, int aNumberOfWalls, Tile.Colors aColor, Point aWideTilePosition)
        {
            Name = aName;
            myTimeOut = 0;
            RemoteUniqueIdentifier = aRemoteUniqueIdentifier;
            NumberOfWalls = aNumberOfWalls;
            Color = aColor;

            WideTilePosition = aWideTilePosition;
        }

        public void DecrementWalls()
        {
            --NumberOfWalls;
        }
    }
}
