using Lidgren.Network;

namespace QuoridorNetwork
{
    public class ActionRejectMessage : Message
    {
        public ActionRejectMessage()
        {
        }

        public ActionRejectMessage(NetIncomingMessage aIncMessage) : base(aIncMessage)
        {
        }

        public static implicit operator NetOutgoingMessage(ActionRejectMessage aMessage)
        {
            NetOutgoingMessage outMessage = NetworkManager.Peer.CreateMessage();
            outMessage.Write((int)MessageType.ActionReject);
            return outMessage;
        }
    }
}