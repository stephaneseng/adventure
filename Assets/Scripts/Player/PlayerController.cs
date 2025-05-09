using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private static readonly float InvincibilityDurationInSeconds = 0.5f;
    private static readonly float DestroyStateDurationInSeconds = 0.15f;

    [SerializeField] private PlayerData playerData;

    private PlayerInput playerInput;
    private new Rigidbody2D rigidbody2D;
    private Animator animator;
    private GameObject level;

    private PlayerStateMachine playerStateMachine;
    private int health;
    private Attack attack;
    private int keys;
    private bool map;
    private Vector2 direction;
    private bool move;
    private float invincibilityCountdown;

    public int Health => health;

    public int MaxHealth => playerData.Health;

    public int Keys => keys;

    public bool Map => map;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        level = GameObject.FindGameObjectWithTag("Level");

        playerStateMachine = new PlayerStateMachine();
        health = playerData.Health;
        attack = playerData.Attack;
        keys = 0;
        map = false;
        direction = Vector2.up;
        move = false;
        invincibilityCountdown = 0.0f;

        playerStateMachine.Initialize(new PlayerIdleState(this));
    }

    private void Update()
    {
        playerStateMachine.Update();

        if (invincibilityCountdown > 0.0f) invincibilityCountdown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        rigidbody2D.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x, direction.y, 0.0f));
        rigidbody2D.linearVelocity = (move ? 1.0f : 0.0f) * playerData.Speed * direction;
    }

    private void OnDestroy()
    {
        // End the game (defeat).
        Destroy(level.gameObject);
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy")) RemoveHealth(1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ItemHealth"))
        {
            Destroy(other.gameObject);
            AddHealth(1);
        }

        if (other.CompareTag("ItemKey") && keys < GameConstants.GameMaxNumberOfKeys)
        {
            Destroy(other.gameObject);
            AddKey();
        }

        if (other.CompareTag("ItemMap"))
        {
            Destroy(other.gameObject);
            AddMap();
        }

        if (other.CompareTag("ItemTripleBulletAttack"))
        {
            Destroy(other.gameObject);
            AddTripleBulletAttack();
        }

        if (other.CompareTag("EnemyAttack"))
        {
            Destroy(other.gameObject);
            RemoveHealth(1);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) attack.Execute("PlayerAttack", transform.position, direction);
    }

    public void SwitchToIdleState()
    {
        playerStateMachine.SwitchState(new PlayerIdleState(this));
    }

    public void SwitchToMoveState()
    {
        playerStateMachine.SwitchState(new PlayerMoveState(this));
    }

    public void SwitchToDamageState()
    {
        playerStateMachine.SwitchState(new PlayerDamageState(this));
    }

    public void SwitchToDestroyState()
    {
        playerStateMachine.SwitchState(new PlayerDestroyState(this));
    }

    public void SwitchToFreezeState()
    {
        playerStateMachine.SwitchState(new PlayerFreezeState(this));
    }

    public Vector2 ReadInputActionMoveVector()
    {
        return playerInput.actions["Move"].ReadValue<Vector2>();
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

    private void AddHealth(int delta)
    {
        health = Mathf.Min(health + delta, playerData.Health);
    }

    private void RemoveHealth(int delta)
    {
        if (invincibilityCountdown > 0.0f) return;

        health = Mathf.Max(0, health - delta);

        // Game over.
        if (health == 0)
            SwitchToDestroyState();
        else
            SwitchToDamageState();
    }

    private void AddKey()
    {
        keys++;
    }

    public void RemoveKey()
    {
        keys--;
    }

    public void AddMap()
    {
        map = true;
        level.GetComponent<LevelController>().UpdateMiniMap();
    }

    public void AddTripleBulletAttack()
    {
        attack = Resources.Load<Attack>(GameConstants.ResourcesAttackFolder + "/TripleBulletAttack");
    }

    public void Damage()
    {
        animator.Play("Damage");

        invincibilityCountdown = InvincibilityDurationInSeconds;
    }

    public void Destroy()
    {
        animator.Play("Destroy");
        Destroy(gameObject, DestroyStateDurationInSeconds);
    }
}
