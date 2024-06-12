using Mirror;
using System;
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
    public void StartMatch(string[] playerDeck, string[] enemyDeck, int matchHeight, bool isEnemy)
    {
        StartCoroutine(DelayStartMatch(playerDeck, enemyDeck, matchHeight, isEnemy));
    }

    private IEnumerator DelayStartMatch(string[] playerDeck, string[] enemyDeck, int matchHeight, bool isEnemy)
    {
        MatchmakingManager matchmakingManager = MatchmakingManager.Instance;
        yield return new WaitUntil(() => matchmakingManager.localSceneDependency != null);
        matchmakingManager.localSceneDependency.cardManager.Init(playerDeck, enemyDeck);
        matchmakingManager.localSceneDependency.SetScene(matchHeight, isEnemy);

        matchmakingManager.DestroyUI();
    }

#if UNITY_SERVER
    public LocalSceneDependency localSceneDependency { get; private set; }

    public void SetLocalSceneDependency(LocalSceneDependency localSceneDependency) 
        => this.localSceneDependency = localSceneDependency;
    public override void OnStopServer()
    {
        MatchmakingManager.Instance.OnLeave(this);
        base.OnStopServer();
    }
#endif

    [Command]
    public void CmdSpawn(string id, Vector3 spawnPoint)
    {
#if UNITY_SERVER
        bool isEnemy = localSceneDependency.IsEnemy(this);
        localSceneDependency.spawner.Spawn(id, spawnPoint, isEnemy);
        FinishSpawn();
#endif
    }

    [TargetRpc]
    private void FinishSpawn()
    {
        MatchmakingManager.Instance.localSceneDependency.spawner.DestroyHologram();
    }

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
