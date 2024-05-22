using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class PlayerPrefab : NetworkBehaviour
{
    public string sqlID { get; private set; }

    public void SetSqlId(string sqlID) => this.sqlID = sqlID;

    public override void OnStartClient()
    {
        base.OnStartClient();
        string id = UserInfo.Instance.ID.ToString();
        OnJoint(id);
    }

#if UNITY_SERVER
    public override void OnStopServer()
    {
        MatchmakingMirrorManager.Instance.OnLeave(this);
        base.OnStopServer();
    }
#endif

    [Command]
    public void OnJoint(string sqlID)
    {
#if UNITY_SERVER
        MatchmakingMirrorManager.Instance.OnJoint(this, sqlID);
#endif
    }

    [TargetRpc]
    public void SuccessConnected(string[] cardsID)
    {
        MatchmakingMirrorManager.Instance.ConnectedFinish(cardsID);
    }
}
