using Lidgren.Network;

namespace QuoridorNetwork
{
    public class PlayerMoveMessage : Message
    {
        public int Column { get; private set; }
        public int Row { get; private set; }
        public int PlayerSlot { get; private set; }

        public PlayerMoveMessage(int column, int row, int playerSlot)
        {
            Column = column;
            Row = row;
            PlayerSlot = playerSlot;
        }

        public PlayerMoveMessage(NetIncomingMessage incMsg) : base(incMsg)
        {
            Column = incMsg.ReadInt32();
            Row = incMsg.ReadInt32();
            PlayerSlot = incMsg.ReadInt32();
        }

        public static implicit operator NetOutgoingMessage(PlayerMoveMessage msg)
        {
            NetOutgoingMessage outMessage = NetworkManager.Peer.CreateMessage();
            outMessage.Write((int)MessageType.PlayerMove);
            outMessage.Write(msg.Column);
            outMessage.Write(msg.Row);
            outMessage.Write(msg.PlayerSlot);
            return outMessage;
        }
    }
}