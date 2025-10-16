using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public RectTransform Parent;
    private GameManager gameManager;
    private BigNumberNormalizer NumberNormalizer;
    private List<Battle> activeBattles = new List<Battle>();
    public Battle battle;
    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        NumberNormalizer = FindFirstObjectByType<BigNumberNormalizer>();
    }
    public void StartBattle(enemy target)
    {
        _ = StartBattleAsync(target);
    }
    public async Task StartBattleAsync(enemy target)
    {
        Battle slide = Instantiate(battle, Parent);
        slide.Init(target);
        target.AttackButton.interactable = false;
        while (slide.IsActive)
        {
            await Task.Delay(1000);
            slide.UpdateBattle();
        }
    }
}
