using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject level;
    private LevelGenerator levelGenerator;

    void Awake()
    {
        level = GameObject.FindGameObjectWithTag("Level");
        levelGenerator = GetComponentInChildren<LevelGenerator>();
    }

    void Start()
    {
        // To simplify room mechanics debugging, the level generator is not called if a room has been manually added.
        if (!GameObject.FindGameObjectWithTag("Room"))
        {
            levelGenerator.Generate();
        }

        level.GetComponent<LevelController>().Initialize();
    }
}
