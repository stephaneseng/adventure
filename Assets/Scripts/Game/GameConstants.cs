public class GameConstants
{
    public static readonly string ResourceAttackFolder = "Data/Attack";
    public static readonly string ResourceAudioFolder = "Audio";
    public static readonly string ResourceEnemyFolder = "Enemy";
    public static readonly string ResourcesItemFolder = "Item";
    public static readonly string ResourceRoomFolder = "Room";
    public static readonly string ResourceTileFolder = "Tiles";
    public static readonly string ResourceUiFolder = "UI";

    public static readonly string ResourceAttackTripleBulletAttackName = "TripleBulletAttack";
    public static readonly string ResourceAudioAttackName = "Attack";
    public static readonly string ResourceAudioDestroyName = "Destroy";
    public static readonly string ResourceAudioDoorName = "Door";
    public static readonly string ResourceAudioItemName = "Item";
    public static readonly string ResourceRoomBlockName = "Block";
    public static readonly string ResourceRoomRoomName = "Room";
    public static readonly string ResourceTileGroundName = "Ground";
    public static readonly string ResourceTileWallUpLeftInnerName = "WallUpLeftInner";
    public static readonly string ResourceTileWallUpRightInnerName = "WallUpRightInner";
    public static readonly string ResourceTileWallDownRightInnerName = "WallDownRightInner";
    public static readonly string ResourceTileWallDownLeftInnerName = "WallDownLeftInner";
    public static readonly string ResourceUiMiniMapCurrentRoomMaskName = "MiniMapCurrentRoomMask";
    public static readonly string ResourceUiMiniMapEndRoomMaskName = "MiniMapEndRoomMask";
    public static readonly string ResourceUiMiniMapRoomName = "MiniMapRoom";
    public static readonly string ResourceUiMiniMapRoomExitName = "MiniMapRoomExit";
    public static readonly string ResourceUiMiniMapStartRoomMaskName = "MiniMapStartRoomMask";
    public static readonly string ResourceUiMiniMapUnvisitedRoomName = "MiniMapUnvisitedRoom";

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
