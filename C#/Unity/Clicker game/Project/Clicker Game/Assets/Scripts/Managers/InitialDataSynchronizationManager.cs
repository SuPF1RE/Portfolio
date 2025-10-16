using UnityEngine;

public class InitialDataSynchronizationManager : MonoBehaviour
{
    [SerializeField]private InitialData InitialData;
    public GameManager gameManager;
    public EconomyManager economyManager;
    public ReincarnationManager reincarnationManager;
    public void Awake()
    {
        SyncData();
    }
    public void SyncData()
    {
        gameManager.Castle.Level = InitialData.CastleLevel;
        economyManager.CoinCount = InitialData.CoinCount;
        gameManager.Timber.Level = InitialData.TimberLevel;
        gameManager.StoneQuarry.Level = InitialData.StoneQuarryLevel;
        gameManager.Mine.Level = InitialData.MineLevel;
        reincarnationManager.ReincarnationCost = InitialData.ReincarnationCost;
        reincarnationManager.Enemy.strength = InitialData.EnemyStrenth;
        reincarnationManager.Enemy.health = InitialData.EnemyHeath;
        reincarnationManager.Enemy.healthmax = InitialData.EnemyHeath;
        economyManager.GrowthRate = InitialData.GrowthRate;
    }
}
