using Lidgren.Network;

namespace Quoridor
{
    public class PlayerWonMessage : IMessage
    {
        public int PlayerSlot { get; private set; }

        public PlayerWonMessage(int aPlayerSlot)
        {
            PlayerSlot = aPlayerSlot;
        }

        public PlayerWonMessage(NetIncomingMessage aIncMessage)
        {
            PlayerSlot = aIncMessage.ReadInt32();
        }

        public void Send()
        {
            NetOutgoingMessage outMessage = NetworkManager.Client.CreateMessage();
            outMessage.Write((int)MessageType.PlayerWon);
            outMessage.Write(PlayerSlot);
            NetworkManager.Client.SendMessage(outMessage, NetDeliveryMethod.ReliableOrdered);
        }
    }
}