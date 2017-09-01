using Lidgren.Network;

namespace QuoridorNetwork
{
    public class ActionRejectMessage : Message
    {
        public ActionRejectMessage()
        {
        }

        public ActionRejectMessage(NetIncomingMessage incMsg) : base(incMsg)
        {
        }

        public static implicit operator NetOutgoingMessage(ActionRejectMessage msg)
        {
            NetOutgoingMessage outMessage = NetworkManager.Peer.CreateMessage();
            outMessage.Write((int)MessageType.ActionReject);
            return outMessage;
        }
    }
}