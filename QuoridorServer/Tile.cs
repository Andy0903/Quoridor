namespace QuoridorServer
{
    class Tile
    {
        public enum TileType
        {
            NarrowVertical,
            NarrowHorizontal,
            Wide
        }

        public enum Colors
        {
            Red,
            Blue,
            Green,
            Yellow,
            NONE
        }

        public bool IsOccupied { get; set; }
        public Colors Color { get; set; }

        public Tile()
        {
            IsOccupied = false;
            Color = Colors.NONE;
        }
    }
}
