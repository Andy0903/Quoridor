using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuoridorServer
{
    class PlayerManager
    {
        public static List<Player> myPlayers = new List<Player>();

        public void Update(RichTextBox aServerLog)
        {
            if (NetworkManager.myServer.ConnectionsCount == myPlayers.Count)
            {
                for (int i = 0; i < myPlayers.Count; i++)
                {
                    myPlayers[i].myTimeOut++;

                    NetworkManager.myOutMsg = NetworkManager.myServer.CreateMessage();
                    NetworkManager.myOutMsg.Write("hey");
                    NetworkManager.myOutMsg.Write(myPlayers[i].myName);
                    NetworkManager.myServer.SendMessage(NetworkManager.myOutMsg, NetworkManager.myServer.Connections,
                        Lidgren.Network.NetDeliveryMethod.Unreliable, 0);

                    if (180 < myPlayers[i].myTimeOut) //Player timed out. Proceed to disconnect.
                    {
                        NetworkManager.myServer.Connections[i].Disconnect("Timed out.");
                        aServerLog.AppendText(myPlayers[i].myName + " has timed out." + "\n");
                        System.Threading.Thread.Sleep(100);

                        if (NetworkManager.myServer.ConnectionsCount != 0)
                        {
                            NetworkManager.myOutMsg = NetworkManager.myServer.CreateMessage();
                            NetworkManager.myOutMsg.Write("Disconnect");
                            NetworkManager.myOutMsg.Write(myPlayers[i].myName);
                            NetworkManager.myServer.SendMessage(NetworkManager.myOutMsg, NetworkManager.myServer.Connections,
                                Lidgren.Network.NetDeliveryMethod.ReliableOrdered, 0);
                        }
                    }

                    myPlayers.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }
    }
}
