using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchmakingUI : MonoBehaviour
{
    [SerializeField] private CardsLibrary library;
    [SerializeField] private Image[] _images;
    [SerializeField] private GameObject _cancelButton;

    void Start()
    {
        _cancelButton.SetActive(false);
        for (int i = 0; i < _images.Length; i++)
        {
            _images[i].enabled = false;
        }
    }

    public void ClickCancel()
    {
        NetworkManager.singleton.StopClient();
    }

    [Client]
    public void SetImages(string[] cardIDs)
    {
        _cancelButton.SetActive(true);

        if (_images.Length != cardIDs.Length)
        {
            Debug.Log($"_images.Length != cardIDs.Length");
            return;
        }

        for (int i = 0; i < cardIDs.Length; i++)
        {
            int.TryParse(cardIDs[i], out int id);
            _images[i].sprite = library.GetCardByID(id).sprite;
            _images[i].enabled = true;
        }
    }
}
