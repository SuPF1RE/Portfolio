using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReincarnationManager : MonoBehaviour
{
    public EconomyManager economyManager;
    public GameManager gameManager;
    public UpgradeGeneration UpgradeGeneration;
    public SettlementListGenerator settlementListGenerator;

    public float ReincarnationCost;
    public int ReincarnationCount;

    private InitialData initialData;

    public enemy Enemy;
    public void Reincarnate()
    {
        InitialData initialData = InitialData.GetInstance();
        if (economyManager.CoinCount >= ReincarnationCost)
        {
            ReincarnationCount++;
            economyManager.CoinCount = initialData.CoinCount;
            ReincarnationCost = ReincarnationCost * 2.5f;
            gameManager.Timber.Level = initialData.TimberLevel;
            gameManager.Mine.Level = initialData.MineLevel;
            gameManager.StoneQuarry.Level = initialData.StoneQuarryLevel;
            gameManager.Castle.Level = initialData.CastleLevel;
            gameManager.Timber.Frame.color = Color.red;
            gameManager.StoneQuarry.Frame.color = Color.red;
            gameManager.Mine.Frame.color = Color.red;
            economyManager.BoughtMultipliers = 0;
            economyManager.BoughtMultipliers += ReincarnationCount * 0.10f;
            UpgradeGeneration.UpgradeListGeneration();
            settlementListGenerator.SettlementListGeneration();
        }
    }
    public void RestartGame()
    {
        InitialData initialData = InitialData.GetInstance();
        gameManager.Timber.Frame.color = Color.red;
        gameManager.StoneQuarry.Frame.color = Color.red;
        gameManager.Mine.Frame.color = Color.red;
        ReincarnationCount = 0;
        economyManager.CoinCount = initialData.CoinCount;
        ReincarnationCost = initialData.ReincarnationCost;
        gameManager.Timber.Level = initialData.TimberLevel;
        gameManager.Mine.Level = initialData.MineLevel;
        gameManager.StoneQuarry.Level = initialData.StoneQuarryLevel;
        gameManager.Castle.Level = initialData.CastleLevel;
        Enemy.healthmax = initialData.EnemyHealthmax;
        Enemy.health = initialData.EnemyHeath;
        Enemy.strength = initialData.EnemyStrenth;
        economyManager.BoughtMultipliers = 0;
        UpgradeGeneration.UpgradeListGeneration();
        settlementListGenerator.SettlementListGeneration();
    }
}
