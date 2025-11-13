using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Camera mainCamera;
    public float cullMargin = 3;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public float speed;
    private float _cullDist;

    private void Start()
    {
        _cullDist = (mainCamera.orthographicSize * 2 * mainCamera.aspect) + cullMargin;
    }
    
    private void Update()
    {
        // Cast to vec2 in order to ignore z
        if (((Vector2)(transform.position - mainCamera.transform.position)).sqrMagnitude > (_cullDist * _cullDist))
        {
            Destroy(gameObject);
        }
    }
}
