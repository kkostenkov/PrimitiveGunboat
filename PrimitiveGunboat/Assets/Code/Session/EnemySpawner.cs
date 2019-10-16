using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner
{
    private readonly IAssetDispenser assetDispenser;
    private readonly ScreenBounds boundsChecker;

    private Dictionary<string, int> spawnedCount = new Dictionary<string, int>();
    private Dictionary<string, int> spawnCapCount = new Dictionary<string, int>();

    private DateTime nextSpawnWaveTime;
    

    public EnemySpawner(IAssetDispenser assetDispenser, ScreenBounds bounds)
    {
        this.assetDispenser = assetDispenser;
        this.boundsChecker = bounds;

        foreach (var preset in Settings.SpawnSettings)
        {
            spawnedCount[preset.GroupId] = 0;
            spawnCapCount[preset.GroupId] = preset.StartCopiesCount;
        }
    }

    internal void SpawnWave()
    {
        foreach (var kvp in spawnCapCount)
        {
            var groupId = kvp.Key;
            var cap = kvp.Value;
            while (spawnedCount[groupId] < cap)
            {
                Spawn(groupId);
            }
        }
    }

    internal void TrySpawnMoreEnemies()
    {
        if (DateTime.UtcNow < nextSpawnWaveTime)
        {
            return;
        }
        SpawnWave();
    }

    private void Spawn(string enemyGroupId)
    {
        var enemy = assetDispenser.TakeEnemy(enemyGroupId);
        enemy.SetBounds(boundsChecker);
        enemy.BoundsBroken += OnEnemyOutOfScreen;
        enemy.Crashed += OnEnemyOutOfScreen;
        enemy.Killed += OnEnemyDie;
        var trajectory = boundsChecker.GetTrajectory();
        spawnedCount[enemyGroupId] += 1;
        nextSpawnWaveTime = DateTime.UtcNow.AddSeconds(Settings.EnemyWaveSpawnCooldown);
        enemy.Launch(trajectory.From, trajectory.To);
    }

    private void OnEnemyOutOfScreen(MovingObject movingObject)
    {
        var enemy = movingObject as Enemy;
        enemy.BoundsBroken -= OnEnemyOutOfScreen;
        enemy.Crashed -= OnEnemyOutOfScreen;
        enemy.Killed -= OnEnemyDie;
        var enemyGroup = enemy.GroupId;
        spawnedCount[enemy.GroupId] -= 1;

        Spawn(enemyGroup);

        assetDispenser.PutEnemy(enemy);
    }

    private void OnEnemyDie(IDamageTaker damageTaker)
    {
        var enemy = damageTaker as Enemy;
        spawnedCount[enemy.GroupId] -= 1;
        Debug.Log("enemy count dead");
        assetDispenser.PutEnemy(enemy);
    }
}