using UnityEngine;

public class CastleButton : BuildingsPrototypes
{
    public float Strength;
    public UIManager manager;
    public void OnMouseDown()
    {
        manager.OnCastlePress();
    }
}
