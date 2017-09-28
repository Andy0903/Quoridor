using Microsoft.Xna.Framework.Input;

namespace Quoridor
{
    static class KeyboardUtility
    {
        static KeyboardState oldState;
        static KeyboardState newState;

        static public bool WasClicked(Keys aKey)
        {
            return oldState.IsKeyUp(aKey) && newState.IsKeyDown(aKey);
        }

        static public bool IsHeldDown(Keys aKey)
        {
            return oldState.IsKeyDown(aKey) && newState.IsKeyDown(aKey);
        }

        static public void Update()
        {
            oldState = newState;
            newState = Keyboard.GetState();
        }
    }

}
