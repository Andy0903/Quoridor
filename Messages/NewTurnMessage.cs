using Lidgren.Network;

namespace Quoridor
{
    public class NewTurnMessage : IMessage
    {
        public int PlayerSlot { get; private set; }

        public NewTurnMessage(int aPlayerSlot)
        {
            PlayerSlot = aPlayerSlot;
        }

        public NewTurnMessage(NetIncomingMessage aIncMessage)
        {
            PlayerSlot = aIncMessage.ReadInt32();
        }

        public void Send()
        {
            NetOutgoingMessage outMessage = NetworkManager.Client.CreateMessage();
            outMessage.Write((int)MessageType.NewTurn);
            outMessage.Write(PlayerSlot);
            NetworkManager.Client.SendMessage(outMessage, NetDeliveryMethod.ReliableOrdered);
        }
    }
}