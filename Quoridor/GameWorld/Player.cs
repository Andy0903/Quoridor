using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quoridor
{
    public class Player
    {
        private Texture2D myTexture;
        public string Name { get; private set; }
        public Color Color { get; set; }
        public int Slot { get; set; } //Which player you are, 1st, 2nd, 3rd, 4th.. (0-3)


        public Player(string aName)
        {
            Name = aName;
            myTexture = Program.Game.Content.Load<Texture2D>("Player");
            Color = Color.Green;
            Slot = -1;
        }

        public void Update()
        {
            NetworkManager.myOutMsg = NetworkManager.myClient.CreateMessage();
            NetworkManager.myOutMsg.Write("Update");
            NetworkManager.myOutMsg.Write(Name);
            NetworkManager.myClient.SendMessage(NetworkManager.myOutMsg, Lidgren.Network.NetDeliveryMethod.Unreliable);
        }
    }
}
