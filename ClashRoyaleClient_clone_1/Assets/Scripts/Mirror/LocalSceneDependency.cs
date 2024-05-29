using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
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
        MatchmakingMirrorManager.Instance.AddNewSceneServer(this);
    }

    public void InitServer(int sceneLevelNumber)
    {
        throw new NotImplementedException();
    }
#endif

    private void StartClient()
    {
        MatchmakingMirrorManager.Instance.AddNewSceneClient(this);
    }
}
