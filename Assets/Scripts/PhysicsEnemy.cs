using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsEnemy : Enemy
{
    [Header("Movement")]
    public float terminalVelocity;
    public float damping;
    public float forceMultiplier;
    public float uTurnHelper;
    public float separationForce;
    [Tooltip("If set to false, everything below it is ignored.")]
    public bool canBuildUp;
    public float buildupMultiplier;
    public float speedBuildupTime;
    public bool breaksAway;

    private float _speedBuildup;
    private bool _pursuing = true;

    protected Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _speedBuildup = Random.Range(0f, 0.7f);
    }
    
    protected void FixedUpdate()
    {
        if (!gameManager.Running) return;
        
        if (!_pursuing)
        {
            _rb.AddForce(-_rb.linearVelocity.normalized * forceMultiplier / 2);
            return;
        };

        // Keep up speed: The longer the enemy has been pursuing the higher the force.
        _speedBuildup += Time.fixedDeltaTime / speedBuildupTime;
        if (_speedBuildup > 1f)
        {
            if (breaksAway)
            {
                BreakAway();
            }
            else
            {
                _speedBuildup = 0.2f;
            }
        }
        //float speedFraction = _rb.linearVelocity.magnitude / terminalVelocity;
        Vector2 thisToPlayer = player.transform.position - transform.position;
        Vector2 targetMovement = thisToPlayer
                                 - _rb.linearVelocity * (Time.fixedDeltaTime * damping);
        Vector2 forceToApply =
            targetMovement.normalized * forceMultiplier;
        if (canBuildUp)
        {
            forceToApply *= Mathf.Clamp(_speedBuildup * buildupMultiplier, 1f, buildupMultiplier);
        }

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

    protected void OnTriggerStay2D(Collider2D other)
    {
        Vector2 away =  transform.position - other.transform.position;
        _rb.AddForce(away.normalized * separationForce);
    }
}
