using Lidgren.Network;

namespace Quoridor
{
    public class PlayerConnectMessage : IMessage
    {
        public string PlayerName { get; private set; }

        public PlayerConnectMessage(string aPlayerName)
        {
            PlayerName = aPlayerName;
        }

        public PlayerConnectMessage(NetIncomingMessage aIncMessage)
        {
            PlayerName = aIncMessage.ReadString();
        }

        public void Send()
        {
            NetOutgoingMessage outMessage = NetworkManager.Client.CreateMessage();
            outMessage.Write((int)MessageType.PlayerConnect);
            outMessage.Write(PlayerName);
            NetworkManager.Client.SendMessage(outMessage, NetDeliveryMethod.ReliableOrdered);
        }
    }
}