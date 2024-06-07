using Mirror;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class PlayerPrefab : NetworkBehaviour
{
    public string[] deck {  get; private set; }
    public void SetDeck(string[] deck) => this.deck = deck;
    public string sqlID { get; private set; }

    public void SetSqlId(string sqlID) => this.sqlID = sqlID;

    public override void OnStartClient()
    {
        print("OnStartClient");
        base.OnStartClient();
        string id = UserInfo.Instance.ID.ToString();
        OnJoint(id);
    }

    [TargetRpc]
    public void StartMatch(string[] playerDeck, string[] enemyDeck)
    {
        StartCoroutine(DelayStartMatch(playerDeck, enemyDeck));
    }

    private IEnumerator DelayStartMatch(string[] playerDeck, string[] enemyDeck)
    {
        MatchmakingManager matchmakingManager = MatchmakingManager.Instance;
        yield return new WaitUntil(() => matchmakingManager.localSceneDependency != null);
        matchmakingManager.localSceneDependency.cardManager.Init(playerDeck, enemyDeck);

        matchmakingManager.DestroyUI();
    }

#if UNITY_SERVER
    public override void OnStopServer()
    {
        MatchmakingManager.Instance.OnLeave(this);
        base.OnStopServer();
    }
#endif

    [Command]
    public void OnJoint(string sqlID)
    {
#if UNITY_SERVER
        if (string.IsNullOrEmpty(this.sqlID) == false) return;
        MatchmakingManager.Instance.OnJoint(this, sqlID);
#endif
    }

    [TargetRpc]
    public void SuccessConnected(string[] cardsID)
    {
        MatchmakingManager.Instance.ConnectedFinish(cardsID);
    }
}
