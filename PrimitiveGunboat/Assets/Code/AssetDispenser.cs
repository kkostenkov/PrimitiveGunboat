using System.Collections.Generic;
using UnityEngine;


public interface IAssetDispenser
{
    SpaceStationController GetSpaceStation();
    Torpedo TakeProjectile();
    void PutProjectile(Torpedo torpedo);
}


public class AssetDispenser : MonoBehaviour, IAssetDispenser
{
    [SerializeField]
    private SpaceStationController spaceStation;

    [SerializeField]
    private Torpedo torpedo;
    private const string TORPEDO_POOL = "torpedoes";

    [SerializeField]
    private List<GameObject> enemies;

    private AssetPool pools;
    private void Awake()
    {
        pools = new AssetPool();
        pools.CreatePool(TORPEDO_POOL, torpedo.gameObject);
    }

    public SpaceStationController GetSpaceStation()
    {
         return spaceStation;
    }

    public Torpedo TakeProjectile()
    {
        return pools.TakeFrom(TORPEDO_POOL).GetComponent<Torpedo>();
    }

    public void PutProjectile(Torpedo torpedo)
    {
        torpedo.Defuse();
        pools.PutTo(TORPEDO_POOL, torpedo.gameObject);
    }
}
