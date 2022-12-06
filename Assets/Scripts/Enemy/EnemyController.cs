using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private static int BaseHealth = 2;
    private static float BaseSpeed = 2.0f;
    private static Vector2[] NextDirectionChoices = new Vector2[] {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left,
        Vector2.zero
    };

    public GameObject bullet;
    public GameObject[] droppedItems;

    private new Rigidbody2D rigidbody2D;
    private Animator animator;

    private EnemyStateMachine enemyStateMachine;
    private int health;
    private Vector2 direction;
    private float speed;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        enemyStateMachine = new EnemyStateMachine(this);
        enemyStateMachine.Start(new EnemyIdleState());
        health = BaseHealth;
        direction = Vector2.down;
        speed = 0.0f;
    }

    void Update()
    {
        enemyStateMachine.Update();
    }

    void FixedUpdate()
    {
        rigidbody2D.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x, direction.y,
            0.0f));
        rigidbody2D.velocity = direction * speed * BaseSpeed;
    }

    void OnDestroy()
    {
        DropItem();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            Destroy(other.gameObject);

            health = Mathf.Max(0, health - 1);
            if (health > 0)
            {
                enemyStateMachine.SwitchState(new EnemyDamageState());
            }
            else
            {
                enemyStateMachine.SwitchState(new EnemyDestroyState());
            }
        }
    }

    public void StartMove()
    {
        Vector2 nextDirection = NextDirectionChoices[Random.Range(0, NextDirectionChoices.Length)];
        if (nextDirection != Vector2.zero)
        {
            direction = nextDirection;
            speed = 1.0f;
        }
        else
        {
            speed = 0.0f;
        }
    }

    public void StopMove()
    {
        speed = 0.0f;
    }

    public void Attack()
    {
        GameObject enemyBullet = (GameObject)Instantiate(bullet, transform.position, transform.rotation, transform);
        enemyBullet.tag = "EnemyAttack";
        enemyBullet.GetComponent<BulletController>().startPosition = transform.position;
        enemyBullet.GetComponent<BulletController>().direction = direction;
    }

    public void Damage()
    {
        animator.Play("Damage");
    }

    public void Destroy()
    {
        animator.Play("Destroy");
    }

    public void DropItem()
    {
        Instantiate(droppedItems[Random.Range(0, droppedItems.Length)], transform.position, Quaternion.identity,
            transform.parent);
    }
}
