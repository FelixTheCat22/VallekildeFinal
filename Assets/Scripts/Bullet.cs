using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public float speed;
    public float cullDistance;

    private Vector2 _origin;

    private void Start()
    {
        _origin = transform.position;
    }

    private void Update()
    {
        if (((Vector2)transform.position - _origin).sqrMagnitude < cullDistance * cullDistance)
        {
            Cull();
        } 
    }

    private void Cull()
    {
        Destroy(gameObject);
    }
}
