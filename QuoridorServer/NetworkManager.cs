using Lidgren.Network;
using System.Windows.Forms;

namespace QuoridorServer
{
    class NetworkManager
    {
        public static NetServer myServer;
        public static NetPeerConfiguration myConfig;
        static NetIncomingMessage myIncMsg;
        public static NetOutgoingMessage myOutMsg;
        static bool myPlayerNameValid;
        private static Game myGame;

        private static bool ReceivedValidMessage { get { return (myIncMsg = myServer.ReadMessage()) != null; } }

        public static void Initialize(TextBox aPortTextBox)
        {
            myConfig = new NetPeerConfiguration("QuoridorConfig");
            int port;
            int.TryParse(aPortTextBox.Text, out port);
            myConfig.Port = port;

            myServer = new NetServer(myConfig);
            myServer.Start();

            myGame = new Game();
        }

        public static void Deintialize()
        {
            myServer.Shutdown("Disconnect");
        }

        private static void UpdatePlayerLabel(Label aPlayerLabel)
        {
            aPlayerLabel.Text = "Players: " + PlayerManager.myPlayers.Count;
        }

        public static void MessageNextTurn(int aSlotToMessage) //TODO message client inc
        {
            myOutMsg = myServer.CreateMessage();
            myOutMsg.Write("New Turn");
            myOutMsg.Write(aSlotToMessage);
            myServer.SendMessage(myOutMsg, myServer.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }

        public static void UpdateWallInfo(int aSlotThatPutWall, Tile.TileType aTileType, int aColumn, int aRow) //TODO message client inc 
        {
            myOutMsg = myServer.CreateMessage();
            myOutMsg.Write("New Wall");
            myOutMsg.Write(aSlotThatPutWall);
            myOutMsg.Write((int)aTileType);
            myOutMsg.Write(aColumn);
            myOutMsg.Write(aRow);
            myServer.SendMessage(myOutMsg, myServer.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }

        private static void PlayerDisconnecting(RichTextBox aServerLog)
        {
            string name = myIncMsg.ReadString();
            for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
            {
                if (myIncMsg.SenderConnection.RemoteUniqueIdentifier == PlayerManager.myPlayers[i].RemoteUniqueIdentifier)
                {
                    myServer.Connections[i].Disconnect("Disconnected");
                    System.Threading.Thread.Sleep(100);
                    aServerLog.AppendText(name + " disconnected." + "\n");

                    if (myServer.ConnectionsCount != 0)
                    {
                        myOutMsg = myServer.CreateMessage();
                        myOutMsg.Write("Disconnect");
                        myOutMsg.Write(name);
                        myServer.SendMessage(myOutMsg, myServer.Connections,
                            NetDeliveryMethod.ReliableOrdered, 0);
                    }

                    PlayerManager.myPlayers.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }

        private static void PlayerConnecting(RichTextBox aServerLog)
        {
            string name = myIncMsg.ReadString();
            myPlayerNameValid = true;

            for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
            {
                if (PlayerManager.myPlayers[i].Name.Equals(name) || PlayerManager.ServerFull)
                {
                    myOutMsg = myServer.CreateMessage();
                    myOutMsg.Write("Duplicate name");
                    aServerLog.AppendText("Duplicate name or full server!\n");

                    myServer.SendMessage(myOutMsg, myIncMsg.SenderConnection,
                        NetDeliveryMethod.ReliableOrdered, 0);
                    System.Threading.Thread.Sleep(100);
                    myIncMsg.SenderConnection.Disconnect("Disconnected");
                    myPlayerNameValid = false;
                    break;
                }
            }

            if (myPlayerNameValid == true)
            {
                System.Threading.Thread.Sleep(100);
                int numberOfWalls = PlayerManager.GameModePlayers == PlayerManager.NumberOfPlayersGameMode.TwoPlayers ? 10 : 5;
                PlayerManager.myPlayers.Add(new Player(name, myIncMsg.SenderConnection.RemoteUniqueIdentifier, numberOfWalls));
                aServerLog.AppendText(name + " connected!" + "\n");

                for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
                {
                    myOutMsg = myServer.CreateMessage();
                    myOutMsg.Write("Connect");
                    myOutMsg.Write(PlayerManager.myPlayers[i].Name);
                    myOutMsg.Write((int)PlayerManager.GameModePlayers);
                    myOutMsg.Write(i); //playerSlot
                    myOutMsg.Write(numberOfWalls);

                    myServer.SendMessage(myOutMsg, myServer.Connections,
                        NetDeliveryMethod.ReliableOrdered, 0);
                }
            }

            
        }

        public static void Update(RichTextBox aServerLog, Label aPlayerLabel)
        {
            UpdatePlayerLabel(aPlayerLabel);
            while (ReceivedValidMessage)
            {
                if (myIncMsg.MessageType == NetIncomingMessageType.Data)
                {
                    string headStringMsg = myIncMsg.ReadString();
                    switch (headStringMsg)
                    {
                        //case "Update":
                        //    for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
                        //    {
                        //        if (myIncMsg.SenderConnection.RemoteUniqueIdentifier == PlayerManager.myPlayers[i].RemoteUniqueIdentifier)
                        //        {
                        //            PlayerManager.myPlayers[i].myTimeOut = 0;
                        //        }
                        //    }
                        //    break;
                        case "Connect":
                            PlayerConnecting(aServerLog);
                            if (PlayerManager.ServerFull)
                            {
                                myGame.NextTurn();
                            }
                            break;
                        case "Place Wall":
                            myGame.PlaceWall(myIncMsg);
                            break;
                        case "Disconnect":
                            PlayerDisconnecting(aServerLog);
                            break;
                    }
                    break;
                }
                myServer.Recycle(myIncMsg);
            }
        }
    }
}
