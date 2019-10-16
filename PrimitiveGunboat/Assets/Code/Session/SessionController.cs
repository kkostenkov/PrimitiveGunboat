using UnityEngine;

public class SessionController : MonoBehaviour
{
    [SerializeField]
    private Camera sessionCam;
    private EnemySpawner enemySpawner;
    private ScreenBounds boundsChecker;
    private InputReader inputReader;
    private IAssetDispenser assetDispenser;

    private Transform sessionSpaceTransfrom;

    private SpaceStationController station;
    
    void Update()
    {
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

        if (boundsChecker == null)
        {
            boundsChecker = new ScreenBounds(sessionCam.ScreenToWorldPoint);
        }

        if (!sessionSpaceTransfrom)
        {
            sessionSpaceTransfrom = GetComponent<Transform>();
        }

        // spawn station
        if (!station)
        {
            var stationPrefab = assetDispenser.GetSpaceStation();
            station = GameObject.Instantiate(stationPrefab, Vector3.zero, 
                Quaternion.identity, sessionSpaceTransfrom);
            station.Killed += OnStationKilled;
            station.Initialize(assetDispenser, inputReader, boundsChecker);
        }
        station.Reset();
        

        if (enemySpawner == null)
        {
            enemySpawner = new EnemySpawner(assetDispenser, boundsChecker);
        }
        enemySpawner.SpawnWave();
    }

    private void OnStationKilled(IDamageTaker station)
    {
        Debug.Log("gameover");
    }
}
