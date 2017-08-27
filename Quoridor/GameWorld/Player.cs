using Microsoft.Xna.Framework;

namespace Quoridor
{
    public class Player : GraphicalObject
    {
        public string Name { get; private set; }
        public int NumberOfWalls { get; set; }

        public Point WideTilePosition { get; private set; }

        private int slot;
        public int Slot //Which player you are, 1st, 2nd, 3rd, 4th.. (0-3)
        {
            get { return slot; }
            set
            {
                switch (value)
                {
                    case 0:
                        Color = Color.Red;
                        Position = Program.Game.GameBoard.GetPositionOfTile(4, 8);
                        break;
                    case 1:
                        Color = Color.Blue;
                        Position = Program.Game.GameBoard.GetPositionOfTile(4, 0);
                        break;
                    case 2:
                        Color = Color.Green;
                        Position = Program.Game.GameBoard.GetPositionOfTile(0, 4);
                        break;
                    case 3:
                        Color = Color.Yellow;
                        Position = Program.Game.GameBoard.GetPositionOfTile(8, 4);
                        break;
                    default:
                        Color = Color.White;
                        Position = new Vector2(0, 0);
                        break;
                }

                slot = value;
            }
        } 

        public Player(string aName, Point aWideTilePosition) : base(32, 32, "Player")
        {
            Name = aName;
            Slot = -1;
            NumberOfWalls = -1;
            WideTilePosition = aWideTilePosition;
        }

        public void Update()
        {
            //NetworkManager.myOutMsg = NetworkManager.myClient.CreateMessage();
            //NetworkManager.myOutMsg.Write("Update");
            ////NetworkManager.myOutMsg.Write(Name);
            //NetworkManager.myClient.SendMessage(NetworkManager.myOutMsg, Lidgren.Network.NetDeliveryMethod.Unreliable);
        }

        public void PlaceWall(Tile.TileType aType, int aColumn, int aRow)
        {
            NetworkManager.myOutMsg = NetworkManager.myClient.CreateMessage();
            NetworkManager.myOutMsg.Write("Place Wall");
            // NetworkManager.myOutMsg.Write(Name);
            NetworkManager.myOutMsg.Write((int)aType);
            NetworkManager.myOutMsg.Write(aColumn);
            NetworkManager.myOutMsg.Write(aRow);
            NetworkManager.myClient.SendMessage(NetworkManager.myOutMsg, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
        }

        public void Move(int aColumn, int aRow)
        {
            NetworkManager.myOutMsg = NetworkManager.myClient.CreateMessage();
            NetworkManager.myOutMsg.Write("Move");
            NetworkManager.myOutMsg.Write(aColumn);
            NetworkManager.myOutMsg.Write(aRow);
            NetworkManager.myClient.SendMessage(NetworkManager.myOutMsg, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
        }

        private bool IsValidMovesetAndNotBlockedByWall(int aColumn, int aRow)
        {
            int playerColumn = PlayerManager.myPlayers[PlayerManager.CurrentSlotTurn].WideTilePosition.X;
            int playerRow = PlayerManager.myPlayers[PlayerManager.CurrentSlotTurn].WideTilePosition.Y;

            bool validColumnMove = ((playerColumn == (aColumn - 1) || playerColumn == (aColumn + 1)) && playerRow == aRow);
            bool validRowMove = ((playerRow == (aRow - 1) || playerRow == (aRow + 1)) && playerColumn == aColumn);

            if (validColumnMove != validRowMove) //XOR checking so it's not a diagonal move.
            {
                if ((aRow - playerRow) < 0) //Up
                {
                    if (Program.Game.GameBoard.myHorizontals[aRow, aColumn].IsOccupied == false)
                    {
                        return true;
                    }
                }
                else if ((aRow - playerRow) > 0) //Down
                {
                    if (Program.Game.GameBoard.myHorizontals[aRow - 1, aColumn].IsOccupied == false)
                    {
                        return true;
                    }
                }
                else if ((aColumn - playerColumn) < 0) //Left
                {
                    if (Program.Game.GameBoard.myVerticals[aRow, aColumn].IsOccupied == false)
                    {
                        return true;
                    }
                }
                else if ((aColumn - playerColumn) > 0) //Right
                {
                    if (Program.Game.GameBoard.myVerticals[aRow, aColumn - 1].IsOccupied == false)
                    {
                        return true;
                    }
                }
            }
            else //Jumping over player possibility
            {
                for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
                {
                    if (i == PlayerManager.CurrentSlotTurn)
                    {
                        continue;
                    }

                    int playerIColumn = PlayerManager.myPlayers[i].WideTilePosition.X;
                    int playerIRow = PlayerManager.myPlayers[i].WideTilePosition.Y;

                    bool validColumnMovePlayerI = ((playerIColumn == (aColumn - 1) || playerIColumn == (aColumn + 1)) && playerIRow == aRow);
                    bool validRowMovePlayerI = ((playerIRow == (aRow - 1) || playerIRow == (aRow + 1)) && playerIColumn == aColumn);

                    if (validColumnMovePlayerI != validRowMovePlayerI) //Valid move from player.
                    {
                        if ((aRow - playerIRow) < 0) //Up
                        {
                            if (Program.Game.GameBoard.myHorizontals[aRow, aColumn].IsOccupied == false)
                            {
                                return true;
                            }
                        }
                        else if ((aRow - playerIRow) > 0) //Down
                        {
                            if (Program.Game.GameBoard.myHorizontals[aRow - 1, aColumn].IsOccupied == false)
                            {
                                return true;
                            }
                        }
                        else if ((aColumn - playerIColumn) < 0) //Left
                        {
                            if (Program.Game.GameBoard.myVerticals[aRow, aColumn].IsOccupied == false)
                            {
                                return true;
                            }
                        }
                        else if ((aColumn - playerIColumn) > 0) //Right
                        {
                            if (Program.Game.GameBoard.myVerticals[aRow, aColumn - 1].IsOccupied == false)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private void MoveIfValid(Player aPlayer, int aWishColumn, int aWishRow)
        {
            if (Program.Game.GameBoard.IsWithinGameBoard(aWishColumn, aWishRow) && Program.Game.GameBoard.TileNotOccupied(aWishColumn, aWishRow))
            {
                if (IsValidMovesetAndNotBlockedByWall(aWishColumn, aWishRow))
                {
                    int oldColumn = aPlayer.WideTilePosition.X;
                    int oldRow = aPlayer.WideTilePosition.Y;
                    Program.Game.GameBoard.SetWideTilesOccupied(oldColumn, oldRow, false);
                    aPlayer.WideTilePosition = new Point(aWishColumn, aWishRow);
                    Program.Game.GameBoard.SetWideTilesOccupied(aWishColumn, aWishRow, true);

                    Move(aWishColumn, aWishRow);

                    //SKa klienten hålla koll på om någon vinner?
                    //if (myWideTiles[aPlayer.WideTilePosition.X, aPlayer.WideTilePosition.Y].Color == PlayerManager.myPlayers[myPlayerIndexThisTurn].Color)
                    //{
                    //    NetworkManager.MessagePlayerWon(myPlayerIndexThisTurn);
                    //}
                }
            }
        }
    }
}
