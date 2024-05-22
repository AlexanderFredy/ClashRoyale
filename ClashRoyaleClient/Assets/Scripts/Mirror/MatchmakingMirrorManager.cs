using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchmakingMirrorManager : Singleton<MatchmakingMirrorManager>
{
    [SerializeField] private MatchmakingMirrorUI _matchmakingMirrorUI;

    [Client]
    public void ConnectedFinish(string[] cardIDs)
    {
        _matchmakingMirrorUI.SetImages(cardIDs);
    }

#if UNITY_SERVER
    public class StringArray
    {
        public string[] arr { get; set; }
    }

    private const string SECRET_KEY = "wlel383783@%!kvkw09!";
    private const string KEY = "key";
    private const string USERID = "userID";

    private List<PlayerPrefab> _players = new List<PlayerPrefab>();

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
        _players.Add(player);
        string json = "{\"arr\":" + arrString + "}";
        string[] cardsId = JsonUtility.FromJson<StringArray>(json).arr;
        player.SetSqlId(sqlID);
        player.SuccessConnected(cardsId);
    }

    [Server]
    public void OnLeave(PlayerPrefab player)
    {
        if (_players.Contains(player))
            _players.Remove(player);
    }
#endif
}
