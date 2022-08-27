using UnityEngine;

public class BulletController : MonoBehaviour
{
    private static float Range = 2.0f;
    private static float Speed = 16.0f;

    public Vector3 startPosition;
    public Vector2 direction;

    private new Rigidbody2D rigidbody2D;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, startPosition) > Range) {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        rigidbody2D.velocity = direction * Speed;
    }
}
