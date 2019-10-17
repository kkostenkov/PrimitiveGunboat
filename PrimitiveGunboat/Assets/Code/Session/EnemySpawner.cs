using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner
{
    private readonly IAssetDispenser assetDispenser;
    private readonly ScreenBounds boundsChecker;
    private readonly ISessionEventsListener eventListener;

    private Dictionary<string, HashSet<Enemy>> spawned = 
        new Dictionary<string, HashSet<Enemy>>();
    private Dictionary<string, int> spawnCapCount = new Dictionary<string, int>();

    private DateTime nextSpawnWaveTime;
    private int score = 0;
    

    public EnemySpawner(IAssetDispenser assetDispenser, ScreenBounds bounds, 
        ISessionEventsListener eventListener)
    {
        this.assetDispenser = assetDispenser;
        this.boundsChecker = bounds;
        this.eventListener = eventListener;

        foreach (var preset in Settings.SpawnSettings)
        {
            spawned[preset.GroupId] = new HashSet<Enemy>();
            spawnCapCount[preset.GroupId] = preset.StartCopiesCount;
        }
    }

    public void Reset()
    {
        var spawnedEnemies = new List<Enemy>();
        foreach (var kvp in spawned)
        {
            spawnedEnemies.AddRange(kvp.Value);
            kvp.Value.Clear();
        }
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            var enemy = spawnedEnemies[i];
            enemy.BoundsBroken -= OnEnemyOutOfScreen;
            enemy.Crashed -= OnEnemyOutOfScreen;
            enemy.Killed -= OnEnemyDie;
            assetDispenser.PutEnemy(enemy);
        }
        score = 0;
    }

    internal void SpawnWave()
    {
        foreach (var kvp in spawnCapCount)
        {
            var groupId = kvp.Key;
            var cap = kvp.Value;
            while (spawned[groupId].Count < cap)
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
        spawned[enemyGroupId].Add(enemy);
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
        spawned[enemy.GroupId].Remove(enemy);
        assetDispenser.PutEnemy(enemy);

        Spawn(enemyGroup);
    }

    private void OnEnemyDie(IDamageTaker damageTaker)
    {
        var enemy = damageTaker as Enemy;
        score += enemy.PointsValue;
        spawned[enemy.GroupId].Remove(enemy);
        Debug.Log("enemy killed");
        assetDispenser.PutEnemy(enemy);
        eventListener.ScoreSet(score);
    }
}