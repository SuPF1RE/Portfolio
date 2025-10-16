using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeGeneration : MonoBehaviour
{
    public InitialData initial;
    public GameObject Upgrade;
    public TextMeshProUGUI UpgradeProcent;
    public TextMeshProUGUI UpgradePrice;
    public RectTransform Parent;
    public int UpgradeCount;
    public int UpgradeProcentAdd = 5;
    BigNumberNormalizer NumberNormalizer;
    UIManager UIManager;
    public void Awake()
    {
        UpgradeListGeneration();
    }

    public void UpgradeListGeneration()
    {
        UpgradeProcentAdd = 5;
        float StartingUpgradePrice = 25;
        float StartingUpgrade = 10;
        UIManager = FindAnyObjectByType<UIManager>();
        NumberNormalizer = FindAnyObjectByType<BigNumberNormalizer>();
        foreach (Transform child in Parent) Destroy(child.gameObject);
        for (int i= 1; i < UpgradeCount; i++) 
        {
            if (i != 1) 
            {
                StartingUpgrade = MathF.Round(StartingUpgrade + UpgradeProcentAdd, 0);
                StartingUpgradePrice = StartingUpgradePrice * 1.5f;
            }
            UpgradeProcent.SetText("+" + StartingUpgrade.ToString() + "% to production");
            UpgradePrice.SetText("Buy for " + NumberNormalizer.Normalize(StartingUpgradePrice));
            GameObject UpgradeClone = Instantiate(Upgrade);
            UpgradeClone.transform.SetParent(Parent);
            UpgradeClone.SetActive(true);
            UpgradeClone.transform.localScale = Vector3.one;
            float cloneStartingUpgrade = StartingUpgrade;
            float cloneStartingUpgradePrice = StartingUpgradePrice;
            Button btn = UpgradeClone.transform.Find("Buy").GetComponent<Button>();
            TextMeshProUGUI Text = btn.GetComponentInChildren<TextMeshProUGUI>();
            btn.onClick.AddListener(() => UIManager.OnCastleCloneUpgradeButtonPress(cloneStartingUpgradePrice, cloneStartingUpgrade, UpgradeClone, btn, Text));
            
        }
    }
}
