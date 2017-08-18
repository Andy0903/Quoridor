using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
