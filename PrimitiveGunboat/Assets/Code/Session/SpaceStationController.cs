using UnityEngine;

public class SpaceStationController : MonoBehaviour
{
    [SerializeField]
    private ProjectileLauncher gun;
    private ICommandSource commandSource;
    

    internal void Initialize(IAssetDispenser assetDispenser, ICommandSource commandSource,
        IScreenBoundsSchecker bounds)
    {
        this.commandSource = commandSource;
        gun.Initialize(assetDispenser, bounds);
    }

    private void Update()
    {
        if (commandSource.HasCommand(CommandType.Fire))
        {
            var fireCommand = commandSource.GetLastCommand(CommandType.Fire);
            gun.IssueFireCommand(fireCommand);
        }
    }
}
