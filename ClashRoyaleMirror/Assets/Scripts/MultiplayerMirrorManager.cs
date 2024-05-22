using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class MultiplayerMirrorManager : NetworkManager
{
    [SerializeField] private PlayerPrefab _palyerPrefab;

    [SerializeField, Scene] private string _onlineSceneName;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (Utils.IsSceneActive(_onlineSceneName))
        {
            var player = Instantiate(_palyerPrefab);
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
        }

        Debug.Log("OnServerAddPlayer");
    }

    public override void OnStartClient()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs").Where(x => x != playerPrefab).ToArray();
        
        for (int i = 0; i < prefabs.Length; i++)
        {
            NetworkClient.RegisterPrefab(prefabs[i]);
        }

        Debug.Log("OnStartClient (NetworkManager)");
    }
}
