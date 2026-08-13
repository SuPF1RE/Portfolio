using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    public enemy Enemy { get; private set; }
    public float Progress;
    public bool IsActive;
    public GameObject slide;
    public Slider battleSlider;
    GameManager gameManager;
    BigNumberNormalizer NumberNormalizer;
    public int battleDuration;
    private int battleTimer;
    private int TimeLeft;
    public void Init(enemy target)
    {
        Enemy = target;
        IsActive = true;
        Progress = 50f;
        battleTimer = 0;
        TimeLeft = 60;
    }
    public void UpdateBattle()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        NumberNormalizer = FindFirstObjectByType<BigNumberNormalizer>();
        TextMeshProUGUI text = slide.GetComponentInChildren<TextMeshProUGUI>();
        
        battleTimer += 1;
        TimeLeft = battleDuration - battleTimer;
        text.SetText("Time left: " + TimeLeft);
        float roll = gameManager.Roll();
        float strengthCheck = gameManager.CheckStrength(Enemy.strength);

        if (roll < strengthCheck)
        {
            Progress += 5f;
        }
        else
        {
            Progress -= 5f;
        }
        Progress = Mathf.Clamp(Progress, 0f, 100f);
        battleSlider.value = Progress;
        if (Progress <= 0f)
        {
            if (Enemy.health <= 0)
            {
                Enemy.ButtonText.SetText("Captured");
                gameManager.addedStrength = +Enemy.strength * 0.05f;
                Enemy.AttackButton.interactable = false;
                Destroy(slide);
            }
            else
            {
                Enemy.ButtonText.SetText("Defeat");
                Enemy.AttackButton.interactable = true;
                Destroy(slide);
            }
        }
        else if (Progress >= 100f)
        {
            Enemy.health = 0;
            Enemy.healthText.SetText(NumberNormalizer.Normalize(Enemy.health));
            Enemy.ButtonText.SetText("Captured");
            gameManager.addedStrength = +Enemy.strength * 0.05f;
            Destroy(slide);
        }

        if (battleTimer >= battleDuration)
        {
            Enemy.health -= gameManager.Castle.Strength;
            Enemy.healthText.SetText(NumberNormalizer.Normalize(Enemy.health));
            if (Enemy.health <= 0)
            {
                Enemy.ButtonText.SetText("Captured");
                Enemy.AttackButton.interactable = false;
                gameManager.addedStrength += Enemy.strength * 0.05f;
                Destroy(slide);
            }
            else
            {
                Enemy.ButtonText.SetText("Time is up!");
                Enemy.AttackButton.interactable = true;
                Destroy(slide);
            }
        }
    }
}
