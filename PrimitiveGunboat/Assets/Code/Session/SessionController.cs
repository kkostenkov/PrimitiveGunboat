using UnityEngine;

public class SessionController : MonoBehaviour
{
    [SerializeField]
    private Camera sessionCam;
    [SerializeField]
    private EnemySpawner enemySpawner;
    private InputReader inputReader;
    private IAssetDispenser assetDispenser;

    private Transform sessionSpaceTransfrom;

    private SpaceStationController station;
    
    void Start()
    {
        var gamePlaneCoordY = 0;
        inputReader = new InputReader(sessionCam, gamePlaneCoordY);   
        sessionSpaceTransfrom = GetComponent<Transform>();
    }
    
    void Update()
    {
        CheckEndGame();
        inputReader.ReadFrameInputs();
        // spawner checks for additional enemy spawns
    }

    internal void Initialize(IAssetDispenser assetDispenser)
    {
        this.assetDispenser = assetDispenser;
    }

    internal void RunSession()
    {
        // spawn station
        var stationPrefab = assetDispenser.GetSpaceStation();
        var station = GameObject.Instantiate(stationPrefab, Vector3.zero, 
            Quaternion.identity, sessionSpaceTransfrom);
        station.Initialize(assetDispenser, inputReader);
        
        // measure screen size to sesison settings
        // init spawner
        enemySpawner.Initialize(assetDispenser);
        // prespawn enemies

    }

    private void CheckEndGame()
    {

    }
}
