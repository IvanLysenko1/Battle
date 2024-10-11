using System.Collections.Generic;

public class Unit 
{
    public string Name { get; private set; }
    public int Health { get; set; }
    public List<Ability> Abilities { get; private set; }
    public List<StatusEffect> ActiveEffects { get; private set; }
    public int Barrier { get; set; }

    public Unit(string name)
    {
        Name = name;
        Health = 100; // Начальное здоровье
        Abilities = new List<Ability>
        {
            new Ability(AbilityType.Attack),
            new Ability(AbilityType.Barrier),
            new Ability(AbilityType.Regeneration),
            new Ability(AbilityType.Fireball),
            new Ability(AbilityType.Cleanse)
        };
        ActiveEffects = new List<StatusEffect>();
        Barrier = 0;
    }

    public void ApplyEffect(StatusEffect effect)
    {
        // Проверка на существование эффекта
        var existing = ActiveEffects.Find(e => e.Type == effect.Type);
        if (existing != null)
        {
            existing.RemainingTurns = effect.RemainingTurns;
        }
        else
        {
            ActiveEffects.Add(effect);
        }
    }

    public void RemoveEffect(StatusEffectType type)
    {
        ActiveEffects.RemoveAll(e => e.Type == type);
    }
}
