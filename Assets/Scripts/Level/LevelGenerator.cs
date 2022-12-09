using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private GameObject level;
    private RoomFactory roomFactory;

    void Awake()
    {
        level = GameObject.FindGameObjectWithTag("Level");
        roomFactory = GetComponentInChildren<RoomFactory>();
    }

    public void Generate()
    {
        LevelConfiguration levelConfiguration = GenerateLevelConfiguration();
        level.GetComponent<LevelController>().levelConfiguration = levelConfiguration;

        InstantiateRooms(levelConfiguration.roomConfigurations, level);
    }

    private LevelConfiguration GenerateLevelConfiguration()
    {
        LevelConfiguration levelConfiguration = ScriptableObject.CreateInstance<LevelConfiguration>();

        RoomConfiguration roomConfiguration10 = ScriptableObject.CreateInstance<RoomConfiguration>();
        roomConfiguration10.type = RoomConfiguration.RoomType.Room;
        roomConfiguration10.position = new Vector2Int(1, 0);
        roomConfiguration10.upExit = true;
        levelConfiguration.roomConfigurations.Add(roomConfiguration10);

        RoomConfiguration roomConfiguration11 = ScriptableObject.CreateInstance<RoomConfiguration>();
        roomConfiguration11.type = RoomConfiguration.RoomType.Room1;
        roomConfiguration11.position = new Vector2Int(1, 1);
        roomConfiguration11.rightExit = true;
        roomConfiguration11.downExit = true;
        roomConfiguration11.leftExit = true;
        roomConfiguration11.rightDoor = true;
        roomConfiguration11.leftDoor = true;
        EnemyConfiguration enemyConfiguration111 = ScriptableObject.CreateInstance<EnemyConfiguration>();
        enemyConfiguration111.type = EnemyConfiguration.EnemyType.Enemy;
        roomConfiguration11.enemyConfigurations.Add(enemyConfiguration111);
        levelConfiguration.roomConfigurations.Add(roomConfiguration11);

        RoomConfiguration roomConfiguration01 = ScriptableObject.CreateInstance<RoomConfiguration>();
        roomConfiguration01.type = RoomConfiguration.RoomType.Room4;
        roomConfiguration01.position = new Vector2Int(0, 1);
        roomConfiguration01.rightExit = true;
        roomConfiguration01.rightDoor = true;
        EnemyConfiguration enemyConfiguration011 = ScriptableObject.CreateInstance<EnemyConfiguration>();
        enemyConfiguration011.type = EnemyConfiguration.EnemyType.Enemy;
        roomConfiguration01.enemyConfigurations.Add(enemyConfiguration011);
        EnemyConfiguration enemyConfiguration012 = ScriptableObject.CreateInstance<EnemyConfiguration>();
        enemyConfiguration012.type = EnemyConfiguration.EnemyType.Enemy;
        roomConfiguration01.enemyConfigurations.Add(enemyConfiguration012);
        EnemyConfiguration enemyConfiguration013 = ScriptableObject.CreateInstance<EnemyConfiguration>();
        enemyConfiguration013.type = EnemyConfiguration.EnemyType.Enemy;
        roomConfiguration01.enemyConfigurations.Add(enemyConfiguration013);
        EnemyConfiguration enemyConfiguration014 = ScriptableObject.CreateInstance<EnemyConfiguration>();
        enemyConfiguration014.type = EnemyConfiguration.EnemyType.Enemy;
        roomConfiguration01.enemyConfigurations.Add(enemyConfiguration014);
        levelConfiguration.roomConfigurations.Add(roomConfiguration01);

        RoomConfiguration roomConfiguration21 = ScriptableObject.CreateInstance<RoomConfiguration>();
        roomConfiguration21.type = RoomConfiguration.RoomType.Room4;
        roomConfiguration21.position = new Vector2Int(2, 1);
        roomConfiguration21.leftExit = true;
        levelConfiguration.roomConfigurations.Add(roomConfiguration21);

        levelConfiguration.startRoomPosition = new Vector2Int(1, 0);

        return levelConfiguration;
    }

    private void InstantiateRooms(List<RoomConfiguration> roomConfigurations, GameObject level)
    {
        roomConfigurations.ForEach(roomConfiguration =>
        {
            GameObject room = roomFactory.Instantiate(roomConfiguration, new Vector3(
                roomConfiguration.position.x * RoomController.RoomSize,
                roomConfiguration.position.y * RoomController.RoomSize, 0.0f),
                level.transform);
            room.name = "Room(" + roomConfiguration.position.x + "," + roomConfiguration.position.y + ")";

            // Disable rooms at their instantiation to avoid triggering colliders too early.
            room.SetActive(false);
        });
    }
}
