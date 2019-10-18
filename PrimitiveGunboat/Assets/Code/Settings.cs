using System.Collections.Generic;

public static class Settings
{
    internal static float TorpedoSpeed = 10f;
    internal static int FiringQueueLimit = 100;
    internal static float LauncherCooldown = 0.5f; // seconds
    internal static float EnemyWaveSpawnCooldown = 0.5f; // seconds
    internal static float ChanceOfPreciseEnemy = 5; // percent

    internal static bool ImitateLongerLoadingTime = true;

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
