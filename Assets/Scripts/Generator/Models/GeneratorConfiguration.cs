public class GeneratorConfiguration
{
    /* Level */
    public int mapWidthHeight;
    public int startRoomMargin;
    public int numberOfRoomsInSectionLowThreshold;
    public int numberOfRoomsInSectionHighThreshold;
    public float numberOfRoomsInSectionThresholdRatio;

    /* Room */
    public int roomWidthHeight;
    public int blockSpawnMargin;
    public int minNumberOfBlocks;
    public int maxNumberOfBlocks;
    public int enemySpawnMargin;
    public int minNumberOfEnemies;
    public int maxNumberOfEnemies;

    public GeneratorConfiguration(int mapWidthHeight, int startRoomMargin, int numberOfRoomsInSectionLowThreshold,
        int numberOfRoomsInSectionHighThreshold, float numberOfRoomsInSectionThresholdRatio, int roomWidthHeight,
        int blockSpawnMargin, int minNumberOfBlocks, int maxNumberOfBlocks, int enemySpawnMargin,
        int minNumberOfEnemies, int maxNumberOfEnemies)
    {
        this.mapWidthHeight = mapWidthHeight;
        this.startRoomMargin = startRoomMargin;
        this.numberOfRoomsInSectionLowThreshold = numberOfRoomsInSectionLowThreshold;
        this.numberOfRoomsInSectionHighThreshold = numberOfRoomsInSectionHighThreshold;
        this.numberOfRoomsInSectionThresholdRatio = numberOfRoomsInSectionThresholdRatio;
        this.roomWidthHeight = roomWidthHeight;
        this.blockSpawnMargin = blockSpawnMargin;
        this.minNumberOfBlocks = minNumberOfBlocks;
        this.maxNumberOfBlocks = maxNumberOfBlocks;
        this.enemySpawnMargin = enemySpawnMargin;
        this.minNumberOfEnemies = minNumberOfEnemies;
        this.maxNumberOfEnemies = maxNumberOfEnemies;
    }
}
