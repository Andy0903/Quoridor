using Lidgren.Network;

namespace QuoridorNetwork
{
    public class PlayerWonMessage : Message
    {
        public int PlayerSlot { get; private set; }

        public PlayerWonMessage(int aPlayerSlot)
        {
            PlayerSlot = aPlayerSlot;
        }

        public PlayerWonMessage(NetIncomingMessage aIncMessage) : base(aIncMessage)
        {
            PlayerSlot = aIncMessage.ReadInt32();
        }

        public static implicit operator NetOutgoingMessage(PlayerWonMessage aMessage)
        {
            NetOutgoingMessage outMessage = NetworkManager.Peer.CreateMessage();
            outMessage.Write((int)MessageType.PlayerWon);
            outMessage.Write(aMessage.PlayerSlot);
            return outMessage;
        }
    }
}