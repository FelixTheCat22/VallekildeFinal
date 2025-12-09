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
    public Bullet bulletPrefab;
    public GameManager gameManager;
    public ArcadeInputType dashButton;

    public int Health {private set; get;}
    //public int OffBeatInputs {private set; get;}

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
        _camera = Camera.current;
        Health = startHealth;
    }

    private void Update()
    {
        ReadInput();

        int nearestBeat = Metronome.Instance.NearedBeatCounter;
        if (_shootThisFrame && Metronome.Instance.MaybeBeat != -1 && _lastShotBeat != nearestBeat)
        {
            _lastShotBeat = nearestBeat;
            if (!grapeshot)
            {
                Shoot();
            }
            else
            {
                Shoot();
                Shoot(45);
                Shoot(-45);
            }
        }

        if (_dashThisFrame && Metronome.Instance.MaybeBeat != -1 && dashAvailable)
        {
            Dash();
        }

        /*
        if ((_dashThisFrame || _shootThisFrame) && metronome.MaybeBeat == -1)
        {
            OffBeatInputs++;
        }
        */
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _movementInput.normalized * speed;
    }

    private void ReadInput()
    {
        _movementInput = ArcadeInput.GetAxises(1);
        _shootThisFrame = JoystickInitiated(2);
        _shootingInput = ArcadeInput.GetAxises(2);
        _dashThisFrame = ArcadeInput.InputInitiated(2, dashButton);
    }

    private bool JoystickInitiated(int player)
    {
        return ArcadeInput.InputInitiated(player, ArcadeInputType.JoystickUp)
            || ArcadeInput.InputInitiated(player, ArcadeInputType.JoystickDown)
            || ArcadeInput.InputInitiated(player, ArcadeInputType.JoystickRight)
            || ArcadeInput.InputInitiated(player, ArcadeInputType.JoystickLeft);
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
        bullet.rb.linearVelocity =
            _shootingInput
            * bullet.speed
            // + _rb.linearVelocity
            ;
    }
    
    private void Shoot(float angle)
    {
        Vector3 spawnPosition = transform.position + Vector3.forward * 0.5f; //Bullets are 0.5 units behind player
        Bullet bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        bullet.rb.linearVelocity = (
            RotateVec2(_shootingInput, angle)
            * bullet.speed
            // + _rb.linearVelocity
            );
    }

    private Vector2 RotateVec2(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
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
