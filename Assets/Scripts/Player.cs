using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public int startHealth;
    public float speed;
    public bool dashAvailable;
    public float dashDistance;
    public bool grapeshot;
    public int grapeshotBulletCount;
    public Bullet bulletPrefab;
    public Metronome metronome;
    public GameManager gameManager;

    public int Health {private set; get;}

    private bool _dashing;
    private Camera _camera;
    private Vector2 _movementInput;
    private Vector2 _shootingInput;
    private bool _shootThisFrame;
    private int _lastShotBeat;
    private bool _dashThisFrame;

    private Rigidbody2D _rb;
    private void Awake()
    {   
        _rb = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        Health = startHealth;
    }

    private void Update()
    {
        ReadInput();
        
        if (_shootThisFrame && metronome.MaybeBeat != -1 && _lastShotBeat != metronome.beatCount)
        {
            _lastShotBeat = metronome.beatCount;
            Shoot();
        }

        if (_dashThisFrame && metronome.MaybeBeat != -1)
        {
            Dash();
        }
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _movementInput.normalized * speed;
    }

    private void ReadInput()
    {
        _movementInput = new Vector2(Input.GetAxisRaw("MoveHorizontal"), Input.GetAxisRaw("MoveVertical"));
        _shootThisFrame = Input.GetButtonDown("ShootHorizontal") || Input.GetButtonDown("ShootVertical");
        _shootingInput = new Vector2(Input.GetAxisRaw("ShootHorizontal"), Input.GetAxisRaw("ShootVertical"));
        _dashThisFrame = Input.GetKeyDown(KeyCode.Space) && dashAvailable; // Hard-coded control, temp
    }

    private void Dash()
    {
        LayerMask wallMask = LayerMask.GetMask("Walls");
        RaycastHit2D raycast = 
            Physics2D.Raycast(transform.position, _movementInput, dashDistance, wallMask);

        if (!raycast)
        {
            transform.position += (Vector3) _movementInput * dashDistance;
        }
        else
        {
            // localScale.x should be equal to localScale.y for this to work as intended
            transform.position += (Vector3) _movementInput * (raycast.distance - transform.localScale.x / 2);
        } 
    }

    private void Shoot()
    {
        int bulletCount = grapeshot ? grapeshotBulletCount : 1;
        float rotationAngle = (90f / grapeshotBulletCount) / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 spawnPosition = transform.position + Vector3.forward * 0.5f; //Bullets are 0.5 units behind player
            Bullet bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            bullet.mainCamera = _camera;
            bullet.rb.linearVelocity = (
                RotateVec2(_shootingInput, rotationAngle * (i + 1))
                * bullet.speed
                // + _rb.linearVelocity
                );
        }
    }

    private Vector2 RotateVec2(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector2(
            v.x * cos - v.y * sin,
            v.y * sin + v.x * cos
            );
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        gameManager.OnPlayerHealthChanged();
    }

    public void ResetHealth()
    {
        TakeDamage(Health - startHealth);
    }
}
