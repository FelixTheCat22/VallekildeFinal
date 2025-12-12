using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    public int health;
    public int collisionDamage;
    public Player player;
    public int killScoreValue;
    public bool destroyBullet = true;
    public bool killable = true;
    public GameManager gameManager;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (other.enabled)
            {
                health--;
            }

            if (health <= 0 && killable)
            {
                AppManager.Instance.IncreaseScore(killScoreValue);
                Destroy(gameObject);
            }

            if (destroyBullet)
            {
                other.enabled = false;
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject == player.gameObject)
        {
            player.TakeDamage(collisionDamage);
        }
    }
}
