using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private IAssetDispenser assetDispenser;

    internal void Initialize(IAssetDispenser assetDispenser)
    {
        this.assetDispenser = assetDispenser;
    }
}
