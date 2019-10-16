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

        if (boundsChecker == null)
        {
            boundsChecker = new ScreenBounds(sessionCam.ScreenToWorldPoint);
        }

        if (!sessionSpaceTransfrom)
        {
            sessionSpaceTransfrom = GetComponent<Transform>();
        }

        // spawn station
        var stationPrefab = assetDispenser.GetSpaceStation();
        var station = GameObject.Instantiate(stationPrefab, Vector3.zero, 
            Quaternion.identity, sessionSpaceTransfrom);
        station.Initialize(assetDispenser, inputReader, boundsChecker);

        if (enemySpawner == null)
        {
            enemySpawner = new EnemySpawner(assetDispenser, boundsChecker);
        }
        enemySpawner.Prespawn();
    }

    private void CheckEndGame()
    {

    }
}
