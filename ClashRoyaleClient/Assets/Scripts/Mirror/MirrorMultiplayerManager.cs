using Mirror;
using System.Linq;
using UnityEngine;

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

        for (int i = 0; i < prefabs.Length; i++)
        {
            NetworkClient.RegisterPrefab(prefabs[i]);
        }
    }
    #endregion
}
