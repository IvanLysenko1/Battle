using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("Health Texts")]
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI enemyHealthText;

    [Header("Health Slider")]
    public Slider playerHealthSlider;
    public Slider enemyHealthSlider;

    [Header("Ability Buttons and Cooldowns")]
    public List<Button> abilityButtons;
    public List<TextMeshProUGUI> abilityCooldownTexts;

    [Header("Restart Button")]
    public Button restartButton;

    [Header("Effect Icons and Timers")]
    public Image playerEffectIcon;
    public TextMeshProUGUI playerEffectTimer;
    public Image enemyEffectIcon;
    public TextMeshProUGUI enemyEffectTimer;

    [Header("Status Effect Sprites")]
    public Sprite barrierSprite;
    public Sprite regenerationSprite;
    public Sprite burnSprite;

    private GameClient client;

    void Start()
    {
        client = FindObjectOfType<GameClient>();
        restartButton.onClick.AddListener(() => client.RestartGame());

        playerHealthSlider.maxValue = 100;
        enemyHealthSlider.maxValue = 100;

    }

    public void UpdateUI(GameState state)
    {
        // Обновление здоровья
        playerHealthText.text = $"Player HP: {state.PlayerUnit.Health}";
        enemyHealthText.text = $"Enemy HP: {state.EnemyUnit.Health}";

        //Обновление Баров
        playerHealthSlider.value = state.PlayerUnit.Health;
        enemyHealthSlider.value = state.EnemyUnit.Health;



        // Обновление способностей
        for (int i = 0; i < state.PlayerUnit.Abilities.Count; i++)
        {
            var ability = state.PlayerUnit.Abilities[i];
            abilityButtons[i].interactable = ability.IsAvailable();
            abilityCooldownTexts[i].text = ability.CurrentCooldown > 0 ? ability.CurrentCooldown.ToString() : "";
            int index = i; // Для захвата в лямбду
            abilityButtons[i].onClick.RemoveAllListeners();
            abilityButtons[i].onClick.AddListener(() => client.SendPlayerAction(ability.Type));
        }

        // Обновление эффектов
        UpdateEffectUI(state.PlayerUnit, playerEffectIcon, playerEffectTimer);
        UpdateEffectUI(state.EnemyUnit, enemyEffectIcon, enemyEffectTimer);
    }

    private void UpdateEffectUI(Unit unit, Image icon, TextMeshProUGUI timerText)
    {
        if (unit.ActiveEffects.Count > 0)
        {
            var effect = unit.ActiveEffects[0];
            icon.enabled = true;
            icon.sprite = GetSpriteForEffect(effect.Type);
            timerText.text = effect.RemainingTurns.ToString();
        }
        else
        {
            icon.enabled = false;
            timerText.text = "";
        }
    }

    private Sprite GetSpriteForEffect(StatusEffectType effectType)
    {
        switch (effectType)
        {
            case StatusEffectType.Barrier:
                return barrierSprite;
            case StatusEffectType.Regeneration:
                return regenerationSprite;
            case StatusEffectType.Burn:
                return burnSprite;
            default:
                return null;
        }
    }

    //private void ShowGameOver(string winner)
    //{
    //    gameOverPanel.SetActive(true);
    //    if (winner == "Player")
    //    {
    //        gameOverText.text = "You Win!";
    //    }
    //    else if (winner == "Enemy")
    //    {
    //        gameOverText.text = "You Lose!";
    //    }
    //    else
    //    {
    //        gameOverText.text = "Draw!";
    //    }
    //}

    //private void HideGameOver()
    //{
    //    gameOverPanel.SetActive(false);
    //}
}
