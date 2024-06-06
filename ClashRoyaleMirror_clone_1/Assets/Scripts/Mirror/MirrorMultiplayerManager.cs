using Mirror;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MirrorMultiplayerManager : NetworkManager
{
    #region SERVER
    [SerializeField] private PlayerPrefab _playerPrefab;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (Utils.IsSceneActive(singleton.onlineScene))
        {
            var player = Instantiate(_playerPrefab);
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
        }
    }
    #endregion

    #region CLIENT
    public override void OnStartClient()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs").Where(x => x != singleton.playerPrefab).ToArray();
        print("pref " + prefabs.Length);
        for (int i = 0; i < prefabs.Length; i++)
        {
            NetworkClient.RegisterPrefab(prefabs[i]);
        }
    }

    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {
        if (sceneOperation == SceneOperation.LoadAdditive) StartCoroutine(LoadAdditive(newSceneName));
        if (sceneOperation == SceneOperation.UnloadAdditive) StartCoroutine(UnloadAdditive(newSceneName));
    }

    private IEnumerator LoadAdditive(string sceneName)
    {
        if (mode == NetworkManagerMode.ClientOnly)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        NetworkClient.isLoadingScene = false;
        OnClientSceneChanged();
    }

    private IEnumerator UnloadAdditive(string sceneName)
    {
        if (mode == NetworkManagerMode.ClientOnly)
        {
            yield return SceneManager.UnloadSceneAsync(sceneName);
            yield return Resources.UnloadUnusedAssets();
        }

        NetworkClient.isLoadingScene = false;
        OnClientSceneChanged();
    }
    #endregion
}
