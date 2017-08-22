using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorServer
{
    class Game
    {
        //väggar
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
                    myWideTiles[i, k] = new Tile();
                }
            }

            for (int i = 0; i < myWideTiles.GetLength(0); i++)
            {
                myWideTiles[i, 0].Color = Tile.Colors.Red;
                myWideTiles[i, myWideTiles.GetLength(0) - 1].Color = Tile.Colors.Blue;
            }

            if (PlayerManager.GameModePlayers == PlayerManager.NumberOfPlayersGameMode.FourPlayers)
            {
                for (int i = 0; i < myWideTiles.GetLength(1); i++)
                {
                    myWideTiles[0, i].Color = Tile.Colors.Yellow;
                    myWideTiles[myWideTiles.GetLength(1) - 1, i].Color = Tile.Colors.Green;
                }
            }

            for (int i = 0; i < myVerticals.GetLength(0); i++)
            {
                for (int k = 0; k < myVerticals.GetLength(1); k++)
                {
                    myVerticals[i, k] = new Tile();
                }
            }

            for (int i = 0; i < myVerticals.GetLength(1); i++)
            {
                for (int k = 0; k < myVerticals.GetLength(0); k++)
                {
                    myHorizontals[i, k] = new Tile();
                }
            }

        }

        public void NextTurn()
        {
            myPlayerIndexThisTurn = (myPlayerIndexThisTurn + 1) % PlayerManager.myPlayers.Count;
            NetworkManager.MessageNextTurn(myPlayerIndexThisTurn);
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
                            if (myVerticals[column, row].IsOccupied == false && row != myVerticals.GetLength(0)) //TODO bug test these two (The one under as well)
                            {
                                player.DecrementWalls();
                                myVerticals[column, row].IsOccupied = true;
                                myVerticals[column, row + 1].IsOccupied = true;
                                NetworkManager.UpdateWallInfo(myPlayerIndexThisTurn, Tile.TileType.NarrowVertical, column, row);
                                NextTurn();
                            }
                            break;
                        case Tile.TileType.NarrowHorizontal:
                            if (myHorizontals[column, row].IsOccupied == false && column != myHorizontals.GetLength(1))
                            {
                                player.DecrementWalls();
                                myHorizontals[column, row].IsOccupied = true;
                                myHorizontals[column + 1, row].IsOccupied = true;
                                NetworkManager.UpdateWallInfo(myPlayerIndexThisTurn, Tile.TileType.NarrowHorizontal, column, row);
                                NextTurn();
                            }
                            break;
                        case Tile.TileType.Wide:
                            break;
                        default:
                            break;
                    }
                }

                //IF TILES AT COLUMN ROW IS FREE AND PLAYER HAS WALL, PLACE WALL, TAKE AWAY WALL FORM PLAYER AND UPDATE EVERYONES INFO. TODO
                //Set new players turn
            }
        }
    }
}
