using Lidgren.Network;

namespace Quoridor
{
    public interface IMessage
    {
        void Send();
    }
}