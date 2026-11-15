using UnityEngine;

public class KeyDefines : MonoBehaviour
{

    public class AudioKey
    {
        public const string EQUIP_SFX = "Equip SFX";

        public const string STEP_SFX1 = "Step SFX 1";
        public const string STEP_SFX2 = "Step SFX 2";

        public const string PARRYING_SFX = "Parrying SFX";
        public const string OPEN_GATE_SFX = "Open Gate SFX";

        public const string HIT_SFX = "Hit SFX";
        public const string DEAD_SFX = "Dead SFX";

        public const string GAME_BGM = "Game BGM";
    }


    public class ResourceKey
    {
        public static string[] START_ITEMS = { "10010_WE_001", "10020_0101" };

        public const string PLAYER_PREFAB = "Player Prefab";
        public const string PLAYER_DATA = "Player Data";

        public const string PARRYING_PARTICLE = "ParryingParticle";
    }


    public class Tag
    {
        public const string PLAYER_INVENTORY = "Player Inventory";
    }


    public class SaveKey
    {
        public const string PLAYER_INVEN_DATA = "Player Inven Data";
    }
}
