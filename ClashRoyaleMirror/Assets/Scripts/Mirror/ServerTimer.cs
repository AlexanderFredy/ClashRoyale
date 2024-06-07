using Mirror;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class ServerTimer : NetworkBehaviour
{
    private const int _startDelay = 10;
    public void StartTick()
    {
        StartCoroutine(StartTickCoroutine());
    }

    private IEnumerator StartTickCoroutine()
    {
        WaitForSeconds oneSecond = new WaitForSeconds(1);

        for (int i = _startDelay; i > 0; i--)
        {
            ServerTicked(i);
            yield return oneSecond;
        }

        ServerTickFinished();
    }

    [ClientRpc]
    public void ServerTicked(int i)
    {
        MatchmakingManager.Instance.localSceneDependency.startTimer.StartTick(i);
    }

    [ClientRpc]
    public void ServerTickFinished()
    {
        MatchmakingManager.Instance.localSceneDependency.startTimer.Destroy();
    }
}
