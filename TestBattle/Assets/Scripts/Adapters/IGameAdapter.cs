public interface IGameAdapter 
{
    void SendPlayerAction(AbilityType ability);
    void OnGameStateUpdated(GameState state);
}
