using System.Collections;
using UnityEngine;

public class ConnectButton : MonoBehaviour
{
    public MultiplayerMirrorManager _manager;

    public void Click()
    {
        _manager.StartClient();
    }
}