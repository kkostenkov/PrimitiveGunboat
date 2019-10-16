using System;
using UnityEngine;

public class SpaceStationController : MonoBehaviour, IDamageTaker
{
    [SerializeField]
    private ProjectileLauncher gun;
    [SerializeField]
    private int MaxHp;
    private int currentHp;
    private ICommandSource commandSource;

    public event Action<IDamageTaker> Killed;

    internal void Initialize(IAssetDispenser assetDispenser, ICommandSource commandSource,
        IScreenBoundsSchecker bounds)
    {
        this.commandSource = commandSource;
        gun.Initialize(assetDispenser, bounds);        
    }

    internal void Reset()
    {
        currentHp = MaxHp;
    }

    private void Update()
    {
        if (commandSource.HasCommand(CommandType.Fire))
        {
            var fireCommand = commandSource.GetLastCommand(CommandType.Fire);
            gun.IssueFireCommand(fireCommand);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        if (currentHp <= 0)
        {
            Killed?.Invoke(this);
        }
    }
}
