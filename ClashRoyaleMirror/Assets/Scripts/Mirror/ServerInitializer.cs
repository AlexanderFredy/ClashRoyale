using Mirror;
using ParrelSync;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerInitializer : MonoBehaviour
{
#if UNITY_SERVER || UNITY_EDITOR
    [Scene, SerializeField] private string _offlineServerScene;
    [Scene, SerializeField] private string _menuScene;
    [SerializeField] private MirrorMultiplayerManager _mirrorNetworkManagerPrefab;

    private void Start()
    {
#if UNITY_SERVER
        StartServer();
#else
    StartClient();
#endif
    }

    private void StartServer()
    {
        if (NetworkManager.singleton == false)
        {
            var manager = Instantiate(_mirrorNetworkManagerPrefab);
            manager.offlineScene = _offlineServerScene;
        }

        NetworkManager.singleton.StartServer();
    }

    private void StartClient()
    {
        if (ClonesManager.IsClone())
        {
            Debug.Log("This is a clone project.");
            // Get the custom argument for this clone project.  
            string customArgument = ClonesManager.GetArgument();
            int.TryParse(customArgument, out int id);
            UserInfo.Instance.SetID(id);
            SceneManager.LoadScene(_menuScene);
        }
        else
        {
            Debug.Log("This is the original project.");
        }
    }
#endif
}
