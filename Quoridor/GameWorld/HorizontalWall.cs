using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor
{
    class HorizontalWall : GraphicalObject
    {
        public HorizontalWall(Vector2 position, Color color) : base(144, 16, "HorizontalWall")
        {
            Position = position;
            Color = color;
        }
    }
}
