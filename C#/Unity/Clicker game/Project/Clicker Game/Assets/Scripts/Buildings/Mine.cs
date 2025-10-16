using UnityEngine;
using UnityEngine.EventSystems;

public class Mine : BuildingsPrototypes
{
    public UIManager manager;
    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) { manager.OnMinePress(); }
    }
}
