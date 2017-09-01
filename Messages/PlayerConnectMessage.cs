using Lidgren.Network;

namespace QuoridorNetwork
{
    public class PlayerConnectMessage : Message
    {
        public string PlayerName { get; private set; }

        public PlayerConnectMessage(string playerName)
        {
            PlayerName = playerName;
        }

        public PlayerConnectMessage(NetIncomingMessage incMsg) : base(incMsg)
        {
            PlayerName = incMsg.ReadString();
        }

        public static implicit operator NetOutgoingMessage(PlayerConnectMessage msg)
        {
            NetOutgoingMessage outMessage = NetworkManager.Peer.CreateMessage();
            outMessage.Write((int)MessageType.PlayerConnect);
            outMessage.Write(msg.PlayerName);
            return outMessage;
        }
    }
}