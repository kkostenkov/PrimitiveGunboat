using System;
using UnityEngine;

public class EnemySpawner
{
    private System.Random rand = new System.Random();
    private readonly IAssetDispenser assetDispenser;
    private readonly Func<Vector3, Vector3> screenToWorld;

    public EnemySpawner(IAssetDispenser assetDispenser, 
        Func<Vector3, Vector3> convertScreenToWorld)
    {
        this.assetDispenser = assetDispenser;
        this.screenToWorld = convertScreenToWorld;
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
        var sidesMaxIndex = 3;
        var side = rand.Next(sidesMaxIndex);
        var otherSide = rand.Next(sidesMaxIndex - 1);
        if (otherSide == side)
        {
            otherSide = sidesMaxIndex;
        }
        var launchCoords = GetSpawnPointAt((ScreenSide)side);
        var targetCoords = GetSpawnPointAt((ScreenSide)otherSide);
        enemy.Launch(launchCoords, targetCoords);
    }


    private const float SCREEN_FRACT_OFFSET = 0.1f; // 10% of screen pixels

    private Vector3 GetSpawnPointAt(ScreenSide side)
    {
        var topOrBot = side == ScreenSide.Top || side == ScreenSide.Bottom;
        var sideSize = topOrBot ? Screen.width : Screen.height;
        var randScreenPos = rand.Next(sideSize);

        var screenPos = Vector3.zero;
        if (topOrBot)
        {
            screenPos.x = randScreenPos;
        }
        else
        {
            screenPos.y = randScreenPos;
        }
        
        switch (side)
        {
            case ScreenSide.Top:
                screenPos.y = Screen.height * (1 + SCREEN_FRACT_OFFSET);
                break;
            case ScreenSide.Bottom:
                screenPos.y = Screen.height * (-SCREEN_FRACT_OFFSET);
                break;
            case ScreenSide.Left:
                screenPos.x = Screen.width * (-SCREEN_FRACT_OFFSET);
                break;
            case ScreenSide.Right:
                screenPos.x = Screen.width * (1 + SCREEN_FRACT_OFFSET);
                break;
        }

        var result = screenToWorld(screenPos);
        return result;
    }
}

public enum ScreenSide
{
    Top = 0,
    Right = 1,
    Bottom = 2,
    Left = 3,
}
