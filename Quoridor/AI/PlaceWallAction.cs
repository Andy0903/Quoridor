using QuoridorNetwork;

namespace Quoridor.AI
{
    public class PlaceWallAction : Action
    {
        public int Column { get; private set; }
        public int Row { get; private set; }
        public WallOrientation WallAlignment { get; private set; }

        public PlaceWallAction(int column, int row, WallOrientation wallAlignment)
        {
            Column = column;
            Row = row;
            WallAlignment = wallAlignment;
        }
    }
}
