namespace QuoridorServer
{
    class Player
    {
        public string Name { get; private set; }
        public int myTimeOut;

        public Player(string aName)
        {
            Name = aName;
            myTimeOut = 0;
        }
    }
}
