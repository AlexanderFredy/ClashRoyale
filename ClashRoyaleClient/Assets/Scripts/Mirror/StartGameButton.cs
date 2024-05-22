using UnityEngine;
using Mirror;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuCanvas;
    public void Click()
    {
        _mainMenuCanvas.SetActive(false);
        NetworkManager.singleton.StartClient();
    }
}
