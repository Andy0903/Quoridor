using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Quoridor
{
    class PlayerManager
    {
        public static List<Player> myPlayers = new List<Player>();

        public static void Update()
        {
            foreach (Player player in myPlayers)
            {
                player.Update();
            }
        }

        public static void Draw(SpriteBatch aSB)
        {
            foreach (Player player in myPlayers)
            {
                player.Draw(aSB);
            }
        }
    }
}
