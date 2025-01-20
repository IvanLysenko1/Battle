using System.Collections;
using System.Linq;
using UnityEngine;

public class GameServer 
{
    private Unit player;
    private Unit enemy;
    private IGameAdapter adapter;
    private GameState state;

    public GameServer(IGameAdapter gameAdapter)
    {
        adapter = gameAdapter;
        InitializeGame();
    }

    private void InitializeGame()
    {
        player = new Unit("Player");
        enemy = new Unit("Enemy");
        state = new GameState
        {
            PlayerUnit = player,
            EnemyUnit = enemy,
            IsPlayerTurn = true,
            GameOver = false
        };
        adapter.OnGameStateUpdated(state);
    }

    public void PlayerAction(AbilityType ability)
    {
        if (!state.IsPlayerTurn || state.GameOver)
            return;

        ExecuteAbility(player, enemy, ability);
        NextTurn();
    }

    private void EnemyAction()
    {
        if (state.GameOver)
            return;

        // Простой ИИ: выбирает случайную доступную способность
        var availableAbilities = enemy.Abilities.FindAll(a => a.IsAvailable());
        if (availableAbilities.Count > 0)
        {
            var ability = availableAbilities[Random.Range(0, availableAbilities.Count)].Type;
            ExecuteAbility(enemy, player, ability);
        }
        NextTurn();
    }

    private void ExecuteAbility(Unit actor, Unit target, AbilityType abilityType)
    {
        var ability = actor.Abilities.Find(a => a.Type == abilityType);
        if (ability == null || !ability.IsAvailable())
            return;

        switch (abilityType)
        {
            case AbilityType.Attack:
                ApplyDamage(target, ability.Damage);
                break;
            case AbilityType.Barrier:
                actor.Barrier += 5;
                actor.ApplyEffect(new StatusEffect(StatusEffectType.Barrier, ability.Duration));
                break;
            case AbilityType.Regeneration:
                actor.ApplyEffect(new StatusEffect(StatusEffectType.Regeneration, ability.Duration));
                break;
            case AbilityType.Fireball:
                ApplyDamage(target, ability.Damage);
                target.ApplyEffect(new StatusEffect(StatusEffectType.Burn, ability.Duration));
                break;
            case AbilityType.Cleanse:
                actor.RemoveEffect(StatusEffectType.Burn);
                break;
        }

        ability.CurrentCooldown = ability.Cooldown;
    }

    private void ApplyDamage(Unit target, int damage)
    {
        // Учитываем барьер
        if (target.Barrier > 0)
        {
            int remainingDamage = damage - target.Barrier;
            target.Barrier = Mathf.Max(target.Barrier - damage, 0);
            if (remainingDamage > 0)
                target.Health -= remainingDamage;
        }
        else
        {
            target.Health -= damage;
        }

        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (player.Health <= 0)
        {
            state.GameOver = true;
            state.Winner = "Enemy";
        }
        else if (enemy.Health <= 0)
        {
            state.GameOver = true;
            state.Winner = "Player";
        }
    }

    private void NextTurn()
    {
        UpdateEffects(player);
        UpdateEffects(enemy);
        UpdateCooldowns(player);
        UpdateCooldowns(enemy);

        if (state.GameOver)
        {
            adapter.OnGameStateUpdated(state);
            return;
        }

        state.IsPlayerTurn = !state.IsPlayerTurn;
        adapter.OnGameStateUpdated(state);

        if (!state.IsPlayerTurn)
        {
            // Задержка для имитации хода ИИ
            CoroutineRunner.Instance.StartCoroutine(EnemyTurnCoroutine());
        }
    }

    private IEnumerator EnemyTurnCoroutine()
    {
        yield return new WaitForSeconds(1f); // Задержка 1 секунда
        EnemyAction();
    }

    private void UpdateEffects(Unit unit)
    {
        foreach (var effect in unit.ActiveEffects.ToList())
        {
            switch (effect.Type)
            {
                case StatusEffectType.Regeneration:
                    unit.Health += 2;
                    break;
                case StatusEffectType.Burn:
                    ApplyDamage(unit, 1);
                    break;
            }

            effect.RemainingTurns--;
            if (effect.RemainingTurns <= 0)
            {
                if (effect.Type == StatusEffectType.Barrier)
                {
                    unit.Barrier -= 5;
                    unit.Barrier = Mathf.Max(unit.Barrier, 0);
                }
                unit.ActiveEffects.Remove(effect);
            }
        }
    }

    private void UpdateCooldowns(Unit unit)
    {
        foreach (var ability in unit.Abilities)
        {
            if (ability.CurrentCooldown > 0)
                ability.CurrentCooldown--;
        }
    }

    public void RestartGame()
    {
        InitializeGame();
    }
}
