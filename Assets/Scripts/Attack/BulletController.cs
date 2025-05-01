using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private BulletData bulletData;

    private new Rigidbody2D rigidbody2D;

    private Vector3 startPosition;
    private Vector2 direction;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Initialize(string tag, Vector3 startPosition, Vector2 direction)
    {
        this.tag = tag;
        this.startPosition = startPosition;
        this.direction = direction;

        transform.position = this.startPosition;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, startPosition) > bulletData.Range) Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        rigidbody2D.linearVelocity = direction * bulletData.Speed;
    }
}
