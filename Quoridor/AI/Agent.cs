namespace Quoridor.AI
{
    public interface Agent
    {
        Action DoAction(GameData status);
        Action RedoAction(GameData status);
    }
}
