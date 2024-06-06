using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchmakingManager : Singleton<MatchmakingManager>
{
    [SerializeField] private MatchmakingUI _matchmakingMirrorUI;

    #region CLIENT
    [Client]
    public void ConnectedFinish(string[] cardIDs)
    {
        _matchmakingMirrorUI.SetImages(cardIDs);
    }

    public LocalSceneDependency localSceneDependency { get; private set; }

    public void AddNewSceneClient(LocalSceneDependency localSceneDependency)
    {
        this.localSceneDependency = localSceneDependency;
    }

    #endregion

    #region SERVER

#if UNITY_SERVER
    public class StringArray
    {
        public string[] arr;
    }

    private const string SECRET_KEY = "wlel383783@%!kvkw09!";
    private const string KEY = "key";
    private const string USERID = "userID";

    [Scene, SerializeField] private string _additiveGameScene;
    private List<PlayerPrefab> _players = new List<PlayerPrefab>();
    private Queue<LocalSceneDependency> _localSceneDependencyQueue = new();

    [Server]
    public void OnJoint(PlayerPrefab player, string sqlID)
    {
        string uri = URILibrary.MAIN + URILibrary.GETDECK;
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {KEY, SECRET_KEY },
            {USERID, sqlID }
        };

        WebRequestToMySQL.Instance.StartPost(uri, data, (arrString) => SuccsessLoadDeck(sqlID, player, arrString));
    }

    private void SuccsessLoadDeck(string sqlID, PlayerPrefab player, string arrString)
    {
        string json = "{\"arr\":" + arrString + "}";
        string[] cardsId = JsonUtility.FromJson<StringArray>(json).arr;
        if (player == null)
        {
            Debug.LogWarning("Player leave match");
            return;
        }

        player.SetSqlId(sqlID);
        player.SuccessConnected(cardsId);

        if (_players.Count == 0)
        {
            _players.Add(player);
            return;
        }

        PlayerPrefab secondPlayer = _players[0];
        _players.RemoveAt(0);

        StartCoroutine(StartMatch(player, secondPlayer));
    }

    private IEnumerator StartMatch(PlayerPrefab player1, PlayerPrefab player2)
    {
        yield return SceneManager.LoadSceneAsync(_additiveGameScene, LoadSceneMode.Additive);
        yield return new WaitUntil(() => _localSceneDependencyQueue.Count > 0);
        var localSceneDependency = _localSceneDependencyQueue.Dequeue();
        localSceneDependency.InitServer(GetMatchHeight());

        ChangePlayerScene(player1, localSceneDependency.gameObject.scene);
        ChangePlayerScene(player2, localSceneDependency.gameObject.scene);
    }

    private void ChangePlayerScene(PlayerPrefab player, Scene scene)
    {
        var client = player.connectionToClient;

        NetworkServer.RemovePlayerForConnection(client, false);
        SceneManager.MoveGameObjectToScene(player.gameObject, scene);
        client.Send(new SceneMessage
        {
            sceneName = scene.name,
            sceneOperation = SceneOperation.LoadAdditive,
            customHandling = true
        });
        NetworkServer.AddPlayerForConnection(client, player.gameObject);
    }

    private int GetMatchHeight()
    {
        int height = 0;
        return height;
    }

    [Server]
    public void OnLeave(PlayerPrefab player)
    {
        if (_players.Contains(player))
            _players.Remove(player);
    }

    public void AddNewSceneServer(LocalSceneDependency localSceneDependency)
    {
        _localSceneDependencyQueue.Enqueue(localSceneDependency);
    }
#endif
    #endregion
}
