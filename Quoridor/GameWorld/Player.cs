using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quoridor
{
    public class Player
    {
        public string myName;
        private Texture2D myTexture;
        public Color Color { get; set; }


        public Player(string aName)
        {
            myName = aName;
            myTexture = Program.Game.Content.Load<Texture2D>("Player");
            Color = Color.Green;
        }

        public void Update()
        {
            NetworkManager.myOutMsg = NetworkManager.myClient.CreateMessage();
            NetworkManager.myOutMsg.Write("Update");
            NetworkManager.myOutMsg.Write(myName);
            NetworkManager.myClient.SendMessage(NetworkManager.myOutMsg, Lidgren.Network.NetDeliveryMethod.Unreliable);
        }
    }
}
