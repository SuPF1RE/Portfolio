using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemyprototype : MonoBehaviour
{
    public float strength;
    public float health;
    public float healthmax;
    public TextMeshProUGUI ButtonText;
    public TextMeshProUGUI strenghtText;
    public TextMeshProUGUI healthText;
    public Button AttackButton;
}
