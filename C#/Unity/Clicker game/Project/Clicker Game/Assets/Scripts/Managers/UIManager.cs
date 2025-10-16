using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public EconomyManager economyManager;
    // all interactions with UI
    public TextMeshProUGUI CoinCountOnScreen;
    public TextMeshProUGUI TimberLevelCountOnScreen;
    public TextMeshProUGUI MineLevelCountOnScreen;
    public TextMeshProUGUI StoneQuarryLevelCountOnScreen;
    public TextMeshProUGUI CastleLevelCountOnScreen;

    public TextMeshProUGUI CastleStrenghtCountOnScreen;
    public TextMeshProUGUI LevelUpXButtonText;

    public TextMeshProUGUI ReincarnationButtonText;
    public TextMeshProUGUI ReincarnationCountText;

    public Button TimberBuyButton;
    public Button CastleBuyButton;
    public Button MineBuyButton;
    public Button StoneQuarryBuyButton;

    public TextMeshProUGUI TimberBuyButtonText;
    public TextMeshProUGUI MineBuyButtonText;
    public TextMeshProUGUI StoneQuarryBuyButtonText;
    public TextMeshProUGUI CastleBuyButtonText;

    public TextMeshProUGUI TimberRPSText;
    public TextMeshProUGUI MineRPSText;
    public TextMeshProUGUI StoneQuarryRPSText;
    public TextMeshProUGUI TotalRPSText;


    public GameObject GameMenu;
    public GameObject GameUI;
    public GameObject CastleMenu;
    public GameObject CastleManageScrollView;
    public GameObject CastleAttackScrollView;
    public GameObject CastleUpgradesScrollView;
    public GameManager gameManager;

    private readonly int[] multipliers = { 1, 5, 10 };
    private int CurrentMultiplierIndex;
    private int Multiplier = 1;

    public BigNumberNormalizer NumberNormalizer;
    public ReincarnationManager reincarnationManager;

    private void Awake()
    {
        LevelUpXButtonText.SetText("x" + Multiplier);
    }
    private void Update()
    {
        UpdateData();
    }
    public void UpdateData()
    {
        if (Multiplier < 5) {gameManager.Timber.NextCost = MathF.Round(gameManager.Timber.BaseCost * (MathF.Pow(economyManager.GrowthRate, gameManager.Timber.Level)), 2); }
        if (Multiplier < 5) {gameManager.Mine.NextCost = MathF.Round(gameManager.Mine.BaseCost * (MathF.Pow(economyManager.GrowthRate, gameManager.Mine.Level)), 2); }
        if (Multiplier < 5) {gameManager.StoneQuarry.NextCost = MathF.Round(gameManager.StoneQuarry.BaseCost * (MathF.Pow(economyManager.GrowthRate, gameManager.StoneQuarry.Level)), 2); }
        if (Multiplier < 5) {gameManager.Castle.NextCost = MathF.Round(gameManager.Castle.BaseCost * (MathF.Pow(economyManager.GrowthRate, gameManager.Castle.Level)), 2); }
        ReincarnationButtonText.SetText("Reincarnate for " + NumberNormalizer.Normalize(reincarnationManager.ReincarnationCost));
        CastleBuyButtonText.SetText("Upgrade for " + NumberNormalizer.Normalize(gameManager.Castle.NextCost));
        TimberLevelCountOnScreen.SetText(gameManager.Timber.Level.ToString());
        CastleLevelCountOnScreen.SetText(gameManager.Castle.Level.ToString());
        MineLevelCountOnScreen.SetText(gameManager.Mine.Level.ToString());
        StoneQuarryLevelCountOnScreen.SetText(gameManager.StoneQuarry.Level.ToString());
        CoinCountOnScreen.SetText(NumberNormalizer.Normalize(economyManager.CoinCount));
        TimberRPSText.SetText(NumberNormalizer.Normalize(gameManager.Timber.ProductionSum));
        StoneQuarryRPSText.SetText(NumberNormalizer.Normalize(gameManager.StoneQuarry.ProductionSum));
        MineRPSText.SetText(NumberNormalizer.Normalize(gameManager.Mine.ProductionSum));
        TotalRPSText.SetText(NumberNormalizer.Normalize(economyManager.TotalProduction));
        CastleStrenghtCountOnScreen.SetText(NumberNormalizer.Normalize(gameManager.Castle.Strength));
        ReincarnationCountText.SetText(reincarnationManager.ReincarnationCount.ToString());
        RefreshBuyButtons();
        UpdateBuyButtonText();
    }
    
    public void OnMenuButtonPress()
    {
        GameMenu.SetActive(true);
        GameUI.SetActive(false);
    }
    public void OnMenuResumeButtonPress()
    {
        GameMenu.SetActive(false);
        GameUI.SetActive(true);
    }
    public void OnMenuExitButtonPress()
    {
        Application.Quit();
    }
    public void OnCastlePress()
    {
        if (!GameMenu.activeInHierarchy) { 
            CastleMenu.SetActive(true);
            GameUI.SetActive(false);
        }
    }
    public void OnCastleExitButtonPress()
    {
        CastleMenu.SetActive(false);
        GameUI.SetActive(true);
    }
    public void OnCastleAttackButtonPress()
    {
        CastleAttackScrollView.SetActive(true);
        CastleManageScrollView.SetActive(false);
        CastleUpgradesScrollView.SetActive(false);
    }
    public void OnCastleCloneUpgradeButtonPress(float UpgradePrice, float Upgrade, GameObject clone, Button buyButton, TextMeshProUGUI ButtonText)
    {
        if (economyManager.CoinCount > UpgradePrice)
        {
            economyManager.CoinCount -= UpgradePrice;
            ButtonText.SetText("Bought");
            buyButton.interactable = false ;
            economyManager.BoughtMultipliers += Upgrade / 100f;
        }
        else 
        {
            ButtonText.SetText("No enought coins");
        }
    }
    public void OnCastleManageButtonPress()
    {
        CastleAttackScrollView.SetActive(false);
        CastleManageScrollView.SetActive(true);
        CastleUpgradesScrollView.SetActive(false);
    }
    public void OnCastleUpgradeButtonPress()
    {
        CastleAttackScrollView.SetActive(false);
        CastleManageScrollView.SetActive(false);
        CastleUpgradesScrollView.SetActive(true);
    }
    public void OnTimberPress() { economyManager.GiveCoin(gameManager.Timber.Level); }
    public void OnMinePress() { economyManager.GiveCoin(gameManager.Mine.Level); }
    public void OnStoneQuarryPress() { economyManager.GiveCoin(gameManager.StoneQuarry.Level); }

    public void BuyCastleButtonClick()
    {
        economyManager.BuyLevel(gameManager.Castle ,Multiplier);
        CastleLevelCountOnScreen.SetText(gameManager.Castle.Level.ToString());
        CastleBuyButtonText.SetText("Buy for " + NumberNormalizer.Normalize(gameManager.Castle.NextCost));
    }

    public void BuyTimberButtonClick() 
    {
        economyManager.BuyLevel(gameManager.Timber, Multiplier);
        TimberLevelCountOnScreen.SetText(gameManager.Timber.Level.ToString());
        TimberBuyButtonText.SetText("Upgrade for "+ NumberNormalizer.Normalize(gameManager.Timber.NextCost));
    }
    public void MultipleLvlUp() 
    {
        CurrentMultiplierIndex = (CurrentMultiplierIndex + 1) % multipliers.Length;
        Multiplier = multipliers[CurrentMultiplierIndex];
        gameManager.Timber.NextCost = economyManager.MultipleLevelUp(Multiplier, gameManager.Timber.BaseCost, gameManager.Timber.Level, gameManager.Timber.NextCost);
        gameManager.StoneQuarry.NextCost = economyManager.MultipleLevelUp(Multiplier, gameManager.StoneQuarry.BaseCost, gameManager.StoneQuarry.Level, gameManager.StoneQuarry.NextCost);
        gameManager.Mine.NextCost = economyManager.MultipleLevelUp(Multiplier, gameManager.Mine.BaseCost, gameManager.Mine.Level, gameManager.Mine.NextCost);
        gameManager.Castle.NextCost = economyManager.MultipleLevelUp(Multiplier, gameManager.Castle.BaseCost, gameManager.Castle.Level, gameManager.Castle.NextCost);
        UpdateMultiplierUI(Multiplier);
    }
    private void UpdateMultiplierUI(int multiplier) 
    {
        LevelUpXButtonText.SetText($"x{multiplier}");
        TimberBuyButtonText.SetText($"Upgrade for {NumberNormalizer.Normalize(gameManager.Timber.NextCost)}");
        MineBuyButtonText.SetText($"Upgrade for {NumberNormalizer.Normalize(gameManager.Mine.NextCost)}");
        StoneQuarryBuyButtonText.SetText($"Upgrade for {NumberNormalizer.Normalize(gameManager.StoneQuarry.NextCost)}");
        CastleBuyButtonText.SetText($"Upgrade for {NumberNormalizer.Normalize(gameManager.Castle.NextCost)}");
    }

    public void BuyMineButtonClick()
    {
        economyManager.BuyLevel(gameManager.Mine, Multiplier);
        MineLevelCountOnScreen.SetText(gameManager.Mine.Level.ToString());
        MineBuyButtonText.SetText("Buy for " + NumberNormalizer.Normalize(gameManager.Mine.NextCost));
    }

    public void BuyStoneQuarryButtonClick()
    {
        economyManager.BuyLevel(gameManager.StoneQuarry, Multiplier);
        StoneQuarryLevelCountOnScreen.SetText(gameManager.StoneQuarry.Level.ToString());
        StoneQuarryBuyButtonText.SetText("Buy for " + NumberNormalizer.Normalize(gameManager.StoneQuarry.NextCost));
    }
    private void RefreshBuyButtons()
    {
        UpdateButtonInteractivity(TimberBuyButton, gameManager.Timber.NextCost);
        UpdateButtonInteractivity(CastleBuyButton, gameManager.Castle.NextCost);
        UpdateButtonInteractivity(MineBuyButton, gameManager.Mine.NextCost);
        UpdateButtonInteractivity(StoneQuarryBuyButton, gameManager.StoneQuarry.NextCost);
    }
    private void UpdateButtonInteractivity(Button button, float nextCost)
    {
        bool canAfford = economyManager.CoinCount > nextCost;
        button.interactable = canAfford;
    }
    public void Reincarnation() 
    {
        reincarnationManager.Reincarnate();
        UpdateData();
    }
    public void UpdateBuyButtonText()
    {
        if (gameManager.Timber.Level != 0)
        {
            TimberBuyButtonText.SetText("Upgrade for " + NumberNormalizer.Normalize(gameManager.Timber.NextCost));
        }
        else
        {
            TimberBuyButtonText.SetText("Buy for " + NumberNormalizer.Normalize(gameManager.Timber.NextCost));
        }
        if (gameManager.StoneQuarry.Level != 0)
        {
            StoneQuarryBuyButtonText.SetText("Upgrade for " + NumberNormalizer.Normalize(gameManager.StoneQuarry.NextCost));
        }
        else
        {
            StoneQuarryBuyButtonText.SetText("Buy for " + NumberNormalizer.Normalize(gameManager.StoneQuarry.NextCost));
        }
        if (gameManager.Mine.Level != 0)
        {
            MineBuyButtonText.SetText("Upgrade for " + NumberNormalizer.Normalize(gameManager.Mine.NextCost));
        }
        else
        {
            MineBuyButtonText.SetText("Buy for " + NumberNormalizer.Normalize(gameManager.Mine.NextCost));
        }
    }
    public void Restart() 
    {
        reincarnationManager.RestartGame();
    }
    public void CloseNotification(GameObject notification) 
    {
        notification.SetActive(false);
    }
}