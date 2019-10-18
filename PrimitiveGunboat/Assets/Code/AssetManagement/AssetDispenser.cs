using System;
using System.Collections.Generic;
using UnityEngine;


public interface IAssetDispenser
{
    SpaceStationController GetSpaceStation();
    Torpedo TakeProjectile();
    void PutProjectile(Torpedo torpedo);
    Enemy TakeEnemy(string gruipId);
    void PutEnemy(Enemy enemy);
    void LoadAsync(List<GameObject> assetsToLoad, Action<GameObject> cb);
}


public class AssetDispenser : MonoBehaviour, IAssetDispenser
{
    [SerializeField]
    private SpaceStationController spaceStation;

    [SerializeField]
    private Torpedo torpedo;
    private const string TORPEDO_POOL = "torpedoes";

    [SerializeField]
    private List<Enemy> enemies;

    private AssetPool pools;
    private AsyncLoader loader;

    public void LoadAsync(List<GameObject> assetsToLoad, Action<GameObject> cb)
    {
        loader.Load(assetsToLoad, cb);
    }

    private void Awake()
    {
        pools = new AssetPool();
        if (torpedo)
        {
            pools.CreatePool(TORPEDO_POOL, torpedo.gameObject);
        }
        
        foreach (var enemy in enemies)
        {
            pools.CreatePool(enemy.GroupId, enemy.gameObject);
        }
        loader = new AsyncLoader(this);
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
        torpedo.Release();
        pools.PutTo(TORPEDO_POOL, torpedo.gameObject);
    }

    public Enemy TakeEnemy(string groupId)
    {
        var go = pools.TakeFrom(groupId);
        return go.GetComponent<Enemy>();
    }

    public void PutEnemy(Enemy enemy)
    {
        enemy.Release();
        pools.PutTo(enemy.GroupId, enemy.gameObject);
    }
}
