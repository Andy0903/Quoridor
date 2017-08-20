using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor
{
    class NarrowTile : Tile
    {
        public NarrowTile(Vector2 aPosition) : base(16, 32, aPosition)
        {
        }
    }
}
