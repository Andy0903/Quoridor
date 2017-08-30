using Lidgren.Network;
using System.Collections.Generic;

namespace Quoridor
{
    public class GameReadyToStartMessage : IMessage
    {
        public List<string> PlayerNames { get; private set; }
        
        public GameReadyToStartMessage(NetIncomingMessage aIncMessage)
        {
            int numberOfPlayers = aIncMessage.ReadInt32();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                PlayerNames.Add(aIncMessage.ReadString());
            }
        }

        public void Send()
        {
            NetOutgoingMessage outMessage = NetworkManager.Client.CreateMessage();
            outMessage.Write((int)MessageType.GameReadyToStart);
            outMessage.Write(PlayerNames.Count);
            foreach (string name in PlayerNames)
            {
                outMessage.Write(name);
            }

            NetworkManager.Client.SendMessage(outMessage, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
