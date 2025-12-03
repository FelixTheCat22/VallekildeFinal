using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    public int health;
    public int collisionDamage;
    public Player player;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            
            health--;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        if (other.gameObject == player.gameObject)
        {
            player.TakeDamage(collisionDamage);
        }
    }
}
