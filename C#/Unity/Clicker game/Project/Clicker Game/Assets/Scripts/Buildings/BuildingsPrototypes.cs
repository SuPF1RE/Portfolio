using UnityEngine;
using UnityEngine.UI;

public abstract class BuildingsPrototypes : MonoBehaviour
{
    public string Name;
    public int Level;
    public float BaseCost;
    public float NextCost;
    public float ProductionBase;
    public float ProductionSum;
    public GameObject skinLevel1;       
    public GameObject skinLevel50;      
    public GameObject skinLevel100;     
    public Image Frame;
    public GameObject Worker;
}
