using Microsoft.Xna.Framework;

namespace Quoridor
{
    public class Player : GraphicalObject
    {
        public string Name { get; private set; }

        public int NumberOfWalls { get; set; }

        private int slot;
        public int Slot //Which player you are, 1st, 2nd, 3rd, 4th.. (0-3)
        {
            get { return slot; }
            set
            {
                switch (value)
                {
                    case 0:
                        Color = Color.Red;
                        Position = Program.Game.GameBoard.GetPositionOfTile(4, 8);
                        break;
                    case 1:
                        Color = Color.Blue;
                        Position = Program.Game.GameBoard.GetPositionOfTile(4, 0);
                        break;
                    case 2:
                        Color = Color.Green;
                        Position = Program.Game.GameBoard.GetPositionOfTile(0, 4);
                        break;
                    case 3:
                        Color = Color.Yellow;
                        Position = Program.Game.GameBoard.GetPositionOfTile(8, 4);
                        break;
                    default:
                        Color = Color.White;
                        Position = new Vector2(0, 0);
                        break;
                }

                slot = value;
            }
        } 


        public Player(string aName) : base(32, 32, "Player")
        {
            Name = aName;
            Slot = -1;
            NumberOfWalls = -1;
        }

        public void Update()
        {
            NetworkManager.myOutMsg = NetworkManager.myClient.CreateMessage();
            NetworkManager.myOutMsg.Write("Update");
            NetworkManager.myOutMsg.Write(Name);
            NetworkManager.myClient.SendMessage(NetworkManager.myOutMsg, Lidgren.Network.NetDeliveryMethod.Unreliable);
        }

        public void PlaceWall(Tile.TileType aType, int aColumn, int aRow)
        {
            NetworkManager.myOutMsg = NetworkManager.myClient.CreateMessage();
            NetworkManager.myOutMsg.Write("Place Wall");
            // NetworkManager.myOutMsg.Write(Name);
            NetworkManager.myOutMsg.Write((int)aType);
            NetworkManager.myOutMsg.Write(aColumn);
            NetworkManager.myOutMsg.Write(aRow);
            NetworkManager.myClient.SendMessage(NetworkManager.myOutMsg, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
        }
    }
}
