using UnityEngine;

[CreateAssetMenu(fileName = "TripleBulletAttack", menuName = "ScriptableObjects/TripleBulletAttack")]
public class TripleBulletAttack : Attack
{
    public GameObject bullet;

    public override void Execute(string tag, Transform transform, Vector2 direction)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject attackBullet = Instantiate(bullet, transform.position, transform.rotation, transform);
            attackBullet.tag = tag;
            attackBullet.GetComponent<BulletController>().startPosition = transform.position;
            attackBullet.GetComponent<BulletController>().direction = Quaternion.AngleAxis(-20.0f + i * 20.0f, Vector3.forward) * new Vector3(direction.x, direction.y, 0.0f);
        }
    }
}
