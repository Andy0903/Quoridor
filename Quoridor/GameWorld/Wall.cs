namespace Quoridor
{
    class Wall : GraphicalObject
    {
        public enum Alignment
        {
            Vertical,
            Horizontal,
        }

        public Alignment Align { get; set; }

        public Wall() : base(16, 144, "Wall")
        {

        }
    }
}
