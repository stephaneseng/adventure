using UnityEngine;

public abstract class Attack : ScriptableObject
{
    public virtual void Execute(string tag, Transform transform, Vector2 direction)
    {
    }
}
