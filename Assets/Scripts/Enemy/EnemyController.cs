using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private static float BaseSpeed = 2.0f;
    private static Vector2[] NextDirectionChoices = new Vector2[] {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left,
        Vector2.zero
    };

    public GameObject bullet;

    private new Rigidbody2D rigidbody2D;
    private BoxCollider2D roomBoxCollider2D;

    private EnemyStateMachine enemyStateMachine;
    private Vector2 direction;
    private float speed;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        enemyStateMachine = new EnemyStateMachine(this);
        enemyStateMachine.Start(new EnemyIdleState());
        direction = Vector2.down;
        speed = 0.0f;
    }

    void Update()
    {
        enemyStateMachine.Update();
    }

    void FixedUpdate()
    {
        rigidbody2D.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x,
            direction.y, 0.0f));
        rigidbody2D.velocity = direction * speed * BaseSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack")) {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }

    public void StartMove()
    {
        Vector2 nextDirection = NextDirectionChoices[Random.Range(0, NextDirectionChoices.Length)];
        if (nextDirection != Vector2.zero) {
            direction = nextDirection;
            speed = 1.0f;
        } else {
            speed = 0.0f;
        }
    }

    public void StopMove()
    {
        speed = 0.0f;
    }

    public void Attack()
    {
        GameObject enemyBullet = (GameObject) Instantiate(bullet, transform.position, transform.rotation,
            transform);
        enemyBullet.tag = "EnemyAttack";
        enemyBullet.GetComponent<BulletController>().startPosition = transform.position;
        enemyBullet.GetComponent<BulletController>().direction = direction;
    }
}
