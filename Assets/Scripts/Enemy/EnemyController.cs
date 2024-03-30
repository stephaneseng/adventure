using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;

    private new Rigidbody2D rigidbody2D;
    private Animator animator;

    public EnemyStateMachine enemyStateMachine;
    public Vector2 direction;
    private int health;
    private bool move;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        enemyStateMachine = new EnemyStateMachine(this);
        direction = Vector2.down;
        health = enemyData.health;
        move = false;

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
        rigidbody2D.velocity = (float)(move ? 1.0 : 0.0f) * enemyData.speed * direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            Destroy(other.gameObject);
            RemoveHealth(1);
        }
    }

    public void Move()
    {
        move = true;
    }

    public void StopMove()
    {
        move = false;
    }

    public void Freeze()
    {
        enemyStateMachine.SwitchState(new EnemyFreezeState());
    }

    public void StopFreeze()
    {
        enemyStateMachine.SwitchState(new EnemyIdleState());
    }

    public void Attack()
    {
        GameObject enemyBullet = Instantiate(enemyData.bullet, transform.position, transform.rotation, transform);
        enemyBullet.tag = "EnemyAttack";
        enemyBullet.GetComponent<BulletController>().startPosition = transform.position;
        enemyBullet.GetComponent<BulletController>().direction = direction;
    }

    private void RemoveHealth(int delta)
    {
        // Prevent switching multiple times to the destroy state. 
        if (health == 0)
        {
            return;
        }

        health = Mathf.Max(0, health - delta);

        if (health > 0)
        {
            enemyStateMachine.SwitchState(new EnemyDamageState());
        }
        else
        {
            enemyStateMachine.SwitchState(new EnemyDestroyState());
        }
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
        if (enemyData.droppedItems.Count == 0) {
            return;
        }

        Instantiate(enemyData.droppedItems[Random.Range(0, enemyData.droppedItems.Count)], transform.position,
            Quaternion.identity, transform.parent);
    }
}
