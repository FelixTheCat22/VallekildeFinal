using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float speed;
    public Bullet bulletPrefab;
    public Metronome metronome;
    private Camera _camera;
    private Vector2 _input;
    private Quaternion _rotation;

    private Rigidbody2D _rb;
    private void Awake()
    {   
        _rb = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }

    private void Update()
    {
        ReadInput();
        CalculateRotationQuaternion();
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Shoot();
        }
    }

    private void ReadInput()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void CalculateRotationQuaternion()
    {
        if (_input == Vector2.zero) return;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(_rb.linearVelocity.x, _rb.linearVelocity.y);
        _rotation = Quaternion.Euler(0, 0, -angle);
    }
    
    private void FixedUpdate()
    {
        _rb.linearVelocity = _input.normalized * speed;
        _rb.MoveRotation(_rotation);
    }

    private void Shoot()
    {
        Vector3 spawnPosition = transform.position + Vector3.forward * 0.5f; //Bullets are 0.5 units behind player
        Bullet bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);
        bullet.mainCamera = _camera;
        bullet.rb.linearVelocity = (Vector2)transform.up * bullet.speed + _rb.linearVelocity;
        if (metronome.MaybeBeat != -1)
        {
            bullet.spriteRenderer.color = Color.blue;
        }
    }
}
