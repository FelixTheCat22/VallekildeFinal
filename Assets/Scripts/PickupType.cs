using UnityEngine;

public abstract class PickupType : ScriptableObject
{
    public Sprite sprite;
    
    public abstract void OnPickup(Player player);
}
