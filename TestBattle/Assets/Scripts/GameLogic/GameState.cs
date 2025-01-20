public class GameState 
{
    public Unit PlayerUnit { get; set; }
    public Unit EnemyUnit { get; set; }
    public bool IsPlayerTurn { get; set; }
    public bool GameOver { get; set; }
    public string Winner { get; set; }
}
