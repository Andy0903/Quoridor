using Lidgren.Network;

namespace Quoridor
{
    public enum TileType
    {
        NarrowVertical,
        NarrowHorizontal,
        Wide
    }

    public class PlaceWallMessage : IMessage
    {
        public TileType WallType { get; private set; }
        public int Column { get; private set; }
        public int Row { get; private set; }
        public int PlayerSlot { get; private set; }

        public PlaceWallMessage(NetIncomingMessage aIncMessage)
        {
            WallType = (TileType)aIncMessage.ReadInt32();
            Column = aIncMessage.ReadInt32();
            Row = aIncMessage.ReadInt32();
            PlayerSlot = aIncMessage.ReadInt32();
        }

        public void Send()
        {
            NetOutgoingMessage outMessage = NetworkManager.Client.CreateMessage();
            outMessage.Write((int)MessageType.PlaceWall);
            outMessage.Write((int)WallType);
            outMessage.Write(Column);
            outMessage.Write(Row);
            outMessage.Write(PlayerSlot);
            NetworkManager.Client.SendMessage(outMessage, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
