using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerData playerData;

    private PlayerInput playerInput;
    private new Rigidbody2D rigidbody2D;
    private Animator animator;

    public PlayerStateMachine playerStateMachine;

    private int health;
    private int maxHealth;
    private Vector2 direction;
    private float speed;
    private bool lockMove;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        playerStateMachine = new PlayerStateMachine(this);
        health = playerData.health;
        maxHealth = playerData.health;
        direction = Vector2.up;
        speed = 0.0f;
        lockMove = false;

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
        rigidbody2D.velocity = direction * speed * playerData.speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ItemHealth"))
        {
            Destroy(other.gameObject);
            health = Mathf.Min(health + 1, maxHealth);
        }

        if (other.CompareTag("EnemyAttack"))
        {
            Destroy(other.gameObject);
            health = Mathf.Max(0, health - 1);
            playerStateMachine.SwitchState(new PlayerDamageState());
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Attack();
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public Vector2 ReadInputActionMoveVector()
    {
        return playerInput.actions["Move"].ReadValue<Vector2>();
    }

    public void Move(Vector2 inputActionMoveVector)
    {
        direction = inputActionMoveVector;
        speed = 1.0f;
    }

    public void StopMove()
    {
        speed = 0.0f;
    }

    public void LockMove()
    {
        lockMove = true;
    }

    public void UnlockMove()
    {
        lockMove = false;
    }

    public bool IsMoveLocked()
    {
        return lockMove;
    }

    public void Attack()
    {
        GameObject playerBullet = (GameObject)Instantiate(playerData.bullet, transform.position, transform.rotation,
            transform);
        playerBullet.tag = "PlayerAttack";
        playerBullet.GetComponent<BulletController>().startPosition = transform.position;
        playerBullet.GetComponent<BulletController>().direction = direction;
    }

    public void Damage()
    {
        animator.Play("Damage");
    }
}
