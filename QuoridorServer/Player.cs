namespace QuoridorServer
{
    class Player
    {
        public string Name { get; private set; }
        public int myTimeOut;
        public long RemoteUniqueIdentifier { get; private set; }
        public int NumberOfWalls { get; private set; }

        public Player(string aName, long aRemoteUniqueIdentifier, int aNumberOfWalls)
        {
            Name = aName;
            myTimeOut = 0;
            RemoteUniqueIdentifier = aRemoteUniqueIdentifier;
            NumberOfWalls = aNumberOfWalls;
        }

        public void DecrementWalls()
        {
            --NumberOfWalls;
        }
    }
}
