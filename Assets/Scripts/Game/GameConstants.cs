public class GameConstants
{
    public static readonly string ResourcesAttackFolder = "Data/Attack";

    // FIXME: Limit the number of keys the player can have due to UI constraints.
    public static readonly int GameMaxNumberOfKeys = 6;

    public static readonly int LevelMapWidthHeight = 10;
    public static readonly int LevelStartRoomMargin = 2;
    public static readonly int LevelNumberOfRoomsInSectionLowThreshold = 3;
    public static readonly int LevelNumberOfRoomsInSectionHighThreshold = 8;
    public static readonly float LevelNumberOfRoomsInSectionThresholdRatio = 0.9f;

    public static readonly int RoomWidthHeight = 10;
    public static readonly int RoomBlockSpawnMargin = 1;
    public static readonly int RoomMinNumberOfBlocks = 5;
    public static readonly int RoomMaxNumberOfBlocks = 10;
    public static readonly int RoomEnemySpawnMargin = 2;
    public static readonly int RoomMinNumberOfEnemies = 0;
    public static readonly int RoomMaxNumberOfEnemies = 5;
}
