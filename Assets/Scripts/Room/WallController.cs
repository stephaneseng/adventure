using UnityEngine;

public class WallController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack") || other.CompareTag("EnemyAttack"))
        {
            Destroy(other.gameObject);
        }
    }
}
