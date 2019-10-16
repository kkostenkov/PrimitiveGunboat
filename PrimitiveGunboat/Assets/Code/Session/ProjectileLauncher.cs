using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    private IAssetDispenser assetDispenser;
    private IScreenBoundsSchecker boundsChecker;
    private Transform launchPoint;

    private Queue<InputCommand> firingQueue;
    private float launcherCooldown;

    public void Initialize(IAssetDispenser assetDispenser, IScreenBoundsSchecker bounds)
    {
        launchPoint = GetComponent<Transform>();

        firingQueue = new Queue<InputCommand>(Settings.FiringQueueLimit);

        this.assetDispenser = assetDispenser;
        boundsChecker = bounds;
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
        torpedo.SetBounds(boundsChecker);
        torpedo.BoundsBroken += OnTorpedoDie;
        torpedo.Died += OnTorpedoDie;
        torpedo.Launch(launchPoint.position, cmd.Coords);

        launcherCooldown = Settings.LauncherCooldown;
    }

    private void OnTorpedoDie(MovingObject movingObject)
    {
        var torpedo = movingObject as Torpedo;
        torpedo.BoundsBroken -= OnTorpedoDie;
        torpedo.Died -= OnTorpedoDie;
        assetDispenser.PutProjectile(torpedo);
    }
}
