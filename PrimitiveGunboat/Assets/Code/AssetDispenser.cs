using System.Collections.Generic;
using UnityEngine;


public interface IAssetDispenser
{
    SpaceStationController GetSpaceStation();
    Torpedo TakeProjectile(Transform parent=null);
    void PutProjectile(Torpedo torpedo);
}


public class AssetDispenser : MonoBehaviour, IAssetDispenser
{
    [SerializeField]
    private SpaceStationController spaceStation;

    [SerializeField]
    private Torpedo torpedo;

    [SerializeField]
    private List<GameObject> enemies;

    public SpaceStationController GetSpaceStation()
    {
         return spaceStation;
    }
    public Torpedo TakeProjectile(Transform parent=null)
    {
        return GameObject.Instantiate(torpedo, parent);
    }

    public void PutProjectile(Torpedo torpedo)
    {
        torpedo.Defuse();
        GameObject.Destroy(torpedo);
    }
}
