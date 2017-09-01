using Lidgren.Network;

namespace QuoridorNetwork
{
    public class PlayerWonMessage : Message
    {
        public int PlayerSlot { get; private set; }

        public PlayerWonMessage(int playerSlot)
        {
            PlayerSlot = playerSlot;
        }

        public PlayerWonMessage(NetIncomingMessage incMsg) : base(incMsg)
        {
            PlayerSlot = incMsg.ReadInt32();
        }

        public static implicit operator NetOutgoingMessage(PlayerWonMessage msg)
        {
            NetOutgoingMessage outMessage = NetworkManager.Peer.CreateMessage();
            outMessage.Write((int)MessageType.PlayerWon);
            outMessage.Write(msg.PlayerSlot);
            return outMessage;
        }
    }
}