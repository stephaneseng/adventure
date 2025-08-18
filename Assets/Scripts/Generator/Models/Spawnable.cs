using UnityEngine;

public abstract class Spawnable
{
    protected Vector2Int position;

    protected Spawnable(Vector2Int position)
    {
        this.position = position;
    }

    public Vector2Int Position => position;
}
