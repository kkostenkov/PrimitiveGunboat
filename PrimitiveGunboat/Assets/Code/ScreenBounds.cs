using System;
using UnityEngine;

public interface IScreenBoundsSchecker
{
    bool ValidateBounds(Vector3 pos);
}

public class ScreenBounds : IScreenBoundsSchecker
{
    private const float SCREEN_FRACT_OFFSET = 0.1f; // 10% of screen pixels

    private System.Random rand = new System.Random();
    private readonly Func<Vector3, Vector3> screenToWorld;
    private Vector3 screenCenter, minWorldOffsetCoords, maxWorldOffsetCoords;

    public ScreenBounds(Func<Vector3, Vector3> convertScreenToWorld)
    {
        this.screenToWorld = convertScreenToWorld;
        CacheOffsetCoords();
        screenCenter = screenToWorld(new Vector3(
            Screen.width / 2, 
            Screen.height / 2, 
            0));
    }

    private void CacheOffsetCoords()
    {
        var minScreenOffsetCoords = new Vector3
        (
            Screen.width * (-SCREEN_FRACT_OFFSET),
            Screen.height * (-SCREEN_FRACT_OFFSET),
            0
        );
        minWorldOffsetCoords = screenToWorld(minScreenOffsetCoords);
        var maxScreenOffsetCoords = new Vector3(
            Screen.width * (1 + SCREEN_FRACT_OFFSET),
            Screen.height * (1 + SCREEN_FRACT_OFFSET),
            0
            );
            
        maxWorldOffsetCoords = screenToWorld(maxScreenOffsetCoords);
    }

    public bool ValidateBounds(Vector3 pos)
    {
        return pos.x > minWorldOffsetCoords.x 
            && pos.x < maxWorldOffsetCoords.x
            && pos.z > minWorldOffsetCoords.z
            && pos.z < maxWorldOffsetCoords.z;
    }

    public Trajectory GetTrajectory()
    {
        var sidesMaxIndex = 3;
        var side = rand.Next(sidesMaxIndex);
        var otherSide = side == 0 ? sidesMaxIndex : rand.Next(sidesMaxIndex - 1);
        if (otherSide == side)
        {
            otherSide = sidesMaxIndex;
        }
        var from = GetSpawnPointAt((ScreenSide)side);
        var isPrecise = rand.Next(100) < Settings.ChanceOfPreciseEnemy;
        var to = isPrecise 
            ? screenCenter
            : GetSpawnPointAt((ScreenSide)otherSide);
        return new Trajectory(from, to);
    }

    public Vector3 GetSpawnPointAt(ScreenSide side)
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

        var result = screenToWorld(screenPos);
        switch (side)
        {
            case ScreenSide.Top:
                result.z = maxWorldOffsetCoords.z;
                break;
            case ScreenSide.Bottom:
                result.z = minWorldOffsetCoords.z;
                break;
            case ScreenSide.Left:
                result.x = minWorldOffsetCoords.x;
                break;
            case ScreenSide.Right:
                result.x = maxWorldOffsetCoords.x;
                break;
        }
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

public struct Trajectory
{
    public Vector3 From;
    public Vector3 To;

    public Trajectory(Vector3 from, Vector3 to)
    {
        this.From = from;
        this.To = to;
    }
}

