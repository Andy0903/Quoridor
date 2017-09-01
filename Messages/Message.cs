using Lidgren.Network;

namespace QuoridorNetwork
{
    public abstract class Message
    {
        public NetConnection Sender { get; private set; }

        public Message()
        {
        }

        public Message(NetIncomingMessage incMsg)
        {
            Sender = incMsg.SenderConnection;
        }
    }
}
