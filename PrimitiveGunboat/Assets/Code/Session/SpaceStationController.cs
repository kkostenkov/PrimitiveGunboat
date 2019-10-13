using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationController : MonoBehaviour
{
    private IAssetDispenser assetDispenser;
    private ICommandSource commandSource;
    private Transform launchPoint;

    private List<Torpedo> launchedTorpedoes = new List<Torpedo>(20);

    private void Start()
    {
        launchPoint = GetComponent<Transform>();
    }

    private void Update()
    {
        if (commandSource.HasCommand(CommandType.Fire))
        {
            var fireCommand = commandSource.GetLastCommand(CommandType.Fire);
            FireProjectile(fireCommand);
        }
        
        CleanTorpedoes();
    }


    private void CleanTorpedoes()
    {
        for (int i = 0; i < launchedTorpedoes.Count; i++)
        {
            var torpedo = launchedTorpedoes[i];
            if (torpedo.IsAlive)
            {
                continue;
            }
            assetDispenser.PutProjectile(torpedo);
        }
    }

    private void FireProjectile(InputCommand fireCommand)
    {
        var torpedo = assetDispenser.TakeProjectile(launchPoint);
        torpedo.Launch(launchPoint.position, fireCommand.Coords);
        launchedTorpedoes.Add(torpedo);
    }

    internal void Initialize(IAssetDispenser assetDispenser, ICommandSource commandSource)
    {
        this.commandSource = commandSource;
        this.assetDispenser = assetDispenser;
    }
}
