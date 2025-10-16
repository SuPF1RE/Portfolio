using UnityEngine;
using UnityEngine.EventSystems;

public class StoneQuarry : BuildingsPrototypes
{
    public UIManager manager;
    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) { manager.OnStoneQuarryPress(); }
    }
}
