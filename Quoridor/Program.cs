using System;

namespace Quoridor
{
#if WINDOWS || LINUX

    public static class Program
    {
        public static Game1 Game { get; private set; }
        
        [STAThread]
        static void Main()
        {
            using (Game = new Game1(new AI.DeterministicAgent()))
                Game.Run();
        }
    }
#endif
}
