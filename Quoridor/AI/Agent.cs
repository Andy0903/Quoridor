namespace Quoridor.AI
{
    public interface Agent
    {
        Action Behavior(GameStatus status);
    }
}
