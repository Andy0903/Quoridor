using Lidgren.Network;
using System.Collections.Generic;

namespace QuoridorNetwork
{
    public class GameReadyToStartMessage : Message
    {
        public List<string> PlayerNames { get; private set; }
        
        public GameReadyToStartMessage(List<string> aPlayerNames)
        {
            PlayerNames = aPlayerNames;
        }

        public GameReadyToStartMessage(NetIncomingMessage aIncMessage)
        {
            PlayerNames = new List<string>();
            int numberOfPlayers = aIncMessage.ReadInt32();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                PlayerNames.Add(aIncMessage.ReadString());
            }
        }

        public static implicit operator NetOutgoingMessage(GameReadyToStartMessage aMessage)
        {
            NetOutgoingMessage outMessage = NetworkManager.Peer.CreateMessage();
            outMessage.Write((int)MessageType.GameReadyToStart);
            outMessage.Write(aMessage.PlayerNames.Count);
            foreach (string name in aMessage.PlayerNames)
            {
                outMessage.Write(name);
            }
            return outMessage;
        }
    }
}
