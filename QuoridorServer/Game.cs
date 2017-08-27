using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorServer
{
    class Game
    {
        public static int myPlayerIndexThisTurn = -1;
        Tile[,] myWideTiles = new Tile[9, 9];
        Tile[,] myVerticals = new Tile[8, 9];
        Tile[,] myHorizontals = new Tile[9, 8];

        public Game()
        {
            for (int i = 0; i < myWideTiles.GetLength(0); i++)
            {
                for (int k = 0; k < myWideTiles.GetLength(1); k++)
                {
                    myWideTiles[i, k] = new Tile(new Microsoft.Xna.Framework.Point(i, k));
                }
            }

            for (int i = 0; i < myWideTiles.GetLength(0); i++)
            {
                myWideTiles[i, 0].Color = Tile.Colors.Red;
                myWideTiles[i, myWideTiles.GetLength(0) - 1].Color = Tile.Colors.Blue;
            }

            myWideTiles[4, 8].IsOccupied = true;
            myWideTiles[4, 0].IsOccupied = true;

            if (PlayerManager.GameModePlayers == PlayerManager.NumberOfPlayersGameMode.FourPlayers)
            {
                for (int i = 0; i < myWideTiles.GetLength(1); i++)
                {
                    myWideTiles[0, i].Color = Tile.Colors.Yellow;
                    myWideTiles[myWideTiles.GetLength(1) - 1, i].Color = Tile.Colors.Green;
                }

                myWideTiles[0, 4].IsOccupied = true;
                myWideTiles[8, 4].IsOccupied = true;
            }

            for (int i = 0; i < myVerticals.GetLength(0); i++)
            {
                for (int k = 0; k < myVerticals.GetLength(1); k++)
                {
                    myVerticals[i, k] = new Tile(new Microsoft.Xna.Framework.Point(i, k));
                }
            }

            for (int i = 0; i < myVerticals.GetLength(1); i++)
            {
                for (int k = 0; k < myVerticals.GetLength(0); k++)
                {
                    myHorizontals[i, k] = new Tile(new Microsoft.Xna.Framework.Point(i, k));
                }
            }

        }

        public void NextTurn()
        {
            myPlayerIndexThisTurn = (myPlayerIndexThisTurn + 1) % PlayerManager.myPlayers.Count;
            NetworkManager.MessageNextTurn(myPlayerIndexThisTurn);
        }

        private bool IsWithinGameBoard(int aColumn, int aRow)
        {
            return (0 <= aColumn && aColumn < myWideTiles.GetLength(0) &&
                    0 <= aRow && aRow < myWideTiles.GetLength(1));
        }

        private bool IsValidMovesetAndNotBlockedByWall(int aColumn, int aRow)
        {
            int playerColumn = PlayerManager.myPlayers[myPlayerIndexThisTurn].WideTilePosition.X;
            int playerRow = PlayerManager.myPlayers[myPlayerIndexThisTurn].WideTilePosition.Y;

            bool validColumnMove = ((playerColumn == (aColumn - 1) || playerColumn == (aColumn + 1)) && playerRow == aRow);
            bool validRowMove = ((playerRow == (aRow - 1) || playerRow == (aRow + 1)) && playerColumn == aColumn);

            if (validColumnMove != validRowMove) //XOR checking so it's not a diagonal move.
            {
                if ((aRow - playerRow) < 0) //Up
                {
                    if (myHorizontals[aRow, aColumn].IsOccupied == false)
                    {
                        return true;
                    }
                }
                else if ((aRow - playerRow) > 0) //Down
                {
                    if (myHorizontals[aRow - 1, aColumn].IsOccupied == false)
                    {
                        return true;
                    }
                }
                else if ((aColumn - playerColumn) < 0) //Left
                {
                    if (myVerticals[aRow, aColumn].IsOccupied == false)
                    {
                        return true;
                    }
                }
                else if ((aColumn - playerColumn) > 0) //Right
                {
                    if (myVerticals[aRow, aColumn - 1].IsOccupied == false)
                    {
                        return true;
                    }
                }
            }
            else //Jumping over player possibility
            {
                for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
                {
                    if (i == myPlayerIndexThisTurn)
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
                            if (myHorizontals[aRow, aColumn].IsOccupied == false)
                            {
                                return true;
                            }
                        }
                        else if ((aRow - playerIRow) > 0) //Down
                        {
                            if (myHorizontals[aRow - 1, aColumn].IsOccupied == false)
                            {
                                return true;
                            }
                        }
                        else if ((aColumn - playerIColumn) < 0) //Left
                        {
                            if (myVerticals[aRow, aColumn].IsOccupied == false)
                            {
                                return true;
                            }
                        }
                        else if ((aColumn - playerIColumn) > 0) //Right
                        {
                            if (myVerticals[aRow, aColumn - 1].IsOccupied == false)
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
            if (IsWithinGameBoard(aWishColumn, aWishRow) && myWideTiles[aWishColumn, aWishRow].IsOccupied == false)
            {
                if (IsValidMovesetAndNotBlockedByWall(aWishColumn, aWishRow))
                {
                    int oldColumn = aPlayer.WideTilePosition.X;
                    int oldRow = aPlayer.WideTilePosition.Y;
                    myWideTiles[oldColumn, oldRow].IsOccupied = false;
                    aPlayer.WideTilePosition = new Microsoft.Xna.Framework.Point(aWishColumn, aWishRow);
                    myWideTiles[aWishColumn, aWishRow].IsOccupied = true;
                    NetworkManager.MessagePlayerMovement(myPlayerIndexThisTurn, aWishColumn, aWishRow, oldColumn, oldRow);

                    if (myWideTiles[aPlayer.WideTilePosition.X, aPlayer.WideTilePosition.Y].Color == PlayerManager.myPlayers[myPlayerIndexThisTurn].Color)
                    {
                        NetworkManager.MessagePlayerWon(myPlayerIndexThisTurn);
                    }

                    NextTurn();
                }
            }
        }

        public void Move(NetIncomingMessage aIncMsg)
        {
            if (aIncMsg.SenderConnection.RemoteUniqueIdentifier == PlayerManager.myPlayers[myPlayerIndexThisTurn].RemoteUniqueIdentifier)
            {
                Player player = PlayerManager.myPlayers[myPlayerIndexThisTurn];
                player.myTimeOut = 0;

                int wishToMoveToColumn = aIncMsg.ReadInt32();
                int wishToMoveToRow = aIncMsg.ReadInt32();

                MoveIfValid(player, wishToMoveToColumn, wishToMoveToRow);

            }
        }

        public void PlaceWall(NetIncomingMessage aIncMsg)
        {
            if (aIncMsg.SenderConnection.RemoteUniqueIdentifier == PlayerManager.myPlayers[myPlayerIndexThisTurn].RemoteUniqueIdentifier)
            {
                Player player = PlayerManager.myPlayers[myPlayerIndexThisTurn];

                player.myTimeOut = 0;
                Tile.TileType type = (Tile.TileType)aIncMsg.ReadInt32();
                int column = aIncMsg.ReadInt32();
                int row = aIncMsg.ReadInt32();

                if (player.NumberOfWalls > 0)
                {
                    switch (type)
                    {
                        case Tile.TileType.NarrowVertical:
                            if (myVerticals[column, row].IsOccupied == false && myVerticals[column, row + 1].IsOccupied == false && row != myVerticals.GetLength(0)) //TODO bug test these two (The one under as well)
                            {
                                myVerticals[column, row].IsOccupied = true;
                                myVerticals[column, row + 1].IsOccupied = true;

                                if (PathToGoalExists())
                                {
                                    player.DecrementWalls();
                                    NetworkManager.UpdateWallInfo(myPlayerIndexThisTurn, Tile.TileType.NarrowVertical, column, row);
                                    NextTurn();
                                }
                                else
                                {
                                    myVerticals[column, row].IsOccupied = false;
                                    myVerticals[column, row + 1].IsOccupied = false;
                                    NetworkManager.DoSomethingElse(myPlayerIndexThisTurn);
                                }

                            }
                            break;
                        case Tile.TileType.NarrowHorizontal:
                            if (myHorizontals[column, row].IsOccupied == false && myHorizontals[column + 1, row].IsOccupied == false && column != myHorizontals.GetLength(1))
                            {
                                myHorizontals[column, row].IsOccupied = true;
                                myHorizontals[column + 1, row].IsOccupied = true;

                                if (PathToGoalExists())
                                {
                                    player.DecrementWalls();
                                    NetworkManager.UpdateWallInfo(myPlayerIndexThisTurn, Tile.TileType.NarrowHorizontal, column, row);
                                    NextTurn();
                                }
                                else
                                {
                                    myHorizontals[column, row].IsOccupied = true;
                                    myHorizontals[column + 1, row].IsOccupied = true;
                                    NetworkManager.DoSomethingElse(myPlayerIndexThisTurn);
                                }

                            }
                            break;
                        case Tile.TileType.Wide:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private bool PathToGoalExists()
        {
            SortedSet<Tile> S = new SortedSet<Tile>();
            Queue<Tile> Q = new Queue<Tile>();

            foreach (Player p in PlayerManager.myPlayers)
            {
                Tile root = myWideTiles[p.WideTilePosition.X, p.WideTilePosition.Y];
                S.Add(root);
                Q.Enqueue(root);

                while (Q.Count > 0)
                {
                    Tile current = Q.Dequeue();

                    if (current.Color == p.Color) //Current is a goal tile for the player.
                    {
                        return true;
                    }

                    foreach (Tile t in GetAdjacentTiles(current.Position))
                    {
                        if (S.Contains(t) == false)
                        {
                            S.Add(t);
                            Q.Enqueue(t);
                        }
                    }
                }
            }

            return false;
        }

        private List<Tile> GetAdjacentTiles(Point aIndex)
        {
            List<Tile> adjacent = new List<Tile>();

            if (aIndex.Y - 1 >= 0)
            {
                if (myHorizontals[aIndex.X, aIndex.Y - 1].IsOccupied == false)
                {
                    adjacent.Add(myWideTiles[aIndex.X, aIndex.Y - 1]); //Up
                }
            }
            if (aIndex.X - 1 >= 0)
            {
                if (myVerticals[aIndex.X - 1, aIndex.Y].IsOccupied == false)
                {
                    adjacent.Add(myWideTiles[aIndex.X - 1, aIndex.Y]); //Left
                }
            }
            if (aIndex.Y + 1 < myWideTiles.Length)
            {
                if (myHorizontals[aIndex.Y, aIndex.X].IsOccupied == false)
                {
                    adjacent.Add(myWideTiles[aIndex.X, aIndex.Y + 1]); //Down
                }
            }
            if (aIndex.X + 1 < myWideTiles.Length)
            {
                if (myVerticals[aIndex.X, aIndex.Y].IsOccupied == false)
                {
                    adjacent.Add(myWideTiles[aIndex.X + 1, aIndex.Y]);  //Right
                }
            }

            return adjacent;
        }
    }
}
