using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : Enemyprototype
{
    BigNumberNormalizer NumberNormalizer;
    public void Awake()
    {
        NumberNormalizer = FindAnyObjectByType<BigNumberNormalizer>();
        strenghtText.SetText(NumberNormalizer.Normalize(strength));
        healthText.SetText(NumberNormalizer.Normalize(health));
    }
    public void Start()
    {
        InvokeRepeating(nameof(Heal), 0f, 30f);
    }
    public void Heal() 
    {
        if (health > 0) 
        { 
            float lostHealth = healthmax - health;
            float restoreAmount = lostHealth * (10f / 100f);
            health += restoreAmount;
            health = Mathf.Min(health, healthmax);
            healthText.SetText(NumberNormalizer.Normalize(health));
        }
    }
}
