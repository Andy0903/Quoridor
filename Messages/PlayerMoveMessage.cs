using Lidgren.Network;

namespace QuoridorNetwork
{
    public class PlayerMoveMessage : Message
    {
        public int Column { get; private set; }
        public int Row { get; private set; }
        public int PlayerSlot { get; private set; }

        public PlayerMoveMessage(int aColumn, int aRow, int aPlayerSlot)
        {
            Column = aColumn;
            Row = aRow;
            PlayerSlot = aPlayerSlot;
        }

        public PlayerMoveMessage(NetIncomingMessage aIncMessage) : base(aIncMessage)
        {
            Column = aIncMessage.ReadInt32();
            Row = aIncMessage.ReadInt32();
            PlayerSlot = aIncMessage.ReadInt32();
        }

        public static implicit operator NetOutgoingMessage(PlayerMoveMessage aMessage)
        {
            NetOutgoingMessage outMessage = NetworkManager.Peer.CreateMessage();
            outMessage.Write((int)MessageType.PlayerMove);
            outMessage.Write(aMessage.Column);
            outMessage.Write(aMessage.Row);
            outMessage.Write(aMessage.PlayerSlot);
            return outMessage;
        }
    }
}