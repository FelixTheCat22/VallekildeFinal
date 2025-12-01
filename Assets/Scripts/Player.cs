using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public int startHealth;
    public float speed;
    public bool dashAvailable;
    public float dashMultiplier;
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
    }

    private void ReadInput()
    {
        _movementInput = new Vector2(Input.GetAxisRaw("MoveHorizontal"), Input.GetAxisRaw("MoveVertical"));
        _shootThisFrame = Input.GetButtonDown("ShootHorizontal") || Input.GetButtonDown("ShootVertical");
        _shootingInput = new Vector2(Input.GetAxisRaw("ShootHorizontal"), Input.GetAxisRaw("ShootVertical"));
        if (Input.GetKeyDown(KeyCode.Space) && dashAvailable) // Hard-coded control, temp
        {
            _dashThisFrame = true;   
        }
    }

    private void FixedUpdate()
    {
        _dashing = false;
        float dash;
        if (_dashThisFrame) // Feels very janky, but works
        {
            _dashThisFrame = false;
            dash = dashMultiplier;
            _dashing = true;
        }
        else
        {
            dash = 1f;
        }
        _rb.linearVelocity = _movementInput.normalized * (speed * dash);
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

    private void TakeDamage(int damage)
    {
        Health -= damage;
        gameManager.OnPlayerHealthChanged();
    }

    public void ResetHealth()
    {
        TakeDamage(Health - startHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_dashing)
        {
            TakeDamage(1);
        }
    }
}
