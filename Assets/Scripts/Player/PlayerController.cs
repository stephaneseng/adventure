using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static int BaseHealth = 3;
    private static int BaseMaxHealth = 3;
    private static float BaseSpeed = 4.0f;

    public GameObject bullet;

    private PlayerInput playerInput;
    private new Rigidbody2D rigidbody2D;
    private Animator animator;

    private PlayerStateMachine playerStateMachine;
    public int health;
    public int maxHealth;
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
        playerStateMachine.Start(new PlayerIdleState());
        health = BaseHealth;
        maxHealth = BaseMaxHealth;
        direction = Vector2.up;
        speed = 0.0f;
        lockMove = false;
    }

    void Update()
    {
        playerStateMachine.Update();
    }

    void FixedUpdate()
    {
        rigidbody2D.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x, direction.y,
            0.0f));
        rigidbody2D.velocity = direction * speed * BaseSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ItemHealth")) {
            Destroy(other.gameObject);

            Debug.Log((health+1) + " VS " + maxHealth);
            health = Mathf.Min(health + 1, maxHealth);
        }

        if (other.CompareTag("EnemyAttack")) {
            Destroy(other.gameObject);

            health = Mathf.Max(0, health - 1);
            playerStateMachine.SwitchState(new PlayerDamageState());
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) {
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

    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
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

    private void Attack()
    {
        GameObject playerBullet = (GameObject) Instantiate(bullet, transform.position, transform.rotation,
            transform);
        playerBullet.tag = "PlayerAttack";
        playerBullet.GetComponent<BulletController>().startPosition = transform.position;
        playerBullet.GetComponent<BulletController>().direction = direction;
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
}
