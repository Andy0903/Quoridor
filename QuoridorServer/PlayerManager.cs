using System.Collections.Generic;
using System.Windows.Forms;

namespace QuoridorServer
{
    class PlayerManager
    {
        public static List<Player> myPlayers = new List<Player>();
        private static bool ConnectionCountAndPlayerCountEqual { get { return NetworkManager.myServer.ConnectionsCount == myPlayers.Count; } }

        private static void CheckTimeOuts(RichTextBox aServerLog)
        {
            if (ConnectionCountAndPlayerCountEqual)
            {
                for (int i = 0; i < myPlayers.Count; i++)
                {
                    myPlayers[i].myTimeOut++;

                    NetworkManager.myOutMsg = NetworkManager.myServer.CreateMessage();
                    NetworkManager.myOutMsg.Write("hey");
                    NetworkManager.myOutMsg.Write(myPlayers[i].Name);
                    NetworkManager.myServer.SendMessage(NetworkManager.myOutMsg, NetworkManager.myServer.Connections,
                        Lidgren.Network.NetDeliveryMethod.Unreliable, 0);

                    if (180 < myPlayers[i].myTimeOut) //Has player timed out?
                    {
                        NetworkManager.myServer.Connections[i].Disconnect("Timed out.");
                        aServerLog.AppendText(myPlayers[i].Name + " has timed out." + "\n");
                        System.Threading.Thread.Sleep(100);

                        if (NetworkManager.myServer.ConnectionsCount != 0)
                        {
                            NetworkManager.myOutMsg = NetworkManager.myServer.CreateMessage();
                            NetworkManager.myOutMsg.Write("Disconnect");
                            NetworkManager.myOutMsg.Write(myPlayers[i].Name);
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

        public static void Update(RichTextBox aServerLog)
        {
            CheckTimeOuts(aServerLog);
        }
    }
}
