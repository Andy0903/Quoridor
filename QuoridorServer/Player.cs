using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorServer
{
    class Player
    {
        public string myName;
        public int myTimeOut;

        public Player(string aName)
        {
            myName = aName;
            myTimeOut = 0;
        }
    }
}
