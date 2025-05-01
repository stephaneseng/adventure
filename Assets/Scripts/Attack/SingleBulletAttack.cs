using UnityEngine;

[CreateAssetMenu(fileName = "SingleBulletAttack", menuName = "ScriptableObjects/SingleBulletAttack")]
public class SingleBulletAttack : Attack
{
    [SerializeField] private GameObject bullet;

    public override void Execute(string tag, Vector3 startPosition, Vector2 direction)
    {
        GameObject attackBullet = Instantiate(bullet);
        attackBullet.GetComponent<BulletController>().Initialize(tag, startPosition, direction);
    }
}
