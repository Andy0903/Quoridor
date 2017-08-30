using Lidgren.Network;

namespace Quoridor
{
    public class PlayerMoveMessage : IMessage
    {
        public int Column { get; private set; }
        public int Row { get; private set; }
        public int OldColumn { get; private set; }
        public int OldRow { get; private set; }
        public int PlayerSlot { get; private set; }

        public PlayerMoveMessage(NetIncomingMessage aIncMessage)
        {
            Column = aIncMessage.ReadInt32();
            Row = aIncMessage.ReadInt32();
            OldColumn = aIncMessage.ReadInt32();
            OldRow = aIncMessage.ReadInt32();
            PlayerSlot = aIncMessage.ReadInt32();
        }

        public void Send()
        {
            NetOutgoingMessage outMessage = NetworkManager.Client.CreateMessage();
            outMessage.Write((int)MessageType.PlayerMove);
            outMessage.Write(Column);
            outMessage.Write(Row);
            outMessage.Write(OldColumn);
            outMessage.Write(OldRow);
            outMessage.Write(PlayerSlot);
            NetworkManager.Client.SendMessage(outMessage, NetDeliveryMethod.ReliableOrdered);
        }
    }
}