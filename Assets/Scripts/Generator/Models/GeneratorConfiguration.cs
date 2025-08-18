public class GeneratorConfiguration
{
    /* Level */
    private readonly int levelMapWidthHeight;
    private readonly int levelStartRoomMargin;
    private readonly int levelNumberOfRoomsInSectionLowThreshold;
    private readonly int levelNumberOfRoomsInSectionHighThreshold;
    private readonly float levelNumberOfRoomsInSectionThresholdRatio;

    /* Room */
    private readonly int roomWidthHeight;
    private readonly int roomBlockSpawnMargin;
    private readonly int roomMinNumberOfBlocks;
    private readonly int roomMaxNumberOfBlocks;
    private readonly int roomEnemySpawnMargin;
    private readonly int roomMinNumberOfEnemies;
    private readonly int roomMaxNumberOfEnemies;

    public GeneratorConfiguration(int levelMapWidthHeight, int levelStartRoomMargin, int levelNumberOfRoomsInSectionLowThreshold,
        int levelNumberOfRoomsInSectionHighThreshold, float levelNumberOfRoomsInSectionThresholdRatio, int roomWidthHeight,
        int roomBlockSpawnMargin, int roomMinNumberOfBlocks, int roomMaxNumberOfBlocks, int roomEnemySpawnMargin,
        int roomMinNumberOfEnemies, int roomMaxNumberOfEnemies)
    {
        this.levelMapWidthHeight = levelMapWidthHeight;
        this.levelStartRoomMargin = levelStartRoomMargin;
        this.levelNumberOfRoomsInSectionLowThreshold = levelNumberOfRoomsInSectionLowThreshold;
        this.levelNumberOfRoomsInSectionHighThreshold = levelNumberOfRoomsInSectionHighThreshold;
        this.levelNumberOfRoomsInSectionThresholdRatio = levelNumberOfRoomsInSectionThresholdRatio;
        this.roomWidthHeight = roomWidthHeight;
        this.roomBlockSpawnMargin = roomBlockSpawnMargin;
        this.roomMinNumberOfBlocks = roomMinNumberOfBlocks;
        this.roomMaxNumberOfBlocks = roomMaxNumberOfBlocks;
        this.roomEnemySpawnMargin = roomEnemySpawnMargin;
        this.roomMinNumberOfEnemies = roomMinNumberOfEnemies;
        this.roomMaxNumberOfEnemies = roomMaxNumberOfEnemies;
    }

    public int LevelMapWidthHeight => levelMapWidthHeight;

    public int LevelStartRoomMargin => levelStartRoomMargin;

    public int LevelNumberOfRoomsInSectionLowThreshold => levelNumberOfRoomsInSectionLowThreshold;

    public int LevelNumberOfRoomsInSectionHighThreshold => levelNumberOfRoomsInSectionHighThreshold;

    public float LevelNumberOfRoomsInSectionThresholdRatio => levelNumberOfRoomsInSectionThresholdRatio;

    public int RoomWidthHeight => roomWidthHeight;

    public int RoomBlockSpawnMargin => roomBlockSpawnMargin;

    public int RoomMinNumberOfBlocks => roomMinNumberOfBlocks;

    public int RoomMaxNumberOfBlocks => roomMaxNumberOfBlocks;

    public int RoomEnemySpawnMargin => roomEnemySpawnMargin;

    public int RoomMinNumberOfEnemies => roomMinNumberOfEnemies;

    public int RoomMaxNumberOfEnemies => roomMaxNumberOfEnemies;
}
