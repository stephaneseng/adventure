using UnityEngine;

[CreateAssetMenu(fileName = "SingleBulletAttack", menuName = "ScriptableObjects/SingleBulletAttack")]
public class SingleBulletAttack : Attack
{
    public GameObject bullet;

    public override void Execute(string tag, Transform transform, Vector2 direction)
    {
        GameObject attackBullet = Instantiate(bullet, transform.position, transform.rotation, transform);
        attackBullet.tag = tag;
        attackBullet.GetComponent<BulletController>().startPosition = transform.position;
        attackBullet.GetComponent<BulletController>().direction = direction;
    }
}
