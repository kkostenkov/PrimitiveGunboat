using System;
using UnityEngine;

public class EnemySpawner
{
    private readonly IAssetDispenser assetDispenser;
    private readonly ScreenBounds boundsChecker;
    

    public EnemySpawner(IAssetDispenser assetDispenser, ScreenBounds bounds)
    {
        this.assetDispenser = assetDispenser;
        this.boundsChecker = bounds;
    }

    internal void Prespawn()
    {
        foreach (var preset in Settings.SpawnSettings)
        {
            for (int i = 0; i < preset.MaxCopiesCount; i++)
            {
                Spawn(preset.GroupId);
            }
        }
    }

    internal void TrySpawnMoreEnemies()
    {
        
    }

    private void Spawn(string enemyGroupId)
    {
        var enemy = assetDispenser.TakeEnemy(enemyGroupId);
        enemy.SetBounds(boundsChecker);
        enemy.BoundsBroken += OnEnemyDie;
        var trajectory = boundsChecker.GetTrajectory();
        enemy.Launch(trajectory.From, trajectory.To);
    }

    private void OnEnemyDie(MovingObject movingObject)
    {
        var enemy = movingObject as Enemy;
        enemy.BoundsBroken -= OnEnemyDie;
        var enemyGroup = enemy.GroupId;

        Spawn(enemyGroup);

        assetDispenser.PutEnemy(enemy);
    }
}