using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public int health = 1;
    public Player player;
    public GameManager gameManager;
    [Header("Movement")]
    public float terminalVelocity;
    public float damping;
    public float forceMultiplier;
    public float buildupMultiplier;
    public float uTurnHelper;
    public float speedBuildupTime;
    public float separationForce;

    private float _speedBuildup;
    private bool _pursuing = true;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _speedBuildup = Random.Range(0f, 0.5f);
    }
    
    private void FixedUpdate()
    {
        if (!_pursuing)
        {
            _rb.AddForce(-_rb.linearVelocity.normalized * forceMultiplier / 2);
            return;
        };

        // Keep up speed: The longer the enemy has been pursuing the higher the force.
        _speedBuildup += Time.fixedDeltaTime / speedBuildupTime;
        if (_speedBuildup > 1f)
        {
            BreakAway();
        }
        //float speedFraction = _rb.linearVelocity.magnitude / terminalVelocity;
        Vector2 thisToPlayer = player.transform.position - transform.position;
        Vector2 targetMovement = thisToPlayer
                                 - _rb.linearVelocity * (Time.fixedDeltaTime * damping * _speedBuildup);
        Vector2 forceToApply =
            targetMovement.normalized * forceMultiplier;
        forceToApply *= Mathf.Clamp(_speedBuildup * buildupMultiplier, 1f, buildupMultiplier);
        
        // When moving away from player quickly, increase force.
        if (Vector2.Dot(_rb.linearVelocity, forceToApply) < -0.7f)
        {
            forceToApply *= uTurnHelper;
        }
        
        // Apply the force and then clamp to terminalVelocity
        _rb.AddForce(forceToApply);
        if (_rb.linearVelocity.magnitude > terminalVelocity)
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * terminalVelocity;
        }
    }

    // Only called once every speedBuildupTime seconds, so the Invoke is probably ok
    // ReSharper disable Unity.PerformanceAnalysis
    private void BreakAway()
    {
        _speedBuildup = 0f;
        _pursuing = false;
        Invoke(nameof(Rejoin), 3f);
    }

    private void Rejoin()
    {
        _pursuing = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            health--;
            if (health <= 0)
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            return;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Vector2 away =  transform.position - other.transform.position;
        _rb.AddForce(away.normalized * separationForce);
    }
}
