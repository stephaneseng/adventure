public class GameConstants
{
    public static readonly string ResourcesAttackFolder = "Data/Attack";
    public static readonly string ResourcesBlockFolder = "Room";
    public static readonly string ResourcesEnemyFolder = "Enemy";
    public static readonly string ResourcesItemFolder = "Item";
    public static readonly string ResourcesRoomFolder = "Room";
    public static readonly string ResourcesTileFolder = "Tiles";
    public static readonly string ResourcesUIFolder = "UI";

    public static readonly string ResourcesBlockName = "Block";
    public static readonly string ResourcesMiniMapCurrentRoomMaskName = "MiniMapCurrentRoomMask";
    public static readonly string ResourcesMiniMapEndRoomMaskName = "MiniMapEndRoomMask";
    public static readonly string ResourcesMiniMapRoomName = "MiniMapRoom";
    public static readonly string ResourcesMiniMapRoomExitName = "MiniMapRoomExit";
    public static readonly string ResourcesMiniMapStartRoomMaskName = "MiniMapStartRoomMask";
    public static readonly string ResourcesMiniMapUnvisitedRoomName = "MiniMapUnvisitedRoom";
    public static readonly string ResourcesRoomName = "Room";
    public static readonly string ResourcesWallUpLeftInnerName = "WallUpLeftInner";
    public static readonly string ResourcesWallUpRightInnerName = "WallUpRightInner";
    public static readonly string ResourcesWallDownRightInnerName = "WallDownRightInner";
    public static readonly string ResourcesWallDownLeftInnerName = "WallDownLeftInner";
    public static readonly string ResourcesGroundName = "Ground";

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
