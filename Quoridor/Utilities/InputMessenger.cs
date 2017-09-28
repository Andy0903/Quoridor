using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor
{
    public static class InputMessenger
    {
        public static void Update()
        {
            KeyboardUtility.Update();
            MouseUtility.Update();
        }

        public static bool PressedW => KeyboardUtility.WasClicked(Keys.W);
        public static bool PressedA => KeyboardUtility.WasClicked(Keys.A);
        public static bool PressedS => KeyboardUtility.WasClicked(Keys.S);
        public static bool PressedD => KeyboardUtility.WasClicked(Keys.D);

        public static Quoridor.AI.Point MousePosition => MouseUtility.Position;
        public static bool PressedLMB => MouseUtility.WasLeftClicked;
    }
}
