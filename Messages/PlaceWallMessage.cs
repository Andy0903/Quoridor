using Lidgren.Network;

namespace QuoridorNetwork
{
    public class PlaceWallMessage : Message
    {
        public TileType WallType { get; private set; }
        public int Column { get; private set; }
        public int Row { get; private set; }
        public int PlayerSlot { get; private set; }

        public PlaceWallMessage(TileType wallType, int column, int row, int playerSlot)
        {
            WallType = wallType;
            Column = column;
            Row = row;
            PlayerSlot = playerSlot;
        }

        public PlaceWallMessage(NetIncomingMessage incMsg) : base(incMsg)
        {
            WallType = (TileType)incMsg.ReadInt32();
            Column = incMsg.ReadInt32();
            Row = incMsg.ReadInt32();
            PlayerSlot = incMsg.ReadInt32();
        }

        public static implicit operator NetOutgoingMessage(PlaceWallMessage msg)
        {
            NetOutgoingMessage outMessage = NetworkManager.Peer.CreateMessage();
            outMessage.Write((int)MessageType.PlaceWall);
            outMessage.Write((int)msg.WallType);
            outMessage.Write(msg.Column);
            outMessage.Write(msg.Row);
            outMessage.Write(msg.PlayerSlot);
            return outMessage;
        }
    }

    public enum TileType
    {
        NarrowVertical,
        NarrowHorizontal,
        Wide
    }
}
