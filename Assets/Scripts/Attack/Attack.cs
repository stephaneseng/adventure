using UnityEngine;

public abstract class Attack : ScriptableObject
{
    public virtual void Execute(string tag, Vector3 startPosition, Vector2 direction)
    {
    }
}
