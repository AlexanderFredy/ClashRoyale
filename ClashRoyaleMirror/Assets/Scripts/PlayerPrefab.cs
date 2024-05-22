using UnityEngine;
using Mirror;
using Unity.VisualScripting;

public class PlayerPrefab : NetworkBehaviour
{
    public NetworkMatch NetworkMatch;

    public override void OnStartClient()
    {
        Debug.Log("OnStartClient (NetworkBehaviour)");
        string sqlID = Random.Range(0, 10000).ToString();
        OnJoint(sqlID);
    }

    [Command]
    public void OnJoint(string sqlID)
    {
        Debug.Log("OnStartClient (NetworkBehaviour)" + sqlID);
        MatchmakingManager.Instance.OnJoint(this, sqlID);
    }
}