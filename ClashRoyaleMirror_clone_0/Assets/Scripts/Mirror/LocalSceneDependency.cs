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

#if UNITY_SERVER
    private void StartServer()
    {
        MatchmakingManager.Instance.AddNewSceneServer(this);
    }

    public void InitServer(int sceneLevelNumber)
    {
   
    }
#endif

    private void StartClient()
    {
        MatchmakingManager.Instance.AddNewSceneClient(this);
    }
}