using UnityEngine;

public class LaserEnemy : PhysicsEnemy
{
    public LaserEnemy partner;
    public SpriteRenderer laser;

    private float _laserScalePerUnit;

    private void Start()
    {
        if (laser)
        {
            _laserScalePerUnit = laser.sprite.pixelsPerUnit / laser.sprite.texture.height;
        }
    }

    void OnDestroy()
    {
        if (partner)
        {
            Destroy(partner.gameObject);
        }
    }

    private void Update()
    {
        Vector2 toPartner = partner.transform.position - transform.position;
        float angleToPartner = Mathf.Atan2(toPartner.y, toPartner.x);

        if (laser)
        {
            laser.transform.localScale = new Vector3(1, toPartner.magnitude * _laserScalePerUnit, 1);
            laser.transform.rotation = Quaternion.AngleAxis((angleToPartner * Mathf.Rad2Deg) - 90, Vector3.forward);
        }
    }
    
    private new void FixedUpdate()
    {
        base.FixedUpdate();
        
        Vector2 toPartner = partner.transform.position - transform.position;
        float angleToPartner = Mathf.Atan2(toPartner.y, toPartner.x);
        _rb.SetRotation(angleToPartner * Mathf.Rad2Deg);

        if (laser)
        {
            laser.transform.localScale = new Vector3(1, toPartner.magnitude * _laserScalePerUnit, 1);
            laser.transform.rotation = Quaternion.AngleAxis((angleToPartner * Mathf.Rad2Deg) - 90, Vector3.forward);
        }
    }

    private new void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == partner.gameObject)
        {
            base.OnTriggerStay2D(collision);
        }
    }
}