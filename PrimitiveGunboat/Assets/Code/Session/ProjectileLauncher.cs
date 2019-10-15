using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    private IAssetDispenser assetDispenser;
    private Transform launchPoint;
    private Vector3 launchPosition;
    private List<Torpedo> launchedTorpedoes = new List<Torpedo>(20);

    private void Start()
    {
        launchPoint = GetComponent<Transform>();
        var pos = launchPoint.position;
        launchPosition = new Vector3(pos.x, 0, pos.z);
    }

    private void Update()
    {
        CleanTorpedoes();
    }

    public void Initialize(IAssetDispenser assetDispenser)
    {
        this.assetDispenser = assetDispenser;
    }

    public void Fire(InputCommand fireCommand)
    {
        var torpedo = assetDispenser.TakeProjectile(launchPoint);
        torpedo.Launch(launchPosition, fireCommand.Coords);
        launchedTorpedoes.Add(torpedo);
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
}
