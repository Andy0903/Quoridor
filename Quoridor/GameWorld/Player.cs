using Microsoft.Xna.Framework;

namespace Quoridor
{
    class Player : GraphicalObject
    {
        public string Name { get; private set; }
        public int NumberOfWalls { get; set; }

        public Point WideTilePosition { get; private set; }
        GameBoard myGameBoard;

        private int mySlot;

        public Player(string aName, int aSlot, int aNumberOfWalls, GameBoard aBoard) : base(32, 32, "Player")
        {
            Name = aName;
            myGameBoard = aBoard;
            mySlot = aSlot;
            NumberOfWalls = aNumberOfWalls;

            switch (mySlot)
            {
                case 0:
                    Color = Color.Red;
                    Position = myGameBoard.GetPositionOfTile(4, 8);
                    break;
                case 1:
                    Color = Color.Blue;
                    Position = myGameBoard.GetPositionOfTile(4, 0);
                    break;
                case 2:
                    Color = Color.Green;
                    Position = myGameBoard.GetPositionOfTile(0, 4);
                    break;
                case 3:
                    Color = Color.Yellow;
                    Position = myGameBoard.GetPositionOfTile(8, 4);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}
