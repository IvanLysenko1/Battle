public class StatusEffect 
{
    public StatusEffectType Type { get; private set; }
    public int RemainingTurns { get; set; }

    public StatusEffect(StatusEffectType type, int duration)
    {
        Type = type;
        RemainingTurns = duration;
    }
}
