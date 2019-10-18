using System.Collections.Generic;

public static class Settings
{
    internal static float TorpedoSpeed = 10f;
    // by design "keep track of ALL firing actions"
    // just sanity check
    internal static int FiringQueueLimit = 10; 
    internal static float LauncherCooldown = 0.5f; // seconds
    internal static float EnemyWaveSpawnCooldown = 0.5f; // seconds
    internal static float ChanceOfPreciseEnemy = 5; // percent

    internal static bool ImitateLongerLoadingTime = false;

    internal static List<GroupSpawnSettings> SpawnSettings = new List<GroupSpawnSettings>()
    {
        new GroupSpawnSettings() {
            GroupId = "PassingByCube",
            StartCopiesCount = 10,
        },
        new GroupSpawnSettings() {
            GroupId = "Ufo",
            StartCopiesCount = 1,
        }
    };
}

public struct GroupSpawnSettings
{
    public string GroupId;
    public int StartCopiesCount;
}
