using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quoridor
{
    public class Player : GraphicalObject
    {
        public string Name { get; private set; }
        public int NumberOfWalls { get; set; }
        public Point WideTilePosition { get; set; }
        GameBoard gameBoard;
        private int slot;

        public Player(string name, int slot, int numberOfWalls, GameBoard board) : base(32, 32, "Player")
        {
            Name = name;
            gameBoard = board;
            this.slot = slot;
            NumberOfWalls = numberOfWalls;

            switch (this.slot)
            {
                case 0:
                    Color = Color.Red;
                    Position = gameBoard.GetPositionOfTile(4, 8);
                    WideTilePosition = new Point(4, 8);
                    break;
                case 1:
                    Color = Color.Blue;
                    Position = gameBoard.GetPositionOfTile(4, 0);
                    WideTilePosition = new Point(4, 0);
                    break;
                case 2:
                    Color = Color.Green;
                    Position = gameBoard.GetPositionOfTile(0, 4);
                    WideTilePosition = new Point(0, 4);
                    break;
                case 3:
                    Color = Color.Yellow;
                    Position = gameBoard.GetPositionOfTile(8, 4);
                    WideTilePosition = new Point(8, 4);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, gameBoard.GetPositionOfTile(WideTilePosition.X, WideTilePosition.Y), Color);
        }
    }
}
