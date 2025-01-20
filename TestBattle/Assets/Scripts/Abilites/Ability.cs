public class Ability 
{
    public AbilityType Type { get; private set; }
    public int Damage { get; private set; }
    public int HealPerTurn { get; private set; }
    public int Duration { get; private set; }
    public int Cooldown { get; private set; }
    public int CurrentCooldown { get; set; }

    public Ability(AbilityType type)
    {
        Type = type;
        switch (type)
        {
            case AbilityType.Attack:
                Damage = 8;
                break;
            case AbilityType.Barrier:
                Duration = 2;
                Cooldown = 4;
                break;
            case AbilityType.Regeneration:
                HealPerTurn = 2;
                Duration = 3;
                Cooldown = 5;
                break;
            case AbilityType.Fireball:
                Damage = 5;
                Duration = 5;
                Cooldown = 6;
                break;
            case AbilityType.Cleanse:
                Cooldown = 5;
                break;
        }
        CurrentCooldown = 0;
    }

    public bool IsAvailable()
    {
        return CurrentCooldown <= 0;
    }
}
