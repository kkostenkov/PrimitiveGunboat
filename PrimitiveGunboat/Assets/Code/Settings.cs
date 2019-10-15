using System.Collections.Generic;

public static class Settings
{
    internal static float TorpedoSpeed = 10f;
    internal static float TorpedoLifetime = 5f; // seconds
    internal static int FiringQueueLimit = 100;
    internal static float LauncherCooldown = 1f; // seconds

    internal static List<GroupSpawnPreset> SpawnSettings = new List<GroupSpawnPreset>()
    {
        new GroupSpawnPreset() {
            GroupId = "PassingByCube",
            MaxCopiesCount = 10,
        }
    };
}

public struct GroupSpawnPreset
{
    public string GroupId;
    public int MaxCopiesCount;
}
