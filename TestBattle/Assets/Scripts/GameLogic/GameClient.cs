using UnityEngine;

public class GameClient : MonoBehaviour, IGameAdapter
{
    public GameUI ui;
    private GameServer server;
    
    void Start()
    {
        server = new GameServer(this);
    }

    public void SendPlayerAction(AbilityType ability)
    {
        server.PlayerAction(ability);
    }

    public void OnGameStateUpdated(GameState state)
    {
        ui.UpdateUI(state);
    }

    public void RestartGame()
    {
        server.RestartGame();
    }
}
