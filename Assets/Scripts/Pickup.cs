using System;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (!player)
        {
            Debug.LogError("Player not found on game object", other.gameObject);
            return;
        }
        
        OnPickup(player);
        Destroy(gameObject);
    }

    protected abstract void OnPickup(Player player);
}
