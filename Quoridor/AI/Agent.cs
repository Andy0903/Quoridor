namespace Quoridor.AI
{
    public abstract class Agent
    {
        public abstract Action DoAction(GameData status);

        public void Start()
        {
            using (Game1 game = new Game1(this))
                game.Run();
        }
    }
}
