using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;

namespace Quoridor
{
    class NetworkManager
    {
        public static NetClient myClient;
        public static NetPeerConfiguration myConfig;
        static NetIncomingMessage myIncMsg;
        public static NetOutgoingMessage myOutMsg;

        private static bool ReceivedValidMessage { get { return (myIncMsg = myClient.ReadMessage()) != null; } }

        private static void PlayerDisconnectedFromServer()
        {
            string name = myIncMsg.ReadString();

            for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
            {
                if (PlayerManager.myPlayers[i].Name.Equals(name))
                {
                    PlayerManager.myPlayers.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }

        private static void ConnectingToServer()
        {
            string name = myIncMsg.ReadString();
            Program.Game.PlayerNumbers = (NumberOfPlayers)myIncMsg.ReadInt32();
            int slot = myIncMsg.ReadInt32();
            int numberOfWalls = myIncMsg.ReadInt32();
            Point startTilePos = new Point(0, 0);
            switch (slot)
            {
                case 0:
                    startTilePos = new Point(4, 8);
                    break;
                case 1:
                    startTilePos = new Point(4, 0);
                    break;
                case 2:
                    startTilePos = new Point(8, 4);
                    break;
                case 3:
                    startTilePos = new Point(0, 4);
                    break;
            }


            Player recentlyJoined = new Player(name, startTilePos);
            Program.Game.ConstructBoard();
            recentlyJoined.Slot = slot;
            recentlyJoined.NumberOfWalls = numberOfWalls;
            PlayerManager.myPlayers.Add(recentlyJoined);

            for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
            {
                for (int k = i + 1; k < PlayerManager.myPlayers.Count; k++)
                {
                    if (i != k && PlayerManager.myPlayers[i].Name.Equals(PlayerManager.myPlayers[k].Name))
                    {
                        PlayerManager.myPlayers.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            Program.Game.State = GameState.Playing;
        }

        public static void Update()
        {
            while (ReceivedValidMessage)
            {
                if (myIncMsg.MessageType == NetIncomingMessageType.Data)
                {
                    string headStringMessage = myIncMsg.ReadString();
                    switch (headStringMessage)
                    {
                        case "Connect":
                            ConnectingToServer();

                            for (int i = 0; i < PlayerManager.myPlayers.Count; i++) //DEBUGGING ONLY
                            {
                                Console.WriteLine("Name: " + PlayerManager.myPlayers[i].Name);
                                Console.WriteLine("Slot: " + PlayerManager.myPlayers[i].Slot);
                                Console.WriteLine("Color: " + PlayerManager.myPlayers[i].Color);
                            }

                            break;
                        case "Disconnect":
                            PlayerDisconnectedFromServer();
                            break;
                        case "New Turn":
                            PlayerManager.CurrentSlotTurn = myIncMsg.ReadInt32();
                            PlayerManager.NumberOfTurns++;
                            break;
                        case "Player Moved":
                            int slotThatMoved = myIncMsg.ReadInt32();
                            int movedToColumn = myIncMsg.ReadInt32();
                            int movedToRow = myIncMsg.ReadInt32();
                            int oldColumn = myIncMsg.ReadInt32();
                            int oldRow = myIncMsg.ReadInt32();

                            PlayerManager.myPlayers[PlayerManager.CurrentSlotTurn].Position = Program.Game.GameBoard.GetPositionOfTile(movedToColumn, movedToRow);
                            Program.Game.GameBoard.MoveOccupation(movedToColumn, movedToRow, oldColumn, oldRow);
                            break;
                        case "New Wall":
                            int slotThatPutDownWall = myIncMsg.ReadInt32();
                            Tile.TileType onWhatTypeOfTile = (Tile.TileType)myIncMsg.ReadInt32();
                            int placedOnColumn = myIncMsg.ReadInt32();
                            int placedOnRow = myIncMsg.ReadInt32();
                            Program.Game.GameBoard.PlaceWall(slotThatPutDownWall, onWhatTypeOfTile, placedOnColumn, placedOnRow);
                            break;
                        case "Player Won":
                            int slotThatWon = myIncMsg.ReadInt32();
                            Program.Game.GameBoard.PlayerWon(slotThatWon);
                            break;
                    }
                }
                break;
            }
            if (myIncMsg != null)
            {
                myClient.Recycle(myIncMsg);
            }
        }
    }
}
