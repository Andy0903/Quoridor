using Lidgren.Network;

namespace QuoridorNetwork
{
    public class NewTurnMessage : Message
    {
        public int PlayerSlot { get; private set; }

        public NewTurnMessage(int playerSlot)
        {
            PlayerSlot = playerSlot;
        }

        public NewTurnMessage(NetIncomingMessage incMsg) : base(incMsg)
        {
            PlayerSlot = incMsg.ReadInt32();
        }

        public static implicit operator NetOutgoingMessage(NewTurnMessage msg)
        {
            NetOutgoingMessage outMessage = NetworkManager.Peer.CreateMessage();
            outMessage.Write((int)MessageType.NewTurn);
            outMessage.Write(msg.PlayerSlot);
            return outMessage;
        }
    }
}