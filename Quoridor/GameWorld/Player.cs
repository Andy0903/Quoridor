using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor
{
    public class Player
    {
        public string myName;

        public Player(string aName)
        {
            myName = aName;
        }

        public void Update()
        {
            NetworkManager.myOutMsg = NetworkManager.myClient.CreateMessage();
            NetworkManager.myOutMsg.Write(myName);
            NetworkManager.myOutMsg.Write("Update");
            NetworkManager.myClient.SendMessage(NetworkManager.myOutMsg, Lidgren.Network.NetDeliveryMethod.Unreliable);
        }
    }
}
