using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public int startHealth;
    public float speed;
    public bool dashAvailable;
    public float dashDistance;
    public Bullet bulletPrefab;
    public Metronome metronome;
    public GameManager gameManager;

    public int Health {private set; get;}

    private bool _dashing;
    private Camera _camera;
    private Vector2 _movementInput;
    private Vector2 _shootingInput;
    private bool _shootThisFrame;
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
        
        if (_shootThisFrame && metronome.MaybeBeat != -1)
        {
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
        Vector3 spawnPosition = transform.position + Vector3.forward * 0.5f; //Bullets are 0.5 units behind player
        Bullet bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        bullet.mainCamera = _camera;
        bullet.rb.linearVelocity = (_shootingInput * bullet.speed);  // + _rb.linearVelocity;
        /*
        if (metronome.MaybeBeat != -1)
        {
            bullet.spriteRenderer.color = Color.blue;
        }
        */
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
