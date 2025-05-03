using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    private new Rigidbody2D rigidbody2D;
    private Animator animator;

    private EnemyStateMachine enemyStateMachine;
    private int health;
    private Attack attack;
    private Vector2 direction;
    private bool move;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        enemyStateMachine = new EnemyStateMachine();
        EnemyBrain = enemyData.EnemyBrain;
        health = enemyData.Health;
        attack = enemyData.Attack;
        direction = Vector2.down;
        move = false;

        enemyStateMachine.Initialize(new EnemyIdleState(this));
    }

    private void Update()
    {
        enemyStateMachine.Update();
    }

    private void FixedUpdate()
    {
        rigidbody2D.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x, direction.y, 0.0f));
        rigidbody2D.linearVelocity = (float)(move ? 1.0 : 0.0f) * enemyData.Speed * direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            Destroy(other.gameObject);
            RemoveHealth(1);
        }
    }

    public void SwitchToIdleState()
    {
        enemyStateMachine.SwitchState(new EnemyIdleState(this));
    }

    public void SwitchToMoveState()
    {
        enemyStateMachine.SwitchState(new EnemyMoveState(this));
    }

    public void SwitchToAttackState()
    {
        enemyStateMachine.SwitchState(new EnemyAttackState(this));
    }

    public void SwitchToDamageState()
    {
        enemyStateMachine.SwitchState(new EnemyDamageState(this));
    }

    public void SwitchToDestroyState()
    {
        enemyStateMachine.SwitchState(new EnemyDestroyState(this));
    }

    public void SwitchToFreezeState()
    {
        enemyStateMachine.SwitchState(new EnemyFreezeState(this));
    }

    public float StateStartTime()
    {
        return enemyStateMachine.StartTime;
    }

    public void Move(Vector2 direction)
    {
        this.direction = direction;
        move = true;
    }

    public void StopMove()
    {
        move = false;
    }

    public void Attack()
    {
        attack.Execute("EnemyAttack", transform.position, direction);
    }

    private void RemoveHealth(int delta)
    {
        // Prevent switching multiple times to the destroy state. 
        if (health == 0) return;

        health = Mathf.Max(0, health - delta);

        if (health > 0)
            SwitchToDamageState();
        else
            SwitchToDestroyState();
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

    private void DropItem()
    {
        if (enemyData.DroppedItems.Count == 0) return;

        Instantiate(enemyData.DroppedItems[Random.Range(0, enemyData.DroppedItems.Count)], transform.position,
            Quaternion.identity, transform.parent);
    }

    public EnemyBrain EnemyBrain { get; private set; }
}
