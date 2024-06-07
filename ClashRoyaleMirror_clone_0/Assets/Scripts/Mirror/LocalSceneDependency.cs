using Mirror;
using System;
using UnityEngine;

public class LocalSceneDependency : MonoBehaviour
{
    private void Start()
    {
#if UNITY_SERVER
        StartServer();
#else
        StartClient();
#endif
    }

    #region SERVER
#if UNITY_SERVER
    [SerializeField] private GameObject _spawnerPrefab;
    [SerializeField] private GameObject _serverTimerPrefab;
    private void StartServer()
    {
        MatchmakingManager.Instance.AddNewSceneServer(this);
        var spawner = Instantiate(_spawnerPrefab, transform);
        NetworkServer.Spawn(spawner);

        var timer = Instantiate(_serverTimerPrefab, transform);
        NetworkServer.Spawn(timer);
        timer.GetComponent<ServerTimer>().StartTick();
    }

    public void InitServer(int sceneLevelNumber)
    {
        
    }
#endif
    #endregion

    #region CLIENT
    [field: SerializeField] public CardManager cardManager;
    [field: SerializeField] public StartTimer startTimer;
    private void StartClient()
    {
        MatchmakingManager.Instance.AddNewSceneClient(this);
    }

    #endregion
}