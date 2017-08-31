using Lidgren.Network;

namespace QuoridorNetwork
{
    public class PlaceWallMessage : Message
    {
        public TileType WallType { get; private set; }
        public int Column { get; private set; }
        public int Row { get; private set; }
        public int PlayerSlot { get; private set; }

        public PlaceWallMessage(TileType aWallType, int aColumn, int aRow, int aPlayerSlot)
        {
            WallType = aWallType;
            Column = aColumn;
            Row = aRow;
            PlayerSlot = aPlayerSlot;
        }

        public PlaceWallMessage(NetIncomingMessage aIncMessage) : base(aIncMessage)
        {
            WallType = (TileType)aIncMessage.ReadInt32();
            Column = aIncMessage.ReadInt32();
            Row = aIncMessage.ReadInt32();
            PlayerSlot = aIncMessage.ReadInt32();
        }

        public static implicit operator NetOutgoingMessage(PlaceWallMessage aMessage)
        {
            NetOutgoingMessage outMessage = NetworkManager.Peer.CreateMessage();
            outMessage.Write((int)MessageType.PlaceWall);
            outMessage.Write((int)aMessage.WallType);
            outMessage.Write(aMessage.Column);
            outMessage.Write(aMessage.Row);
            outMessage.Write(aMessage.PlayerSlot);
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
