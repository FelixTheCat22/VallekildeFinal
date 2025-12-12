using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Pickup : MonoBehaviour
{
    public PickupType pickupType;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = pickupType.sprite;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (!player)
        {
            Debug.LogError("Player not found on game object", other.gameObject);
            return;
        }
        
        pickupType.OnPickup(player);
        Destroy(gameObject);
    }
}
