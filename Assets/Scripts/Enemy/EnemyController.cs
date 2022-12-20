using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;

    private new Rigidbody2D rigidbody2D;
    private Animator animator;

    public EnemyStateMachine enemyStateMachine;
    public Vector2 direction;
    public float speed;
    private int health;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        enemyStateMachine = new EnemyStateMachine(this);
        direction = Vector2.down;
        speed = 0.0f;
        health = enemyData.health;

        enemyStateMachine.Initialize(new EnemyIdleState());
    }

    void Update()
    {
        enemyStateMachine.Update();
    }

    void FixedUpdate()
    {
        rigidbody2D.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x, direction.y,
            0.0f));
        rigidbody2D.velocity = direction * speed * enemyData.speed;
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
        speed = 1.0f;
    }

    public void StopMove()
    {
        speed = 0.0f;
    }

    public void Attack()
    {
        GameObject enemyBullet = (GameObject)Instantiate(enemyData.bullet, transform.position, transform.rotation,
            transform);
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
        DropItem();
    }

    public void DropItem()
    {
        Instantiate(enemyData.droppedItems[Random.Range(0, enemyData.droppedItems.Length)], transform.position,
            Quaternion.identity, transform.parent);
    }
}
