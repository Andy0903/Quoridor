using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor
{
    class NetworkManager
    {
        public static NetClient myClient;
        public static NetPeerConfiguration myConfig;
        static NetIncomingMessage myIncMsg;
        public static NetOutgoingMessage myOutMsg;

        private static bool ReceivedValidMessage { get { return (myIncMsg = myClient.ReadMessage()) != null; } }

        public static void Update()
        {
            while (ReceivedValidMessage)
            {
                switch (myIncMsg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        {
                            string headStringMessage = myIncMsg.ReadString();

                            switch (headStringMessage)
                            {
                                case "Connect":
                                    {
                                        string name = myIncMsg.ReadString();

                                        PlayerManager.myPlayers.Add(new Player(name));

                                        for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
                                        {
                                            for (int k = i + 1; k < PlayerManager.myPlayers.Count; k++)
                                            {
                                                if (i != k && PlayerManager.myPlayers[i].myName.Equals(PlayerManager.myPlayers[k].myName))
                                                {
                                                    PlayerManager.myPlayers.RemoveAt(i);
                                                    i--;
                                                    break;
                                                }
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
                myClient.Recycle(myIncMsg);
            }
        }
    }
}
