using UnityEngine;

public class SpaceStationController : MonoBehaviour
{
    [SerializeField]
    private ProjectileLauncher gun;
    private ICommandSource commandSource;
    

    internal void Initialize(IAssetDispenser assetDispenser, ICommandSource commandSource)
    {
        this.commandSource = commandSource;
        gun.Initialize(assetDispenser);
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
