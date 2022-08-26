using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static float BaseSpeed = 4.0f;

    public GameObject bullet;

    private PlayerInput playerInput;
    private new Rigidbody2D rigidbody2D;

    private Vector2 direction;
    private float speed;
    private bool lockMove;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        direction = Vector2.up;
        speed = 0.0f;
        lockMove = false;
    }

    void Update()
    {
        Vector2 move = playerInput.actions["Move"].ReadValue<Vector2>();

        if (move.magnitude > 0 && !lockMove) {
            direction = move;
            speed = move.magnitude;
        } else {
            speed = 0.0f;
        }
    }

    void FixedUpdate()
    {
        rigidbody2D.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x, direction.y,
            0.0f));
        rigidbody2D.velocity = direction * speed * BaseSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyAttack")) {
            Destroy(gameObject);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) {
            Attack();
        }
    }

    private void Attack()
    {
        GameObject playerBullet = (GameObject) Instantiate(bullet, transform.position, transform.rotation,
            gameObject.transform);
        playerBullet.tag = "PlayerAttack";
        playerBullet.GetComponent<BulletController>().fromPosition = transform.position;
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
}