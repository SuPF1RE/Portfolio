[System.Serializable]
public class InitialData
{
    public float CoinCount;
    public int TimberLevel;
    public float TimberProductionSum;
    public int StoneQuarryLevel;
    public float StoneQuarryProductionSum;
    public int MineLevel;
    public float MineProductionSum;
    public float TotalProduction;
    public float SettlementProduction;
    public float BoughtMultipliers;
    public float ReincarnationCost;
    public int ReincarnationCount;
    public int CastleLevel;
    public float CastleStrength;
    public float GrowthRate;
    public float EnemyStrenth;
    public float EnemyHeath;
    public float EnemyHealthmax;
    private static InitialData instance;
    public InitialData()
    {
        this.CoinCount = 5;
        this.TimberLevel = 0;
        this.TimberProductionSum = 0;

        this.StoneQuarryLevel = 0;
        this.StoneQuarryProductionSum = 0;

        this.MineLevel = 0;
        this.MineProductionSum = 0;

        this.TotalProduction = 0;
        this.SettlementProduction = 0;
        this.BoughtMultipliers = 0;
        this.ReincarnationCost = 1000000;
        this.ReincarnationCount = 0;
        this.CastleStrength = 0;
        this.CastleLevel = 1;
        this.EnemyStrenth = 10;
        this.EnemyHeath = 100;
        this.EnemyHealthmax = this.EnemyHeath;

        this.GrowthRate = 1.47f;
    }
    public static InitialData GetInstance()
    {
        if (instance == null)
        {
            instance = new InitialData();
        }
        return instance;
    }
}
