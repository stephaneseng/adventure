using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerData playerData;

    private PlayerInput playerInput;
    private new Rigidbody2D rigidbody2D;
    private Animator animator;

    public PlayerStateMachine playerStateMachine;
    public int health;
    private Vector2 direction;
    private bool move;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        playerStateMachine = new PlayerStateMachine(this);
        health = playerData.health;
        direction = Vector2.up;
        move = false;

        playerStateMachine.Initialize(new PlayerIdleState());
    }

    void Update()
    {
        playerStateMachine.Update();
    }

    void FixedUpdate()
    {
        rigidbody2D.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x, direction.y,
            0.0f));
        rigidbody2D.velocity = (float)(move ? 1.0f : 0.0f) * playerData.speed * direction;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            RemoveHealth(1);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ItemHealth"))
        {
            Destroy(other.gameObject);

            AddHealth(1);
        }

        if (other.CompareTag("EnemyAttack"))
        {
            Destroy(other.gameObject);

            RemoveHealth(1);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Attack();
        }
    }

    public Vector2 ReadInputActionMoveVector()
    {
        return playerInput.actions["Move"].ReadValue<Vector2>();
    }

    public void Move(Vector2 inputActionMoveVector)
    {
        direction = inputActionMoveVector;
        move = true;
    }

    public void StopMove()
    {
        move = false;
    }

    public void Freeze()
    {
        playerStateMachine.SwitchState(new PlayerFreezeState());
    }

    public void StopFreeze()
    {
        playerStateMachine.SwitchState(new PlayerIdleState());
    }

    private void Attack()
    {
        GameObject playerBullet = (GameObject)Instantiate(playerData.bullet, transform.position, transform.rotation,
            transform);
        playerBullet.tag = "PlayerAttack";
        playerBullet.GetComponent<BulletController>().startPosition = transform.position;
        playerBullet.GetComponent<BulletController>().direction = direction;
    }

    private void AddHealth(int delta)
    {
        health = Mathf.Min(health + delta, playerData.health);
    }

    private void RemoveHealth(int delta)
    {
        health = Mathf.Max(0, health - delta);

        playerStateMachine.SwitchState(new PlayerDamageState());
    }

    public void Damage()
    {
        animator.Play("Damage");
    }
}
