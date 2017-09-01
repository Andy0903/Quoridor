namespace Quoridor.AI
{
    class MoveAction : Action
    {
        public int Column { get; private set; }
        public int Row { get; private set; }

        public MoveAction(int column, int row)
        {
            Column = column;
            Row = row;
        }
    }
}
