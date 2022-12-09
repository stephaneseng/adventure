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
        if (!GameObject.FindGameObjectWithTag("Room"))
        {
            levelGenerator.Generate();
        }

        level.GetComponent<LevelController>().Initialize();
    }
}
