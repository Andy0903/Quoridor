using Lidgren.Network;

namespace QuoridorNetwork
{
    public class PlayerConnectMessage : Message
    {
        public string PlayerName { get; private set; }

        public PlayerConnectMessage(string aPlayerName)
        {
            PlayerName = aPlayerName;
        }

        public PlayerConnectMessage(NetIncomingMessage aIncMessage) : base(aIncMessage)
        {
            PlayerName = aIncMessage.ReadString();
        }

        public static implicit operator NetOutgoingMessage(PlayerConnectMessage aMessage)
        {
            NetOutgoingMessage outMessage = NetworkManager.Peer.CreateMessage();
            outMessage.Write((int)MessageType.PlayerConnect);
            outMessage.Write(aMessage.PlayerName);
            return outMessage;
        }
    }
}