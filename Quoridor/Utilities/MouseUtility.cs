using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Quoridor
{
    static class MouseUtility
    {
        static MouseState newState;
        static MouseState oldState;

        static public bool WasLeftClicked
        {
            get { return oldState.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed; }
        }

        static public bool WasRightClicked
        {
            get { return oldState.RightButton == ButtonState.Released && newState.RightButton == ButtonState.Pressed; }
        }

        static public Point Position
        {
            get { return newState.Position; }
        }

        static public void Update()
        {
            oldState = newState;
            newState = Mouse.GetState();
        }
    }
}
