using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static void Initialize()
        {
            myConfig = new NetPeerConfiguration("QuoridorConfig");
            myConfig.Port = 14242;

            myServer = new NetServer(myConfig);
            myServer.Start();
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
                                            if (PlayerManager.myPlayers[i].Equals(name))
                                            {
                                                myOutMsg = myServer.CreateMessage();
                                                myOutMsg.Write("Duplicate name!");

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
                                            aServerLog.AppendText(name + " connected!" + "\r\n");

                                            for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
                                            {
                                                myOutMsg = myServer.CreateMessage();
                                                myOutMsg.Write("Connect");
                                                myOutMsg.Write(PlayerManager.myPlayers[i].myName);

                                                myServer.SendMessage(myOutMsg, myServer.Connections,
                                                    NetDeliveryMethod.ReliableOrdered, 0);
                                            }
                                        }
                                    }
                                    break;

                                case "Disconnect":
                                    {
                                        string name = myIncMsg.ReadString();

                                        for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
                                        {
                                            if (PlayerManager.myPlayers[i].myName.Equals(name))
                                            {
                                                myServer.Connections[i].Disconnect("Disconnected");
                                                System.Threading.Thread.Sleep(100);
                                                aServerLog.AppendText(name + " disconnected." + "\r\n");

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
