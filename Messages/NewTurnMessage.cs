using Lidgren.Network;

namespace QuoridorNetwork
{
    public class NewTurnMessage : Message
    {
        public int PlayerSlot { get; private set; }

        public NewTurnMessage(int aPlayerSlot)
        {
            PlayerSlot = aPlayerSlot;
        }

        public NewTurnMessage(NetIncomingMessage aIncMessage) : base(aIncMessage)
        {
            PlayerSlot = aIncMessage.ReadInt32();
        }

        public static implicit operator NetOutgoingMessage(NewTurnMessage aMessage)
        {
            NetOutgoingMessage outMessage = NetworkManager.Peer.CreateMessage();
            outMessage.Write((int)MessageType.NewTurn);
            outMessage.Write(aMessage.PlayerSlot);
            return aMessage;
        }
    }
}