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
                                        Program.Game.PlayerNumbers = (NumberOfPlayers)myIncMsg.ReadInt32();
                                        int slot = myIncMsg.ReadInt32();
                                        int numberOfWalls = myIncMsg.ReadInt32();
                                        Player recentlyJoined = new Player(name);
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

                                        for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
                                        {
                                            Console.WriteLine("Name: " + PlayerManager.myPlayers[i].Name);
                                            Console.WriteLine("Slot: " + PlayerManager.myPlayers[i].Slot);
                                            Console.WriteLine("Color: " + PlayerManager.myPlayers[i].Color);
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
