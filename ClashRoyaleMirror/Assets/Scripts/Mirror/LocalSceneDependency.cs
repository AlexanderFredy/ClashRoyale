using Mirror;
using UnityEngine;

public class LocalSceneDependency : MonoBehaviour
{
    public const float LiftingHeight = 10f;
    private void Start()
    {
#if UNITY_SERVER
        StartServer();
#else
        StartClient();
#endif
    }
    public Spawner spawner { get; private set; }
    public void SetSpawner(Spawner spawner) => this.spawner = spawner;

    #region SERVER
    [SerializeField] private Transform[] _playerTowerPoints;
    [SerializeField] private Transform[] _enemyTowerPoints;
    [SerializeField] private Tower _towerPrefab;
    [SerializeField] private Spawner _spawnerPrefab;
    [SerializeField] private ServerTimer _serverTimerPrefab;
    [SerializeField] private MapInfo _mapInfo;
#if UNITY_SERVER
    private PlayerPrefab _player1Player;
    private PlayerPrefab _player2Enemy;

    private void StartServer()
    {
        CreateSpawner();
        CreateTowers();
        CreateTimer();
    }
    private void CreateSpawner()
    {
        MatchmakingManager.Instance.AddNewSceneServer(this);
        this.spawner = Instantiate(_spawnerPrefab, transform);
        spawner.InitServer(cardManager, _mapInfo);
        NetworkServer.Spawn(spawner.gameObject);
    }
    private void CreateTowers()
    {
        for (int i = 0; i < _playerTowerPoints.Length; i++)
        {
            var point = _playerTowerPoints[i];
            var tower = Instantiate(_towerPrefab,point.position,point.rotation,point);
            _mapInfo.AddTower(tower, false);
            NetworkServer.Spawn(tower.gameObject);
        }

        for (int i = 0; i < _enemyTowerPoints.Length; i++)
        {
            var point = _enemyTowerPoints[i];
            var tower = Instantiate(_towerPrefab, point.position, point.rotation, point);
            _mapInfo.AddTower(tower, true);
            NetworkServer.Spawn(tower.gameObject);
        }
    }

    private void CreateTimer()
    {
        var timer = Instantiate(_serverTimerPrefab, transform);
        NetworkServer.Spawn(timer.gameObject);
        timer.StartTick();
    }

    

    public void InitServer(PlayerPrefab player1Player, PlayerPrefab player2Enemy, int sceneLevelNumber)
    {
        _player1Player = player1Player;
        _player2Enemy = player2Enemy;
        player1Player.SetLocalSceneDependency(this);
        player2Enemy.SetLocalSceneDependency(this);

        spawner.SetHeight(sceneLevelNumber * LiftingHeight);

        SetSceneHeight(sceneLevelNumber);
    }

    public bool IsEnemy(PlayerPrefab player) => player == _player2Enemy;

#endif
    #endregion

    [SerializeField] private Transform[] _objectsForLift;
    #region CLIENT
    
    [SerializeField] private Transform[] _spawnPlanes;
    [field: SerializeField] public CardManager cardManager;
    [field: SerializeField] public StartTimer startTimer;
    private void StartClient()
    {
        MatchmakingManager.Instance.AddNewSceneClient(this);
    }

    public void SetScene(int matchHeight, bool isEnemy)
    {
        if (isEnemy) SetEnemyScene();

        SetSceneHeight(matchHeight);
    }  

    private void SetEnemyScene()
    {
        Transform cameraTrans = Camera.main.transform;
        Vector3 cameraRotation = cameraTrans.eulerAngles;
        cameraRotation.y = 180;
        cameraTrans.eulerAngles = cameraRotation;
        Vector3 cameraPosition = cameraTrans.position;
        cameraPosition.z *= -1;
        cameraTrans.position = cameraPosition;

        for (int i = 0; i < _spawnPlanes.Length; i++)
        {
            Transform planeTrans = _spawnPlanes[i];
            Vector3 planeRotation = planeTrans.eulerAngles;
            planeRotation.y = 180;
            planeTrans.eulerAngles = planeRotation;
            Vector3 planePosition = planeTrans.position;
            planePosition.z *= -1;
            planeTrans.position = planePosition;
        }
    }

    #endregion
    private void SetSceneHeight(int levelNumber)
    {
        float y = levelNumber * LiftingHeight;

        Transform camera = Camera.main.transform;
        Vector3 cameraPosition = camera.position;
        cameraPosition.y += y;
        camera.position = cameraPosition;

        for (int i = 0; i < _objectsForLift.Length; i++)
        {
            Vector3 position = _objectsForLift[i].position;
            position.y = y;
            _objectsForLift[i].position = position;
        }
    }
}