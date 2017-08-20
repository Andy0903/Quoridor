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

        private static bool ReceivedValidMessage { get { return (myIncMsg = myServer.ReadMessage()) != null; } }

        public static void Initialize(TextBox aPortTextBox)
        {
            myConfig = new NetPeerConfiguration("QuoridorConfig");

            int port;
            int.TryParse(aPortTextBox.Text, out port);
            myConfig.Port = port;

            myServer = new NetServer(myConfig);
            myServer.Start();
        }

        public static void Deintialize()
        {
            myServer.Shutdown("Disconnect");
        }

        public static void Update(RichTextBox aServerLog)
        {
            while (ReceivedValidMessage)
            {
                switch (myIncMsg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        {
                            string headStringMsg = myIncMsg.ReadString();

                            switch (headStringMsg)
                            {
                                case "Connect":
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
                                            PlayerManager.myPlayers.Add(new Player(name));
                                            aServerLog.AppendText(name + " connected!" + "\n");

                                            for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
                                            {
                                                myOutMsg = myServer.CreateMessage();
                                                myOutMsg.Write("Connect");
                                                myOutMsg.Write(PlayerManager.myPlayers[i].Name);
                                                myOutMsg.Write((int)PlayerManager.GameModePlayers);

                                                myServer.SendMessage(myOutMsg, myServer.Connections,
                                                    NetDeliveryMethod.ReliableOrdered, 0);
                                            }
                                        }
                                    }
                                    break;

                                case "Update":
                                    {
                                        string name = myIncMsg.ReadString();
                                        for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
                                        {
                                            if (PlayerManager.myPlayers[i].Name.Equals(name))
                                            {
                                                PlayerManager.myPlayers[i].myTimeOut = 0;
                                            }
                                        }
                                    }
                                    break;

                                case "Disconnect":
                                    {
                                        string name = myIncMsg.ReadString();

                                        for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
                                        {
                                            if (PlayerManager.myPlayers[i].Name.Equals(name))
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
                                    break;
                            }
                        }
                        break;
                }
                myServer.Recycle(myIncMsg);
            }
        }
    }
}
