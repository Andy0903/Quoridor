using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quoridor
{
    public class Player
    {
        private Texture2D myTexture;
        public string Name { get; private set; }
        public Color Color { get; set; }
        public Vector2 Position { get; private set; }

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
                        Color = Color.LightGoldenrodYellow;
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


        public Player(string aName)
        {
            Name = aName;
            myTexture = Program.Game.Content.Load<Texture2D>("Player");
            Slot = -1;
        }

        public void Update()
        {
            NetworkManager.myOutMsg = NetworkManager.myClient.CreateMessage();
            NetworkManager.myOutMsg.Write("Update");
            NetworkManager.myOutMsg.Write(Name);
            NetworkManager.myClient.SendMessage(NetworkManager.myOutMsg, Lidgren.Network.NetDeliveryMethod.Unreliable);
        }

        public void Draw(SpriteBatch aSB)
        {
            aSB.Draw(myTexture, Position, Color);
        }
    }
}
