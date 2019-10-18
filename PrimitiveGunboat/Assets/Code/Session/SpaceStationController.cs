using System;
using UnityEngine;

public class SpaceStationController : MonoBehaviour, IDamageTaker
{
    [SerializeField]
    private ProjectileLauncher gun;
    [SerializeField]
    private VisualsRequest visualsRequest;
    [SerializeField]
    private int MaxHp;
    public int CurrentHp { get { return currentHp; } }
    private int currentHp;
    private ICommandSource commandSource;
    private ISessionEventsListener eventListener;

    public event Action<IDamageTaker> Killed;

    internal void Initialize(IAssetDispenser assetDispenser, ICommandSource commandSource,
        IScreenBoundsSchecker bounds, ISessionEventsListener eventListener)
    {
        this.commandSource = commandSource;
        this.eventListener = eventListener;
        gun.Initialize(assetDispenser, bounds, eventListener);   
        this.visualsRequest.RequestLoad(assetDispenser);
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
        eventListener.HealthSet(currentHp);
        if (currentHp < 0) //By design. "Player loses when HP is below 0" 
        {
            Killed?.Invoke(this);
        }
    }
}
