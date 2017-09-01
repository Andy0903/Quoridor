using Lidgren.Network;
using System.Collections.Generic;

namespace QuoridorNetwork
{
    public class GameReadyToStartMessage : Message
    {
        public List<string> PlayerNames { get; private set; }
        
        public GameReadyToStartMessage(List<string> playerNames)
        {
            PlayerNames = playerNames;
        }

        public GameReadyToStartMessage(NetIncomingMessage incMsg)
        {
            PlayerNames = new List<string>();
            int numberOfPlayers = incMsg.ReadInt32();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                PlayerNames.Add(incMsg.ReadString());
            }
        }

        public static implicit operator NetOutgoingMessage(GameReadyToStartMessage msg)
        {
            NetOutgoingMessage outMessage = NetworkManager.Peer.CreateMessage();
            outMessage.Write((int)MessageType.GameReadyToStart);
            outMessage.Write(msg.PlayerNames.Count);
            foreach (string name in msg.PlayerNames)
            {
                outMessage.Write(name);
            }
            return outMessage;
        }
    }
}
