using System;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float productionInterval = 1f;
    public BuildingsAppearenceManager buildingsAppearenceManager;
    private float productionTimer;
    public float CoinCount;

    public float BoughtMultipliers;
    public float TotalProduction;

    public float GrowthRate;
    private void Update()
    {
        productionTimer += Time.deltaTime;
        if (productionTimer >= productionInterval)
        {
            UpdateProduction();
            productionTimer = 0f;
        }
    }

    public void UpdateProduction()
    {
        if (BoughtMultipliers != 0)
        {
            gameManager.Timber.ProductionSum = gameManager.Timber.ProductionBase * gameManager.Timber.Level * (1 + BoughtMultipliers);
            gameManager.Mine.ProductionSum = gameManager.Mine.ProductionBase * gameManager.Mine.Level * (1 + BoughtMultipliers);
            gameManager.StoneQuarry.ProductionSum = gameManager.StoneQuarry.ProductionBase * gameManager.StoneQuarry.Level * (1 + BoughtMultipliers);
        }
        else
        {
            gameManager.Timber.ProductionSum = gameManager.Timber.ProductionBase * gameManager.Timber.Level;
            gameManager.Mine.ProductionSum = gameManager.Mine.ProductionBase * gameManager.Mine.Level;
            gameManager.StoneQuarry.ProductionSum = gameManager.StoneQuarry.ProductionBase * gameManager.StoneQuarry.Level;
        }
        TotalProduction = gameManager.StoneQuarry.ProductionSum + gameManager.Mine.ProductionSum + gameManager.Timber.ProductionSum + gameManager.SettlementProduction;
        CoinCount += TotalProduction;
    }

    public bool BuyLevel(BuildingsPrototypes building, int levelAdd)
    {
        if (CoinCount >= building.NextCost)
        {
            CoinCount -= building.NextCost;
            building.Level += levelAdd;
            building.NextCost = MathF.Round(building.BaseCost * (Mathf.Pow(GrowthRate, building.Level)), 2);
            buildingsAppearenceManager.UpdateAppearance();
            return true; // покупка прошла   
        }
        return false; // не хватило монет
    }
    public float MultipleLevelUp(int LevelsToBuy, float BaseCost, int Level, float totalcost)
    {
        totalcost = 0f;
        for (int i = 1; i <= LevelsToBuy; i++)
        {
            totalcost += MathF.Round(BaseCost * (MathF.Pow(GrowthRate, Level + i)), 2);
        }
        return totalcost;
    }
    public void GiveCoin(int Level) { CoinCount += Level; }
}
