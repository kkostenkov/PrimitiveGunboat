using UnityEngine;

public class SessionController : MonoBehaviour
{
    [SerializeField]
    private Camera sessionCam;
    private EnemySpawner enemySpawner;
    private InputReader inputReader;
    private IAssetDispenser assetDispenser;

    private Transform sessionSpaceTransfrom;

    private SpaceStationController station;
    
    void Update()
    {
        CheckEndGame();
        inputReader.ReadFrameInputs();
        enemySpawner.TrySpawnMoreEnemies();
    }

    internal void Initialize(IAssetDispenser assetDispenser)
    {
        this.assetDispenser = assetDispenser;
    }

    internal void RunSession()
    {
        if (inputReader == null)
        {
            inputReader = new InputReader(sessionCam.ScreenToWorldPoint);   
        }

        if (!sessionSpaceTransfrom)
        {
            sessionSpaceTransfrom = GetComponent<Transform>();
        }

        // spawn station
        var stationPrefab = assetDispenser.GetSpaceStation();
        var station = GameObject.Instantiate(stationPrefab, Vector3.zero, 
            Quaternion.identity, sessionSpaceTransfrom);
        station.Initialize(assetDispenser, inputReader);
        
        if (enemySpawner == null)
        {
            enemySpawner = new EnemySpawner(assetDispenser, sessionCam.ScreenToWorldPoint);
        }
        enemySpawner.Prespawn();
    }

    private void CheckEndGame()
    {

    }
}
