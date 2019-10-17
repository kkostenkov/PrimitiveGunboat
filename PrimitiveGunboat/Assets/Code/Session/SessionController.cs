using System;
using UnityEngine;

public class SessionController : MonoBehaviour, ISessionPlayer
{
    [SerializeField]
    private Camera sessionCam;
    private EnemySpawner enemySpawner;
    private ScreenBounds boundsChecker;
    private InputReader inputReader;
    private IAssetDispenser assetDispenser;
    private Transform sessionSpaceTransfrom;
    private SpaceStationController station;

    private SessionEvents sessionEvents = new SessionEvents();
    public ISessionEventsProvider SessionEventsProvider => sessionEvents;

    public int LastScore => enemySpawner.LastScore;
    public int CurrentScore => enemySpawner.Score;
    public int TopScore => enemySpawner.TopScore;

    public int CurrentHealth => station.CurrentHp;

    private bool isSessionActive = false;

    public event Action GameOver;

    void Update()
    {
        if (!isSessionActive)
        {
            return;
        }
        inputReader.ReadFrameInputs();
        enemySpawner.TrySpawnMoreEnemies();
    }

    internal void Initialize(IAssetDispenser assetDispenser)
    {
        this.assetDispenser = assetDispenser;
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

        if (!station)
        {
            var stationPrefab = assetDispenser.GetSpaceStation();
            station = GameObject.Instantiate(stationPrefab, Vector3.zero, 
                Quaternion.identity, sessionSpaceTransfrom);
            station.Killed += OnStationKilled;
            station.Initialize(assetDispenser, inputReader, boundsChecker, sessionEvents);
        }

        if (enemySpawner == null)
        {
            enemySpawner = new EnemySpawner(assetDispenser, boundsChecker, sessionEvents);
        }
    }

    public void Play()
    {
        station.Reset();
        isSessionActive = true;
        enemySpawner.Reset();
        enemySpawner.SpawnWave();
    }

    private void OnStationKilled(IDamageTaker station)
    {
        isSessionActive = false;
        Debug.Log("gameover");
        enemySpawner.Reset();
        GameOver?.Invoke();
    }
}
