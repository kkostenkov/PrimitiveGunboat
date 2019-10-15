using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    private IAssetDispenser assetDispenser;
    private Transform launchPoint;
    private Vector3 launchPosition;

    private Queue<InputCommand> firingQueue;
    private float launcherCooldown;

    public void Initialize(IAssetDispenser assetDispenser)
    {
        launchPoint = GetComponent<Transform>();
        var pos = launchPoint.position;
        launchPosition = new Vector3(pos.x, 0, pos.z);

        firingQueue = new Queue<InputCommand>(Settings.FiringQueueLimit);

        this.assetDispenser = assetDispenser;
    }

    public bool IssueFireCommand(InputCommand cmd)
    {
        var canEnqueue = firingQueue.Count < Settings.FiringQueueLimit;
        if (canEnqueue)
        {
            firingQueue.Enqueue(cmd);
        }
        return canEnqueue;
    }

    private void Update()
    {
        if (launcherCooldown > 0)
        {
            launcherCooldown -= Time.deltaTime;
        }
        
        var canFire = launcherCooldown <= 0 && firingQueue.Count > 0;
        if (canFire)
        {
            var fireCommand = firingQueue.Dequeue();
            LaunchTorpedo(fireCommand);
        }
    }

    private void LaunchTorpedo(InputCommand cmd)
    {
        var torpedo = assetDispenser.TakeProjectile();

        torpedo.Died += OnTorpedoDie;
        torpedo.Launch(launchPosition, cmd.Coords);

        launcherCooldown = Settings.LauncherCooldown;
    }

    private void OnTorpedoDie(Torpedo torpedo)
    {
        torpedo.Died -= OnTorpedoDie;
        assetDispenser.PutProjectile(torpedo);
    }
}
