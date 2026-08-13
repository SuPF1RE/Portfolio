using UnityEngine;
[System.Serializable]
public class BuildingAppearance
{
    public BuildingsPrototypes building;
}
public class BuildingsAppearenceManager : MonoBehaviour
{
    [SerializeField] private BuildingAppearance[] buildings;
    public void Awake()
    {
        UpdateAppearance();
    }
    public void UpdateAppearance()
    {
        foreach (var b in buildings)
        {
            int level = b.building.Level;
            if (b.building.skinLevel1 != null) b.building.skinLevel1.SetActive(false);
            if (b.building.skinLevel50 != null) b.building.skinLevel50.SetActive(false);
            if (b.building.skinLevel100 != null) b.building.skinLevel100.SetActive(false);
            if (level > 0 && level < 50)
            {
                if (b.building.skinLevel1 != null) b.building.skinLevel1.SetActive(true);
                if (b.building.Frame != null) b.building.Frame.color = Color.green;
            }
            else if (level >= 50 && level < 100)
            {
                if (b.building.skinLevel50 != null) b.building.skinLevel50.SetActive(true);
            }
            else if (level >= 100)
            {
                if (b.building.skinLevel100 != null) b.building.skinLevel100.SetActive(true);
            }
        }
    }
}
