using UnityEngine;

[CreateAssetMenu(fileName = "TripleBulletAttack", menuName = "ScriptableObjects/TripleBulletAttack")]
public class TripleBulletAttack : Attack
{
    [SerializeField] private GameObject bullet;

    public override void Execute(string tag, Vector3 startPosition, Vector2 direction)
    {
        for (var i = 0; i < 3; i++)
        {
            GameObject attackBullet = Instantiate(bullet);
            attackBullet.GetComponent<BulletController>().Initialize(tag, startPosition,
                Quaternion.AngleAxis(-20.0f + i * 20.0f, Vector3.forward) * new Vector3(direction.x, direction.y, 0.0f));
        }
    }
}
