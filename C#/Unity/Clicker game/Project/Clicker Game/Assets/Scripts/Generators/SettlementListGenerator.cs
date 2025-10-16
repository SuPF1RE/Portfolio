using UnityEngine;

public class SettlementListGenerator : MonoBehaviour
{
    public enemy ETarget;
    public RectTransform Parent;
    public int TargetCount;
    public BigNumberNormalizer NumberNormalizer;
    public UIManager UIManager;
    public GameManager gameManager;
    public ReincarnationManager reincarnationManager;
    public void Awake()
    {
        SettlementListGeneration();
    }
    public void SettlementListGeneration()
    {
        enemy TargetClone = ETarget; 
        if (reincarnationManager.ReincarnationCount == 0) 
        {
            TargetClone.health = 100;
            TargetClone.strength = 10;
            TargetClone.healthmax = TargetClone.health;
        }
        foreach (Transform child in Parent) Destroy(child.gameObject);
        for (int i = 0; i < TargetCount; i++)
        {
            if (i != 0)
            {
                TargetClone.strength += TargetClone.strength * 0.1f;
                TargetClone.health += TargetClone.health * 0.1f;
                TargetClone.healthmax += TargetClone.healthmax * 0.1f;
            }
            enemy TargetClone2 = Instantiate(TargetClone, Parent);
            TargetClone2.strength = TargetClone.strength;
            TargetClone2.health = TargetClone.health;
            TargetClone2.healthmax = TargetClone.healthmax;
            TargetClone2.strenghtText.SetText(NumberNormalizer.Normalize(TargetClone2.strength));
            TargetClone2.healthText.SetText(NumberNormalizer.Normalize(TargetClone2.health) + "/" + NumberNormalizer.Normalize(TargetClone2.healthmax));
            TargetClone2.transform.localScale = Vector3.one;
        }
    }
}
